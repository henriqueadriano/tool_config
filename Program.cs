using ConsoleApp1.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ListObjectInfo listObjectInfo = ReadConfigFile();
            LoadReplaceJsonFile(listObjectInfo);
            
            //LoadXMLFile();
        }

        static void LoadXMLFile()
        {
            string xmlPath = @"C:\aspenCode_\agisms_server\Aspen_Code\External.Resource.Server.API\Connections.config";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.GetElementsByTagName("connectionStrings");
            foreach (XmlNode item in nodeList.Item(0).ChildNodes)
            {
                var log = (XmlNode)item.Attributes.GetNamedItem("name");
                if (log.Value.Equals("CORE_CONNECTION"))
                {
                    var connectionString = (XmlNode)item.Attributes.GetNamedItem("connectionString");
                }
            }
        }

        private static void CreateJsonFile(ObjFields objFileds)
        {
            JObject jsonFile = JObject.Parse(File.ReadAllText(objFileds.path));

            JObject appConfiguration = (JObject)jsonFile["AppConfiguration"];

            string devConn = (string)appConfiguration["DevelopmentConnection"];

            appConfiguration["DevelopmentConnection"] = BuildStringConnection(objFileds, devConn);

            FileStream fsOverwrite = new FileStream(objFileds.path, FileMode.Create);
            StreamWriter swOverwrite = new StreamWriter(fsOverwrite);
            using (JsonTextWriter writer = new JsonTextWriter(swOverwrite))
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonFile.WriteTo(writer);
            }
        }

        private static void LoadReplaceJsonFile(ListObjectInfo listObjectInfo)
        {
            if (listObjectInfo.StudentAuthObj.IsValid)
                CreateJsonFile(listObjectInfo.StudentAuthObj);
            if (listObjectInfo.StudentObj.IsValid)
                CreateJsonFile(listObjectInfo.StudentObj);
        }

        private static ListObjectInfo ReadConfigFile()
        {
            string[] lines = File.ReadAllLines("config.txt");

            StudentAuthObj studentAuthObj = FillAuthenticationFileData(lines);

            StudentObj studentObj = FillStudentFileData(lines);

            return new ListObjectInfo
            {
                StudentAuthObj = studentAuthObj,
                StudentObj = studentObj
            };
        }

        private static StudentObj FillStudentFileData(string[] lines)
        {
            bool isValid = false;

            StudentObj saObj = new StudentObj();

            foreach (string line in lines)
            {
                if (line.Contains("S_Server"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.server = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("S_Database"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.database = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("S_UserId"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.userId = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("S_Password"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.password = line.Substring(line.IndexOf("=") + 1).Trim();
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
                if (line.Contains("SA_Server"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.server = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("SA_Database"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.database = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("SA_UserId"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.userId = line.Substring(line.IndexOf("=") + 1).Trim();
                }

                if (line.Contains("SA_Password"))
                {
                    string value = line.Substring(line.IndexOf("=") + 1).Trim();
                    isValid = value != string.Empty && value != "?";
                    saObj.password = line.Substring(line.IndexOf("=") + 1).Trim();
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

        private static string BuildStringConnection(ObjFields info, string devConn)
        {
            string[] listInfo = devConn.Split(";");

            StringBuilder sb = new StringBuilder();

            foreach (string item in listInfo)
            {
                if (item.Split('=')[0].Equals("Server"))
                {
                    sb.Append(item.Split('=')[0]);
                    sb.Append("=");
                    sb.Append(info.server);
                    sb.Append(";");
                }
                if (item.Split('=')[0].Equals("Database"))
                {
                    sb.Append(item.Split('=')[0]);
                    sb.Append("=");
                    sb.Append(info.database);
                    sb.Append(";");
                }
                if (item.Split('=')[0].Equals("User Id"))
                {
                    sb.Append(item.Split('=')[0]);
                    sb.Append("=");
                    sb.Append(info.userId);
                    sb.Append(";");
                }
                if (item.Split('=')[0].Equals("Password"))
                {
                    sb.Append(item.Split('=')[0]);
                    sb.Append("=");
                    sb.Append(info.password);
                }
            }

            return sb.ToString();
        }

    }

    /*RESOURCES*/

    /*
    https://www.dotnetperls.com/path
    https://stackoverflow.com/questions/7405828/streamwriter-rewrite-the-file-or-append-to-the-file/7405989
    */

    /*HELPER FUNCTIONS*/

    /*private static void LoadJsonFile(string fileLocation)
    {
        JObject o1 = JObject.Parse(File.ReadAllText(fileLocation));

        // read JSON directly from a file
        using (StreamReader file = File.OpenText(fileLocation))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JObject o2 = (JObject)JToken.ReadFrom(reader);

            foreach (var item in o2)
            {
                if (item.Key.Equals("AppConfiguration"))
                {
                    var tokens = item.Value.SelectToken("DevelopmentConnection");

                }
            }
        }
    }*/

    /*
    //GET USER INFO TYPED
    //Console.WriteLine("Type your file path: ");
    //string s1 = Console.ReadLine();

    //GET FILE NAME
    //string jsonFile = @"C:\aspenCode_\student_authentication\Student.Auth.API\Student.Auth.API\appsettings.json";
    //string filename = Path.GetFileName(jsonFile);
     */

    /*
    private static void PrintFile(string sa_path)
    {
        foreach (var item in File.ReadAllLines(sa_path))
        {
            Console.WriteLine(item);
        }
    }
    */
}
