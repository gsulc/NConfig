using System;

namespace NConfig.Csv
{
    public static class ExtendedConvert
    {
        public static object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
                return (value is string) ?
                    Enum.Parse(type, value as string) :
                    Enum.ToObject(type, value);
            else
                return Convert.ChangeType(value, type);
        }
    }
}
