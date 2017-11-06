using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace EngineManager
{
    public class DBTable : DBContainsColumns, IContainsColumns
    {
        public DBTable() : base()
        {
            this.type = DB.DBObjectType.Table;

            this.Columns = new MyBindingList<DBColumn>(this);
            this.Triggers = new MyBindingList<DBTrigger>(this);
            this.Business = new MyBindingList<DBStoredProcedure>();  
            this.Policies = new MyBindingList<DBView>();           
            this.ForeignKeys = new MyBindingList<DBFKConstraint>(this);            
            this.Extensions = new MyBindingList<DBView>();
            this.RowActions = new MyBindingList<RowAction>(this);
            this.CheckConstraints = new MyBindingList<DBCheckConstraint>(this);           
            this.TriggerConstraints = new MyBindingList<TriggerConstraint>(this);            
            this.DefaultConstraints = new MyBindingList<DBDefaultConstraint>(this);

            this.AdditionalRelatedColumns = new MyBindingList<AdditionalColumn>(this);           
            this.AdditionalComputedColumns = new MyBindingList<AdditionalComputedColumn>(this);

            this.SecurityPolicyQuestions = new MyBindingList<SecurityPolicyQuestion>(this);

            this.Validators = new MyBindingList<Validator>(this);

            this.AvailablePolicies = new MyBindingList<SecurityPolicy>(this);

            this.UpdateCommands = new MyBindingList<SQLUpdateCommand>(this);
            this.InsertCommands = new MyBindingList<SQLInsertCommand>(this);
            this.DeleteCommands = new MyBindingList<SQLDeleteCommand>(this);

            this.BasicProcedures = new MyBindingList<BasicProcedure>(this);           
        }
     

        public Workflow Workflow { get; set; }

        public BindingList<SecurityPolicy> AvailablePolicies { get; set; }

        //related columns and security policies
        public MyBindingList<AdditionalColumn> AdditionalRelatedColumns { get; set; }
        public MyBindingList<AdditionalComputedColumn> AdditionalComputedColumns { get; set; }
        public MyBindingList<SecurityPolicyQuestion> SecurityPolicyQuestions { get; set; }

        [Category("Security")]
        [ShowInManage(true)]
        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy SelectPolicy { get; set; }
        [Category("Security")]
        [ShowInManage(true)]
        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy UpdatePolicy { get; set; }
        [Category("Security")]
        [ShowInManage(true)]
        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy DeletePolicy { get; set; }
        [Category("Validation")]
        [ShowInManage(true)]
        public MyBindingList<Validator> Validators { get; set; }

        public enum SortDirections { Ascending, Descending }

        private DBPKConstraint primary_key;
        [Browsable(false)]
        public DBPKConstraint PrimaryKey {
            get { return this.primary_key; }
            set
            {
                this.primary_key = value;
                if (value != null && this.primary_key.Name == null)
                {
                    this.primary_key.Name = "PK_" + this.Schema.Name + "_" + this.Name;
                    this.primary_key.Parent = this;
                    this.primary_key.Schema = this.Schema;
                    this.primary_key.Connection = this.Connection;
                }                   
            }
        }

        [Browsable(false)]
        public MyBindingList<DBStoredProcedure> Business { get; set; }

        
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(false)]
        public MyBindingList<DBColumn> Columns {
            get;
            set;          
        }

        [Browsable(true), ShowInTree(false)]
        public BindingList<DBColumn> CombinedColumns 
        {
            get 
            {
                
                var cols = new List<DBColumn>();

                this.Columns.ToList().ForEach(x => cols.Add(x)); //this table

                if (this.ExtendedView != null)
                    this.ExtendedView.Columns.Where(c => this.Columns.Where(tc => tc.Name == c.Name).Count() == 0).ToList().ForEach(x => cols.Add(x)); //views.schema_table
                    
                if (this.ComputedView != null)
                    this.ComputedView.Columns.Where(c => this.Columns.Where(tc => tc.Name == c.Name).Count() == 0).ToList().ForEach(x => cols.Add(x)); //computed.schema_table

                var list = new BindingList<DBColumn>(cols.OrderBy(x => int.Parse(string.IsNullOrEmpty(x.DisplayOrder) ? "0" : x.DisplayOrder)).ToList());
                list.ListChanged += list_ListChanged;
                return list;
            }
            //set { this.combined_columns = value; } 
        }

        void list_ListChanged(object sender, ListChangedEventArgs e)
        {
            var list = (sender as BindingList<DBColumn>);
            if(State != DBObjectState.None)
                if(e.ListChangedType == ListChangedType.ItemAdded)
                {
                    //prepare commands for reorder
                    for (int i = 0; i < list.Count; i++)
                        list[i].DisplayOrder = i.ToString();
                }
        }

        private DBColumn default_sort;
        [DataMember, Category("Data"), Description("Default Sort Column")]
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn DefaultSort
        {
            get { return default_sort; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "default_sort", this.default_sort == null ? "" : this.default_sort.Name, value == null ? "" : value.Name);
                }

                default_sort = value;
            }
        }

        private DBColumn sort_domain;
        [DataMember, Category("Data"), Description("Sort Domain")]
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn SortDomain
        {
            get { return sort_domain; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "sort_domain", this.sort_domain == null ? "" : this.sort_domain.Name, value == null ? "" : value.Name);
                }

                sort_domain = value;
            }
        }

        private string ms_description = "";
        [DataMember, Category("Appearance"), Description("Custom Table Description")]
        public string Description
        {
            get { return ms_description; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", this.ms_description, value);
                }

                ms_description = value;
            }
        }

        private SortDirections sort_direction;
        [DataMember, Category("Data"), Description("Default Sort Column Order")]
        public SortDirections Direction
        {
            get { return sort_direction; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "sort_direction", this.sort_direction.ToString(), value.ToString());
                }

                sort_direction = value;
            }
        }

        [Browsable(false), DisplayName("Views")]
        public BindingList<DBView> Extensions { get; set; }
        [Category("Actions")]
        [DisplayName("Row Actions")]
        [Browsable(false)]
        [ShowInManage(true)]
        public MyBindingList<RowAction> RowActions { get; set; }

        [Browsable(false)]
        public MyBindingList<DBView> Policies { get; set; }
        [Browsable(false)]
        public MyBindingList<DBFKConstraint> ForeignKeys { get; set; }
        
        private bool use_history;
        [DataMember, Description("Enable History and show columns for created date, created by, updated by, and last modified date")]
        public bool UseHistory
        {
            get { return this.use_history; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "use_history", this.use_history.ToString(), value.ToString());
                }

                use_history = value;
            }
        }

        [Category("Appearance")]
        [Editor(typeof(DBIconClassEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DBIconClass IconClass
        {
            get { return this.icon_class; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "iconclass", this.icon_class.FinalClass, value.FinalClass);
                }

                this.icon_class = value;
            }
        }
        private DBIconClass icon_class = new DBIconClass();

        [Browsable(false)]
        public BindingList<DBTrigger> Triggers { get; set; }
        [Browsable(false)]
        [ReadOnly(true)]
        public DBView ExtendedView { get; set; }
        [Browsable(false)]
        [ReadOnly(true)]
        public DBView ComputedView { get; set; }
        [Browsable(false)]
        [ReadOnly(true)]
        public DBView SecureView { get; set; }

        /* MIGRATION*///////
        public enum MigrationDataSources { Development, Production, Query, Clear, NoMigration }
        private MigrationDataSources migration_data_source;
        [Browsable(false)]
        [Category("Migration"), Description("Source of data to use when migrating")]
        public MigrationDataSources MigrationDataSource
        {
            get { return this.migration_data_source; }
            set
            {
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "migration_data_source", migration_data_source.ToString(), value.ToString());
                }

                SetField(ref this.migration_data_source, value);

                //this.migration_data_source = value;
            }
        }

        private string migration_data_query;
        [Browsable(false)]
        [Category("Migration")]
        public string MigrationDataQuery
        {
            get { return this.migration_data_query; }
            set
            {
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "migration_data_query", migration_data_query, value);
                }
                this.migration_data_query = value;
            }
        }

        public override string GetAdditionalSyntax()
        {
            //must always check for table existence 
            var check_exists = string.Format("IF OBJECT_ID('{0}') IS NOT NULL\r\n\t", this.FullName);//IF OBJECT_ID('[schema].[tablename]') IS NOT NULL (if exists)            

            var syntax = "";
            //TRIGGER SYNTAX
            if (this.Triggers != null)
                foreach (var trigger in this.Triggers)
                {                    
                    syntax += string.Format("{0}\r\nBEGIN\r\n\t{1}\r\nEND\r\n", check_exists, trigger.GetSQL().Replace("\r\n", "\r\n\t"));
                }
            return syntax;
        }

        public override string GetCommandDetailSyntax()
        {
            var syntax = "";

            //add PK definition here
            //var pk_syntax = this.PrimaryKey.GetSQL(); //constraints are created on their own

            switch (this.Action)
            {
                case DB.DBAction.Create:
                    var cols = new List<DBColumn>();

                    cols.AddRange(this.Columns); //add this table's cols

                    var cols_syntax = string.Join(",\r\n\t", cols.Select(x => x.GetSQL()).ToList().ToArray()); //transform cols to syntax

                    syntax = string.Format("(\r\n\t{0}\r\n)", cols_syntax);
                    break;
                case DB.DBAction.Alter:
                    //alter must be one by one
                    var alters = this.Columns.Where(x => x.Action == DB.DBAction.Alter).Select(x => string.Format("{0}\r\nGO", x.GetSQL())).ToList().ToArray();
                    var alter_cols_syntax = alters.Length > 0 ? string.Join(string.Format("ALTER TABLE {0}\r\n\t", this.FullName), alters) + "\r\nGO" : "";//alter table x alter c GO ... alter table x altet y GO ...

                    //add/drop can be multiple
                    var adds = this.Columns.Where(x => x.Action == DB.DBAction.Add).Select(x => x.GetSQL()).ToList().ToArray();
                    var add_cols_syntax = adds.Length > 0 ? string.Format("ADD {0}\r\nGO", string.Join(",\r\n\t", adds)) : "";

                    var drops = this.Columns.Where(x => x.Action == DB.DBAction.Drop).Select(x => x.GetSQL()).ToList().ToArray();
                    var drop_cols_syntax = drops.Length > 0 ? string.Format("DROP COLUMN {1}\r\nGO", syntax, string.Join(",\r\n\t", drops)) : "";

                    //primary key


                    syntax = string.Join("\r\n\t", alter_cols_syntax, add_cols_syntax, drop_cols_syntax);
                    break;
            }

            return syntax;
        }

        public override string GetCommandSyntax()
        {
            //correct action
            if (this.Action == DB.DBAction.Add)
                this.Action = DB.DBAction.Create;

            return base.GetCommandSyntax();
        }

        public override void DrawRelationalGraph(Graph g, Node pn)
        {
            g.AddNode(this.DisplayName);

            foreach(var r in this.ForeignKeys)
            {
                var reference_fullname = r.ReferenceSchemaName + "." + r.ReferenceTableName;
                var node = g.AddNode(reference_fullname);
                g.AddEdge(this.DisplayName, r.Column.Name, reference_fullname);
            }
        }

        public enum PolicyTypes { Select, Update, Delete }
        public enum ViewTypes { Views, Computed }

        public enum ProcedureTypes { Business }

        [Browsable(false)]
        public MyBindingList<DBCheckConstraint> CheckConstraints { get; set; }
        [Browsable(false)]
        public MyBindingList<DBDefaultConstraint> DefaultConstraints { get; set; }
        [Browsable(false)]
        public MyBindingList<TriggerConstraint> TriggerConstraints { get; set; }

        [Category("Operations")]
        [ShowInManage(true)]
        public MyBindingList<SQLUpdateCommand> UpdateCommands { get; internal set; }
        [Category("Operations")]
        [ShowInManage(true)]
        public MyBindingList<SQLInsertCommand> InsertCommands { get; internal set; }
        [Category("Operations")]
        [ShowInManage(true)]
        public MyBindingList<SQLDeleteCommand> DeleteCommands { get; internal set; }
        [Category("Operations")]
        [ShowInManage(true)]
        public MyBindingList<BasicProcedure> BasicProcedures { get; private set; }
    }
}
