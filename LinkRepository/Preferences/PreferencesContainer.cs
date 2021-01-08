using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LinkRepository.Preferences
{
    public class PreferencesContainer
    {
        private string _preferencesFile;
        public List<string> VisibleColumns { get; set; }

        public PreferencesContainer(string file)
        {
            _preferencesFile = file;
            VisibleColumns = new List<string>();
        }

        private void SerializeVisibleColumns(XmlElement xmlElement)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var columnName in VisibleColumns)
            {
                builder.Append(columnName).Append("|");
            }

            xmlElement.InnerText = builder.ToString();
        }

        private void DeserializeVisibleColumns(XmlElement xmlElement)
        {
            VisibleColumns = new List<string>();
            string namesString = xmlElement.InnerText;
            string[] names = namesString.Split('|');
            VisibleColumns.AddRange(names);
        }

        public void Serialize()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement visibleColumnsElement = doc.CreateElement(string.Empty, "VisibleColumns", string.Empty);
            doc.AppendChild(visibleColumnsElement);
            SerializeVisibleColumns(visibleColumnsElement);

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
            XmlElement visibleColumnsElement = doc["VisibleColumns"];
            DeserializeVisibleColumns(visibleColumnsElement);

            return true;
        }
    }
}
