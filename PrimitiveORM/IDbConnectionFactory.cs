using System.Data;

namespace PrimitiveORM
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}