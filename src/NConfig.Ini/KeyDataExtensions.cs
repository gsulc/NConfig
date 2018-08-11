using IniParser.Model;
using System;
using System.Linq;

namespace NConfig.Ini
{
    internal static class KeyDataExtensions
    {
        internal static void AddKeyData(this object obj, KeyDataCollection keyDataCollection)
        {
            foreach (var key in keyDataCollection)
            {
                var propertyInfos = obj.GetType().GetProperties();
                var keyInfo = propertyInfos.Where(i => string.Equals(i.Name, key.KeyName)).First();
                var keyType = keyInfo.PropertyType;
                object keyObject = Activator.CreateInstance(keyType);
                object value = Convert.ChangeType(key.Value, keyType);
                keyInfo.SetValue(obj, value);
            }
        }
    }
}
