using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EngineManager
{
    public partial class ManageRelatedColumns : Form
    {
        public ManageRelatedColumns()
        {
            InitializeComponent();

            // Load from XML
            syntaxEditor1.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);
            syntaxEditor2.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.JSON.xml", 0);
            syntaxEditor3.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);

            cmb_aggregation.DataSource = Enum.GetValues(typeof(AdditionalColumn.DBAggregation));
            cmb_sort_dir.DataSource = Enum.GetValues(typeof(AdditionalColumn.DBSort));
         
        }

        private DBTable original_table;
        private DBTable table;

        private List<AdditionalColumn> available_related_columns = new List<AdditionalColumn>();
        public DBTable Table
        {
            get { return this.table; }
            set
            {
                this.original_table = value;
                var db = this.get_db_basic_data(value.Connection);
                var schema = db.Schemas.Where(x => x.Name == value.Schema.Name).FirstOrDefault();
                var tbl = schema.Tables.Where(t => t.Name == value.Name).FirstOrDefault();
                tbl.PrimaryKey = original_table.PrimaryKey;
                this.table = tbl;

                RefreshView();
            }
        }
       
        private List<AdditionalColumnDefinition> get_related_columns_definition(DBTable t)
        {
            List<AdditionalColumnDefinition> definition = new List<AdditionalColumnDefinition>();
            var ep = t.Properties.Where(p=>p.Name == "related_columns").FirstOrDefault();

            if (ep!=null)
                definition = ep.GetObject(typeof(List<AdditionalColumnDefinition>)) as List<AdditionalColumnDefinition>;

            return definition;
        }
        private List<AdditionalComputedColumn> get_computed_columns_definition(DBTable t)
        {
            List<AdditionalComputedColumn> definition = new List<AdditionalComputedColumn>();
            var ep = t.Properties.Where(p => p.Name == "computed_columns").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<AdditionalComputedColumn>)) as List<AdditionalComputedColumn>;

            return definition;
        }

        private List<SecurityPolicyQuestionDefinition> get_security_policy_questions_definition(DBTable t)
        {
            List<SecurityPolicyQuestionDefinition> definition = new List<SecurityPolicyQuestionDefinition>();
            var ep = t.Properties.Where(p => p.Name == "security_questions").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<SecurityPolicyQuestionDefinition>)) as List<SecurityPolicyQuestionDefinition>;

            return definition;
        }
        private List<SecurityPolicyDefinition> get_available_policies_definition(DBTable t)
        {
            var definition = new List<SecurityPolicyDefinition>();
            var ep = t.Properties.Where(p => p.Name == "security_policies").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<SecurityPolicyDefinition>)) as List<SecurityPolicyDefinition>;

            return definition;
        }
        private SecurityPolicy get_security_policy(DBTable t, string policy) //eg. select_policy
        {

            var ep = t.Properties.Where(p => p.Name == policy).FirstOrDefault();

            if (ep != null)
            {
                var id = ep.GetObject(typeof(string)) as string;
                var p = t.AvailablePolicies.Where(x => x.ID == id).FirstOrDefault();
                return p;
            }

            return null;
        }

        private List<RowActionDefinition> get_actions_definition(DBTable t)
        {
            List<RowActionDefinition> definition = new List<RowActionDefinition>();
            var ep = t.Properties.Where(p => p.Name == "table_actions").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<RowActionDefinition>)) as List<RowActionDefinition>;

            return definition;
        }

        private List<ValidatorDefinition> get_validators_definition(DBTable t)
        {
            List<ValidatorDefinition> definition = new List<ValidatorDefinition>();
            var ep = t.Properties.Where(p => p.Name == "validators").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<ValidatorDefinition>)) as List<ValidatorDefinition>;

            return definition;
        }

        private List<SQLUpdateCommandDefinition> get_update_commands_definition(DBTable t)
        {
            List<SQLUpdateCommandDefinition> definition = new List<SQLUpdateCommandDefinition>();
            var ep = t.Properties.Where(p => p.Name == "update_commands").FirstOrDefault();

            if (ep != null)
                definition = ep.GetObject(typeof(List<SQLUpdateCommandDefinition>)) as List<SQLUpdateCommandDefinition>;

            return definition;
        }

        private List<AdditionalColumn> get_available_related_columns(DBTable t, bool root_only = true)
        {
            List<AdditionalColumn> available_related_columns = new List<AdditionalColumn>();

            //read fks
            var fks = t.DBObjects.Where(x => x is DBFKConstraint).Cast<DBFKConstraint>().ToList();
            foreach (var fk in fks)
            {
                var rc = new AdditionalColumn { CurrentTable = t, Column = fk.Column, Constraint = fk };
                if (root_only)
                {
                    available_related_columns.Add(rc);
                }
                else
                {
                    foreach (TreeNode n in rc.GetTree().Nodes)
                        available_related_columns.Add(n.Tag as AdditionalColumn);
                }
            }

            return available_related_columns;
        }

        public void RefreshView()
        {
            lbl_source.Text = this.table.FullName;
            propertyGrid2.SelectedObject = table;

            //make sure only attributes of interest are shown
            propertyGrid2.BrowsableAttributes = new AttributeCollection(new ShowInManage(true));

            //clear all
            treeView_current.Nodes.Clear();
            treeView_parents.Nodes.Clear();
            treeView_sub.Nodes.Clear();
            treeView_added.Nodes.Clear();
            treeView_computed.Nodes.Clear();
            treeView_policies.Nodes.Clear();

            //read stored definitions
            this.cached_definition = get_related_columns_definition(this.table);
            this.computed_columns = get_computed_columns_definition(this.table);
            this.available_related_columns = get_available_related_columns(this.table);
            this.security_questions_definition = get_security_policy_questions_definition(this.table);
            this.available_policies_definition = get_available_policies_definition(this.table);
            this.table_actions_definition = get_actions_definition(this.table);
            this.validators_definition = get_validators_definition(this.table);
            this.update_commands_definitions = get_update_commands_definition(this.table);

            //update commands todo.. save them then load them here...
            
            //available current table columns
            foreach (var c in this.table.Columns)
                treeView_current.Nodes.Add(new TreeNode(c.Name) { Tag = new AdditionalColumn { Column = c, CurrentTable = table, FromThisTable = true, Realtime = false } });     

            //populate the available parents and sub tables trees...
            foreach (var r in this.available_related_columns)
            {
                var table_node = r.GetTree();
                if (r.IsFromSubTable) //this is the pk table
                {
                    treeView_sub.Nodes.Add(table_node);                   
                }
                else //this is the fk table 
                {
                     treeView_parents.Nodes.Add(table_node);
                }  
            }

            ////// CONVERT DEFINITIONS INTO OBJECTS /////

            //load saved definition (added columns)
            
            if (this.cached_definition != null)
            {
                table.AdditionalRelatedColumns.Clear();
                foreach (var r in this.cached_definition)
                {
                    var new_rel = r.GetObject(table);

                    if (new_rel != null)
                    {
                        //fill the added tree
                        treeView_added.Nodes.Add(new_rel.GetNode());
                        this.table.AdditionalRelatedColumns.Add(new_rel);//
                    }
                }         
            }

            //load saved definition (computed_columns)
            
            if (this.computed_columns != null)
            {
                table.AdditionalComputedColumns.Clear();
                foreach(var cc in this.computed_columns)
                {
                    //fill the added tree
                    treeView_computed.Nodes.Add(cc.GetNode());
                    this.table.AdditionalComputedColumns.Add(cc);//
                }
            }

            //load saved definition (security_questions)
            if(this.security_questions_definition != null)
            {
                table.SecurityPolicyQuestions.Clear();
                foreach(var qd in this.security_questions_definition)
                {
                    table.SecurityPolicyQuestions.Add(qd.GetObject(table));
                }
            }

            //load saved definition (security_policies)
            if (this.available_policies_definition != null)
            {
                table.AvailablePolicies.Clear();
                foreach (var pd in this.available_policies_definition)
                {
                    table.AvailablePolicies.Add(pd.GetObject(table));
                }
            }

            //questions
            listBox_questions.DataSource = new BindingSource(this.table.SecurityPolicyQuestions, null);

            //render the policy tree
            foreach (var p in table.AvailablePolicies)
                treeView_policies.Nodes.Add(p.GetTree());

            //load saved definition (table_actions)
            if (this.table_actions_definition != null)
            {
                table.RowActions.Clear();
                foreach (var ra in this.table_actions_definition)
                {                                       
                    //add in table and in GUI
                    AddRowAction(ra.GetObject(table));
                }
            }

            //row validators..
            //TODO

            //update commands todo
            if(this.update_commands_definitions != null)
            {
                foreach (var uc in this.update_commands_definitions)
                {
                    table.UpdateCommands.Add(uc.GetObject(table));
                }
            }

            listBox_update_commands.DataSource = new BindingSource(table.UpdateCommands, null);
            listBox_insert_commands.DataSource = new BindingSource(table.InsertCommands, null);
            listBox_delete_commands.DataSource = new BindingSource(table.DeleteCommands, null);
            listBox_basic_procedures.DataSource = new BindingSource(table.BasicProcedures, null);

            //insert commands...

            //delete commands...

            //basic procedures..

            //combo
            refresh_policy_combos();

            //load the definition and lookup from available policis the specified policy for: select, delete, update
            table.SelectPolicy = get_security_policy(this.table, "select_policy");
            table.UpdatePolicy = get_security_policy(this.table, "update_policy");
            table.DeletePolicy = get_security_policy(this.table, "delete_policy");

            //selections
            comboBox_select_policy.SelectedItem = table.SelectPolicy;
            comboBox_update_policy.SelectedItem = table.UpdatePolicy;
            comboBox_delete_policy.SelectedItem = table.DeletePolicy;

            //validators
            dataGridView_validators.Columns.Clear();
            dataGridView_validators.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewComboBoxColumn { HeaderText = "Policy", DataPropertyName = "SecurityPolicy", DataSource = new BindingSource(table.AvailablePolicies, null), ValueType = typeof(SecurityPolicy), ValueMember = "Self", DisplayMember = "ID" },
                new DataGridViewCheckBoxColumn { HeaderText = "Insert", DataPropertyName = "Insert"},
                new DataGridViewCheckBoxColumn { HeaderText = "Update", DataPropertyName = "Update"}
            });
            dataGridView_validators.AutoGenerateColumns = false;
            dataGridView_validators.DataSource = new BindingSource(this.table.Validators, null);

            

           
            update_syntax(false);  

        }

        private void btn_add_current_Click(object sender, EventArgs e)
        {
            var node = treeView_current.SelectedNode;
            if (node != null)
            {
                //treeView_added.Nodes.Add(new TreeNode(((node.Tag as DBRelatedColumn).CurrentTable as DBTable).FullName + '.' + node.Text) { Tag = node.Tag });
                treeView_added.Nodes.Add((node.Tag as AdditionalColumn).GetNode());
                this.table.AdditionalRelatedColumns.Add(node.Tag as AdditionalColumn);//
                update_syntax();
            }
        }

        private void btn_add_parent_Click(object sender, EventArgs e)
        {
            var node = treeView_parents.SelectedNode;
            if (node != null && node.Tag is AdditionalColumn)
            {
                treeView_added.Nodes.Add((node.Tag as AdditionalColumn).GetNode());
                this.table.AdditionalRelatedColumns.Add(node.Tag as AdditionalColumn);//
                update_syntax();
            }
        }

        private void btn_add_sub_Click(object sender, EventArgs e)
        {
            var node = treeView_sub.SelectedNode;
            if (node != null && node.Tag is AdditionalColumn)
            {
                var src = node.Tag as AdditionalColumn;
                var db_related_col = new AdditionalColumn
                {
                    CurrentTable = this.table,
                    Constraint = src.Constraint,
                    Column = src.Column,
                    Aggregation = (AdditionalColumn.DBAggregation)Enum.Parse(typeof(AdditionalColumn.DBAggregation), cmb_aggregation.SelectedValue.ToString(), true),
                    SortColumn = cmb_sort_column.SelectedValue as  DBColumn,
                    SortDirection = (AdditionalColumn.DBSort)Enum.Parse(typeof(AdditionalColumn.DBSort), cmb_sort_dir.SelectedValue.ToString(), true)
                };
                //treeView_added.Nodes.Add(new TreeNode((node.Parent.Tag as DBTable).FullName + '.' + node.Text) { Tag = db_related_col });
                treeView_added.Nodes.Add(db_related_col.GetNode());
                this.table.AdditionalRelatedColumns.Add(db_related_col);//
                update_syntax();
            }
        }

        private void update_syntax()
        {
            cached_view_statement = update_syntax(false);
            computed_view_statement = update_syntax(true);
        }

        private string update_syntax(bool realtime)
        {
            var sql_outermost_select = string.Format("select [t].{0}", original_table.PrimaryKey.Columns.First().FullName);
            var sql_outer_select = string.Format("select [t].{0}", original_table.PrimaryKey.Columns.First().FullName);
            var sql_select = string.Format("select [tbl].{0}", original_table.PrimaryKey.Columns.First().FullName);
            var sql_join = string.Format("\r\n\tfrom {0} as [tbl]", this.table.FullName);

            var definition = new List<AdditionalColumnDefinition>(); //definition to be saved for the EXTENDED VIEW
            var referenced_fks = new List<string>();
            var joined_fks = new List<string>();

            //for (int i = 0; i < treeView_added.Nodes.Count; i++)

            //additional columns
            foreach(var current_rel in this.table.AdditionalRelatedColumns.Where(x=> x.Realtime == realtime).ToList())
            {
                //var current_rel = (treeView_added.Nodes[i].Tag as DBRelatedColumn);
                definition.Add(current_rel.GetDefinition());
                
                var related_col = current_rel;
                var required_col = related_col.Column;
                var parent_col = related_col.Constraint != null ? related_col.Constraint.ReferenceColumn : related_col.Column;
                var parent_table = related_col.Column.Parent;
                var sub_table = related_col.Constraint != null ? related_col.Constraint.Parent as DBTable : table;
                var sub_col = related_col.Constraint != null ? related_col.Constraint.Column : related_col.Column;
                var aggr = related_col.Aggregation != AdditionalColumn.DBAggregation.None ? related_col.Aggregation.ToString() : "";
                var sort_col = related_col.SortColumn;
                var sort_dir = related_col.SortDirection.ToString();
                var col_owner = required_col.Parent.FullName;

                var fk_key = related_col.Constraint != null ? related_col.Constraint.FullName + "-" + col_owner : col_owner;
                //keep reference to the constraint so it can be reused
                if (!referenced_fks.Contains(fk_key))
                    referenced_fks.Add(fk_key);

                if(related_col.FromThisTable) //self column (from the same table)
                {
                    var cmd_select = related_col.GetSelectSyntax();
                    sql_select += cmd_select;
                }
                else if (related_col.IsFromSubTable) //aggregation from sub tables
                {
                    var cmd_select = related_col.GetSelectSyntax();
                    sql_select += cmd_select;
                }
                else //select from parent tables
                {
                    //var index = referenced_fks.IndexOf(fk_key);
                    var cmd_select = related_col.GetSelectSyntax();
                    sql_select += cmd_select;

                    if (!joined_fks.Contains(fk_key))
                    {
                        var cmd_join = related_col.GetJoinSyntax();
                        sql_join += cmd_join;
                        joined_fks.Add(fk_key);
                    }
                    else //reuse the same join if used before
                    {

                    }
                    
                }

                //if using an expression
                if (string.IsNullOrEmpty(related_col.Expression)) 
                    related_col.Expression = "{0}";

                //if (related_col.Output)
                //    sql_outer_select += "\r\n\t, [" + related_col.Alias + "] = " + string.Format(related_col.Expression, "[t].[" + related_col.Alias + "]");
                sql_outer_select += related_col.GetOuterSelectSyntax();
                sql_outermost_select += related_col.GetOuterSelectSyntax();
            }

            //computed columns
            foreach(var cc in this.table.AdditionalComputedColumns.Where(x=> x.Realtime == realtime).ToList())
            {
                sql_outer_select += cc.GetOuterSelectSyntax();
                sql_outermost_select += cc.GetOuterMostSelectSyntax();
            }

            //actions columns
            if (realtime)
            {
                foreach (var ra in this.table.RowActions)
                {
                    sql_outermost_select += ra.GetOuterSelectSyntax();
                }
            }

            //preview sql
            var final_sql = sql_outer_select + "\r\nfrom\r\n(\r\n\t" + sql_select + sql_join + "\r\n) [t]";

            final_sql = sql_outermost_select + "\r\nfrom\r\n(\r\n\t" + final_sql + "\r\n) [t]";

            syntaxEditor1.Text = final_sql;
           
            if(realtime)
                this.computed_definition = definition; //save the new generated definition
            else
                this.cached_definition = definition; //save the new generated definition
            
            //preview JSON to be saved
            var serialized = JsonConvert.SerializeObject(definition, Formatting.Indented);
            syntaxEditor2.Text = serialized;


            
            //var deserialized = JsonConvert.DeserializeObject(serialized, typeof(List<DBRelatedColumnDefinition>));

            //preview query results
            var data = this.table.Connection.GetDataTable(final_sql);
            dataGridView1.DataSource = data;
            
            return final_sql;
        }

        private string cached_view_statement;
        private string computed_view_statement;

        private void treeView_sub_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Tag is AdditionalColumn)
            {
                cmb_sort_column.DataSource = (e.Node.Tag as AdditionalColumn).Constraint.Parent.DBObjects.Where(x => x is DBColumn).ToList();
            }
        }

        private Dictionary<string, DBFKConstraint> get_foreign_keys(DBConnection conn)
        {
            //this.db_objects
            var sql = @"
            select
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
		        is_constraint,
		        is_disabled,
		        relation_type
		
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
						, is_constraint = cast(1 as bit)
				        , kc.is_disabled
						, relation_type = 'constraint'
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
					where
						fk_schema.name <> 'extensions'

					union all

					select 
						fk_name = 'FK_history_eventlog_' + concat(schema_name(t.schema_id), '_', t.name),
						pk_column_id = concat(schema_name(t.schema_id),'.',t.name, '.', c.name),
				        fk_column_id = 'history.EventLog.identity_value',
						fk_schema = 'history',
						[fk_table] = 'EventLog',
						[fk_column] = 'identity_value', 
						pk_schema = schema_name(t.schema_id),
						pk_table = t.name,
						pk_column = c.name,
						delete_action = 'NO_ACTION',
						update_action = 'NO_ACTION',
						[respect] = null,
						title = null,
						is_constraint = cast(0 as bit),
						is_disabled = cast(1 as bit),
						'for_history'

					from
						sys.tables t 
						join sys.columns c on c.object_id = t.object_id and c.is_identity = 1
						join sys.index_columns ic on ic.column_id = c.column_id and ic.object_id = t.object_id
						join sys.indexes i on ic.index_id = i.index_id and i.object_id = t.object_id and i.is_primary_key = 1
						join sys.tables h on h.name = 'EventLog' and object_schema_name(h.object_id) = 'history'
					where
						exists(select 1 from sys.extended_properties ep where ep.major_id = t.object_id and ep.minor_id = 0 and ep.name = 'use_history' and ep.value = 'true')
		        ) as t1";

            var fks = conn.GetDataTable(sql).AsEnumerable().Select(x => new DBFKConstraint()
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
                RelationType = x["relation_type"].ToString(),
                OnDelete = x["delete_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                x["delete_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                    x["delete_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,
                OnUpdate = x["update_action"].ToString() == "CASCADE" ? DBFKConstraint.CascadeModes.Cascade :
                                x["update_action"].ToString() == "SET_NULL" ? DBFKConstraint.CascadeModes.SetNull :
                                    x["update_action"].ToString() == "SET_DEFAULT" ? DBFKConstraint.CascadeModes.SetDefault : DBFKConstraint.CascadeModes.DoNothing,
                Parent = db_objects[x["parent_id"].ToString()].Parent,//parent of the folder
                Action = DB.DBAction.Alter,
                Connection = db_objects[x["parent_id"].ToString()].Parent.Connection,
            }).ToList().ToDictionary(o => o.PathID, o => o);

            return fks;

        }
        private Dictionary<string, DBObject> db_objects = new Dictionary<string, DBObject>();
        private DB get_db_basic_data(DBConnection conn)
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
		, [definition] = cast(ep.value as varchar(max))
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
	where
		s.schema_id between 5 and 16383

