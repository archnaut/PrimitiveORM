using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit.DatabaseTests
{
    [TestClass]
    public class ExectueQueryTests
    {
        private IDbConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbCommand _command;
        private IDataReader _reader;
        private bool _read;
        private IQueryDef<object> _queryDef;
        private readonly List<object> _expectedResult = new List<object>{new object()};
        private List<object> _actualResult;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _connectionFactory = MockRepository.GenerateStub<IDbConnectionFactory>();
            _connection = MockRepository.GenerateStub<IDbConnection>();
            _command = MockRepository.GenerateStub<IDbCommand>();
            _reader = MockRepository.GenerateStub<IDataReader>();
            var sut = new Database(_connectionFactory);

            _queryDef = MockRepository.GenerateStub<IQueryDef<object>>();

            _connectionFactory
                .Stub(x => x.CreateConnection())
                .Return(_connection);

            _connection
                .Stub(x => x.CreateCommand())
                .Return(_command);

            _command
                .Stub(x => x.ExecuteReader())
                .Return(_reader);

            _reader
                .Stub(x => x.Read())
                .Do(new Func<bool>(() => _read = !_read));

            _queryDef
                .Stub(x => x.Map(_reader))
                .Return(_expectedResult.Single());

            _actualResult = sut.ExecuteQuery(_queryDef);
        }

        [TestMethod]
        public void ShouldPrepareCommand()
        {
            _queryDef.AssertWasCalled(x=>x.Prepare(_command));
        }

        [TestMethod]
        public void ShouldReturnResults()
        {
            CollectionAssert.AreEquivalent(_expectedResult, _actualResult);
        }
    }
}