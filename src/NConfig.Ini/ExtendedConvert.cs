using System;

namespace NConfig.Ini
{
    public static class ExtendedConvert
    {
        public static object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
                return (value is string) ?
                    Enum.Parse(type, value as string) :
                    Enum.ToObject(type, value);
            else if (type == typeof(TimeSpan) && value is string)
                return TimeSpan.Parse(value as string);
            else
                return Convert.ChangeType(value, type);
        }
    }
}
