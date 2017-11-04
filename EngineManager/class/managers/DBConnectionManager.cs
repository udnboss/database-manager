using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
	/// <summary>
	/// Description of DBConnectionManager.
	/// </summary>
	public static class DBConnectionManager
	{
		public static DBConnection Connect(string server, string database, string user, string password)
		{
			var conn = new DBConnection {
				Server = server,
				InitialDatabase = database,
				User = user,
				Password = password
			};
			
			return conn;
		}
		
		public static void Refresh(DBConnection c)
		{
			const string sql = "select name from sys.databases order by name";
			var cmd = new DBCommand { Sql = sql, Owner = new DBObject { Connection = c }, Description = "Get Databases" };
			
			c.Databases.Clear();
			
			c.Databases.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DB()
            {
                Name = x["name"].ToString(),
                Connection = c
            }).ToList());

            var initial_db = c.Databases.FirstOrDefault(x => x.Name == c.InitialDatabase);
            DBManager.Refresh(initial_db);
		}
	}
}
