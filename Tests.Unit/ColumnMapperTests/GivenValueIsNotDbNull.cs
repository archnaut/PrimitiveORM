using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit.ColumnMapperTests
{
    [TestClass]
    public class GivenValueIsNotDbNull
    {
        private IDataRecord _record;

        [TestInitialize]
        public void TestInitialize()
        {
            _record = MockRepository.GenerateStub<IDataRecord>();

        }

        [TestMethod]
        public void ValueTypeIsString()
        {
            // Arange
            _record
                .Stub(x => x[0])
                .Return("Value");

            var sut = new ColumnMapper<string>(0);
            
            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual("Value", result);
        }

        [TestMethod]
        public void ValueTypeIsInt()
        {
            // Arange
            _record
                .Stub(x => x[0])
                .Return(int.MinValue);


            var sut = new ColumnMapper<int>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(int.MinValue, result);
        }

        [TestMethod]
        public void ValueTypeIsDecimal()
        {
            // Arange
            _record
                .Stub(x => x[0])
                .Return(decimal.MinValue);

            var sut = new ColumnMapper<decimal>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(decimal.MinValue, result);
        }

        [TestMethod]
        public void ValueTypeIsDateTime()
        {
            // Arange
            _record
                .Stub(x => x[0])
                .Return(DateTime.MaxValue);

            var sut = new ColumnMapper<DateTime>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(DateTime.MaxValue, result);
        }

        [TestMethod]
        public void ValueTypeIsBoolean()
        {
            // Arange
            _record
                .Stub(x => x[0])
                .Return(true);

            var sut = new ColumnMapper<Boolean>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueTypeIsGuid()
        {
            // Arange
            var value = Guid.NewGuid();
            _record
                .Stub(x => x[0])
                .Return(value);


            var sut = new ColumnMapper<Guid>(0);

            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual(value, result);
        }
    }
}