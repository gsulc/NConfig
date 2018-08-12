using IniParser;
using IniParser.Model;
using System;
using System.Reflection;

namespace NConfig.Ini
{
    public class IniSerializer
    {
        private readonly FileIniDataParser _parser = new FileIniDataParser();
        private readonly Type _type;
        private readonly PropertyInfo[] _propertyInfos;
        private object _object;

        public IniSerializer(Type type)
        {
            _type = type;
            _propertyInfos = _type.GetProperties();
        }

        public void Serialize(object obj, string path)
        {
            _object = obj;
            var iniData = ConstructIniData();
            Serialize(path, iniData);
        }

        private IniData ConstructIniData()
        {
            IniData data = new IniData();
            foreach (var propertyInfo in _propertyInfos)
            {
                var sa = GetIniSectionAttribute(propertyInfo);
                if (sa != null)
                {
                    var sectionData = CreateSectionData(sa.Name, propertyInfo);
                    data.Sections.Add(sectionData);
                }
                else
                {
                    var value = propertyInfo.GetValue(_object);
                    data.Global.AddKey(propertyInfo.Name, value.ToString());
                }
            }
            return data;
        }

        // The Section Attribute can be applied to either a property or the class of the property.
        private IniSectionAttribute GetIniSectionAttribute(PropertyInfo propertyInfo)
        {
            var sa = propertyInfo.GetCustomAttribute<IniSectionAttribute>();
            if (sa == null)
                sa = propertyInfo.PropertyType.GetCustomAttribute<IniSectionAttribute>();
            return sa;
        }

        private SectionData CreateSectionData(string name, PropertyInfo sectionInfo)
        {
            var sectionData = new SectionData(name);
            var sectionValue = sectionInfo.GetValue(_object);
            foreach (var keyInfo in sectionValue.GetType().GetProperties())
            {
                var keyValue = keyInfo.GetValue(sectionValue);
                sectionData.Keys.AddKey(keyInfo.Name, keyValue.ToString());
            }
            return sectionData;
        }

        private void Serialize(string path, IniData data)
        {
            _parser.WriteFile(path, data);
        }

        public object Deserialize(string path)
        {
            var data = _parser.ReadFile(path);
            return Deserialize(data);
        }

        private object Deserialize(IniData data)
        {
            _object = Activator.CreateInstance(_type);
            PopulateGlobalData(data);
            PopulateSectionData(data);
            return _object;
        }

        private void PopulateGlobalData(IniData data)
        {
            foreach (var key in data.Global)
            {
                var keyInfo = _type.GetProperty(key.KeyName);
                var keyType = keyInfo.PropertyType;
                object keyObject = Activator.CreateInstance(keyType);
                object value = Convert.ChangeType(key.Value, keyType);
                keyInfo.SetValue(_object, value);
            }
        }

        private void PopulateSectionData(IniData data)
        {
            foreach (var section in data.Sections)
            {
                var sectionInfo = FindSectionProperty(section.SectionName);
                if (sectionInfo != null)
                {
                    var sectionType = sectionInfo.PropertyType;
                    object sectionObject = section.Deserialize(sectionType);
                    sectionInfo.SetValue(_object, sectionObject);
                }
            }
        }

        private PropertyInfo FindSectionProperty(string sectionName)
        {
            foreach (var propertyInfo in _propertyInfos)
            {
                var sa = GetIniSectionAttribute(propertyInfo);
                if (sa != null && string.Equals(sectionName, sa.Name))
                    return propertyInfo;
            }
            return null;
        }
    }
}