using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrimitiveORM
{
    public class QueryBuilder<T>
    {
        private readonly List<IParameter> _parameters = new List<IParameter>();

        public string CommandText { get; set; }
        public Func<IDataRecord, T> Map { get; set; }

        public void AddParameter<TValue>(string name, TValue value)
        {
            _parameters.Add(new Parameter<TValue>(name, value));
        }

        public IQueryDef<T> Build()
        {
            return new QueryDef<T>(CommandText, _parameters, Map);
        }

        internal interface IParameter
        {
            void AppendTo(IDbCommand command);
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

        private class QueryDef<TRecord> : IQueryDef<TRecord>
        {
            private readonly string _commandText;
            private readonly Func<IDataRecord, TRecord> _map;
            private readonly List<IParameter> _parameters;

            public QueryDef(string commandText, IEnumerable<IParameter> parameters, Func<IDataRecord, TRecord> map)
            {
                _commandText = commandText;
                _parameters = parameters.ToList();
                _map = map;
            }

            public void Prepare(IDbCommand command)
            {
                command.CommandText = _commandText;

                foreach(var parameter in _parameters)
                    parameter.AppendTo(command);
            }

            public TRecord Map(IDataRecord record)
            {
                return _map(record);
            }
        }
    }
}