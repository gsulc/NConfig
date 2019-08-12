using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NConfig.Csv
{
    public class CsvFileConfiguration<TConfig> : 
        FileConfiguration<TConfig>, IListConfiguration<TConfig>
        where TConfig : class, new()
    {
        private PropertyInfo[] _configProperties = typeof(TConfig).GetProperties();

        public CsvFileConfiguration(string filePath)
            : base(filePath)
        {
        }

        public bool UsingHeader { get; set; } = true;

        public static CsvFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}s.csv");
            return new CsvFileConfiguration<TConfig>(path);
        }

        public List<TConfig> Load()
        {
            try
            {
                var list = new List<TConfig>();
                var lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var elements = line.Split(',');
                    if (UsingHeader && line == lines.First())
                    {
                        ParseHeader(line);
                    }
                    else
                    {
                        var obj = CreateObject(elements);
                        list.Add(obj);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                throw new ConfigFileLoadException(FilePath, e);
            }
        }

        // preserves the order of the properties in the file
        private void ParseHeader(string line)
        {
            var elements = line.Split(',');
            var properties = typeof(TConfig).GetProperties();
            _configProperties = new PropertyInfo[properties.Length];
            for (int i = 0; i < elements.Length; ++i)
            {
                var name = elements[i];
                _configProperties[i] = properties.Where(p => p.Name == name).First();
            }
        }

        private TConfig CreateObject(string[] propertyStringValues)
        {
            var obj = (TConfig)Activator.CreateInstance(typeof(TConfig));
            for (int i = 0; i < _configProperties.Length; ++i)
            {
                var stringValue = propertyStringValues[i];
                var type = _configProperties[i].PropertyType;
                var value = ExtendedConvert.ChangeType(stringValue, type);
                _configProperties[i].SetValue(obj, value);
            }
            return obj;
        }

        public void Save(List<TConfig> list)
        {
            using (var stream = new StreamWriter(FullPath))
            {
                if (UsingHeader)
                    stream.WriteLine(GetHeader());
                foreach (var item in list)
                    stream.WriteLine(CreateItemLine(item));
            }
        }

        private string GetHeader()
        {
            return BuildLine(_configProperties.Select(p => p.Name));
        }

        private string CreateItemLine(object item)
        {
            var values = _configProperties.Select(p => p.GetValue(item).ToString());
            return BuildLine(values);
        }

        private string BuildLine(IEnumerable<string> elements)
        {
            var builder = new StringBuilder();
            var lastElement = elements.Last();
            foreach (var element in elements)
            {
                builder.Append(element);
                bool atTheEnd = element == lastElement;
                if (!atTheEnd)
                    builder.Append(",");
            }
            return builder.ToString();
        }
    }
}
