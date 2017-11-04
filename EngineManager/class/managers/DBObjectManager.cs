using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EngineManager
{
	public static class DBObjectManager
    {
		public static void Refresh(DBObject o)
		{
			//redirect to proper manager
			if (o is DB)
				DBManager.Refresh(o as DB);
			else if(o is DBSchema)
				DBSchemaManager.Refresh(o as DBSchema);
			else if(o is DBTable)
				DBTableManager.Refresh(o as DBTable);
			else if(o is DBView)
				DBViewManager.Refresh(o as DBView);
			
		}

        public static DBCommand GetCommand(DBObject o, DB.DBAction a)
        {
            var previous_action = o.Action;
            o.Action = a;
            var cmd = new DBCommand { Owner = o, Sql = o.GetSQL(), Description = string.Format("{0} {1}", a, o) };
            o.Action = previous_action;
            return cmd;
        }
        public static void BeforeRefresh(DBObject o)
        {
            //reset state
            o.State = EngineManager.DBObject.DBObjectState.None;

            //clear all collections
            foreach (var c in o.GetCollections())
            {
                c.Value.Clear();
            }
        }

        public static void RefreshColumns(DBContainsColumns o)
        {
            if (o is IContainsColumns)
            {
                var oc = o as IContainsColumns;

                var connection = o.Connection;

                var sql = @"
SELECT       
	s.name as [schema_name], 
	t.name as [table_name], 
	c.name as [column_name], 
	st.name as [datatype], 
	ut.name as [usertype], 
	CASE 
		WHEN c.max_length = 1 THEN 1
		WHEN c.is_ansi_padded = 1 THEN c.max_length / 2 
		ELSE c.max_length 
	END as [length],
	c.is_computed, 
	c.is_nullable, 
	c.is_identity, 
	ISNULL(i.is_primary_key, 0) AS is_primary_key, 
	c.column_id as [column_order], 
	cc.definition [computed_value], 
	d.definition [default_value],
    d.name [default_constraint_name],
	title = cast(ep.value as nvarchar(max)),
	displayorder = isnull(cast(ep_o.value as nvarchar(max)), cast(c.column_id as nvarchar(max))),
    sortorder = cast(isnull(cast(ep_o.value as nvarchar(max)), cast(c.column_id as nvarchar(max))) as int),
    custom_datatype = case when isnull(cast(ep_d.value as nvarchar(max)),'none') = st.name then 'none' else isnull(cast(ep_d.value as nvarchar(max)),'none') end,
	iconclass = cast(ep_i.value as nvarchar(max)),
	referencedcolumn = cast(ep_r.value as nvarchar(max)),
	[procedure] = cast(ep_a.value as nvarchar(max)),
	[date_mode] = cast(ep_dt.value as nvarchar(max)),
	[value_unit] = cast(ep_vu.value as nvarchar(max)),
	[color] = cast(ep_c.value as nvarchar(max))

FROM            
	sys.indexes AS i INNER JOIN
	sys.index_columns AS ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id and i.is_primary_key = 1 RIGHT OUTER JOIN
	sys.columns AS c INNER JOIN
	sys.objects AS t ON c.object_id = t.object_id ON ic.column_id = c.column_id AND ic.object_id = t.object_id INNER JOIN 
	sys.systypes st ON c.system_type_id = st.xusertype INNER JOIN
	sys.systypes ut ON c.user_type_id = ut.xusertype INNER JOIN
	sys.schemas s ON s.schema_id = t.schema_id LEFT JOIN
	sys.computed_columns cc ON cc.object_id = t.object_id AND cc.column_id = c.column_id left join 
	sys.default_constraints d on c.column_id = d.parent_column_id and c.object_id = d.parent_object_id
	left join sys.extended_properties ep on ep.major_id = t.object_id and ep.minor_id = c.column_id and ep.name = 'title'
	left join sys.extended_properties ep_o on ep_o.major_id = t.object_id and ep_o.minor_id = c.column_id and ep_o.name = 'displayorder'
	left join sys.extended_properties ep_i on ep_i.major_id = t.object_id and ep_i.minor_id = c.column_id and ep_i.name = 'iconclass'
	left join sys.extended_properties ep_d on ep_d.major_id = t.object_id and ep_d.minor_id = c.column_id and ep_d.name = 'datatype'
	left join sys.extended_properties ep_r on ep_r.major_id = t.object_id and ep_r.minor_id = c.column_id and ep_r.name = 'referenced_column'
	left join sys.extended_properties ep_a on ep_a.major_id = t.object_id and ep_a.minor_id = c.column_id and ep_a.name = 'procedure'
	left join sys.extended_properties ep_dt on ep_dt.major_id = t.object_id and ep_dt.minor_id = c.column_id and ep_dt.name = 'date_mode'
	left join sys.extended_properties ep_vu on ep_vu.major_id = t.object_id and ep_vu.minor_id = c.column_id and ep_vu.name = 'value_unit'
	left join sys.extended_properties ep_c on ep_c.major_id = t.object_id and ep_c.minor_id = c.column_id and ep_c.name = 'color'


                                WHERE        t.name = '{0}' and s.name = '{1}'
    order by sortorder ";
                var db_cols = connection.GetDataTable(string.Format(sql, o.Name, o.Schema.Name)).AsEnumerable().Select(
                                x => new DBColumn
                                {
                                    Schema = o.Schema,
                                    Name = x["column_name"].ToString(),
                                    DataType = DB.GetDataTypeFromString(x["datatype"]),
                                    UserDataType = x["usertype"].ToString(), //tod add schema
                                    Identity = (bool)x["is_identity"] ? new DBColumn.DBColumnIdentity { } : null,
                                    Nullable = (bool)x["is_nullable"],
                                    Length = (int)x["length"],
                                    IsComputed = (bool)x["is_computed"],
                                    ComputeExpression = x["computed_value"].ToString(),
                                    Default = x["default_value"].ToString(),
                                    default_constraint_name = x["default_constraint_name"].ToString(),
                                    Title = x["title"].ToString(),
                                    DisplayOrder = x["displayorder"].ToString(),
                                    IconClass = new DBIconClass { Class = x["iconclass"].ToString(), Value = x["iconclass"].ToString() },
                                    ReferencedColumn = x["referencedcolumn"].ToString(),
                                    ValueUnit = x["value_unit"].ToString(),
                                    Color = System.Drawing.ColorTranslator.FromHtml(x["color"].ToString()),
                                    Procedure = x["procedure"].ToString(),
                                    CustomDataType = Enum.IsDefined(typeof(DBColumn.CustomDataTypes), x["custom_datatype"].ToString()) ? (DBColumn.CustomDataTypes)Enum.Parse(typeof(DBColumn.CustomDataTypes), x["custom_datatype"].ToString()) : DBColumn.CustomDataTypes.none,
                                    DateMode = Enum.IsDefined(typeof(DBColumn.DateModes), x["date_mode"].ToString()) ? (DBColumn.DateModes)Enum.Parse(typeof(DBColumn.DateModes), x["date_mode"].ToString()) : DBColumn.DateModes.none,
                                    Parent = o,
                                    Action = DB.DBAction.Alter,
                                    Connection = o.Connection,
                                    State = DBObject.DBObjectState.Intact
                                }
                            ).ToList();

                oc.Columns.Clear();
                foreach (var c in db_cols)
                    oc.Columns.Add(c);
            }
        }

        public static void RefreshProperties(DBObject o)
        {
        	var so = o as DBSchemaObject;
        	
            if (so != null)
            {
                //get tables
                var sql = "";
                if (o is DBColumn)
                {
                    sql = string.Format(@"select SchemaName = s.name, ObjectName = o.name, ObjectID = s.name + '.' + o.name, PropertyName = ep.name, PropertyValue = cast(ep.value as varchar(max))
                                        from sys.schemas s join sys.objects o on s.schema_id = o.schema_id 
                                        join sys.columns c on c.object_id = o.object_id
                                        join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = c.column_id
                                        where s.name = '{0}' and o.name = '{1}' and c.name = '{2}' order by o.name", so.Schema.Name, so.Owner.Name, so.Name);
                }
                else
                {
                    sql = string.Format(@"
                select SchemaName = s.name, ObjectName = o.name, ObjectID = s.name + '.' + o.name, PropertyName = ep.name, PropertyValue = cast(ep.value as varchar(max))
                from sys.schemas s join sys.objects o on s.schema_id = o.schema_id join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
                where s.name = '{0}' and o.name = '{1}' order by ep.name", so.Schema.Name, so.Name);
                }

                var properties = o.Connection.GetDataTable(sql).AsEnumerable().Select(x => new DBExtendedProperty()
                {
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Name = x["PropertyName"].ToString(),
					//Schema = so.Schema.Database.Schemas.FirstOrDefault(s => s.Name == x["SchemaName"].ToString()),
                    Value = x["PropertyValue"].ToString(),
                    Action = DB.DBAction.Create,
                    State = DBObject.DBObjectState.Intact,
                    Owner = so
                }).ToList();

                o.Properties.Clear();
                foreach (var p in properties)
                    o.Properties.Add(p);

            }
        }

        public static void AfterRefresh(DBObject o)
        {
            //set to intact
            o.State = EngineManager.DBObject.DBObjectState.Intact;
        }
    
		public static void CreatePermissionCommand(DBPermission permission)
        {		
            DBProjectManager.Project.Commands.Add(new DBCommand()
            {
                Owner = permission.Owner,
                Sql = permission.GetSQL(),
                Description = string.Format("Set Permissions on {0} to '{1}'", permission.Owner.FullName, permission.Grantee.FullName)
            });
            
        }
	
		public static DBCommand CreateExtendedProperty(DBObject o, string name, string old_value, string new_value)
        {
            var cmd = new DBCommand();

            if (old_value != new_value)
            {
                if(new_value == "none")
                    new_value = "";

                var property = new DBExtendedProperty() { Name = name, Action = DB.DBAction.Create, Value = new_value, Owner = o };

                cmd = new DBCommand()
                {
                    PropertyName = name,
                    Owner = o,
                    Sql = property.GetSQL(),
                    Description = string.Format("Change {0} from '{1}' to '{2}'", name, old_value, new_value)
                };

                var existing_cmd = o.Connection.Project.Commands.FirstOrDefault(c => c.Owner == o && c.PropertyName == cmd.PropertyName && c.State == DBCommand.CommandState.New);

                if (existing_cmd != null) //update existing non executed command.
                {
                    existing_cmd.Sql = cmd.Sql;
                    existing_cmd.Description = cmd.Description;
                }
                else //otherwise create a new one
                {
                    o.Connection.Project.Commands.Add(cmd);
                }
            }

           return cmd;
        }

	}
}
