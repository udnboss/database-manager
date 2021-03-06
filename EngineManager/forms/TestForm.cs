﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EngineManager
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            
        }
        public static char HexToChar(string hex)
        {
            return (char)ushort.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            populate_tree();
            return;

            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(Application.StartupPath + @"\font\fontawesome-webfont.ttf");
            FontFamily fontFamily = new FontFamily("FontAwesome", collection);
            Font font = new Font(fontFamily, 18);

            comboBox1.Font = font;

            //comboBox1.Items.Add("&#xf042");

            string a = HexToChar("f2ba").ToString();

            textBox1.Font = font;
            textBox1.Text = a;

            textBox2.Text = a;


            var items = new List<DBIconClass>();

            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Class";

            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + @"\xml\icon_classes.xml");
            foreach(XmlNode node in doc.SelectNodes("//i"))
            {
                items.Add(new DBIconClass { Class = node.Attributes["class"].Value, Value = HexToChar(node.Attributes["value"].Value).ToString() });
            }

            comboBox1.Items.AddRange(items.ToArray());
            comboBox1.ItemHeight = 18;
        }
        private Dictionary<string, DBObject> db_objects = new Dictionary<string,DBObject>();
        private DBProject project = new DBProject();
        private void populate_tree()
        {
            #region sql
            var sql = @"
declare @schema_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @schema_folders
	select distinct 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_name] = o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

declare @object_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_folders
	select distinct --constraints etc
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_name] = o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.objects po on o.parent_object_id = po.object_id
	where
		s.schema_id between 5 and 16383
		and o.parent_object_id <> 0

-------------------------------------------------------------------------------------------------------------------------------

declare @object_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_property_folders
	select distinct 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
	where
		s.schema_id between 5 and 16383
-------------------------------------------------------------------------------------------------------------------------------

declare @sub_object_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @sub_object_property_folders
	select distinct --constraints etc
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name  + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.objects po on o.parent_object_id = po.object_id
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.class = 1
	where
		s.schema_id between 5 and 16383
		and o.parent_object_id <> 0

-------------------------------------------------------------------------------------------------------------------------------

declare @object_column_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_column_folders
	select distinct --columns
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN'
		, [object_name] = 'COLUMN'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.columns c on c.object_id = o.object_id
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

declare @object_column_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_column_property_folders
	select distinct --columns
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/' + c.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/' + c.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.columns c on c.object_id = o.object_id
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = c.column_id
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

select 
	[id] = cast([id] as nvarchar(200))
	, [object_name] = cast([object_name] as nvarchar(200))
	, [object_type] = cast([object_type] as nvarchar(200))
	, [parent_id] = cast([parent_id] as nvarchar(200))
	, [definition] = cast([definition] as nvarchar(max))
