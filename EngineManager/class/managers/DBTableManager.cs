using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace EngineManager
{
    public static class DBTableManager
    {
        public static void Refresh(DBTable t)
        {
            //reset state and collections
            DBObjectManager.BeforeRefresh(t);

            //columns
            DBObjectManager.RefreshColumns(t);

            //primary key
            RefreshPrimaryKey(t);

            //triggers
            RefreshTriggers(t);

            //policies (select, update, delete)
            RefreshPolicies(t);

            //extensions (views/computed/secure) for extra columns
            RefreshExtensions(t);

            //table level properties
            RefreshTableProperties(t);

            //business
            RefreshBusinessProcedures(t);

            //foreign key relations
            RefreshForeignKeys(t);

            //check constraints
            RefreshCheckConstraints(t);

            //default constraints
            RefreshDefaultConstraints(t);

            //properties
            DBObjectManager.RefreshProperties(t);
            
            //state intact
            DBObjectManager.AfterRefresh(t);
        }

        public static void RefreshDefaultConstraints(DBTable t)
        {
            var sql = string.Format(@"
                    select
	                    ds.name
	                    , ds.definition
	                    , ds.type_desc
	                    , [column] = c.name
                    from
	                    sys.default_constraints ds
	                    join sys.tables t on ds.parent_object_id = t.object_id
	                    join sys.schemas s on t.schema_id = s.schema_id
	                    join sys.columns c on c.object_id = t.object_id and c.column_id = ds.parent_column_id
                    where
	                    s.name = '{0}' and t.name = '{1}'
                    ", t.Schema.Name, t.Name);

        	var cmd = new DBCommand { Sql = sql, Owner = t, Description = "Get Default Constraints" };
        	
        	t.DefaultConstraints.Clear();
        	t.DefaultConstraints.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBDefaultConstraint
            {
                Name = x["name"].ToString(),
                Expression = x["definition"].ToString(),
				Column = t.Columns.FirstOrDefault(c => c.Name == x["column"].ToString()),
                Parent = t,
                Action = DB.DBAction.Alter,
                Connection = t.Connection,
                State = EngineManager.DBObject.DBObjectState.Intact
            }).ToList()
        	                             );
        }

        public static void RefreshCheckConstraints(DBTable t)
        {
            var sql = string.Format(@"
                    select
	                    cs.name
	                    , cs.definition
	                    , cs.is_disabled
                        , fail_message = (select top 1 ep.value from sys.extended_properties ep where ep.major_id = cs.object_id and ep.minor_id = 0 and ep.name = 'MS_Description')
                    from
	                    sys.check_constraints cs
	                    join sys.tables t on cs.parent_object_id = t.object_id
                        join sys.schemas s on t.schema_id = s.schema_id
                    where
                        s.name = '{0}' and t.name = '{1}'
                    ", t.Schema.Name, t.Name);
        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Check Constraints" };
        	
        	t.CheckConstraints.Clear();
        	t.CheckConstraints.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBCheckConstraint
            {
                Name = x["name"].ToString(),
                Expression = x["definition"].ToString(),
                Enabled = !(bool)x["is_disabled"],
                FailMessage = x["fail_message"].ToString(),
                Action = DB.DBAction.Alter,
                State = EngineManager.DBObject.DBObjectState.Intact
            }).ToList());
        }

        public static void RefreshForeignKeys(DBTable t)
        {
            var sql = string.Format(@"
            select
                ConstraintName = t1.fk_name,
                SchemaName = t1.fk_schema,
                TableName = t1.fk_table, 
                ColumnName = t1.fk_column, 
                referencedSchema = t1.pk_schema,
                referencedTable = t1.pk_table, 
                referencedColumn = t1.pk_column, 
                t1.update_action,
                t1.delete_action,
		        respect_read = cast(case when respect like '%R%' then 1 else 0 end as bit),
		        respect_create = cast(case when respect like '%C%' then 1 else 0 end as bit),
		        respect_update = cast(case when respect like '%U%' then 1 else 0 end as bit),
		        respect_delete = cast(case when respect like '%D%' then 1 else 0 end as bit),
		        cascade_lookup = cast(case when respect like '%L%' then 1 else 0 end as bit),
		        refresh_parents_on_change = cast(case when respect like '%S%' then 1 else 0 end as bit),
		        respect,
                title,
                is_lookup,
		        is_constraint = cast(1 as bit),
		        is_disabled,
		        relation_type = 'constraint',
		        for_finding_child = cast(1 as bit),
		        for_finding_parent = cast(1 as bit)
		
	        from 
		        (
			        SELECT   
				        fk_name = kc.name,    
				        pk_column_id = concat(pk_schema.name,'.',pktable.name, '.', pkcolumn.name),
				        fk_column_id = concat(fk_schema.name,'.',fktable.name, '.', fkcolumn.name),
				        fk_schema.name as [fk_schema]
				        , fktable.name AS fk_table
				        , fkcolumn.name AS fk_column
				        , pk_schema.name as [pk_schema]
				        , pktable.name AS pk_table
				        , pkcolumn.name AS pk_column
				        , kc.delete_referential_action_desc [delete_action]
				        , kc.update_referential_action_desc [update_action]			
				        , respect = cast(ep.value as nvarchar(max))
				        , title = cast(ept.value as nvarchar(max))
				        , is_lookup = cast(epl.value as nvarchar(max))
				        , kc.is_disabled
			        FROM 
				        sys.foreign_key_columns AS fkc INNER JOIN
				        sys.tables AS fktable ON fkc.parent_object_id = fktable.object_id INNER JOIN
				        sys.columns AS fkcolumn ON fkc.parent_column_id = fkcolumn.column_id AND fktable.object_id = fkcolumn.object_id INNER JOIN
				        sys.tables AS pktable ON fkc.referenced_object_id = pktable.object_id INNER JOIN
				        sys.columns AS pkcolumn ON fkc.referenced_column_id = pkcolumn.column_id  AND pktable.object_id = pkcolumn.object_id
				        INNER JOIN sys.schemas fk_schema ON fktable.schema_id = fk_schema.schema_id
				        INNER JOIN sys.schemas pk_schema ON pktable.schema_id = pk_schema.schema_id
				        INNER JOIN sys.foreign_keys kc on fkc.constraint_object_id = kc.object_id
				        left join sys.extended_properties ep on ep.major_id = fkc.constraint_object_id and ep.name = 'MS_Description'
				        left join sys.extended_properties ept on ept.major_id = fkc.constraint_object_id and ept.name = 'title'
				        left join sys.extended_properties epl on epl.major_id = fkc.constraint_object_id and epl.name = 'is_lookup'
			        where 
				        (fk_schema.name = '{0}' and fktable.name = '{1}')
				       -- or (pk_schema.name = '{0}' and pktable.name = '{1}')
				 
		        ) as t1", t.Schema, t.Name);

        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Foreign Key Constraints" };
        	
        	t.ForeignKeys.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBFKConstraint()
            {
                OnAlreadyExists = DBObject.DBOnExists.DropBeforePerformingAction,
                Name = x["ConstraintName"].ToString(),
				Schema = t.Schema.Database.Schemas.FirstOrDefault(s => s.Name == x["SchemaName"].ToString()),
                ReferenceSchemaName = x["referencedSchema"].ToString(),
                ReferenceTableName = x["referencedTable"].ToString(),
                ReferenceColumnName = x["referencedColumn"].ToString(),
				Column = t.Columns.FirstOrDefault(z => z.Name == x["ColumnName"].ToString()),
                RespectRead = (bool)x["respect_read"],
                RespectCreate = (bool)x["respect_create"],
                RespectUpdate = (bool)x["respect_update"],
                RespectDelete = (bool)x["respect_delete"],
                RefreshParent = (bool)x["refresh_parents_on_change"],
                CascadeLookup = (bool)x["cascade_lookup"],
                Title = x["title"].ToString(),
				IsLookup = x["is_lookup"].ToString() != "" && bool.Parse(x["is_lookup"].ToString()),
                OnDelete = x["delete_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                x["delete_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                    x["delete_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,
                OnUpdate = x["update_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                x["update_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                    x["update_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,

                Action = DB.DBAction.Alter
            }).ToList()
   			);
        }

        public static void RefreshBusinessProcedures(DBTable t)
        {
            var sql = string.Format(@"select
		                                SchemaName = s.name,
		                                ProcedureName = p.name,
		                                Definition = m.definition
	                                from
		                                sys.schemas s
		                                join sys.procedures p on p.schema_id = s.schema_id
		                                join sys.sql_modules m on m.object_id = p.object_id
	                                where
		                                s.name = 'business'
		                                and p.name like '{0}[_]{1}[_][_]%'", t.Schema, t.Name);
        	
        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Business Procedures" };
        	
        	t.Business.Clear();
        	t.Business.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBStoredProcedure()
            {
                OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                Name = x["ProcedureName"].ToString(),
				Schema = t.Schema.Database.Schemas.FirstOrDefault(s => s.Name == x["SchemaName"].ToString()),
                DefinitionSQL = x["Definition"].ToString(),
                Action = DB.DBAction.Alter,
                State = EngineManager.DBObject.DBObjectState.Intact
        	    }).ToList());
        }

        public static void RefreshTableProperties(DBTable t)
        {
            var sql = string.Format(@"
                select *
                from
                (
	                select TableName = t.name, property_name = ep.name, property_value = ep.value
		                from sys.schemas s 
		                join sys.tables t on s.schema_id = t.schema_id 
		                join sys.extended_properties ep on ep.major_id = t.object_id and ep.minor_id = 0
		                where s.name = '{0}' and t.name = '{1}'
		
                ) d
                pivot
                (
	                max(property_value)
	                for property_name in (title, iconclass, MS_Description, row_name, row_title, row_group, row_iconclass, row_image, row_labeltype, row_color, row_badge, row_date, row_subtitle, migration_data_source, migration_structure_source, migration_data_query, migration_structure_query, use_history, lookup_tooltip_columns)
                ) p
                order by TableName
                ", t.Schema.Name, t.Name);
        	
        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Table Properties" };

        	var row = DBProjectManager.ExecuteQuery(cmd).AsEnumerable().FirstOrDefault();

            if (row != null)
            {
                var cols = t.SecureView != null ? t.SecureView.Columns : t.Columns;

                t.Title = row["title"].ToString();
                t.IconClass = new DBIconClass { Class = row["iconclass"].ToString(), Value = row["iconclass"].ToString() };
                t.Description = row["MS_Description"].ToString();

				t.RowBadge = cols.FirstOrDefault(c => c.Name == row["row_badge"].ToString());
				t.RowSubTitle = cols.FirstOrDefault(c => c.Name == row["row_subtitle"].ToString());
				t.RowDate = cols.FirstOrDefault(c => c.Name == row["row_date"].ToString());
				t.RowIconClass = cols.FirstOrDefault(c => c.Name == row["row_iconclass"].ToString());
				t.RowImage = cols.FirstOrDefault(c => c.Name == row["row_image"].ToString());
				t.RowLabelType = cols.FirstOrDefault(c => c.Name == row["row_labeltype"].ToString());
				t.RowLabelType = cols.FirstOrDefault(c => c.Name == row["row_color"].ToString());
				t.RowName = cols.FirstOrDefault(c => c.Name == row["row_name"].ToString());
				t.RowTitle = cols.FirstOrDefault(c => c.Name == row["row_title"].ToString());
				t.RowGroup = cols.FirstOrDefault(c => c.Name == row["row_group"].ToString());
				t.UseHistory = !string.IsNullOrEmpty(row["use_history"].ToString()) && bool.Parse(row["use_history"].ToString());

                t.MigrationDataSource = !string.IsNullOrEmpty(row["migration_data_source"].ToString()) ? (EngineManager.DBTable.MigrationDataSources)Enum.Parse(typeof(EngineManager.DBTable.MigrationDataSources), row["migration_data_source"].ToString()) : EngineManager.DBTable.MigrationDataSources.Development;
                t.MigrationDataQuery = row["migration_data_query"].ToString();

                t.LookupTooltipColumns = row["lookup_tooltip_columns"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            }
        }

        public static void RefreshExtensions(DBTable t)
        {
            var sql = string.Format(@"
                select
		            SchemaName = s.name,
		            ViewName = v.name,
		            Definition = m.definition
	            from
		            sys.schemas s
		            join sys.views v on v.schema_id = s.schema_id
		            join sys.sql_modules m on m.object_id = v.object_id
	            where
		            s.name in ('computed', 'views', 'secure')
                    and v.name = '{0}_{1}'", t.Schema.Name, t.Name);
			
        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Table Extensions" };
            
        	t.Extensions.Clear();
        	t.Extensions.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBView()
            {
                OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                Name = x["ViewName"].ToString(),
				Schema = t.Schema.Database.Schemas.FirstOrDefault(s => s.Name == x["SchemaName"].ToString()),
                DefinitionSQL = x["Definition"].ToString(),
                Action = DB.DBAction.Alter,
                Connection = t.Connection,
                State = EngineManager.DBObject.DBObjectState.Intact
        	}).ToList());

            //objects for views
			t.ExtendedView = t.Extensions.FirstOrDefault(x => x.Schema.Name == "views");
			t.ComputedView = t.Extensions.FirstOrDefault(x => x.Schema.Name == "computed");
			t.SecureView = t.Extensions.FirstOrDefault(x => x.Schema.Name == "secure");

            foreach (var e in t.Extensions)
                DBObjectManager.RefreshColumns(e);
        }

        public static void RefreshPolicies(DBTable t)
        {
            var sql = string.Format(@"
                select
		            SchemaName = s.name,
		            ViewName = v.name,
		            Definition = m.definition
	            from
		            sys.schemas s
		            join sys.views v on v.schema_id = s.schema_id
		            join sys.sql_modules m on m.object_id = v.object_id
	            where
		            s.name in ('select', 'update', 'delete')
                    and v.name = '{0}_{1}'", t.Schema, t.Name);

        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Table Policies" };
        	
        	t.Policies.Clear();
        	
        	t.Policies.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBView()
            {
                OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                Name = x["ViewName"].ToString(),
				Schema = t.Schema.Database.Schemas.FirstOrDefault(s => s.Name == x["SchemaName"].ToString()),
                DefinitionSQL = x["Definition"].ToString(),
                Action = DB.DBAction.Alter,
                Connection = t.Connection,
                State = EngineManager.DBObject.DBObjectState.Intact
        	}).ToList());
        }

        public static void RefreshTriggers(DBTable t)
        {
            var sql = string.Format(@"
	                        select 
		                        SchemaName = s.name,
		                        ObjectName = o.name,
		                        ObjectType = o.type_desc,
		                        Definition = m.definition
	                        from
		                        sys.schemas s
		                        join sys.tables t on t.schema_id = s.schema_id
		                        join sys.triggers o on o.parent_id = t.object_id
		                        join sys.sql_modules m on m.object_id = o.object_id
	                        where 
		                        (s.name = '{0}') 
                                and (t.name = '{1}')", t.Schema.Name, t.Name);

        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Table Triggers" };
        	
        	t.Triggers.Clear();
        	
        	t.Triggers.AddRange(DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBTrigger()
            {
                OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                Name = x["ObjectName"].ToString(),
                DefinitionSQL = x["Definition"].ToString(),
                Action = DB.DBAction.Alter,                
                State = EngineManager.DBObject.DBObjectState.Intact
            }).ToList()
            );
        }

        public static void RefreshPrimaryKey(DBTable t)
        {
            var sql = string.Format(@"
                select
	                  pk_name = i.name
	                , col_name = c.name

                from
	                sys.schemas s
	                join sys.tables t on t.schema_id = s.schema_id
	                join sys.indexes i on i.object_id = t.object_id
	                join sys.index_columns ic on ic.index_id = i.index_id and ic.object_id = t.object_id
	                join sys.columns c on t.object_id = c.object_id and c.column_id = ic.column_id

                where
	                i.is_primary_key = 1
	                and s.name = '{0}'
	                and t.name = '{1}'
                ", t.Schema.Name, t.Name);
        	
        	var cmd = new DBCommand{ Sql = sql, Owner = t, Description = "Get Table Primary Key Constraint" };

        	t.PrimaryKey = DBProjectManager.ExecuteQuery(cmd).AsEnumerable().Select(x => new DBPKConstraint
            {
                Name = x["pk_name"].ToString(),
                Columns = new MyBindingList<DBColumn> { t.Columns.FirstOrDefault(c => c.Name == x["col_name"].ToString()) },
                Action = DB.DBAction.Alter,
                State = EngineManager.DBObject.DBObjectState.Intact
            }).FirstOrDefault();

            //set flag in pk columns
            if (t.PrimaryKey != null)
                foreach (var c in t.Columns)
                    if (t.PrimaryKey.Columns.Contains(c))
                    {
                        c.State = EngineManager.DBObject.DBObjectState.None;
                        c.IsPrimaryKey = true;
                        c.State = EngineManager.DBObject.DBObjectState.Intact;
                    }
        }

        public static DBView CreateSecureView(DBTable t)
        {
            var sql = string.Format("exec engine.secure_CreateSecure '{0}', '{1}'", t.Schema.Name, t.Name);
            t.Connection.GetDataTable(sql);
            DBTableManager.Refresh(t); //refresh extensions -> secure view only!
            DBViewManager.Refresh(t.SecureView);
            return t.SecureView;
        }

        public static DBView CreateView(DBTable t, EngineManager.DBTable.ViewTypes type)
        {
            var view = t.Extensions.FirstOrDefault(x => x.Schema.Name == type.ToString().ToLower());
            if (view == null)
            {
                view = new DBView
                {
					Schema = t.Schema.Database.Schemas.FirstOrDefault(x => x.Name == type.ToString().ToLower()),
                    Name = t.Schema.Name + "_" + t.Name,
                    DefinitionSQL = string.Format("create view [{0}].[{1}_{2}]\r\nas\r\nselect t.id from {3} t", type.ToString().ToLower(), t.Schema.Name, t.Name, t.FullName),
                    Action = DB.DBAction.Create,
                    Owner = t,
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Connection = t.Connection,
                    State = DBObject.DBObjectState.New
                };

                t.Extensions.Add(view);

                if (type == EngineManager.DBTable.ViewTypes.Computed)
                    t.ComputedView = view;
                else if (type == EngineManager.DBTable.ViewTypes.Views)
                    t.ExtendedView = view;

            }
            else
            {
                DBViewManager.Refresh(view);
                //MessageBox.Show("there is an existing \""  + type.ToString().ToLower() + "\" policy for this table"); 
            }

            return view;
        }

        public static DBView CreatePolicy(DBTable t, EngineManager.DBTable.PolicyTypes type)
        {
            var policy_view = t.Policies.FirstOrDefault(x => x.Schema.Name == type.ToString().ToLower());
            if (policy_view == null)
            {
                policy_view = new DBView
                {
					Schema = t.Schema.Database.Schemas.FirstOrDefault(x => x.Name == type.ToString().ToLower()),
                    Name = t.Schema.Name + "_" + t.Name,
                    DefinitionSQL = string.Format("create view [{0}].[{1}_{2}]\r\nas\r\nselect t.id from {3} t", type.ToString().ToLower(), t.Schema.Name, t.Name, t.FullName),
                    Action = DB.DBAction.Create,
                    Owner = t,
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Connection = t.Connection,
                    State = DBObject.DBObjectState.New
                };

                t.Policies.Add(policy_view);

            }
            else
            {
                DBViewManager.Refresh(policy_view);
                //MessageBox.Show("there is an existing \""  + type.ToString().ToLower() + "\" policy for this table"); 
            }

            return policy_view;
        }

        public static DBStoredProcedure CreateBusinessProcedure(DBTable t, EngineManager.DBTable.ProcedureTypes type)
        {
            var proc = new DBStoredProcedure();

            if (type == EngineManager.DBTable.ProcedureTypes.Business)
            {
                proc = new DBStoredProcedure
                {
					Schema = t.Schema.Database.Schemas.FirstOrDefault(x => x.Name == type.ToString().ToLower()),
                    Name = t.Schema.Name + "_" + t.Name,
                    DefinitionSQL = string.Format(@"
                        CREATE Procedure [{0}].[{1}_{2}__modify]
                        (
	                        @user NVARCHAR(50),
	                        @operation NVARCHAR(10),
	                        @inserted_keys dbo.list readonly,
	                        @deleted_keys dbo.list readonly,
	                        @deleted_data xml = NULL
                        )
                        AS
                        BEGIN
                            print 'business proc not implemented yet'
                        END

                        ",
                    type.ToString().ToLower(),
                    t.Schema.Name,
                    t.Name),
                    Action = DB.DBAction.Create,
                    Parent = t,
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Connection = t.Connection,
                    State = DBObject.DBObjectState.New
                };
            }

            return proc;
        }


        public static void DisableTriggers(DBTable t)
        {
            var sql = string.Format("disable trigger all on {0}", t.FullName);
            t.Connection.GetDataTable(sql);
        }

        public static void EnableTriggers(DBTable t)
        {
            var sql = string.Format("enable trigger all on {0}", t.FullName);
            t.Connection.GetDataTable(sql);
        }

        public static void AddIconClassColumn(DBTable t)
        {
            if (t.Columns.Count(c => c.UserDataType == "iconclass") == 0)
            {
                var c = new DBColumn
                {
                    Parent = t,
                    Name = "Icon",
                    DataType = DB.DBDataType.nvarchar,
                    Length = 50,
                    UserDataType = "iconclass",
                    CustomDataType = DBColumn.CustomDataTypes.iconclass,
                    Nullable = true,
                    Action = DB.DBAction.Add,
                    OnAlreadyExists = EngineManager.DBObject.DBOnExists.AlterElsePerformAction
                };

                var sql = c.GetSQL();

                var db_cmd = new DBCommand { Owner = t, Sql = sql, Description = "Add Icon Class Column" };
                t.Connection.Project.Commands.Add(db_cmd);
                //db_cmd.Execute();

                t.RowIconClass = c;

                //this.Refresh();
            }
        }

        public static void AddColorColumn(DBTable t)
        {
            if (t.Columns.Count(c => c.UserDataType == "color") == 0)
            {
                var c = new DBColumn
                {
                    Parent = t,
                    Name = "color",
                    DataType = DB.DBDataType.nvarchar,
                    Length = 50,
                    UserDataType = "color",
                    CustomDataType = DBColumn.CustomDataTypes.color,
                    Nullable = true,
                    Action = DB.DBAction.Add,
                    OnAlreadyExists = EngineManager.DBObject.DBOnExists.AlterElsePerformAction
                };

                var sql = c.GetSQL();

                var db_cmd = new DBCommand { Owner = t, Sql = sql, Description = "Add Color Column" };
                t.Connection.Project.Commands.Add(db_cmd);
                //db_cmd.Execute();
                t.RowColor = c;

                //this.Refresh();
            }
        }

        public static void AddDisplayOrderColumn(DBTable t)
        {
            if (t.Columns.Count(c => c.UserDataType == "displayorder") == 0)
            {
                var c = new DBColumn
                {
                    Parent = t,
                    Name = "Display_Order",
                    DataType = DB.DBDataType.@int,
                    Length = 4,
                    UserDataType = "displayorder",
                    CustomDataType = DBColumn.CustomDataTypes.displayorder,
                    Nullable = true,
                    Action = DB.DBAction.Add,
                    OnAlreadyExists = EngineManager.DBObject.DBOnExists.AlterElsePerformAction
                };

                var sql = c.GetSQL();

                var db_cmd = new DBCommand { Owner = t, Sql = sql, Description = "Add Display Order Column" };
                t.Connection.Project.Commands.Add(db_cmd);
                //db_cmd.Execute();
                t.DefaultSort = c;
                //this.Refresh();
            }
        }

        public static void RefreshCache(DBTable t)
        {
            var sql = string.Format("exec engine.extensions_CreateExtension '{0}_{1}'", t.Schema.Name, t.Name);
            var db_cmd = new DBCommand { Owner = t, Sql = sql, Description = "Refresh Cache" };
            t.Connection.Project.Commands.Add(db_cmd);
            DBProjectManager.Execute(db_cmd);
            MessageBox.Show("Extension Created.");
        }

        public static void CreateGenericTrigger(DBTable t)
        {
            var sql = string.Format("exec engine.secure_CreateGenericTrigger '{0}', '{1}'", t.Schema.Name, t.Name);
            var db_cmd = new DBCommand { Owner = t, Sql = sql, Description = "Create Generic Trigger" };
            t.Connection.Project.Commands.Add(db_cmd);
            DBProjectManager.Execute(db_cmd);
            MessageBox.Show("Generic Trigger Created.");
        }
    }
}
