using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EngineManager
{
    public static class Util
    {
    	/// <summary>
	    /// Adds all the data to a binding list
	    /// </summary>
	    public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> data)
	    {
	        if (list == null || data == null)
	        {
	            return;
	        }
	
	        foreach (T t in data)
	        {
	            list.Add(t);
	        }
	    }
	    
	    public static void AddRange<T>(this MyBindingList<T> list, IEnumerable<T> data)
	    {
	        if (list == null || data == null)
	        {
	            return;
	        }
	
	        foreach (T t in data)
	        {
	            list.Add(t);	            
	        }
	    }

        public enum AppData
        {
            Connections,
            Migration_Database_Development_Server,
            Migration_Database_Test_Server,
            Migration_Database_Production_Server,
            Migration_Database_Pre_Scripts,
            Migration_Database_Post_Scripts,
            Migration_Profiles
        }

        public static bool SaveData(AppData file, object data)
        {
            return SaveFile(file.ToString(), JsonConvert.SerializeObject(data, Formatting.Indented));            
        }

        private static bool SaveFile(string filename, string data)
        {
            if (!System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\data\"))
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\data\");

            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\data\" + filename + ".json", data);
            return true;
        }

        public static object ReadData(AppData file)
        {
            Type t = typeof(object);
            switch(file)
            {
                case AppData.Connections: t = typeof(List<SimpleConnection>); break;

                case AppData.Migration_Database_Development_Server: t = typeof(MigrationServerDetail); break;
                case AppData.Migration_Database_Test_Server: t = typeof(MigrationServerDetail); break;
                case AppData.Migration_Database_Production_Server: t = typeof(MigrationServerDetail); break;

                case AppData.Migration_Database_Pre_Scripts: t = typeof(List<DBMigrationScript>); break;
                case AppData.Migration_Database_Post_Scripts: t = typeof(List<DBMigrationScript>); break;
            }

            var s = ReadFile(file.ToString());

            if (s != null)
            {
                var o = JsonConvert.DeserializeObject(s, t);
                return o;
            }
            else
                return null;

            
        }

        private static string ReadFile(string filename)
        {
            var full_path = System.IO.Directory.GetCurrentDirectory() + @"\data\" + filename + ".json";
            if (System.IO.File.Exists(full_path))
                return System.IO.File.ReadAllText(full_path);

            return null;
        }

        public static string GetSql(string key)
        {
            var data = "";
            var full_path = System.IO.Directory.GetCurrentDirectory() + @"\sql\queries.sql";
            if (System.IO.File.Exists(full_path))
                data = System.IO.File.ReadAllText(full_path);

            var from = data.IndexOf("/* " + key + " */");
            var length = data.IndexOf("/* " + key + "_end */") - from;

            return data.Substring(from, length);
        }
        
        
    }
}
