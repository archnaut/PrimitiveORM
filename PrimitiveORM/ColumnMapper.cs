using System;
using System.Data;

namespace PrimitiveORM
{
    public class ColumnMapper<TValue>
    {
        private readonly int _index;

        public ColumnMapper(int index)
        {
            _index = index;
        }

        public TValue MapFrom(IDataRecord record)
        {
            var valueType = typeof(TValue);
            var value = record[_index];

            if(value == DBNull.Value)
                if(valueType == typeof(string))
                    value = string.Empty;
                else
                    return default(TValue);

            return (TValue) Convert.ChangeType(value, valueType);
        }
    }
}