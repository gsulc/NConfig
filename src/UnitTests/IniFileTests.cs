using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NConfig.Abstractions;
using NConfig.Ini;

namespace UnitTests
{
    [TestClass]
    public class IniFileTests
    {
        [TestMethod]
        public void TestReadBadType()
        {
            var config = new IniFileConfiguration<SimpleIntConfig>(@"Files\\BadIntConfig.ini");
            try
            {
                config.Load();
                Assert.Fail();
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
