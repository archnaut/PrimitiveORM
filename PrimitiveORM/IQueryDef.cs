using System.Data;

namespace PrimitiveORM
{
    public interface IQueryDef<out T> 
    {
        void Prepare(IDbCommand command);

        T Map(IDataRecord record);
    }
}