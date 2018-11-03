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

        public static TSection Deserialize<TSection>(this SectionData section) where TSection : class, new()
        {
            return (TSection)section.Deserialize(typeof(TSection));
        }

        //public static object Serialize(this SectionData section, object sectionObject)
        //{

        //}
    }
}
