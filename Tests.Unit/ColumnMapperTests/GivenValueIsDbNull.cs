using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit.ColumnMapperTests
{
    [TestClass]
    public class GivenValueIsDbNull
    {
        private IDataRecord _record;

        [TestInitialize]
        public void TestInitialize()
        {
            _record = MockRepository.GenerateStub<IDataRecord>();

            _record
                .Stub(x => x[0])
                .Return(DBNull.Value);
        }

        [TestMethod]
        public void ValueTypeIsString()
        {
            // Arange
            var sut = new ColumnMapper<string>(0);
            
            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValueTypeIsInt()
        {
            // Arange
            var sut = new ColumnMapper<int>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ValueTypeIsDecimal()
        {
            // Arange
            var sut = new ColumnMapper<decimal>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ValueTypeIsDateTime()
        {
            // Arange
            var sut = new ColumnMapper<DateTime>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(DateTime.MinValue, result);
        }

        [TestMethod]
        public void ValueTypeIsBoolean()
        {
            // Arange
            var sut = new ColumnMapper<Boolean>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueTypeIsGuid()
        {
            // Arange
            var sut = new ColumnMapper<Guid>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(Guid.Empty, result);
        }
    }
}