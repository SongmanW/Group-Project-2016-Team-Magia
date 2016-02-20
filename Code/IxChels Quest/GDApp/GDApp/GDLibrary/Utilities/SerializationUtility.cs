using System.IO;
using System.Xml.Serialization;

namespace GDLibrary
{
    public class SerializationUtility
    {
        //See http://www.codeproject.com/Articles/483055/XML-Serialization-and-Deserialization-Part
        //See http://msdn.microsoft.com/en-us/library/ms172873.aspx
        public static void Save<T>(T obj, string filePath, string fileName, FileMode fileMode)
        {
            //make sure path + name will concatenate correctly by terminating path with "/" e.g. not c:/tempfilename.txt
            if (!filePath.EndsWith("/"))
                filePath += "/";

            if (!File.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);


            //open the file
            FileStream fStream = File.Open(filePath + fileName, fileMode);
            //convert to XML
            XmlSerializer xmlSerial = new XmlSerializer(typeof(T));
            //write to stream
            xmlSerial.Serialize(fStream, obj);
            //close the stream
            fStream.Close();
        }

        //See http://www.codeproject.com/Articles/487571/XML-Serialization-and-Deserialization-Part-2
        //See http://msdn.microsoft.com/en-us/library/ms172872.aspx
        public static T Load<T>(string filePath, string fileName, FileAccess fileAccess)
        {
            if (!filePath.EndsWith("/"))
                filePath += "/";

            if (File.Exists(filePath + fileName))
            {
                //open for read
                FileStream fStream = File.Open(filePath + fileName, FileMode.Open, fileAccess);

                //read the data from file
                XmlSerializer xmlSerial = new XmlSerializer(typeof(T));
                T data = (T)xmlSerial.Deserialize(fStream);

                //housekeeping
                fStream.Close();
                return data;
            }

            return default(T); //doesnt exist
        }

        public static void Delete(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}
