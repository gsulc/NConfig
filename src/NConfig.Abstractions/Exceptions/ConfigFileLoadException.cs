using System;

namespace NConfig
{
    public class ConfigFileLoadException : Exception
    {
        public ConfigFileLoadException(string filePath)
            : base (GetMessage(filePath))
        {
            
        }

        public ConfigFileLoadException(string filePath, Exception e)
            : base(GetMessage(filePath), e)
        {

        }

        private static string GetMessage(string filePath)
        {
            return $"Error loading the configuration from '{filePath}'.";
        }
    }
}
