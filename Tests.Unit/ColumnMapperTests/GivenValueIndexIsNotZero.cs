using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveORM;
using Rhino.Mocks;

namespace Tests.Unit.ColumnMapperTests
{
    [TestClass]
    public class GivenValueIndexIsNotZero
    {
        private IDataRecord _record;

        [TestInitialize]
        public void TestInitialize()
        {
            _record = MockRepository.GenerateStub<IDataRecord>();
        }

        [TestMethod]
        public void IndexIsOne()
        {
            // Arange
            const int index = 1;

            _record
                .Stub(x => x[index])
                .Return("Value");

            var sut = new ColumnMapper<string>(index);
            
            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual("Value", result);
        }

        [TestMethod]
        public void IndexIsMaxValue()
        {
            // Arange
            const int index = int.MaxValue;

            _record
                .Stub(x => x[index])
                .Return("Value");

            var sut = new ColumnMapper<string>(index);
            
            // Act
            var result = sut.MapFrom(_record);

            // Assert
            Assert.AreEqual("Value", result);
        }
    }
}