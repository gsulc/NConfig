using IniParser.Model;
using System;

namespace NConfig.Ini
{
    public static class SectionDataExtensions
    {
        public static object Deserialize(this SectionData section, Type sectionType)
        {
            object sectionObject = Activator.CreateInstance(sectionType);
            foreach (var key in section.Keys)
            {
                var keyInfo = sectionType.GetProperty(key.KeyName);
                object value = Convert.ChangeType(key.Value, keyInfo.PropertyType);
                keyInfo.SetValue(sectionObject, value);
            }
            return sectionObject;
        }

        public static object Deserialize<TSection>(this SectionData section)
        {
            return section.Deserialize(typeof(TSection));
        }
    }
}
