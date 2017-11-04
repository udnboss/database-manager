using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
	/// <summary>
	/// Description of DBSchemaManager.
	/// </summary>
	public static class DBSchemaManager
	{
		public static void RefreshTables(DBSchema s)
		{
			var sql = string.Format(@"select TableName = t.name 
                                        , title = isnull( (select ep.value from sys.extended_properties ep where ep.major_id = t.object_id and ep.minor_id = 0 and ep.name = 'title') , t.name)
                                        from sys.schemas s 
                                        join sys.tables t on s.schema_id = t.schema_id 
                                        where s.name = '{0}' 
                                        order by t.name", s.Name);

			var cmd = new DBCommand { Sql = sql, Owner = s, Description = "Get Tables" };
			s.Tables.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBTable()
                {
                    OnAlreadyExists = DBObject.DBOnExists.DoNothingElsePerformAction,
                    Name = x["TableName"].ToString(),
                    Action = DB.DBAction.Alter,
                    Title = x["title"].ToString()
				}).ToList());
		}

		public static void RefreshViews(DBSchema s)
		{
			var sql = string.Format(@"select Name = o.name, Definition = m.definition 
                                        from sys.schemas s 
                                            join sys.objects o on s.schema_id = o.schema_id 
                                            join sys.sql_modules AS m ON o.object_id = m.object_id 
                                        where s.name = '{0}' and o.type = 'V'
                                        order by o.Name", s.Name);
			var cmd = new DBCommand { Sql = sql, Owner = s, Description = "Get Views" };
			s.Views.Clear();
			s.Views.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBView()
                {
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Name = x["Name"].ToString(),
                    DefinitionSQL = x["Definition"].ToString(),
                    Action = DB.DBAction.Alter
                }).ToList()
			    );
		}

		public static void RefreshProcedures(DBSchema s)
		{
			var sql = string.Format(@"select Name = o.name, Definition = m.definition 
                                        from sys.schemas s 
                                            join sys.objects o on s.schema_id = o.schema_id 
                                            join sys.sql_modules AS m ON o.object_id = m.object_id 
                                        where s.name = '{0}' and o.type = 'P'
                                        order by o.Name", s.Name);
			
			var cmd = new DBCommand { Sql = sql, Owner = s, Description = "Get Procedures" };
			
			s.StoredProcedures.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBStoredProcedure()
	            {
	                OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
	                Name = x["Name"].ToString(),
	                DefinitionSQL = x["Definition"].ToString(),
	                Action = DB.DBAction.Alter
	            }).ToList()
			);
		}

		public static void Refresh(DBSchema s)
		{
			DBObjectManager.BeforeRefresh(s);
			
			//get tables
			RefreshTables(s);

            //get views
            RefreshViews(s);

            //get procedures
            RefreshProcedures(s);

            //table valued functions

            //scalar functions

            //datatypes
			
            //properties
            DBObjectManager.RefreshProperties(s);
            
			DBObjectManager.AfterRefresh(s);
		}
	}
}
