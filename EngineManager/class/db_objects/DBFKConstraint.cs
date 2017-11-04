using System.ComponentModel;

namespace EngineManager
{
    public class DBFKConstraint : DBConstraint
    {
        //public override string Name
        //{
        //    get
        //    {               
        //        return this.Name;// != null ? this.Name : string.Format("FK_{0}_{1}_{2}_REF_{3}_{4}_{5}", SchemaName, TableName, ColumnName, ReferenceSchemaName, ReferenceTableName, ReferenceColumnName);
        //    }
        //}
        public DBFKConstraint()
            : base()
        {
            this.type = DB.DBObjectType.Constraint;
            this.constraint_type = DBConstraintTypes.ForeignKey;
            this.OnDelete = CascadeModes.DoNothing;
            this.OnUpdate = CascadeModes.DoNothing;
            this.OnAlreadyExists = DBOnExists.DropBeforePerformingAction;
        }

        public enum CascadeModes { Cascade, SetNull, SetDefault, DoNothing }

        public DBColumn Column { get; set; }

        private CascadeModes on_delete;
        public CascadeModes OnDelete
        {
            get
            {
                return on_delete;
            }
            set
            {
                on_delete = value;

                if (this.State != DBObjectState.None)
                {
                    var sql = this.GetSQL();
                    var cmd = new DBCommand
                    {
                        Sql = sql,
                        Owner = this,
                        Description = "Change OnDelete Action"
                    };
                    this.Connection.Project.Commands.Add(cmd);
                }
            }
        }

        private CascadeModes on_update;
        public CascadeModes OnUpdate 
        {
            get
            {
                return on_update;
            }
            set
            {
                on_update = value;

                if (this.State != DBObjectState.None)
                {
                    var sql = this.GetSQL();
                    var cmd = new DBCommand
                    {
                        Sql = sql,
                        Owner = this,
                        Description = "Change OnUpdate Action"
                    };
                    this.Connection.Project.Commands.Add(cmd);
                }
            }
        }

        [TypeConverter(typeof(StringListTypeConverter))]
        public DBColumn ReferenceColumn { get; set; }
        public string ReferenceColumnName { get; set; }
        public string ReferenceSchemaName { get; set; }
        public string ReferenceTableName { get; set; }
        
        private bool refreshparent;
        public bool RefreshParent 
        {
            get { return refreshparent; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    refreshparent = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                 refreshparent = value;
            }
        }


        private bool is_lookup;
        public bool IsLookup
        {
            get { return is_lookup; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "is_lookup", is_lookup.ToString(), value.ToString());
                }
                is_lookup = value;
            }
        }


        private string get_extended_property_value()
        {
            var v = (this.RespectCreate ? "C" : "") 
                    + (this.RespectRead ? "R" : "") 
                    + (this.RespectUpdate ? "U" : "") 
                    + (this.RespectDelete? "D" : "") 
                    + (this.RefreshParent ? "S" : "") 
                    + (this.CascadeLookup ? "L" : "");

            return v;
        }

        private bool respectcreate;
        public bool RespectCreate {
            get { return respectcreate; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    respectcreate = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                respectcreate = value;
            }
        }

        private bool respectdelete;
        public bool RespectDelete { 
                    get { return respectdelete; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    respectdelete = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                 respectdelete = value;
            }
        }

        private bool respectread;
        public bool RespectRead {
            get { return respectread; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    respectread = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                respectread = value;
            }
        }

        private bool respectupdate;
        public bool RespectUpdate {
            get { return respectupdate; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    respectupdate = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                respectupdate = value;
            }
        }

        private string title;
        [Description("table title to show when used as sub form")]
        public string Title
        {
            get { return this.title; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    this.title = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "title", old_val, new_val);
                }
                this.title = value;
            }
        }

        private bool cascadelookup;
        [Description("Show cascading parent lookup when this table is used for lookup. e.g. Category->Item selection")]
        public bool CascadeLookup {
            get { return cascadelookup; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    var old_val = get_extended_property_value();
                    cascadelookup = value;
                    var new_val = get_extended_property_value();
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", old_val, new_val);
                }
                cascadelookup = value;
            }
        }
        [ReadOnly(true)]
        public string RelationType { get; internal set; }

        [ReadOnly(true)]

        public string get_statement()
        {
            var cmd = "";
            if (Action != DB.DBAction.Drop)
            {
                Action = DB.DBAction.Drop;
                var drop_cmd = GetCheckExistSyntax() + GetCommandSyntax();

                Action = DB.DBAction.Add;
                var add_cmd = GetCommandSyntax() + GetCommandDetailSyntax();

                cmd = drop_cmd + "\r\n" + add_cmd;
            }
            else
            {
                var drop_cmd = GetCheckExistSyntax() + GetCommandSyntax();
                cmd = drop_cmd;
            }

            return cmd;    
        }
        public override string GetCommandDetailSyntax()
        {
            var cascade = "";
            if (OnDelete == CascadeModes.Cascade)
                cascade = "ON DELETE CASCADE";

            if (OnUpdate == CascadeModes.Cascade)
                cascade += " ON UPDATE CASCADE";

            if (Action == DB.DBAction.Create || Action == DB.DBAction.Alter)
                Action = DB.DBAction.Add;

            if (Action == DB.DBAction.Add)
                return string.Format("\r\nFOREIGN KEY ({0})\r\nREFERENCES [{1}].[{2}]({3}) {4}", Column.FullName,
                    ReferenceColumn != null ? ReferenceColumn.Owner.Schema.Name : ReferenceSchemaName, 
                    ReferenceColumn != null ? ReferenceColumn.Owner.Name : ReferenceTableName, 
                    ReferenceColumn != null ? ReferenceColumn.Name : ReferenceColumnName, 
                    cascade);
            else
                return "";
        }

        public override string GetCommandSyntax()
        {
            if (Action == DB.DBAction.Create || Action == DB.DBAction.Alter)
                Action = DB.DBAction.Add;

            var final_syntax = string.Format("ALTER TABLE {0}\r\n\t{1} CONSTRAINT [{2}]", this.Parent.FullName, Action.ToString().ToUpper(), Name);

            return final_syntax;
        }

        protected override string GetCheckExistSyntax()
        {
            var syntax = "";
            if (this.Action != DB.DBAction.None)
                syntax = string.Format("IF {0} EXISTS(select name from sys.foreign_keys where name = '{1}')\r\n", OnAlreadyExists == DBOnExists.DoNothingElsePerformAction ? "NOT" : "", Name);
           
            return syntax;
        }
    }
}
