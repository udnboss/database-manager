using System;
using System.Data;
using System.Linq;
using System.Security.Policy;

namespace EngineManager
{
	/// <summary>
	/// Description of DBProjectManager.
	/// </summary>
	public static class DBProjectManager 
	{
		public static DBProject Project = new DBProject{ Name = "New Project" };
		
		public static DBConnection Connect(string server, string database, string user, string password) 
		{
			var c = new DBConnection { Server = server, InitialDatabase = database, User = user, Password = password };
			Project.Connections.Add(c);
			return c;
		}
		
		public static void Execute(DBCommand c)
		{
			if (c.State == DBCommand.CommandState.New || c.State == DBCommand.CommandState.Failed)
            {
                var data = c.Owner.Connection.GetDataTable(c.Sql);
				c.State = data != null ? DBCommand.CommandState.Executed : DBCommand.CommandState.Failed;
            }
        }
        
        public static DataTable ExecuteQuery(DBCommand c, DBConnection conn = null) //query
		{
			var connection = c.Owner.Connection ?? conn;
			if (connection != null) 
			{
				var data = c.Owner.Connection.GetDataTable(c.Sql);
				c.State = data != null ? DBCommand.CommandState.Executed : DBCommand.CommandState.Failed;
				return data;
			}
			
			c.State = DBCommand.CommandState.Failed;
			return null;
            
		}
	}
}
