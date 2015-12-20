using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit.DatabaseTests
{
    [TestClass]
    public class ExecuteNonQueryTests
    {
        private IDbConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbCommand _command;
        private ICommandDef _commandDef;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _connectionFactory = MockRepository.GenerateStub<IDbConnectionFactory>();
            _connection = MockRepository.GenerateStub<IDbConnection>();
            _command = MockRepository.GenerateStub<IDbCommand>();
            _commandDef = MockRepository.GenerateStub<ICommandDef>();

            _connectionFactory
                .Stub(x => x.CreateConnection())
                .Return(_connection);

            _connection
                .Stub(x => x.CreateCommand())
                .Return(_command);

            var sut = new Database(_connectionFactory);

            sut.ExecuteNonQuery(_commandDef);
        }

        [TestMethod]
        public void ShouldPrepareCommand()
        {
            _commandDef.AssertWasCalled(x=>x.Prepare(_command));
        }

        [TestMethod]
        public void ShouldExecuteCommand()
        {
            _command.AssertWasCalled(x=>x.ExecuteNonQuery());
        }
    }
}