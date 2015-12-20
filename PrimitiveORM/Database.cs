using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

namespace PrimitiveORM
{
    public class Database
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public Database(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public TValue ExecuteScalar<TValue>(ICommandDef command)
        {
            using(var cnn = _connectionFactory.CreateConnection())
            using(var cmd = cnn.CreateCommand())
            {
                command.Prepare(cmd);

                if(cnn.State == ConnectionState.Closed)
                    cnn.Open();

                var result = cmd.ExecuteScalar();

                return result == null || result == DBNull.Value
                           ? default(TValue)
                           : (TValue) Convert.ChangeType(result, typeof(TValue));
            }
        }

        public void ExecuteNonQuery(ICommandDef command)
        {
            using(var cnn = _connectionFactory.CreateConnection())
            using(var cmd = cnn.CreateCommand())
            {
                command.Prepare(cmd);

                if(cnn.State == ConnectionState.Closed)
                    cnn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public List<T> ExecuteQuery<T>(IQueryDef<T> query)
        {
            var list = new List<T>();

            using(var cnn = _connectionFactory.CreateConnection())
            using(var cmd = cnn.CreateCommand())
            {
                query.Prepare(cmd);

                if(cnn.State == ConnectionState.Closed)
                    cnn.Open();

                using(var reader = cmd.ExecuteReader())
                    while(reader != null
                          && reader.Read())
                        list.Add(query.Map(reader));
            }

            return list;
        }

        public List<T> ExecuteQuery<T>(IQueryDef<T> query, TransactionScopeOption option)
        {
            using(var scope = new TransactionScope(option))
            {
                var result = ExecuteQuery(query);
                scope.Complete();
                return result;
            }
        }

        public ICommand CreateCommand(Action<CommandBuilder> prepare)
        {
            var builder = new CommandBuilder(_connectionFactory);
            prepare(builder);
            return builder.Build();
        }
    }

    public class CommandBuilder
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly List<IParameter> _parameters = new List<IParameter>();

        public CommandBuilder(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public string CommandText { get; set; }

        public void AddParameter<TValue>(string name, TValue value)
        {
            _parameters.Add(new Parameter<TValue>(name, value));                
        }

        public ICommand Build()
        {
            return new Command(_connectionFactory, CommandText, _parameters);
        }

        private class Parameter<TValue> : IParameter
        {
            private readonly string _name;
            private readonly TValue _value;

            public Parameter(string name, TValue value)
            {
                _name = name;
                _value = value;
            }

            public void AppendTo(IDbCommand command)
            {
                command.AddParameter(_name, _value);
            }
        }

        private class Command : ICommand
        {
            private readonly IDbConnectionFactory _connectionFactory;
            private readonly string _commandText;
            private readonly List<IParameter> _parameters;

            public Command(IDbConnectionFactory connectionFactory, string commandText, List<IParameter> parameters)
            {
                _connectionFactory = connectionFactory;
                _commandText = commandText;
                _parameters = parameters;
            }

            public IEnumerable<IDataRecord> ExecuteQuery()
            {
                using (var cnn = _connectionFactory.CreateConnection())
                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = _commandText;

                    foreach(var parameter in _parameters)
                        parameter.AppendTo(cmd);

                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    using (var reader = cmd.ExecuteReader())
                        while(reader != null && reader.Read())
                            yield return reader;
                }
            }

            public TValue ExectueScalar<TValue>()
            {
                using (var cnn = _connectionFactory.CreateConnection())
                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = _commandText;

                    foreach(var parameter in _parameters)
                        parameter.AppendTo(cmd);

                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    var result = cmd.ExecuteScalar();

                    return result == null || result == DBNull.Value
                               ? default(TValue)
                               : (TValue)Convert.ChangeType(result, typeof(TValue));
                }

            }

            public void ExectureNonQuery()
            {
                using (var cnn = _connectionFactory.CreateConnection())
                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = _commandText;

                    foreach(var parameter in _parameters)
                        parameter.AppendTo(cmd);

                    if (cnn.State == ConnectionState.Closed)
                        cnn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public interface ICommand
    {
        IEnumerable<IDataRecord> ExecuteQuery();
        TValue ExectueScalar<TValue>();
        void ExectureNonQuery();
    }

    public interface IParameter
    {
        void AppendTo(IDbCommand command);
    }
}