union all

select --special case for history
	[id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT'
	, [object_name] = o.name
	, [object_type] = 'FOLDER'
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [definition] = OBJECT_DEFINITION(o.object_id)
from
	sys.objects o 
	join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
where
	s.name = 'history' and o.name = 'EventLog'

union all

select --special case for history fk
	[id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT/' + 'FK_history_eventlog_' + s.name + '_' + o.name
	, [object_name] = 'FK_history_eventlog_' + s.name + '_' + o.name
	, [object_type] = 'FOREIGN_KEY_CONSTRAINT'
	, [parent_id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT'
	, [definition] = null
from
	sys.objects o 
	join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
where
	object_id('history.EventLog') is not null
	and exists(select 1 from sys.extended_properties ep where ep.major_id = o.object_id and ep.minor_id = 0 and ep.name = 'use_history' and ep.value = 'true')

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
	, [definition] = cast(ep.value as varchar(max))
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
	, [definition] = cast(ep.value as varchar(max))
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

            var db = new DB();

            var data = conn.Connection.GetDataTable(sql); //all objects

            var nodes = new List<TreeNode>();

            foreach (DataRow r in data.Rows)
            {
                var id = r["id"].ToString();
                var object_name = r["object_name"].ToString();
                var object_type = r["object_type"].ToString();
                var parent_id = r["parent_id"].ToString();
                var definition = r["definition"].ToString();

                var db_obj = new DBObject();


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

                    if (db_obj is DBExtendedProperty)
                        (db_obj as DBExtendedProperty).Value = db_obj.DefinitionSQL;

                    db_objects.Add(id, db_obj); //flat list

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
                        var schema_obj = db_obj as DBSchemaObject;

                        if (schema_obj != null && parent_obj.Parent != null && parent_obj.Parent is DBSchema)
                        {
                            schema_obj.Schema = parent_obj.Parent as DBSchema;
                        }

                        //set database for schemas
                        if (db_obj is DBSchema)
                            (db_obj as DBSchema).Database = db;

                        
                            

                    }
                    else
                    {
                        db = db_obj as DB;
                    }
                }
            }

            //db_objects list populated.


            //for extended/computed views, wire them to their owner table..
            foreach (var db_obj in db_objects.Where(x => x.Key.Contains("/SCHEMA/views/VIEW/") || x.Key.Contains("/SCHEMA/computed/VIEW/") || x.Key.Contains("/SCHEMA/secure/VIEW/")).Where(x=> x.Value is DBView).Select(x => x.Value).Cast<DBSchemaObject>().ToList())
            {
                if (db_obj is DBView)
                {
                    var schema_name = db_obj.Name.Substring(0, db_obj.Name.IndexOf("_"));
                    var table_name = db_obj.Name.Substring(db_obj.Name.IndexOf("_") + 1);
                    var table_id = string.Format("{0}/SCHEMA/{1}/USER_TABLE/{2}", db.Name, schema_name, table_name);

                    if (db_objects.ContainsKey(table_id)) //if related to a table
                    {
                        var tbl = db_objects[table_id] as DBTable;

                        if (db_obj.Schema.Name == "views")
                            tbl.ExtendedView = db_obj as DBView;

                        if (db_obj.Schema.Name == "computed")
                            tbl.ComputedView = db_obj as DBView;

                        if (db_obj.Schema.Name == "secure")
                            tbl.SecureView = db_obj as DBView;
                    }
                    
                }
            }
            


            var fks = this.get_foreign_keys(conn); //fks
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
                if (!v.ReferenceColumn.Parent.DBObjects.Contains(o))
                {
                    var owner = o.Parent;
                    var parent = o.Parent;
                    v.ReferenceColumn.Parent.DBObjects.Add(o); //this affects the owner and thus need to be reset
                    o.Owner = owner as DBSchemaObject;
                    o.Parent = parent;
                }

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
                            foreach (var i in collection)
                                if (obj != null)
                                    method.Invoke(obj, new object[] { i });
                        }
                    }
                }

            }
            
            return db;
        }

        private void apply()
        {
            update_syntax();

            //related
            var cmd = DBObjectManager.CreateExtendedProperty(original_table, "related_columns", "-", JsonConvert.SerializeObject(this.cached_definition));
            DBProjectManager.Execute(cmd);//save the prop

            //computed
            var cmd_cc = DBObjectManager.CreateExtendedProperty(original_table, "computed_columns", "-", JsonConvert.SerializeObject(this.table.AdditionalComputedColumns));
            DBProjectManager.Execute(cmd);//save the prop



            //create the cached view
            var verb = this.original_table.ExtendedView == null ? "create" : "alter";
            var sql = string.Format("{0} view [views].[{1}_{2}]\r\nas\r\n\r\n{3}", verb, this.original_table.Schema.Name, this.original_table.Name, cached_view_statement);
            this.original_table.Connection.GetDataTable(sql); //this triggers recreation of secure view within the database..

            //create the computed view
            verb = this.original_table.ComputedView == null ? "create" : "alter";
            sql = string.Format("{0} view [computed].[{1}_{2}]\r\nas\r\n\r\n{3}", verb, this.original_table.Schema.Name, this.original_table.Name, computed_view_statement);
            this.original_table.Connection.GetDataTable(sql); //this triggers recreation of secure view within the database..

            //refresh?!
            DBTableManager.Refresh(this.original_table);

            //save security_questions
            var questions = this.table.SecurityPolicyQuestions.Select(x => x.GetDefinition()).ToList();
            var cmd_questions = DBObjectManager.CreateExtendedProperty(original_table, "security_questions", "-", JsonConvert.SerializeObject(questions));
            DBProjectManager.Execute(cmd_questions);//save the prop

            //save available_policies
            var available_policies = this.table.AvailablePolicies.Select(x => x.GetDefinition()).ToList();
            var cmd_available_policies = DBObjectManager.CreateExtendedProperty(original_table, "security_policies", "-", JsonConvert.SerializeObject(available_policies));
            DBProjectManager.Execute(cmd_available_policies);//save the prop

            //select policy
            save_policy(comboBox_select_policy.SelectedItem as SecurityPolicy, "select");

            //update policy
            save_policy(comboBox_update_policy.SelectedItem as SecurityPolicy, "update");

            //delete policy
            save_policy(comboBox_delete_policy.SelectedItem as SecurityPolicy, "delete");

            //save validations
            var validations = this.table.Validators.Select(x => x.GetDefinition()).ToList();
            var cmd_validations = DBObjectManager.CreateExtendedProperty(original_table, "validators", "-", JsonConvert.SerializeObject(validations));
            DBProjectManager.Execute(cmd_validations);

            //update commands
            var update_commands = this.table.UpdateCommands.Select(x => x.GetDefinition()).ToList();
            var cmd_update_commands = DBObjectManager.CreateExtendedProperty(original_table, "update_commands", "-", JsonConvert.SerializeObject(update_commands));
            DBProjectManager.Execute(cmd_update_commands);

            //insert
            var insert_commands = this.table.InsertCommands.Select(x => x.GetDefinition()).ToList();
            var cmd_insert_commands = DBObjectManager.CreateExtendedProperty(original_table, "insert_commands", "-", JsonConvert.SerializeObject(insert_commands));
            DBProjectManager.Execute(cmd_insert_commands);

            //delete
            var delete_commands = this.table.DeleteCommands.Select(x => x.GetDefinition()).ToList();
            var cmd_delete_commands = DBObjectManager.CreateExtendedProperty(original_table, "delete_commands", "-", JsonConvert.SerializeObject(delete_commands));
            DBProjectManager.Execute(cmd_delete_commands);

            //basic procs
            var basic_procedures_commands = this.table.BasicProcedures.Select(x => x.GetDefinition()).ToList();
            var cmd_basic_procedures_commands = DBObjectManager.CreateExtendedProperty(original_table, "basic_procedures_commands", "-", JsonConvert.SerializeObject(basic_procedures_commands));
            DBProjectManager.Execute(cmd_basic_procedures_commands);



            //save actions
            var actions = this.table.RowActions.Select(x => x.GetDefinition()).ToList();
            var cmd_actions = DBObjectManager.CreateExtendedProperty(original_table, "table_actions", "-", JsonConvert.SerializeObject(actions));
            DBProjectManager.Execute(cmd_actions);

            //view must have been already created!
            //create extended property for each action procedure
            DBViewManager.Refresh(this.original_table.ExtendedView);//refresh so we now have its columns..
            foreach (var a in actions)
            {
                var col = this.original_table.ExtendedView.Columns.FirstOrDefault(c => c.Name == a.ActionName);
                if (col != null)
                {
                    var cmd_act_proc = DBObjectManager.CreateExtendedProperty(col, "procedure", "-", (a.BasicProcedure != null ? a.BasicProcedure : a.Procedure) + " " + table.PrimaryKey.Columns.First().FullName);
                    DBProjectManager.Execute(cmd_act_proc);

                    var cmd_act_datatype = DBObjectManager.CreateExtendedProperty(col, "datatype", "-", "action");
                    DBProjectManager.Execute(cmd_act_datatype);

                    var cmd_act_icon = DBObjectManager.CreateExtendedProperty(col, "iconclass", "-", a.IconClass);
                    DBProjectManager.Execute(cmd_act_icon);
                }

            }
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            apply();
        }

        private void save_policy(SecurityPolicy policy, string operation)
        {
            var cmd_policy = DBObjectManager.CreateExtendedProperty(original_table, operation + "_policy", "-", policy == null ? "" : JsonConvert.SerializeObject(policy.ID));
            DBProjectManager.Execute(cmd_policy);

            var view = this.original_table.Connection.GetDataTable(string.Format("select [name] = object_id('{0}.{1}_{2}')", operation, this.original_table.Schema.Name, this.original_table.Name)).AsEnumerable().Select(x => x["name"].ToString()).FirstOrDefault();
            
            var verb = "";
            var sql = "";

            if (policy != null) //create or alter
            {
                verb = string.IsNullOrEmpty(view) ? "create" : "alter";
                sql = string.Format("{0} view [{1}].[{2}_{3}]\r\nas\r\n\r\n{4}", verb, operation, this.original_table.Schema.Name, this.original_table.Name, policy.GetQuery(true));
            }
            else //delete
            {
                sql = string.Format("if object_id('[{0}].[{1}_{2}]') is not null drop view [{0}].[{1}_{2}]", operation, this.original_table.Schema.Name, this.original_table.Name);
            }

            this.original_table.Connection.GetDataTable(sql); //this triggers recreation of secure view within the database..
        }

        private void treeView_added_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_added.SelectedNode != null)
                propertyGrid1.SelectedObject = treeView_added.SelectedNode.Tag;
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            update_syntax(false);
        }

        private void treeView_parents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
         
        }

        //definition storage variables
        private List<AdditionalColumnDefinition> cached_definition;
        private List<AdditionalComputedColumn> computed_columns = new List<AdditionalComputedColumn>();
        private List<SecurityPolicyQuestionDefinition> security_questions_definition;
        private List<SecurityPolicyDefinition> available_policies_definition;
        private List<RowActionDefinition> table_actions_definition;
        private List<ValidatorDefinition> validators_definition;
        private List<SQLUpdateCommandDefinition> update_commands_definitions;

        private void treeView_computed_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView_computed.SelectedNode != null)
            {
                propertyGrid1.SelectedObject = treeView_computed.SelectedNode.Tag;
            }
        }
        //Security Questions
        private void toolStripButton_new_question_Click(object sender, EventArgs e)
        {
            var question = new SecurityPolicyQuestion { Table = this.table, Question = "New Question ?", Operator = SecurityPolicyQuestion.SqlOperator.Equals, ConstantValue = "1" };
            this.table.SecurityPolicyQuestions.Add(question);           
        }

        private void listBox_questions_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBox_questions.SelectedItem;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(listBox_questions.SelectedItem != null)
                this.table.SecurityPolicyQuestions.Remove(listBox_questions.SelectedItem as SecurityPolicyQuestion);
        }

        //security policy
        private void toolStripButton_new_node_Click(object sender, EventArgs e)
        {
            var selected_node = treeView_policies.SelectedNode;
            var question_item = listBox_questions.SelectedItem;
            if (question_item != null)
            {
                var question = (listBox_questions.SelectedItem as SecurityPolicyQuestion);
                var condition = new SecurityPolicyCondition { Question = question };
                var target_node = selected_node;
                if (target_node == null) //root
                {
                    return;
                    //target_node = treeView_policies.Nodes[0];
                }         
                else if(target_node.Parent != null) //not the root
                {
                    (target_node.Tag as SecurityPolicyCondition).SubConditions.Add(condition);
                }
                
                var new_node = new TreeNode(condition.Question.ToString()) { Tag = condition };
                target_node.Nodes.Add(new_node);

                treeView_policies.SelectedNode = new_node;
                treeView_policies.ExpandAll();

                if (target_node.Parent == null)
                {
                    var policy = GetRootPolicyFromNode(target_node);

                    policy.Conditions.Add(condition);
                }

            }
        }

        private void toolStripButton_orize_Click(object sender, EventArgs e)
        {
            var selected_node = treeView_policies.SelectedNode;
            if (selected_node != null)
            {
                var parent = selected_node.Parent;
                var grand_parent = parent.Parent;

                if (grand_parent != null)
                {

                    var condition = selected_node.Tag as SecurityPolicyCondition;

                    selected_node.Remove();
                    grand_parent.Nodes.Add(selected_node);

                    (parent.Tag as SecurityPolicyCondition).SubConditions.Remove(condition); //remove from parent

                    var grant_parent_condition = grand_parent.Tag as SecurityPolicyCondition;
                    if (grant_parent_condition != null) //not root
                    {
                        
                        grant_parent_condition.SubConditions.Add(condition); //add to grand parent
                    }
                    else
                    {
                        var root = grand_parent;
                        condition.Parent = null;
                        var policy = GetRootPolicyFromNode(grand_parent);

                        policy.Conditions.Add(condition);
                    }

                    treeView_policies.SelectedNode = selected_node;
                    treeView_policies.ExpandAll();
                }
            }            
        }

        private void toolStripButton_andize_Click(object sender, EventArgs e)
        {            
            var selected_node = treeView_policies.SelectedNode;
            if (selected_node != null)
            {
                var condition = selected_node.Tag as SecurityPolicyCondition;

                var parent = selected_node.Parent;
                var index = parent.Nodes.IndexOf(selected_node);
                if (index > 0)
                {
                    selected_node.Remove();
                    parent.Nodes[index - 1].Nodes.Add(selected_node);
                    (parent.Nodes[index - 1].Tag as SecurityPolicyCondition).SubConditions.Add(condition);//add to new parent

                    var parent_condition = parent.Tag as SecurityPolicyCondition;
                    if (parent_condition != null) //not root
                    {                        
                        parent_condition.SubConditions.Remove(condition); //remove from previous parent
                    }
                    else
                    {
                        var root = parent;
                        condition.Parent = null;
                        var policy = GetRootPolicyFromNode(parent);

                        policy.Conditions.Remove(condition);
                    }

                    treeView_policies.SelectedNode = selected_node;
                    treeView_policies.ExpandAll();
                }
            }
        }

        private void toolStripButton_delete_node_Click(object sender, EventArgs e)
        {
            var selected_node = treeView_policies.SelectedNode;
            if (selected_node != null)
            {
                if (selected_node.Tag is SecurityPolicy)
                {
                    selected_node.Remove();
                    table.AvailablePolicies.Remove(selected_node.Tag as SecurityPolicy);
                }
                else if (selected_node.Tag is SecurityPolicyCondition)
                {
                    var parent = selected_node.Parent;

                    var parent_condition = parent.Tag as SecurityPolicyCondition;
                    var condition = selected_node.Tag as SecurityPolicyCondition;

                    if (parent_condition != null) //if not root
                        parent_condition.SubConditions.Remove(condition);
                    else
                    {
                        //root
                        var root = parent;
                        var policy = GetRootPolicyFromNode(parent);

                        policy.Conditions.Remove(condition);
                    }
                    treeView_policies.SelectedNode = parent;
                    selected_node.Remove();
                }
                
                treeView_policies.ExpandAll();
            }
        }

        private void toolStripButton_up_Click(object sender, EventArgs e)
        {
            var selected_node = treeView_policies.SelectedNode;
            if (selected_node != null)
            {
                var condition = selected_node.Tag as SecurityPolicyCondition;

                var parent = selected_node.Parent;
                var index = parent.Nodes.IndexOf(selected_node);
                if (index > 0)
                {
                    selected_node.Remove();
                    parent.Nodes.Insert(index - 1, selected_node);

                    var parent_condition = parent.Tag as SecurityPolicyCondition;
                    if (parent_condition != null) //not root
                    {
                        parent_condition.SubConditions.Remove(condition);
                        parent_condition.SubConditions.Insert(index - 1, condition);
                    }
                    else //root
                    {
                        condition.Parent = null;
                        var policy = GetRootPolicyFromNode(selected_node);
                        policy.Conditions.Remove(condition);
                        policy.Conditions.Insert(index - 1, condition);
                    }

                    treeView_policies.SelectedNode = selected_node;
                    treeView_policies.ExpandAll();
                }
            }
        }

        private SecurityPolicy GetRootPolicyFromNode(TreeNode n)
        {
            var root = n;
            while (root.Parent != null)
                root = root.Parent;

            return root.Tag as SecurityPolicy;
        }

        private void toolStripButton_down_Click(object sender, EventArgs e)
        {
            var selected_node = treeView_policies.SelectedNode;
            if (selected_node != null)
            {
                var parent = selected_node.Parent;
                var index = parent.Nodes.IndexOf(selected_node);
                if (index < parent.Nodes.Count - 1)
                {
                    var condition = selected_node.Tag as SecurityPolicyCondition;
                    selected_node.Remove();
                    parent.Nodes.Insert(index + 1, selected_node);

                    var parent_condition = parent.Tag as SecurityPolicyCondition;
                    if(parent_condition != null)// not root
                    {
                        parent_condition.SubConditions.Remove(condition);
                        parent_condition.SubConditions.Insert(index + 1, condition);
                    }
                    else //root
                    {
                        var root = parent;
                        condition.Parent = null;

                        var policy = GetRootPolicyFromNode(parent);

                        policy.Conditions.Remove(condition);
                        policy.Conditions.Insert(index + 1, condition);
                        
                    }

                    treeView_policies.SelectedNode = selected_node;
                    treeView_policies.ExpandAll();                    
                }
            }
        }
        
        private string GenerateExpression(TreeNode root, string prefix = "")
        {
            var expression = prefix;
            var condition = root.Tag as ISecurityPolicyExpression;
            return expression = condition.WhereExpression;
        }

        private string GetPolicyJSON()
        {
            //todo
            //generate hierachical order using nodes and save it.
            return "";

           // var json = JsonConvert.SerializeObject(select_definition, Formatting.Indented);
           // return json;

        }

        private void treeView_select_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            syntaxEditor3.Text = GenerateExpression(e.Node);
            var obj = e.Node.Tag;
            if (obj != null)
                propertyGrid1.SelectedObject = obj is SecurityPolicy ? obj : (obj as SecurityPolicyCondition).Question;
            else
                propertyGrid1.SelectedObject = null;
        }

        private void toolStripButton_policy_definition_Click(object sender, EventArgs e)
        {
            syntaxEditor2.Text = GetPolicyJSON();

            var policy = GetRootPolicyFromNode(treeView_policies.SelectedNode);

            var final_query = policy.GetQuery();
            syntaxEditor3.Text = final_query;

            dataGridView1.DataSource = table.Connection.GetDataTable(final_query);
        }

        

        private List<SecurityPolicyQuestion> get_used_questions(SecurityPolicy policy)
        {
            var used_questions = new List<SecurityPolicyQuestion>();

            foreach(var condition in policy.Conditions)
            {
                used_questions.AddRange(get_condition_question(condition));
            }

            return used_questions.Distinct().ToList();
        }

        private List<SecurityPolicyQuestion> get_condition_question(SecurityPolicyCondition c)
        {
            var qs = new List<SecurityPolicyQuestion>();
            qs.Add(c.Question);
            foreach(var sc in c.SubConditions)
                qs.AddRange(get_condition_question(sc));
            return qs;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var o = propertyGrid1.SelectedObject;

            if(o is SecurityPolicyQuestion)
            {
                //refresh the policy tree nodes associated with this question
                foreach (TreeNode n in treeView_policies.Nodes)
                    RefreshPolicyNodes(n);

                listBox_questions.Items.IndexOf(o);
            }
            else if(o is SecurityPolicy)
            {
                foreach (TreeNode n in treeView_policies.Nodes)
                    if (n.Tag == o)
                    {
                        n.Text = o.ToString();
                        break;
                    }
            }
            else if(o is AdditionalComputedColumn)
            {
                foreach(TreeNode n in treeView_computed.Nodes)
                    if (n.Tag == o)
                    {
                        n.Text = o.ToString();
                        break;
                    }
            }

            
        }
        private void RefreshPolicyNodes(TreeNode n)
        {
            if(n.Tag != null)
                n.Text = n.Tag.ToString();

            foreach (TreeNode sn in n.Nodes)
            {
                RefreshPolicyNodes(sn);
            }
        }

        private void toolStripButton_added_new_question_Click(object sender, EventArgs e)
        {
            if (treeView_added.SelectedNode != null)
            {
                var col = treeView_added.SelectedNode.Tag as AdditionalColumn;
                var q = new SecurityPolicyQuestion
                {
                    SourceColumn = col,
                    ConstantValue = "1",
                    Operator = SecurityPolicyQuestion.SqlOperator.Equals,
                    Question = string.Format("Is {0} ..", col),
                    Table = this.table
                };

                table.SecurityPolicyQuestions.Add(q);

                listBox_questions.SelectedItem = q;
                propertyGrid1.SelectedObject = q;
            }
            
        }

        private void toolStripButton_added_new_computation_Click(object sender, EventArgs e)
        {
            if (treeView_added.SelectedNode != null)
            {
                var col = treeView_added.SelectedNode.Tag as AdditionalColumn;
                var cc = new AdditionalComputedColumn { Alias = "computed_column_" + table.AdditionalComputedColumns.Count, Expression = "[" + col.Alias + "]" };
                table.AdditionalComputedColumns.Add(cc);
                var cc_node = new TreeNode(col.Alias) { Tag = cc };
                treeView_computed.Nodes.Add(cc_node);
                treeView_computed.SelectedNode = cc_node;

                update_syntax();

                tabControl2.SelectedIndex = 1;

                propertyGrid1.SelectedObject = cc;
            }
        }

        private void treeView_added_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void toolStripButton_col_delete_Click(object sender, EventArgs e)
        {
            var n = treeView_added.SelectedNode;
            if (n != null)
            {
                if (n.Parent != null)
                {
                    treeView_added.Nodes.Remove(n.Parent);
                }
                else
                {
                    treeView_added.Nodes.Remove(n);
                    this.table.AdditionalRelatedColumns.Remove(n.Tag as AdditionalColumn);//
                }

                update_syntax();
            }
        }

        private void toolStripButton_new_computed_Click(object sender, EventArgs e)
        {
            var col = new AdditionalComputedColumn { Alias = "computed_column_" + computed_columns.Count, Expression = "1" };
            this.computed_columns.Add(col);
            treeView_computed.Nodes.Add(new TreeNode(col.Alias) { Tag = col });
            this.table.AdditionalComputedColumns.Add(col);//
            update_syntax();
        }

        private void toolStripButton_computed_delete_Click(object sender, EventArgs e)
        {
            if (treeView_computed.SelectedNode != null)
            {
                var col = treeView_computed.SelectedNode.Tag as AdditionalComputedColumn;
                this.computed_columns.Remove(col);
                treeView_computed.Nodes.Remove(treeView_computed.SelectedNode);
                this.table.AdditionalComputedColumns.Remove(col);//
                update_syntax();
            }
        }

        private void treeView_added_AfterCheck(object sender, TreeViewEventArgs e)
        {
            (e.Node.Tag as AdditionalColumn).Output = e.Node.Checked;
            update_syntax();
        }

        private void toolStripButton_new_action_Click(object sender, EventArgs e)
        {
            var action = new RowAction { Table = table, ActionName = "Some Action " + table.RowActions.Count };
            AddRowAction(action);
        }

        void AddRowAction(RowAction action)
        {
            table.RowActions.Add(action);
            var b = new RadioButton
            {

                Appearance = System.Windows.Forms.Appearance.Button,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = action.ActionName,
                Tag = action,
                Height = 25,
                Width = 120,
                Left = 10
            };

            b.Click += b_Click;

            flowLayoutPanel_actions.Controls.Add(b);
        }

        void b_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (sender as RadioButton).Tag;
        }

        private void toolStripButton_new_policy_Click(object sender, EventArgs e)
        {
            var p = new SecurityPolicy { ID = "New Policy " + DateTime.Now.ToString(), Table = this.table };
            table.AvailablePolicies.Add(p);

            treeView_policies.Nodes.Add(p.GetTree());
        }

        private void button_manage_workflow_Click(object sender, EventArgs e)
        {
            var form = new WorkflowForm() { Table = table };
            form.ShowDialog();
        }

        private void comboBox_select_policy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var cmb = (sender as ComboBox);
            //if (cmb.SelectedItem != null)
            //    table.SelectPolicy = (cmb.SelectedItem as SecurityPolicy).ID != null ? (cmb.SelectedItem as SecurityPolicy) : null;
        }

        private void comboBox_update_policy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var cmb = (sender as ComboBox);
            //if (cmb.SelectedItem != null)
            //    table.UpdatePolicy = (cmb.SelectedItem as SecurityPolicy).ID != null ? (cmb.SelectedItem as SecurityPolicy) : null;
        }

        private void comboBox_delete_policy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var cmb = (sender as ComboBox);
            //if (cmb.SelectedItem != null)
            //    table.DeletePolicy = (cmb.SelectedItem as SecurityPolicy).ID != null ? (cmb.SelectedItem as SecurityPolicy) : null;
        }


        private void toolStripButton_refresh_operation_policies_Click(object sender, EventArgs e)
        {
            refresh_policy_combos();
        }

        private void refresh_policy_combos()
        {
            var ps = new List<SecurityPolicy>();
            ps.Add(new SecurityPolicy());
            ps.AddRange(table.AvailablePolicies);

            //remember last value! TODO

            //combo
            comboBox_select_policy.DisplayMember = "ID";
            var selection = comboBox_select_policy.SelectedItem;
            comboBox_select_policy.DataSource = ps.ToList();
            comboBox_select_policy.SelectedItem = selection;

            selection = comboBox_update_policy.SelectedItem;
            comboBox_update_policy.DisplayMember = "ID";
            comboBox_update_policy.DataSource = ps.ToList();
            comboBox_update_policy.SelectedItem = selection;

            selection = comboBox_delete_policy.SelectedItem;
            comboBox_delete_policy.DisplayMember = "ID";
            comboBox_delete_policy.DataSource = ps.ToList();
            comboBox_delete_policy.SelectedItem = selection;
        }

        private void toolStripButton_new_update_command_Click(object sender, EventArgs e)
        {
            var update_command = new SQLUpdateCommand
            {
                Table = table,
                Name = string.Format("update_{0}_{1}_set_value", table.Schema.Name, table.Name)
            };

            table.UpdateCommands.Add(update_command);
        }

        private void listBox_commands_MouseClick(object sender, MouseEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as SQLStatement;
            if(item!=null)
            {
                propertyGrid1.SelectedObject = item;
                syntaxEditor1.Text = item.GetSyntax();
            }
        }

        private void toolStripButton_new_insert_command_Click(object sender, EventArgs e)
        {
            var command = new SQLInsertCommand
            {
                Table = table,
                Name = string.Format("insert_from_{0}_{1}_to_", table.Schema.Name, table.Name)
            };

            table.InsertCommands.Add(command);
        }

        private void toolStripButton_new_basic_procedure_Click(object sender, EventArgs e)
        {
            var proc = new BasicProcedure
            {
                Table = table,
                Name = string.Format("basic_proc_{0}_{1}_to_", table.Schema.Name, table.Name)
            };

            table.BasicProcedures.Add(proc);
        }

        private void listBox_basic_procedures_MouseClick(object sender, MouseEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as BasicProcedure;
            if (item != null)
            {
                propertyGrid1.SelectedObject = item;
                syntaxEditor1.Text = item.GetSyntax();
            }
        }

        private void toolStripButton_delete_update_command_Click(object sender, EventArgs e)
        {
            var cmd = listBox_update_commands.SelectedItem as SQLUpdateCommand;
            if(cmd != null)
            {
                table.UpdateCommands.Remove(cmd);
            }
        }

        public List<AdditionalColumnDefinition> computed_definition { get; set; }

        private void btn_preview_computed_Click(object sender, EventArgs e)
        {
            update_syntax(true);
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            apply();
            RefreshView();
        }
    }
}
