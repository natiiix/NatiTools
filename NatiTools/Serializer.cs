using System.IO;
using System.Xml.Serialization;

namespace NatiTools.xIO
{
    public static class SerializerXML
    {
        public static void Serialize<T>(T obj, string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(xmlFile))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T Deserialize<T>(string xmlFile)
        {
            T obj;

            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(xmlFile))
            {
                obj = (T)deserializer.Deserialize(reader);
                reader.Close();
            }

            return obj;
        }
    }
}
