using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit
{
    [TestClass]
    public class ExecuteScalarTests
    {
        private IDbConnectionFactory _connectionFactory;
        private ICommandDef _commandDef;
        private IDbCommand _command;
        private IDbConnection _connection;
        private readonly object _expectedResult = new object();
        private object _actualResult;

        [TestInitialize]
        public void TestInitialize()
        {
            _connectionFactory = MockRepository.GenerateStub<IDbConnectionFactory>();
            _commandDef = MockRepository.GenerateStub<ICommandDef>();
            _command = MockRepository.GenerateStub<IDbCommand>();
            _connection = MockRepository.GenerateStub<IDbConnection>();

            _connectionFactory
                .Stub(x => x.CreateConnection())
                .Return(_connection);

            _connection
                .Stub(x => x.CreateCommand())
                .Return(_command);

            _command
                .Stub(x => x.ExecuteScalar())
                .Return(_expectedResult);

            var sut = new Database(_connectionFactory);

            _actualResult = sut.ExecuteScalar<object>(_commandDef);
        }

        [TestMethod]
        public void ShouldPrepareCommand()
        {
            _commandDef.AssertWasCalled(x=>x.Prepare(_command));
        }

        [TestMethod]
        public void ShouldExecuteCommand()
        {
            Assert.AreEqual(_expectedResult, _actualResult);
        }
    }
}