using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace LinkRepository.Preferences
{
    public class PreferencesContainer
    {
        private readonly string _preferencesFile;

        private string[] _visibleColumns;

        [SerializableField]
        private float _linkTableFontSize = 10;
        [SerializableField]
        private int _thumbnailMaxHeight = 320;

        public float LinkTableFontSize
        {
            get => _linkTableFontSize;
            set
            {
                _linkTableFontSize = value;
                PreferencesChangedEvent?.Invoke(this);
            }
        }

        public string[] VisibleColumns
        {
            get => _visibleColumns;
            set
            {
                _visibleColumns = value;
                PreferencesChangedEvent?.Invoke(this);
            }
        }

        public int ThumbnailMaxHeight { get => _thumbnailMaxHeight; set => _thumbnailMaxHeight = value; }

        public event Action<PreferencesContainer> PreferencesChangedEvent;

        public PreferencesContainer(string file)
        {
            _preferencesFile = file;
            _visibleColumns = null;
        }

        private XmlElement SerializeVisibleColumns(XmlDocument doc)
        {
            XmlElement xmlElement = doc.CreateElement(string.Empty, "_visibleColumns", string.Empty);
            StringBuilder builder = new StringBuilder();
            foreach (var columnName in _visibleColumns)
            {
                builder.Append(columnName).Append("|");
            }

            xmlElement.InnerText = builder.ToString();
            return xmlElement;
        }

        private XmlElement SerializeField(XmlDocument doc, FieldInfo fieldInfo)
        {
            XmlElement xmlElement = doc.CreateElement(string.Empty, fieldInfo.Name, string.Empty);

            object value = fieldInfo.GetValue(this);
            xmlElement.InnerText = value.ToString();

            return xmlElement;
        }

        private void DeserializeField(XmlElement parentElement, FieldInfo fieldInfo)
        {
            XmlElement xmlElement = parentElement[fieldInfo.Name];
            if (xmlElement == null)
            {
                Debug.WriteLine($"Xml element with name {fieldInfo.Name} not found");
                return;
            }

            string text = xmlElement.InnerText;
            if (fieldInfo.FieldType == typeof(int))
            {
                int value = int.Parse(text);
                fieldInfo.SetValue(this, value);
            }
            if (fieldInfo.FieldType == typeof(float))
            {
                float value = float.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
                fieldInfo.SetValue(this, value);
            }
        }

        private void DeserializeVisibleColumns(XmlElement xmlElement)
        {
            string namesString = xmlElement.InnerText;
            string[] names = namesString.Split('|');
            _visibleColumns = names;
        }

        private static bool HasAttribute(FieldInfo fieldInfo, Type attributeType)
        {
            foreach (var attributeInfo in fieldInfo.CustomAttributes)
            {
                if (attributeInfo.AttributeType == attributeType)
                {
                    return true;
                }
            }

            return false;
        }

        private void SerializeFields(XmlDocument doc, XmlElement bodyElement)
        {
            Type selfType = GetType();
            FieldInfo[] fieldInfos = selfType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldInfo in fieldInfos)
            {
                if (HasAttribute(fieldInfo, typeof(SerializableFieldAttribute)))
                {
                    bodyElement.AppendChild(SerializeField(doc, fieldInfo));
                }
            }
        }

        private void DeserializeFields(XmlElement parentElement)
        {
            Type selfType = GetType();
            FieldInfo[] fieldInfos = selfType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldInfo in fieldInfos)
            {
                if (HasAttribute(fieldInfo, typeof(SerializableFieldAttribute)))
                {
                    DeserializeField(parentElement, fieldInfo);
                }
            }
        }


        public void Serialize()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement bodyElement = doc.CreateElement(string.Empty, "LinkRepositoryPreferences", string.Empty);
            doc.AppendChild(bodyElement);

            bodyElement.AppendChild(SerializeVisibleColumns(doc));
            
            SerializeFields(doc, bodyElement);
           
            doc.Save(_preferencesFile);
        }

        public bool Deserialize()
        {
            if (!File.Exists(_preferencesFile))
            {
                return false;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(_preferencesFile);
            XmlElement bodyElement = doc["LinkRepositoryPreferences"];
            if (bodyElement == null)
            {
                throw new PreferencesParsingException();
            }
            XmlElement visibleColumnsElement = bodyElement["_visibleColumns"];
            DeserializeVisibleColumns(visibleColumnsElement);
            DeserializeFields(bodyElement);

            return true;
        }
    }
}
