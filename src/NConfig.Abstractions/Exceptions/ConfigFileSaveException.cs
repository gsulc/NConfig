using System;

namespace NConfig
{
    public class ConfigFileSaveException : Exception
    {
        public ConfigFileSaveException(Type type, string filePath)
            : base(GetMessage(type, filePath))
        {

        }

        public ConfigFileSaveException(Type type, string filePath, Exception e)
            : base(GetMessage(type, filePath), e)
        {

        }

        private static string GetMessage(Type type, string filePath)
        {
            return $"Error saving the configuration of type '{type.Name}' to '{filePath}'.";
        }
    }
}
