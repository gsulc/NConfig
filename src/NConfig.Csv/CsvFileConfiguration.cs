using NConfig.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NConfig.Csv
{
    public class CsvFileConfiguration<TConfig> : FileConfiguration<TConfig>
        where TConfig : class, IList, new()
    {
        private Type _innerType = typeof(TConfig).GetGenericArguments().First();
        private PropertyInfo[] _innerProperties;

        public CsvFileConfiguration(string filePath)
            : base(filePath)
        {
        }

        private PropertyInfo[] InnerProperties => 
            _innerProperties ?? (_innerProperties = _innerType.GetProperties());

        public static CsvFileConfiguration<TConfig> FromDirectory(string directory)
        {
            string path = Path.Combine(directory, $"{nameof(TConfig)}.csv");
            return new CsvFileConfiguration<TConfig>(path);
        }

        public override TConfig Load()
        {
            var list = Activator.CreateInstance(typeof(TConfig)) as IList;
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var elements = line.Split(',');
                if (line == lines.First())
                {
                    ParseHeader(line);
                }
                else
                {
                    var obj = CreateObject(elements);
                    list.Add(obj);
                }
            }
            return list as TConfig;
        }

        // preserves the order of the properties
        private void ParseHeader(string line)
        {
            var elements = line.Split(',');
            var innerProperties = _innerType.GetProperties();
            _innerProperties = new PropertyInfo[innerProperties.Length];
            for (int i = 0; i < elements.Length; ++i)
            {
                var name = elements[i];
                _innerProperties[i] = innerProperties.Where(p => p.Name == name).First();
            }
        }

        private object CreateObject(string[] propertyStringValues)
        {
            var obj = Activator.CreateInstance(_innerType);
            for (int i = 0; i < _innerProperties.Length; ++i)
            {
                var stringValue = propertyStringValues[i];
                var type = _innerProperties[i].PropertyType;
                var value = Convert.ChangeType(stringValue, type);
                _innerProperties[i].SetValue(obj, value);
            }
            return obj;
        }

        public override void Save(TConfig value)
        {
            var list = value as IList;
            using (var stream = new StreamWriter(FullPath))
            {
                stream.WriteLine(GetHeader());
                foreach (var item in list)
                    stream.WriteLine(CreateItemLine(item));
            }
        }

        private string GetHeader()
        {
            return BuildLine(InnerProperties.Select(p => p.Name));
        }

        private string CreateItemLine(object item)
        {
            var values = InnerProperties.Select(p => p.GetValue(item).ToString());
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
