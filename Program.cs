using ConsoleApp1.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ListObjectInfo listObjectInfo = ReadConfigFile();
            LoadReplaceFile(listObjectInfo);
        }

        private static void CreateJsonFile(ObjFields objFileds)
        {
            JObject jsonFile = JObject.Parse(File.ReadAllText(objFileds.path));

            JObject appConfiguration = (JObject)jsonFile["AppConfiguration"];

            appConfiguration["DevelopmentConnection"] = objFileds.StringConn;

            FileStream fsOverwrite = new FileStream(objFileds.path, FileMode.Create);
            StreamWriter swOverwrite = new StreamWriter(fsOverwrite);
            using (JsonTextWriter writer = new JsonTextWriter(swOverwrite))
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonFile.WriteTo(writer);
            }
        }

        private static void LoadReplaceFile(ListObjectInfo listObjectInfo)
        {
            if (listObjectInfo.StudentAuthObj.IsValid)
                CreateJsonFile(listObjectInfo.StudentAuthObj);
            if (listObjectInfo.StudentObj.IsValid)
                CreateJsonFile(listObjectInfo.StudentObj);
            if (listObjectInfo.ExternalObject.IsValid)
                CreateXMLFile(listObjectInfo.ExternalObject);
        }

        private static void CreateXMLFile(ExternalObject externalObject)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(externalObject.path);
            XmlNodeList nodeList = doc.GetElementsByTagName("connectionStrings");
            foreach (XmlNode item in nodeList.Item(0).ChildNodes)
            {
                var log = (XmlNode)item.Attributes.GetNamedItem("name");
                if (log.Value.Equals("CORE_CONNECTION"))
                {
                    item.Attributes.GetNamedItem("connectionString").Value = externalObject.StringConn;
                }
            }
            doc.Save(externalObject.path);
        }

        private static ListObjectInfo ReadConfigFile()
        {
            string[] lines = File.ReadAllLines("config.txt");

            StudentAuthObj studentAuthObj = FillAuthenticationFileData(lines);

            StudentObj studentObj = FillStudentFileData(lines);

            ExternalObject externalObject = FillExternalFileData(lines);

            return new ListObjectInfo
            {
                StudentAuthObj = studentAuthObj,
                StudentObj = studentObj,
                ExternalObject = externalObject
            };
        }

        private static ExternalObject FillExternalFileData(string[] lines)
        {
            bool isValid = false;

            ExternalObject saObj = new ExternalObject();

            foreach (string line in lines)
            {
                if (line.Contains("E_StringConn"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.StringConn = line.Substring(line.IndexOf("=") + 1).Trim();
                }
                if (line.Contains("E_PATH_XML_FILE"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.path = line.Substring(line.IndexOf("=") + 1).Trim();
                }
            }

            saObj.IsValid = isValid;

            return saObj;
        }

        private static StudentObj FillStudentFileData(string[] lines)
        {
            bool isValid = false;

            StudentObj saObj = new StudentObj();

            foreach (string line in lines)
            {
                if (line.Contains("S_StringConn"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.StringConn = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("S_PATH_JSON_FILE"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.path = line.Substring(line.IndexOf("=") + 1).Trim();
                }
            }
            saObj.IsValid = isValid;
            return saObj;
        }

        private static StudentAuthObj FillAuthenticationFileData(string[] lines)
        {
            bool isValid = false;

            StudentAuthObj saObj = new StudentAuthObj();

            foreach (string line in lines)
            {
                if (line.Contains("SA_StringConn"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.StringConn = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("SA_PATH_JSON_FILE"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.path = line.Substring(line.IndexOf("=") + 1).Trim();
                }
            }
            saObj.IsValid = isValid;
            return saObj;
        }

    }
}
