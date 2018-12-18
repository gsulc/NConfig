using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NConfig.Abstractions;

namespace UnitTests
{
    [TestClass]
    public class ConvertTest
    {
        enum TestEnum
        {
            One = 1,
            Two = 2,
            Three = 3
        }

        [TestMethod]
        public void TestEnumStringCast()
        {
            var value = ExtendedConvert.ChangeType("Two", typeof(TestEnum));
            Assert.AreEqual(TestEnum.Two, value);
        }

        [TestMethod]
        public void TestEnumNumberCast()
        {
            var value = ExtendedConvert.ChangeType(2, typeof(TestEnum));
            Assert.AreEqual(TestEnum.Two, value);
        }
    }
}
