using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
	/// <summary>
	/// Description of DBManager.
	/// </summary>
	public static class DBManager
	{
		public static void RefreshSchemas(DB db)
		{
			const string sql = @"SELECT name [SchemaName], 
                            [SchemaType] = case when name in('cache','class','db', 'delete', 'dynamic', 'engine', 'filters', 'functions', 'history', 'log', 'migration','procedures', 'query', 'requests', 'search', 'secure', 'security', 'select', 'survey','update','views', 'workflow', 'computed', 'extensions', 'process', 'unprocess', 'sql', 'business') then 'engine' else 'user' end
                            FROM sys.schemas
                            WHERE schema_id between 5 and 16383 or schema_id = 1
                            order by name";
			
			var cmd = new DBCommand { Owner = db, Sql = sql };
			
			db.Schemas.AddRange(
				DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => 
		             new DBSchema() {
					Name = x["SchemaName"].ToString(),
					SchemaType = x["SchemaType"].ToString() == "engine" ? DBSchema.DBSchemaTypes.Engine : DBSchema.DBSchemaTypes.User,
					Action = DB.DBAction.Create
				}).ToList()
			);
			
		}

		public static void RefreshTriggers(DB db)
		{
			const string sql = @"SELECT name [Name], Definition = m.definition
                            FROM sys.triggers t
                                join sys.sql_modules m on t.object_id = m.object_id
                            WHERE t.parent_id = 0 
                            order by name";
			
			var cmd = new DBCommand { Owner = db, Sql = sql };
			
			db.DatabaseTriggers.AddRange(
				DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBDatabaseTrigger() {
					Name = x["Name"].ToString(),
					DefinitionSQL = x["Definition"].ToString(),
					Action = DB.DBAction.Alter,
					State = DBObject.DBObjectState.Intact,                
				}).ToList()
			);
		}

		public static void RefreshRoles(DB db)
		{
			const string sql = @"select name from sys.database_principals where type = 'r' and is_fixed_role = 0 order by name";
            
			var cmd = new DBCommand { Owner = db, Sql = sql };
			
			db.Roles.AddRange(
				DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBRole()
	            {
	                Name = x["name"].ToString(),
	                Action = DB.DBAction.Alter,
	                State = DBObject.DBObjectState.Intact,
	            }).ToList()
			);
		}

		public static void RefreshUsers(DB db)
		{
			const string sql = @"select name, type from sys.database_principals where type in( 'u', 's') and is_fixed_role = 0 order by name";
            
			var cmd = new DBCommand { Owner = db, Sql = sql };
			
			db.Users.AddRange(
				DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBUser()
	            {
	                Name = x["name"].ToString(),
	                Action = DB.DBAction.Alter,
	                State = DBObject.DBObjectState.Intact,
	            }).ToList()
			);
		}
		
		public static void Refresh(DB db)
        {
			//reset
			DBObjectManager.BeforeRefresh(db);
			
            //self data

            //get schemas
            RefreshSchemas(db);

            //get database triggers
            RefreshTriggers(db);

            //get database roles
            RefreshRoles(db);

            //get database users
            RefreshUsers(db);

            //properties
            DBObjectManager.RefreshProperties(db);
            
            //intact
            DBObjectManager.AfterRefresh(db);
        }
		
		public static void DisableAllTriggers(DB db)
        {
            var sql = string.Format("use {0}; disable trigger all on database;", db.FullName);
            DBProjectManager.Execute(new DBCommand { Owner = db, Sql = sql, Description = "Disable database triggers" });
            
            //MessageBox.Show("Database Triggers Disabled Successfully.");
        }

        public static void EnableAllTriggers(DB db)
        {
            var sql = string.Format("use {0}; enable trigger all on database;", db.FullName);
            DBProjectManager.Execute(new DBCommand { Owner = db, Sql = sql, Description = "Enable database triggers" });
            
            //MessageBox.Show("Database Triggers Enabled Successfully.");
        }
	}
}
