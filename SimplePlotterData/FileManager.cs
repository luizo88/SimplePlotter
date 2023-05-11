using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimplePlotterData
{
    public static class FileManager
    {
        public static void SaveXML(DataObject dataObject, string fileName)
        {
            var serializer = new XmlSerializer(typeof(DataObject));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, dataObject);
            }
        }

        public static DataObject OpenXML(string fileName)
        {
            DataObject result;
            var serializer = new XmlSerializer(typeof(DataObject));
            using (var reader = XmlReader.Create(fileName))
            {
                result = (DataObject)serializer.Deserialize(reader);
            }
            return result;
        }

    }
}

