using IniParser.Model;
using System;

namespace NConfig.Ini
{
    internal static class KeyDataExtensions
    {
        internal static void SetKeyDataToProperties(this KeyDataCollection keyDataCollection, object obj)
        {
            foreach (var key in keyDataCollection)
            {
                var keyInfo = obj.GetType().GetProperty(key.KeyName);
                var keyType = keyInfo.PropertyType;
                object keyObject = Activator.CreateInstance(keyType);
                object value = Convert.ChangeType(key.Value, keyType);
                keyInfo.SetValue(obj, value);
            }
        }
    }
}
