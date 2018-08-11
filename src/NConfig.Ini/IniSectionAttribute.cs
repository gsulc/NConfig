using System;

namespace NConfig.Ini
{
    public class IniSectionAttribute : Attribute
    {
        public IniSectionAttribute()
        {
            Name = TypeId.GetType().Name;
        }

        public IniSectionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
