using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EngineManager
{
    public class DBObject : IGenerateTreeNode, IRefreshableObject, IGenerateGraph, INotifyPropertyChanged
    {
        private DB database;
        [Browsable(false)]
        [ReadOnly(true)]
        public DB Database {
            get
            {
                return this is DB ? this as DB : database;
            }
            set
            {
                database = value;
            }
        }

        public DBObject Parent {get;set;}
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            this.State = DBObjectState.Modified;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        [Browsable(false)]
        public string PathID { get; set; }

        private MyBindingList<DBObject> db_objects = new MyBindingList<DBObject>();
        [Browsable(false), ShowInTree(false)]
        public MyBindingList<DBObject> DBObjects { get { return this.db_objects; } set { this.db_objects = value; } }

        protected DB.DBObjectType type;
        [Browsable(false)]
        public DBObject Self { get { return this; } }

        public enum DBObjectState { None, Intact, New, Modified, Deleted} //intact means the object has been populated from db

        [Browsable(false), ReadOnly(true)]
        public DBObjectState State { get; set; }

		protected List<DB.DBObjectType> types_not_needing_fullname = new List<DB.DBObjectType> {
			DB.DBObjectType.None,
			DB.DBObjectType.Schema, 
			DB.DBObjectType.Column, 
			DB.DBObjectType.Constraint, 
			DB.DBObjectType.Role,
			DB.DBObjectType.User, 
			DB.DBObjectType.Folder, 
			DB.DBObjectType.Database, 
			DB.DBObjectType.Property,
			DB.DBObjectType.Permission
		};

        public DBObject()
        {
           DBObjects.AddingNew += SubCollections_AddingNew;
           
            //todo remove will genereate drop command..(if object exist in db)
        }



        public virtual void SubCollections_AddingNew(object sender, AddingNewEventArgs e)
        {
        	var o = (e.NewObject as DBObject);
            o.Parent = this; //own whatever added as sub object.
            o.Connection = Connection;
            o.Database = Database;
        }
        [Browsable(false), ShowInTree(false)]
        public MyBindingList<DBFolder> Folders
        {
            get
            {
                return new MyBindingList<DBFolder>(this.db_objects.Where(x => x.Type == DB.DBObjectType.Folder).Cast<DBFolder>().ToList());
            }
        }

        public enum DBOnExists { PerformActionWithNoExistenceCheck, DropBeforePerformingAction, AlterElsePerformAction, DoNothingElsePerformAction, DoNothing, PerformActionElseDoNothing }

        public virtual string Name { get; set; }

        [Browsable(false)]
        public DB.DBAction Action { get; set; }

        [Browsable(false)]
        public DBConnection Connection { get; set;}

        //
        //[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        public string DefinitionSQL { get; set; }

        [Browsable(false)]
        [ReadOnly(true)]
        public virtual string FullName
        {
            get
            {
            	return this.Name;
            }
        }
        [Browsable(false)]
        public virtual string DisplayName { get { return this.Name; } }

        [Browsable(false)]
        public DBOnExists OnAlreadyExists { get; set; }

        

        [Browsable(false)]
        [ReadOnly(true)]
        public DB.DBObjectType Type { get { return this.type; } }

        [Browsable(false)]
        public bool WrapInExec { get; set; }

        protected BindingList<DBExtendedProperty> properties = new BindingList<DBExtendedProperty>();
        
        [Browsable(true), ShowInTree(false)]
        public BindingList<DBExtendedProperty> Properties { get { return this.properties; } set { this.properties = value; } }
        

        

        public virtual string GetAdditionalSyntax()
        {
            return "";
        }

        public virtual string GetCommandDetailSyntax()
        {
            return "";
        }

        public virtual string GetCommandSyntax()
        {
            var syntax = "";
            if (this.Action != DB.DBAction.None)
                syntax = string.Format("{0} {1} {2}", this.Action.ToString().ToUpper(), this.Type.ToString().ToUpper(), this.FullName).Trim(); //CREATE TABLE [schema].[tablename]
            return syntax;
        }

        public string GetDefinitionSQL()
        {
            var sql = this.DefinitionSQL;

            if (this.Action == DB.DBAction.Alter)
            {
                var regEx = new Regex("create " + this.Type.ToString() + " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                this.DefinitionSQL = regEx.Replace(sql, "alter " + this.type.ToString() + " ");
            }

            //if (this.AutoNameSources != null)
            //{
            //    var replacements = GetStringFormatReplacements();
            //    if(replacements.Length > 0)
            //        sql = string.Format(this.DefinitionSQL, replacements);

            //    return sql;
            //}
            return this.DefinitionSQL;
        }

        public virtual string GetSQL(bool forceWrapInExec = false)
        {
            var syntax = "";
            var exist_syntax = GetCheckExistSyntax();//IF OBJECT_ID('[schema].[tablename]') IS NOT NULL            
            var command_syntax = GetCommandSyntax();//CREATE TABLE [schema].[tablename]
            var command_detail_syntax = GetCommandDetailSyntax().Replace("\r\n", "\r\n\t"); //columns etc.
            var additional_syntax = GetAdditionalSyntax();

            var wrapped_command = WrapCommand(command_syntax, command_detail_syntax, forceWrapInExec);

            if (this.OnAlreadyExists == DBOnExists.PerformActionElseDoNothing)
            {
                if (exist_syntax == "")
                    syntax = string.Format("{0}\r\n", wrapped_command);
                else
                    syntax = string.Format("{0}BEGIN\r\n\t{1}\r\nEND\r\n", exist_syntax, wrapped_command);
            }
            else if (this.OnAlreadyExists == DBOnExists.AlterElsePerformAction) //if a table already exists, alter it, otherwise create it
            {
                var original_action = this.Action;
                this.Action = DB.DBAction.Alter;
                var alter_command_syntax = GetCommandSyntax();//ALTER TABLE
                var alter_command_detail_syntax = GetCommandDetailSyntax(); //columns etc.
                var wrapped_alter_command = WrapCommand(alter_command_syntax, alter_command_detail_syntax, forceWrapInExec);

                this.Action = original_action;

                syntax = string.Format("{0}BEGIN\r\n\t{1}\r\nEND\r\nELSE\r\nBEGIN\r\n\t{2}\r\nEND\r\n", exist_syntax, wrapped_alter_command, wrapped_command);
            }
            else if (this.OnAlreadyExists == DBOnExists.DropBeforePerformingAction) //if a trigger/function exists, drop it then create it always
            {
                var original_action = this.Action;
                this.Action = DB.DBAction.Drop;
                var drop_command_syntax = GetCommandSyntax();//DROP TABLE
                var drop_command_detail_syntax = GetCommandDetailSyntax(); //columns etc.
                var wrapped_drop_command = WrapCommand(drop_command_syntax, drop_command_detail_syntax, forceWrapInExec);

                this.Action = original_action;

                

                if(Action != DB.DBAction.Drop)
                {
                    syntax = string.Format("{0}BEGIN\r\n\t{1}\r\nEND\r\nBEGIN\r\n\t{2}\r\nEND\r\n", exist_syntax, wrapped_drop_command, wrapped_command);
                }
                else //no need to drop twice
                {
                    syntax = string.Format("{0}BEGIN\r\n\t{1}\r\nEND", exist_syntax, wrapped_drop_command);
                }
            }
            else if (this.OnAlreadyExists == DBOnExists.DoNothingElsePerformAction)
            {
                //exist syntax will be [if not exists then] ..
                syntax = string.Format("{0}BEGIN\r\n\t{1}\r\nEND\r\n", exist_syntax, wrapped_command);
            }
            else if (OnAlreadyExists == DBOnExists.PerformActionWithNoExistenceCheck)
            {
                syntax = wrapped_command;
            }

            //   if(!WrapInExec)
            //       syntax += "\r\nGO";
            syntax += "\r\n" + additional_syntax;

            return syntax;
        }

        public virtual string GetSQL()
        {
        	return GetSQL(false);
        }
        
        public virtual TreeNode GetTree()
        {
            var node = new TreeNode(this is DBView ? this.DisplayName : this.Name) { Tag = this, ToolTipText = this is DBTable ? (this as DBTable).Title : this.Name };
            var collections = this.GetCollections(); //get sub collections of this object
            foreach (var collection in collections)
            {
                if (collection.Value == null)
                    continue;

                var collection_node = new TreeNode(collection.Key) { Name = collection.Key }; //e.g. List<DBColumn>
                node.Nodes.Add(collection_node);
                foreach (var obj in collection.Value)
                {
                    obj.Parent = this;
                    collection_node.Nodes.Add(obj.GetTree()); //e.g. columns
                }
            }
            return node;
        }

        public virtual void DrawGraph(Graph g, Node pn)
        {
            if (string.IsNullOrEmpty(this.DisplayName))
                return;
            Node node = g.AddNode(this.DisplayName);
            node.UserData = this;
            //return;
            /****/

            if (pn != null)
                g.AddEdge(pn.Id, node.Id);
            

            var collections = this.GetCollections(); //get sub collections of this object
            foreach (var collection in collections)
            {
                if (collection.Value == null)
                    continue;

                var collection_node = g.AddNode(collection.Key);
                
                g.AddEdge(node.Id, collection.Key);

                foreach (var obj in collection.Value)
                {
                  //  obj.DrawGraph(g, collection_node);
                    var sub_node = g.AddNode(obj.DisplayName);
                    sub_node.UserData = obj;
                    g.AddEdge(collection_node.Id, obj.DisplayName);
                }
            }
        }
        public virtual void DrawRelationalGraph(Graph g, Node pn)
        {
            
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        protected virtual string GetCheckExistSyntax()
        {
            var syntax = "";
            if (this.Action != DB.DBAction.None)
            {
                var check_func = this.Type == DB.DBObjectType.Schema ? "SCHEMA_ID" : "OBJECT_ID";
                syntax = string.Format("IF {0}('{1}') IS {2} NULL\r\n", check_func, this.Type == DB.DBObjectType.Schema ? this.Name : this.FullName, this.OnAlreadyExists == DBOnExists.DoNothingElsePerformAction ? "" : "NOT");//IF OBJECT_ID('[schema].[tablename]') IS NOT NULL (if exists)
            }
            return syntax;
        }
        protected List<DBObject> GetCollection(PropertyInfo info, object obj)
        {
            if (obj == null)
                return new List<DBObject>();

            var result = new List<DBObject>((obj as IEnumerable<object>).Cast<DBObject>().ToList());
            return result;
        }

        public Dictionary<string, List<DBObject>> GetCollections()
        {
            var result = new Dictionary<string, List<DBObject>>();
            var infos = GetPropertiesOfTypeList();
            foreach (var info in infos)
            {
                var display_name = info.Key.GetCustomAttribute(typeof(DisplayNameAttribute));
                var name = info.Key.Name;
                if (display_name != null)
                {
                    name = (display_name as DisplayNameAttribute).DisplayName; //custom name from attribute
                }
                result.Add(name, GetCollection(info.Key, info.Value));
            }
            return result;
        }

        protected Dictionary<PropertyInfo, DBObject> GetPropertiesInheritingDBObject()
        {
            var result = new Dictionary<PropertyInfo, DBObject>();
            var type = this.GetType();
            var infos = type.GetProperties();
            foreach (var info in infos)
            {
                //check the type
                if (typeof(DBObject).IsAssignableFrom(info.PropertyType)) //if inherits DBObject
                {
                    var obj = info.GetValue(this);
                    result.Add(info, obj as DBObject);
                }
            }
            return result;
        }

        public Dictionary<PropertyInfo, object> GetPropertiesOfTypeList()
        {
            var result = new Dictionary<PropertyInfo, object>();
            var type = this.GetType();
            var infos = type.GetProperties();
            foreach (var info in infos)
            {
                //skip collections with showintree = false attribute
                var show_in_tree = info.GetCustomAttribute(typeof(ShowInTree));
                if (show_in_tree != null)
                {
                    if ((show_in_tree as ShowInTree).Show == false)
                        continue; //skip as explicity we want it to be hidden
                }

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
                        var obj = info.GetValue(this);
                        result.Add(info, obj);
                    }
                }
            }
            return result;
        }

        public delegate void subcollections_handler(object sender, AddingNewEventArgs e);
        
        protected void PrepareSubCollections()
        {
        	var infos = GetPropertiesOfTypeList();
        	foreach(var i in infos)
        	{
        		var t = i.Key.PropertyType.GetGenericTypeDefinition();
        		if(t.Name.Contains("BindingList"))
        		{
        			Type st = i.Key.PropertyType.GetGenericArguments()[0];
        			if(st.IsSubclassOf(typeof(DBObject)))
        			{       				
        				var list = i.Value as BindingList<DBDatabaseTrigger>;
        				if(list != null)
        					list.AddingNew += SubCollections_AddingNew;
        			}
        		}
        			
        	}
        }
        
        protected string WrapCommand(string command, string detail, bool forceWrapInExec = false)
        {
            if (!string.IsNullOrEmpty(detail))
                detail = "\r\n\t" + detail;
            return WrapInExec || forceWrapInExec ? string.Format("EXEC('{0}{1}')", command, detail.Replace("'", "''")) : string.Format("{0}{1}", command, detail);
        }

    }
}
