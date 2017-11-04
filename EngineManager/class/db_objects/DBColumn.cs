using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace EngineManager
{
    public class DBColumn : DBSchemaObject
    {
        
        private string title;

        public string OwnerName { get { return this.Parent != null ? this.Parent.DisplayName : "-"; } }

        public DBColumn() : base()
        {
            this.type = DB.DBObjectType.Column;
            IsComputed = false;
            this.Nullable = true;
            this.SequenceIdentifiers = new List<string>();
        }

        public enum CustomDataTypes { none, action, mobile, pagefilter, phone, email, iconclass, labeltype, color, link, dynamic_xml, title, name, description, xml_to_table, address, rating, percentage, docfile, imagefile, subforms, displayorder }

        public enum LookupControlTypes { ListBox, ComboBox, TextBox, TreeComboBox, OpenTree, AutoSuggestBox }

        private bool is_primary_key = false;

        public bool IsPrimaryKey
        {
            get { return this.is_primary_key; }
            set
            {
                if(this.State != DBObjectState.None && this.Parent is DBTable)
                {
                    var pk = (this.Parent as DBTable).PrimaryKey;
                    if (value && pk == null) // new
                    {
                        pk = new DBPKConstraint() { State = DBObjectState.New, Action = DB.DBAction.Add };
                        (this.Parent as DBTable).PrimaryKey = pk;
                    }
                    
                    if (value && !pk.Columns.Contains(this))
                        pk.Columns.Add(this);
                    else if(pk.Columns.Contains(this))
                        pk.Columns.Remove(this);
                }
                this.is_primary_key = value;
            }
        }

        private bool is_identity = false;

        public bool IsIdentity
        {
            get { return this.is_identity; }
            set { this.is_identity = value;
                if (value)
                {
                    this.Identity = new DBColumnIdentity { Increment = 1, Seed = 1 };
                }
                else
                    this.Identity = null;
            }
        }


        [Category("Appearance"), Description("Column Title")]
        public string Title
        {
            get { return title; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "title", this.title, value);
                }

                title = value;
            }
        }


        private string value_unit;
        [Category("Appearance"), Description("Value Unit")]
        public string ValueUnit
        {
            get { return value_unit; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "value_unit", this.value_unit.ToString().ToLower(), value.ToString().ToLower());
                }

                value_unit = value;
            }
        }


        private DB.DBDataType datatype;
        public DB.DBDataType DataType 
        {
            get
            {
                return this.datatype;
            }
            set
            {
                if (this.State != DBObjectState.None && this.datatype != value)
                {
                    this.datatype = value;
                    var sql = this.GetSQL();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Description = string.Format("set column datatype to {0}", value),
                        Sql = sql
                    });
                }
                this.datatype = value;
            }
        
        }
        private int length;
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                if (this.State != DBObjectState.None && this.length != value)
                {
                    this.length = value;
                    var sql = this.GetSQL();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Description = string.Format("set column length to {0}", value),
                        Sql = sql
                    });
                }this.length = value;
            }
        }


        public enum DateModes { none, year, month, day, hour, minute, second}
        private DateModes datemode;
        public DateModes DateMode
        {
            get
            {
                return this.datemode;
            }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "date_mode", this.datemode.ToString(), value.ToString());
                }

                this.datemode = value;
            }

        }



        private bool nullable;
        public bool Nullable 
        { 
            get
            {
                return this.nullable;
            }
            set
            {
                if (this.State != DBObjectState.None && this.nullable != value)
                {
                    this.nullable = value;
                    var sql = this.GetSQL();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Description = string.Format("set column nullable to {0}", value),
                        Sql = sql
                    });                   
                }this.nullable = value;                
            }
        }
        [Category("Appearance"), Description("Column Display Data Type")]

        private CustomDataTypes customdatatype;
        public CustomDataTypes CustomDataType 
        {
            get { return this.customdatatype; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "datatype", this.customdatatype.ToString(), value.ToString());
                }

                this.customdatatype = value;
            }
        }

        [Category("Appearance"), Description("Column Display Order")]
        public string DisplayOrder
        {
            get
            {
                return this.display_order;
            }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "displayorder", this.display_order, value);
                }

                this.display_order = value;
            }
        }
        private string display_order;

        private string description;

        [Category("Appearance"), Description("Column Description")]
        public string Description
        {
            get { return this.description; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", this.description, value);
                }

                this.description = value;
            }
        }
        [Editor(typeof(DBIconClassEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Appearance"), Description("Column Icon")]
        public DBIconClass IconClass 
        { 
            get { return this.icon_class; } 
            set {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "iconclass", this.icon_class.FinalClass, value.FinalClass);
                }

                this.icon_class = value;
            } 
        }
        private DBIconClass icon_class;


        [Category("Appearance"), Description("Column Color")]
        public System.Drawing.Color Color
        {
            get { return this.color; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "color", System.Drawing.ColorTranslator.ToHtml(color), System.Drawing.ColorTranslator.ToHtml(value));
                }

                this.color = value;
            }
        }
        private System.Drawing.Color color;


        
        public enum CodeLanguages { none, html, json, css, sql }
        
        [DataMember, Category("Code")]
        private CodeLanguages codelanguage;
        public CodeLanguages CodeLanguage
        {
            get { return this.codelanguage; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "code_language", this.codelanguage.ToString().ToLower(), value.ToString().ToLower());
                }

                this.codelanguage = value;
            } 
        }

        private string computeexpression;
        public string ComputeExpression 
        {
            get
            {
                return this.computeexpression;
            }
            set
            {
                if (this.State != DBObjectState.None && this.computeexpression != value)
                {
                    this.computeexpression = value;
                    var sql = this.GetSQL();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Description = string.Format("set column compute expression to {0}", value),
                        Sql = sql
                    });
                }
                
                this.computeexpression = value;
            }
        
        }

        private string @default;
        public string default_constraint_name;
        public string Default 
        {
            get
            {
                return this.@default;
            }
            set
            {
                if (this.State != DBObjectState.None && this.@default != value)
                {
                    this.@default = value;
                    var default_constraint = new DBDefaultConstraint 
                    {
                        Name = string.IsNullOrEmpty(this.default_constraint_name) ? 
                            string.Format("DF_{0}_{1}_{2}", this.Owner.Schema.Name, this.Parent.Name, this.Name) : this.default_constraint_name,
                        Expression = value,
                        Column = this,
                        Parent = this.Parent,
                        Action = DB.DBAction.Alter,
                        Connection = this.Connection,
                        OnAlreadyExists = DBOnExists.DropBeforePerformingAction
                    };
                    var sql = default_constraint.GetSQL();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Description = string.Format("set column default to {0}", value),
                        Sql = sql
                    });
                }
                
                this.@default = value;
            }
        }



        //[ReadOnly(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DBColumnIdentity Identity { get; set; }

        [Browsable(false), ReadOnly(true)]
        public bool IsComputed { get; set; }

        [Category("Validation"), Description("Specifies that the column does not accept update modifications")]
        public bool IsReadOnly { get; set; } //todo



        [DataMember, Category("Properties"), Description("Default Lookup Control Type")]
        public LookupControlTypes LookupControlType { get; set; }

        [DataMember, Category("Validation")]
        public int MaximumValue { get; set; } //todo

        [DataMember, Category("Validation")]
        public int MinimumValue { get; set; } //todo

        [DataMember, Category("Validation")]
        public int MinimumLength { get; set; } //todo

        private string procedure;

        [DataMember, Category("Action")] //exec oe.org_processes_set_reviewed [id]
        [Description("Example: exec oe.org_processes_set_reviewed [id]")]
        public string Procedure {
            get
            {
                return this.procedure;
            }
            set
            {
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "procedure", this.procedure, value);
                }
                this.procedure = value;
            }
        } //todo

        private string referenced_column;
        [DataMember, Category("Appearance"), Description("Specifies Lookup source for this column"), TypeConverter(typeof(StringListTypeConverter))]
        public string ReferencedColumn {
            get
            {
                return this.referenced_column;
            }
            set
            {
                if(this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "referenced_column", this.referenced_column, value);
                }
                this.referenced_column = value;
            }
        }
        
        [Category("Data"), Description("Use Sequence for the value of this column")]
        public bool UseSequence { get; set; }

        [Category("Data"), Description("Sequence Identifiers"), Editor(typeof(MultiSelectEditor), typeof(UITypeEditor))]
        public List<string> SequenceIdentifiers { get; set; } //todo

        
        [ReadOnly(true)]
        public string UserDataType { get; set; } //todo
        public override string GetCommandDetailSyntax()
        {
            var syntax = "";
            if (this.Action == DB.DBAction.Drop)
            {
                syntax = string.Format("[{0}]", this.Name);
            }
            else
            {
                syntax = string.Format("[{0}] {1} {2} {3} NULL {4}",
                    this.Name,
                    DB.GetDataTypeSyntax(this.DataType, this.CustomDataType.ToString()),
                    DB.GetLengthSyntax(this.DataType, this.UserDataType, this.Length),
                    this.Nullable ? "" : "NOT",
                    this.Action == DB.DBAction.Add ? string.IsNullOrEmpty(Default) == false ? string.Format("DEFAULT {0}", Default) : "" : "");

                if (this.Identity != null)
                    syntax = string.Format("{0} IDENTITY({1},{2})", syntax, this.Identity.Seed, this.Identity.Increment);
                else if (IsComputed || !string.IsNullOrEmpty(this.ComputeExpression))
                {
                    syntax = string.Format("{0} as {1}", this.Name, this.ComputeExpression);
                }

            }


            //todo col constraints

            return syntax;
        }

        public override string GetCommandSyntax()
        {
            return "";
        }

        public override string GetSQL()
        {
            var sql = "";
            var sql_col = this.GetCommandDetailSyntax();

            if (this.Parent!= null && this.Parent.Action == DB.DBAction.Create) //new table?
            {
                sql = sql_col;
            }
            else if(this.Action == DB.DBAction.Alter)
            {               
                sql = string.Format("ALTER TABLE {0} ALTER COLUMN {1}", this.Parent.FullName, sql_col);
            }
            else if(this.Action == DB.DBAction.Add)
            {
                sql = string.Format("ALTER TABLE {0} ADD {1}", this.Parent.FullName, sql_col);
            }
            else if (this.Action == DB.DBAction.Drop)
            {
                sql = string.Format("ALTER TABLE {0} DROP COLUMN {1}", this.Parent.FullName, this.FullName);
            }

            return sql;// column have no check for if exists for now //todo
        }

        public class DBColumnIdentity
        {

            public DBColumnIdentity()
            {
                this.Seed = 1;
                this.Increment = 1;
            }

            public int Increment { get; set; }
            public int Seed { get; set; }
        }

        internal void CreateFK(DBColumn ref_column)
        {
            if(this.Parent is DBTable)
            {
                var fk = new DBFKConstraint
                    {
                        Action = DB.DBAction.Add,
                        Column = this,
                        ReferenceColumn = ref_column,
                        Connection = this.Parent.Connection,
                        Schema = this.Owner.Schema,
                        Parent = this.Parent,
                        Name = string.Format("FK_{0}_{1}_{2}_ref_{3}_{4}_{5}", this.Owner.Name, this.Parent.Name, this.Name, ref_column.Schema.Name, ref_column.Parent.Name, ref_column.Name),
                        State = DBObject.DBObjectState.New
                    };

                var command = new DBCommand { Sql = fk.GetSQL(), Description = "Create FK", Owner = this };
                this.Connection.Project.Commands.Add(command);
            }
            
        }
    }
}
