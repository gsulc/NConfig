using System;

namespace NConfig.Ini
{
    public class IniSectionAttribute : Attribute
    {
        public IniSectionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