from
(
select	--database
	[id] = db_name()
	, [object_name] = db_name()
	, [object_type] = 'DATABASE' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = null
	, [definition] = null

union all

select --database folders
	[id] = db_name() + '/SCHEMA'
	, [object_name] = 'SCHEMA'
	, [object_type] = 'FOLDER' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name()
	, [definition] = null

union all

select --schemas
	[id] = db_name() + '/SCHEMA/' + s.name
	, [object_name] = s.name
	, [object_type] = 'SCHEMA' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA' collate SQL_Latin1_General_CP1_CI_AS	
	, [definition] = null
from 
	sys.schemas s
where
	s.schema_id between 5 and 16383

union all

	select --schema folders
		*, [definition] = null 
	from @schema_folders

union all

select --tables etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [object_name] = o.name
	, [object_type] = o.type_desc
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
	, [definition] = OBJECT_DEFINITION(o.object_id)
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
where
	s.schema_id between 5 and 16383
	and o.parent_object_id = 0

union all

	select 
		*, [definition] = null 
	from @object_folders

union all

	select 
		*, [definition] = null 
	from @object_column_folders

union all

	select 
		*, [definition] = null 
	from @object_property_folders

union all

	select --table properties etc 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY/' + ep.name
		, [object_name] = ep.name
		, [object_type] = 'PROPERTY'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [definition] = cast(ep.value as nvarchar(max))
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
	where
		s.schema_id between 5 and 16383

union all

select --constraints etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [object_name] = o.name
	, [object_type] = o.type_desc
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
	, [definition] = OBJECT_DEFINITION(o.object_id)
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.objects po on o.parent_object_id = po.object_id
where
	s.schema_id between 5 and 16383
	and o.parent_object_id <> 0

union all

select --properties of constraints etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY/' + ep.name
	, [object_name] = ep.name
	, [object_type] = 'PROPERTY'
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
	, [definition] = cast(ep.value as nvarchar(max))
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.objects po on o.parent_object_id = po.object_id
	join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
where
	s.schema_id between 5 and 16383
	and o.parent_object_id <> 0

union all

	select 
		*, [definition] = null 
	from @sub_object_property_folders

union all

select --columns
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/'  + c.name
	, [object_name] = c.name
	, [object_type] = 'COLUMN' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name  + '/COLUMN'
	, [definition] = null
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.columns c on c.object_id = o.object_id
where
	s.schema_id between 5 and 16383

union all

	select --property folders
		*, [definition] = null
	from @object_column_property_folders

union all

select --columns properties
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/'  + c.name + '/PROPERTY/' + ep.name
	, [object_name] = ep.name
	, [object_type] = 'PROPERTY' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name  + '/COLUMN/' + c.name + '/PROPERTY'
	, [definition] = cast(ep.value as nvarchar(max))
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.columns c on c.object_id = o.object_id
	join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = c.column_id
where
	s.schema_id between 5 and 16383

) t
order by id





                ";

            #endregion

            var conn = new DBConnection() { Server = @"pmvd\sql2012", InitialDatabase = "fe", User = "PTIU", Password = "Migrate.12" };
            this.project.Connections.Add(conn);


            var data = conn.GetDataTable(sql); //all objects

            var nodes = new List<TreeNode>();
            var root_node = new TreeNode();


            foreach (DataRow r in data.Rows)
            {
                var id = r["id"].ToString();
                var object_name = r["object_name"].ToString();
                var object_type = r["object_type"].ToString();
                var parent_id = r["parent_id"].ToString();
                var definition = r["definition"].ToString();

                DBObject db_obj = new DBObject();


                switch (r["object_type"].ToString())
                {
                    case "DATABASE": db_obj = new DB(); break;
                    case "FOLDER": db_obj = new DBFolder(); break;
                    case "SCHEMA": db_obj = new DBSchema(); break;
                    case "USER_TABLE": db_obj = new DBTable(); break;
                    case "VIEW": db_obj = new DBView(); break;
                    case "SQL_STORED_PROCEDURE": db_obj = new DBStoredProcedure(); break;
                    case "SQL_TRIGGER": db_obj = new DBTrigger(); break;
                    case "FOREIGN_KEY_CONSTRAINT": db_obj = new DBFKConstraint(); break;
                    case "PRIMARY_KEY_CONSTRAINT": db_obj = new DBPKConstraint(); break;
                    case "SQL_SCALAR_FUNCTION": db_obj = new DBFunction(); break;//
                    case "SQL_INLINE_TABLE_VALUED_FUNCTION": db_obj = new DBFunction(); break;//
                    case "SQL_TABLE_VALUED_FUNCTION": db_obj = new DBFunction(); break;//
                    case "CHECK_CONSTRAINT": db_obj = new DBCheckConstraint(); break;
                    case "UNIQUE_CONSTRAINT": db_obj = new DBUniqueConstraint(); break;
                    case "DEFAULT_CONSTRAINT": db_obj = new DBDefaultConstraint(); break;
                    case "COLUMN": db_obj = new DBColumn(); break;
                    case "PROPERTY": db_obj = new DBExtendedProperty(); break;
                    default: db_obj = null; break;
                }

                if (db_obj != null)
                {
                    db_obj.PathID = id;
                    db_obj.Name = object_name;
                    db_obj.Connection = conn;
                    db_obj.DefinitionSQL = definition;

                    db_objects.Add(id, db_obj); //flat list

                    var n = new TreeNode(db_obj.Name) { Tag = db_obj };
                    nodes.Add(n);

                    if (parent_id != "")
                    {
                        //update parent reference in this object
                        var parent_obj = db_objects[parent_id];
                        if (db_obj is DBFolder)
                            db_obj.Parent = parent_obj; //an object owns a folder
                        else
                            db_obj.Parent = parent_obj.Parent; //parent of the folder owns the object

                        //add this object as a child
                        if (parent_obj is DBFolder)
                            parent_obj.Parent.DBObjects.Add(db_obj);//add it as direct child of folder owner

                        parent_obj.DBObjects.Add(db_obj); //folder owns automatically

                        //set schema reference if applicable
                        if (parent_obj.Parent != null && parent_obj.Parent is DBSchema)
                            (db_obj as DBSchemaObject).Schema = parent_obj.Parent as DBSchema;

                        //tree
                        var parent_node = nodes.Where(x => (x.Tag as DBObject).PathID == parent_id).FirstOrDefault();
                        parent_node.Nodes.Add(n);
                    }
                    else
                    {
                        root_node = n;
                    }

                }
            }

            //db_objects list populated.

            var fks = this.get_foreign_keys(); //fks
            //wire the foreign keys to columns
            foreach (var fk in fks)
            {
                var o = (db_objects[fk.Key] as DBFKConstraint);
                var v = fk.Value;

                o.CascadeLookup = v.CascadeLookup;
                o.Column = v.Column;
                o.OnDelete = v.OnDelete;
                o.OnUpdate = v.OnUpdate;
                o.ReferenceColumn = v.ReferenceColumn;
                o.RefreshParent = v.RefreshParent;
                o.RespectCreate = v.RespectCreate;
                o.RespectDelete = v.RespectDelete;
                o.RespectRead = v.RespectRead;
                o.RespectUpdate = v.RespectUpdate;

                //update sub tables collection
                //var sub_relations = new DBFolder { Name = "SUB_RELATION" };
                //sub_relations.DBObjects.Add(o);
                if (!v.ReferenceColumn.Parent.DBObjects.Contains(o))
                    v.ReferenceColumn.Parent.DBObjects.Add(o);

            }

            //distribute db_objects into owner collections (properties of type list and same dbobject type)
            foreach (var ob in db_objects.Select(x => x.Value).ToList())
            {
                var type = ob.GetType();
                var infos = type.GetProperties();
                foreach (var info in infos)
                {
                    //check the type
                    Type proptype = info.PropertyType;
                    if (proptype.IsGenericType
                        && (proptype.GetGenericTypeDefinition() == typeof(List<>)
                        || proptype.GetGenericTypeDefinition() == typeof(BindingList<>)
                        || proptype.GetGenericTypeDefinition() == typeof(MyBindingList<>)))
                    {
                        Type itemType = proptype.GetGenericArguments()[0];
                        if (typeof(DBObject).IsAssignableFrom(itemType)) //if inherits DBObject
                        {
                            var obj = info.GetValue(ob);
                            var collection = new List<DBObject>();
                            collection.AddRange(ob.DBObjects.Where(o => o.GetType() == itemType).ToList());
                            var method = proptype.GetMethod("Add");
                            foreach(var i in collection)
                                if(obj != null)
                                    method.Invoke(obj, new object[] {i});                            
                        }
                    }
                }

            }


            treeView1.Nodes.Add(root_node);
        }
        
        private Dictionary<string, DBFKConstraint> get_foreign_keys()
        {
            //this.db_objects
            var sql = @"select
                parent_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/FOREIGN_KEY_CONSTRAINT',
                constraint_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/FOREIGN_KEY_CONSTRAINT/' + t1.fk_name,
                column_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/COLUMN/' + t1.fk_column,
                referenced_column_id = db_name() + '/SCHEMA/' + t1.pk_schema + '/USER_TABLE/' + t1.pk_table + '/COLUMN/' + t1.pk_column,
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
				 
		        ) as t1";

            var fks = this.project.Connections[0].GetDataTable(sql).AsEnumerable().Select(x => new DBFKConstraint()
                {
                    PathID = x["constraint_id"].ToString(),
                    OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck,
                    Name = x["ConstraintName"].ToString(),
                    ReferenceSchemaName = x["referencedSchema"].ToString(),
                    ReferenceTableName = x["referencedTable"].ToString(),
                    ReferenceColumnName = x["referencedColumn"].ToString(),
                    Column = db_objects[x["column_id"].ToString()] as DBColumn,
                    ReferenceColumn = db_objects[x["referenced_column_id"].ToString()] as DBColumn,
                    RespectRead = (bool)x["respect_read"],
                    RespectCreate = (bool)x["respect_create"],
                    RespectUpdate = (bool)x["respect_update"],
                    RespectDelete = (bool)x["respect_delete"],
                    CascadeLookup = (bool)x["cascade_lookup"],
                    Title = x["title"].ToString(),
                    OnDelete = x["delete_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                    x["delete_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                        x["delete_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,
                    OnUpdate = x["update_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                    x["update_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                        x["update_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,
                    Parent = db_objects[x["parent_id"].ToString()].Parent,//parent of the folder
                    Action = DB.DBAction.Alter,
                    Connection = db_objects[x["parent_id"].ToString()].Parent.Connection,
                }).ToList().ToDictionary(o=> o.PathID, o => o);

            return fks;

        }
        private DBObject selected_db_object;
        [Bindable(true)]
        public DBObject SelectedDBObject 
        {
            set
            {
                selected_db_object = value;
                propertyGrid1.SelectedObject = value;
            }
            get
            {
                return treeView1.SelectedNode != null ? treeView1.SelectedNode.Tag as DBObject : null;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SelectedDBObject = treeView1.SelectedNode.Tag as DBObject;
            if (treeView1.SelectedNode.Tag is DBTable)
            {
                var table = treeView1.SelectedNode.Tag as DBTable;
                var form = new ManageRelatedColumns { Table = table };
                form.ShowDialog();
            }

        }    
        
    }




    public class DBIconClass
    {
        public string Class { get; set; }
        public string Value { get; set; }

        public bool FixedWidth { get; set; }

        public string Rotate { get; set; }

        public string Color { get; set; }

        public override string ToString()
        {
            return Class;
        }

        public string FinalClass
        {
            get
            {
                return Class + (FixedWidth ? " fa-fw" : "") + (string.IsNullOrEmpty(Rotate) ? "" : " ") + Rotate + (string.IsNullOrEmpty(Color) ? "" : " ") + Color;
            }
        }
    }
}
