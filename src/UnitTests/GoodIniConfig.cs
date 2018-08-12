using NConfig.Ini;

namespace UnitTests
{
    internal enum AnEnum
    {
        Value0,
        Value1,
        Value2,
    }

    /// <summary>
    /// Ini configurations can only go a second layer deep, unlike xml and json.
    /// </summary>
    internal class GoodIniConfig
    {
        #region Global Settings

        public int Integer { get; set; }
        public bool Boolean { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public char Character { get; set; }
        public AnEnum SomeEnum { get; set; }

        #endregion

        public Section1 Section1 { get; set; }

        // The section attribute can be applied to either the class or the property.
        [IniSection("Section Two")]
        public Section2 Section2 { get; set; }
    }

    // The section attribute can be applied to either the class or the property.
    [IniSection("Section One")]
    internal class Section1
    {
        public int Integer { get; set; }
        public bool Boolean { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public char Character { get; set; }
        public AnEnum SomeEnum { get; set; }
    }

    internal class Section2
    {
        public int Integer { get; set; }
        public bool Boolean { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public char Character { get; set; }
        public AnEnum SomeEnum { get; set; }
    }
}
