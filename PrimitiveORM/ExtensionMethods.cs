using System;
using System.Data;

namespace PrimitiveORM
{
    public static class ExtensionMethods
    {
        public static void AddParameter<TValue>(this IDbCommand command, string name, TValue value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            command.Parameters.Add(parameter);
        }

        public static void AddOutParameter(this IDbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;

            command.Parameters.Add(parameter);
        }

        public static TValue GetValue<TValue>(this IDataRecord record, string columnName)
        {
            var valueType = typeof(TValue);
            var value = record[columnName];

            if(value == DBNull.Value)
                if(valueType == typeof(DateTime))
                    value = new DateTime(1753, 1, 1);
                else
                    value = default(TValue);

            return (TValue) Convert.ChangeType(value, valueType);
        }
    }
}