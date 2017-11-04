using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace EngineManager
{
    public class DB : DBObject, IGenerateTreeNode
    {
        public DB() : base()
        {
            this.Schemas = new MyBindingList<DBSchema>(this);           
            this.Roles = new MyBindingList<DBRole>(this);           
            this.Users = new MyBindingList<DBUser>(this);           
            this.DatabaseTriggers = new MyBindingList<DBDatabaseTrigger>(this);
          
            this.DevelopmentServer = new MigrationServerDetail();
            this.TestServer = new MigrationServerDetail();
            this.ProductionServer = new MigrationServerDetail();
            //todo user datatypes..
            

            this.PreMigrationScripts = new List<DBMigrationScript>();
            this.PostMigrationScripts = new List<DBMigrationScript>();
            
            PrepareSubCollections();
        }

        public enum DBAction { None, Create, Add, Alter, Drop }

        public enum DBDataType { none, @int, bigint, tinyint, @long, @char, nchar, varchar, nvarchar, bit, date, datetime, @float, money, uniqueidentifier }

        public enum DBObjectType { None, Database, Folder, Schema, Table, Column, View, Function, Trigger, Constraint, Type, Procedure, Role, User, DatabaseTrigger, Property, Permission }

        public MyBindingList<DBDatabaseTrigger> DatabaseTriggers { get; set; }

        public MyBindingList<DBRole> Roles { get; set;}

        public MyBindingList<DBUser> Users { get; set;}

        public MyBindingList<DBSchema> Schemas { get; set;}

        [Category("Migration")]
        public List<DBMigrationScript> PreMigrationScripts { get; set; }
        [Category("Migration")]
        public List<DBMigrationScript> PostMigrationScripts { get; set; }

        [Category("Servers"), TypeConverter(typeof(ExpandableObjectConverter))]

        public MigrationServerDetail ProductionServer { get; set; }
        [Category("Servers"), TypeConverter(typeof(ExpandableObjectConverter))]
        public MigrationServerDetail TestServer { get; set; }

        

        public static DB.DBDataType GetDataTypeFromString(object type)
        {
            string t = type.ToString().ToLower();

            foreach (var dt in Enum.GetNames(typeof(DB.DBDataType)))
            {
                if (t == dt)
                {
                    return (DB.DBDataType)Enum.Parse(typeof(DB.DBDataType), t);
                }
            }
            return DB.DBDataType.none;
        }

        public static string GetDataTypeSyntax(DBDataType datatype, string userdatatype = null)
        {
            //todo
            //return datatype.ToString().ToUpper();

            if (userdatatype == "none" || string.IsNullOrEmpty(userdatatype) || userdatatype.Equals(datatype.ToString(), StringComparison.OrdinalIgnoreCase)) //no user type specified
                return datatype.ToString().ToUpper();
            else
                return "dbo." + userdatatype;
        }

        public static string GetLengthSyntax(DBDataType datatype, string userdatatype, int length)
        {
            var str_length = "";

            if (userdatatype == "none" || (userdatatype == null) ||(userdatatype != "none" && userdatatype.ToLower() == datatype.ToString().ToLower()))
            {
                switch (datatype)
                {
                    case DBDataType.@char:
                    case DBDataType.nchar:
                    case DBDataType.varchar:
                    case DBDataType.nvarchar:
                        str_length = string.Format("({0})", length == 0 ? "MAX" : length.ToString());
                        break;
                }
            }

            return str_length;
        }

        public override TreeNode GetTree()
        {
            var root = new TreeNode(this.Name) { Tag = this };

            //existing schemas
            var schemasNode = new TreeNode("Schemas");
            root.Nodes.Add(schemasNode);
            schemasNode.Expand();

            //engine schemas
            var engine_schemasNode = new TreeNode("Engine Schemas");
            root.Nodes.Add(engine_schemasNode);

            foreach (var s in this.Schemas)
            {
                if(s.SchemaType == DBSchema.DBSchemaTypes.User)
                    schemasNode.Nodes.Add(new TreeNode(s.Name) { Tag = s });
                else
                    engine_schemasNode.Nodes.Add(new TreeNode(s.Name) { Tag = s });
            }

            //roles
            var roles_node = new TreeNode("Roles");
            root.Nodes.Add(roles_node);
            foreach (var r in this.Roles)
            {
                roles_node.Nodes.Add(new TreeNode(r.Name) { Tag = r });
            }

            //users
            var users_node = new TreeNode("Users");
            root.Nodes.Add(users_node);
            foreach (var u in this.Users)
            {
                users_node.Nodes.Add(new TreeNode(u.Name) { Tag = u });
            }

            //existing triggers
            var triggersNode = new TreeNode("Database Triggers");
            root.Nodes.Add(triggersNode);
            foreach (var t in this.DatabaseTriggers)
            {
                triggersNode.Nodes.Add(new TreeNode(t.Name) { Tag = t });
            }

            return root;
        }

        [Category("Servers"), TypeConverter(typeof(ExpandableObjectConverter))]
        public MigrationServerDetail DevelopmentServer { get; set; }
    }
}
