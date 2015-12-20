using System.Data;

namespace PrimitiveORM
{
    public interface ICommandDef
    {
        void Prepare(IDbCommand command);
    }
}