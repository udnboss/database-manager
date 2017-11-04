using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineManager
{
    public partial class ManageRolePermissions : Form
    {
        public ManageRolePermissions()
        {
            InitializeComponent();
        }

        public DBRole Role { get { return listBox_roles.SelectedItem as DBRole; } }

        public DBSchema Schema { get { return cmb_schemas.SelectedItem as DBSchema; } }

        private DBRole initial_role;
        public DBRole InitialRole { get { return initial_role; } set { initial_role = value; } }
        public DBSchemaObject DBObject {
            get { return dataGridView_object.SelectedCells.Count > 0 ? (dataGridView_object.Rows[dataGridView_object.SelectedCells[0].RowIndex].DataBoundItem as DBObjectPermissionDescription).Object as DBSchemaObject : null; }
        }

        private List<DBObjectPermissionDescription> object_permissions = new List<DBObjectPermissionDescription>();
        private List<DBColumnPermission> column_permissions = new List<DBColumnPermission>();

        public void RefreshView()
        {
            var last_selection = listBox_roles.SelectedIndex;


            var roles = InitialRole.Database.Roles;
            listBox_roles.DataSource = roles;

            var last_schema_index = cmb_schemas.SelectedIndex;
            cmb_schemas.DataSource = initial_role.Database.Schemas;
            
            var last_table_index = dataGridView_object.SelectedRows.Count > 0 ? dataGridView_object.SelectedRows[0].Index : -1;
            
            //dataGridView_object.DataSource = initial_role.Database.Tables; //todo views...

            if (Role == null) return;

            var object_permissions_sql = string.Format(@"
--store forms
declare @objects table(original_schema nvarchar(100), original_table nvarchar(100), table_id nvarchar(100), 
	original_type nvarchar(100), [use_what_schema] nvarchar(100), [use_what_name] nvarchar(100), use_what nvarchar(100), use_parameters nvarchar(100))

insert into @objects
	select
		t.[schema],
		t.[table],
		t.table_id,
		[original_type] = t.type,
		[use_what_schema] = case when t.procedure_id is not null then 'procedures'
						  when t.function_id is not null then 'functions'
						  when t.view_id is not null then 'secure'
						  else t.[schema] end,
		[use_what_name] = case when t.procedure_id is not null then t.[procedure]
						  when t.function_id is not null then t.[function]
						  when t.view_id is not null then t.[view]
						  else t.[table] end,
		[use_what] = isnull(t.procedure_id, isnull(t.function_id, isnull(t.view_id,  concat('[', t.[schema], '].[', t.[table], ']')))),
		[use_type] = case when t.procedure_id is not null then 'procedure'
						  when t.function_id is not null then 'function'
						  when t.view_id is not null then 'view'
						  else t.[type] end
		--				  ,	[use_parameters] = isnull(t.[procedure_parameters], t.[function_parameters])
	from
	(
		select 
			[type] = case when t.type = 'U' then 'table' else 'view' end,
			[schema] = s.name,
			[table] = t.name,
			[table_id] = concat(s.name, '.', t.name),
			[view_id] = '[secure].' + '[' + v.name + ']',
			[function_id] =   '[functions].[' + f.name + ']',
			[function_parameters] = (select [@name] = pr.name, [@type] = t.name, [@length] = case when t.name in('nchar', 'nvarchar') then pr.max_length / 2 else pr.max_length end
								from sys.parameters pr
								join sys.types t on pr.system_type_id = t.system_type_id and t.system_type_id = t.user_type_id 
								where pr.object_id = f.object_id for xml path('p'), root('parameters'), type),
			[procedure_id] = '[procedures].[' + p.name + ']',
			[procedure_parameters] = (select [@name] = pr.name, [@type] = t.name, [@length] = case when t.name in('nchar', 'nvarchar') then pr.max_length / 2 else pr.max_length end
								from sys.parameters pr
								join sys.types t on pr.system_type_id = t.system_type_id and t.system_type_id = t.user_type_id 
								where pr.object_id = p.object_id for xml path('p'), root('parameters'), type)
			,
			[view] = v.name,
			[function] = f.name,
			[procedure] = p.name

		from 
			sys.schemas s
			join (select o.object_id, o.name, o.schema_id, o.type
					 from sys.objects o where o.type in('U', 'V', 'IF') and schema_name(o.schema_id) not in('secure')) t on t.schema_id = s.schema_id
			left join sys.views v on schema_name(v.schema_id) = 'secure' and v.name = concat(s.name, '_', t.name)
			left join sys.objects f on schema_name(f.schema_id) = 'functions' and f.name = concat(s.name, '_', t.name)
			left join sys.procedures p on schema_name(p.schema_id) = 'procedures' and p.name = concat(s.name, '_', t.name)
		where (t.name <> 'sysdiagrams' or t.name is null)
	) t
	order by [table_id]

    declare @actual table (role_name nvarchar(100), [object_name] nvarchar(100),
	original_schema nvarchar(100),
				original_table nvarchar(100),
				original_type nvarchar(100),
				 [secure_object_name] nvarchar(100), [schema] nvarchar(100), [table] nvarchar(100),[permission_name] nvarchar(100), [permission_state] nvarchar(100))

    insert @actual
	    SELECT    
		    [role_name] = roleprinc.[name] COLLATE SQL_Latin1_General_CP1_CI_AS,     
			[object_name] = o.use_what,
			o.original_schema,
			o.original_table,
			o.original_type,
			[secure_object_name] = o.use_what, 
		    [schema] = o.use_what_schema,
			[table] = o.use_what_name, 
			[permission_name] = perm.[permission_name] COLLATE SQL_Latin1_General_CP1_CI_AS,       
		    [permission_state] = perm.[state_desc] COLLATE SQL_Latin1_General_CP1_CI_AS
	    FROM    
		    sys.database_principals roleprinc
		    join sys.database_permissions perm ON perm.[grantee_principal_id] = roleprinc.[principal_id]
		    join @objects o on perm.major_id = object_id(o.use_what) and perm.minor_id = 0
	    where		                
			roleprinc.name = '{0}' --specific role

    select * from 
    (
		select * from @actual

		union all

		select 
				[role_name] = roleprinc.[name], 
				[object_name] = o.use_what, 
				o.original_schema,
				o.original_table,
				o.original_type,
				[secure_object_name] = o.use_what, 
				[schema] = o.use_what_schema,
				[table] = o.use_what_name, 
				p.permission_name, 
				s.permission_state
			from sys.database_principals roleprinc 
				cross join @objects o
				cross join (values('VIEW DEFINITION'), ('SELECT'), ('INSERT'), ('UPDATE'), ('DELETE')) p(permission_name)
				cross join (values('REVOKE')) s(permission_state)
			where 

				roleprinc.type = 'R' 
				and roleprinc.name = '{0}'
				and roleprinc.is_fixed_role = 0 
				and roleprinc.name + use_what + p.permission_name not in(select role_name + object_name + permission_name from @actual)
						
    ) t 
	pivot
	(
		max(permission_state)
		for [permission_name] in([VIEW DEFINITION], [SELECT], [INSERT], [UPDATE], [DELETE])
	) p
	order by [role_name]
                ", Role.Name);

            this.object_permissions = this.InitialRole.Connection.GetDataTable(object_permissions_sql).AsEnumerable().Select(x =>
                new DBObjectPermissionDescription
                {
                    original_schema = x["original_schema"].ToString(),
                    Object = x["original_type"].Equals("table") ? new DBTable { 
                        Name = x["table"].ToString(), 
					Schema = initial_role.Database.Schemas.FirstOrDefault(s => s.Name == x["schema"].ToString()),
                        Connection = initial_role.Connection
                    } as DBSchemaObject :
                    new DBView { 
                        Name = x["table"].ToString(), 
					Schema = initial_role.Database.Schemas.FirstOrDefault(s => s.Name == x["schema"].ToString()),
                        Connection = initial_role.Connection
                    } as DBSchemaObject
                    ,
                    Role = Role,
                    ViewDefinition = (DBObjectPermissionDescription.Permission)Enum.Parse(typeof(DBObjectPermissionDescription.Permission), x["VIEW DEFINITION"].ToString()),
                    Select = (DBObjectPermissionDescription.Permission)Enum.Parse(typeof(DBObjectPermissionDescription.Permission), x["SELECT"].ToString()),
                    Insert = (DBObjectPermissionDescription.Permission)Enum.Parse(typeof(DBObjectPermissionDescription.Permission), x["INSERT"].ToString()),
                    Update = (DBObjectPermissionDescription.Permission)Enum.Parse(typeof(DBObjectPermissionDescription.Permission), x["UPDATE"].ToString()),
                    Delete = (DBObjectPermissionDescription.Permission)Enum.Parse(typeof(DBObjectPermissionDescription.Permission), x["DELETE"].ToString())
                }
            ).ToList();

            foreach(var op in this.object_permissions)
            {
                op.view_definition = new DBObjectPermission
                {
                    DBSchemaObject = op.Object,
                    Role = op.Role,
                    PermissionName = DBObjectPermission.PermissionNames.View_Definition,
                    Grant = op.ViewDefinition == DBObjectPermissionDescription.Permission.GRANT,
                    Deny = op.ViewDefinition == DBObjectPermissionDescription.Permission.DENY,
                    State = EngineManager.DBObject.DBObjectState.Intact
                };

                op.select = new DBObjectPermission
                {
                    DBSchemaObject = op.Object,
                    Role = op.Role,
                    PermissionName = DBObjectPermission.PermissionNames.Select,
                    Grant = op.Select == DBObjectPermissionDescription.Permission.GRANT,
                    Deny = op.Select == DBObjectPermissionDescription.Permission.DENY,
                    State = EngineManager.DBObject.DBObjectState.Intact
                };

                op.insert = new DBObjectPermission
                {
                    DBSchemaObject = op.Object,
                    Role = op.Role,
                    PermissionName = DBObjectPermission.PermissionNames.Insert,
                    Grant = op.Insert == DBObjectPermissionDescription.Permission.GRANT,
                    Deny = op.Insert == DBObjectPermissionDescription.Permission.DENY,
                    State = EngineManager.DBObject.DBObjectState.Intact
                };

                op.update = new DBObjectPermission
                {
                    DBSchemaObject = op.Object,
                    Role = op.Role,
                    PermissionName = DBObjectPermission.PermissionNames.Update,
                    Grant = op.Update == DBObjectPermissionDescription.Permission.GRANT,
                    Deny = op.Update == DBObjectPermissionDescription.Permission.DENY,
                    State = EngineManager.DBObject.DBObjectState.Intact
                };

                op.delete = new DBObjectPermission
                {
                    DBSchemaObject = op.Object,
                    Role = op.Role,
                    PermissionName = DBObjectPermission.PermissionNames.Delete,
                    Grant = op.Delete == DBObjectPermissionDescription.Permission.GRANT,
                    Deny = op.Delete == DBObjectPermissionDescription.Permission.DENY,
                    State = EngineManager.DBObject.DBObjectState.Intact
                };
            }


            //object permissions
            dataGridView_object.AutoGenerateColumns = false;
            dataGridView_object.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_object.RowTemplate.Height = 25;
           // dataGridView_object.AutoResizeColumns();

            dataGridView_object.Columns.Clear();
            dataGridView_object.Columns.AddRange(
                new DataGridViewColumn[] { 
                    new DataGridViewTextBoxColumn  { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "Object", DataPropertyName = "Object" },
                    new DataGridViewComboBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "View Definition", DataPropertyName = "ViewDefinition", DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ValueType = typeof(DBObjectPermissionDescription.Permission), DataSource = Enum.GetValues(typeof(DBObjectPermissionDescription.Permission)) },
                    new DataGridViewComboBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "Select", DataPropertyName = "Select", DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ValueType = typeof(DBObjectPermissionDescription.Permission), DataSource = Enum.GetValues(typeof(DBObjectPermissionDescription.Permission)) },
                    new DataGridViewComboBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "Insert", DataPropertyName = "Insert", DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ValueType = typeof(DBObjectPermissionDescription.Permission), DataSource = Enum.GetValues(typeof(DBObjectPermissionDescription.Permission)) },
                    new DataGridViewComboBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "Update", DataPropertyName = "Update", DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ValueType = typeof(DBObjectPermissionDescription.Permission), DataSource = Enum.GetValues(typeof(DBObjectPermissionDescription.Permission)) },
                    new DataGridViewComboBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, Width = 200, HeaderText = "Delete", DataPropertyName = "Delete", DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ValueType = typeof(DBObjectPermissionDescription.Permission), DataSource = Enum.GetValues(typeof(DBObjectPermissionDescription.Permission)) },
                }
                );
            if(Schema != null)
            dataGridView_object.DataSource = new BindingSource(object_permissions.Where(x => x.original_schema == Schema.Name ).ToList(), null);

           
            

            listBox_roles.SelectedIndex = last_selection;
            cmb_schemas.SelectedIndex = last_schema_index;
            
            //default role selection..
            if(last_selection < 0)
            {
                listBox_roles.SelectedItem = initial_role;
            }
            clear_grids();

        }

        private void refresh_grids()
        {
            
            if (DBObject != null)
            {
                //get selected record
                DBObjectManager.Refresh(DBObject);

                var column_permissions_sql = string.Format(@"
                declare @actual table (role_name nvarchar(100), [column] nvarchar(100), [permission_name] nvarchar(100), [permission_state] nvarchar(100))

                insert @actual
	                SELECT    
		                [role_name] = roleprinc.[name] COLLATE SQL_Latin1_General_CP1_CI_AS,
		                [column] = c.name,
		                [permission_name] = perm.[permission_name] COLLATE SQL_Latin1_General_CP1_CI_AS,       
		                [permission_state] = perm.[state_desc] COLLATE SQL_Latin1_General_CP1_CI_AS
	                FROM    
		                sys.database_principals roleprinc
		                join sys.database_permissions perm ON perm.[grantee_principal_id] = roleprinc.[principal_id]
		                join sys.objects o on perm.major_id = o.object_id 
		                join sys.schemas s on o.schema_id = s.schema_id
		                join sys.columns c on c.object_id = o.object_id and perm.minor_id = c.column_id
	                where
		                roleprinc.principal_id <> 0 and s.name = N'{0}' and o.name = N'{1}'
                        and roleprinc.name = '{2}'
                
                select * from
                (
				select * from @actual

				union all

                select [role_name] = roleprinc.[name], [column] = c.name, p.permission_name, s.permission_state
	                from sys.database_principals roleprinc 
	                cross join (values('SELECT'),('UPDATE')) p(permission_name)
	                cross join (values('REVOKE')) s(permission_state)
	                cross join (select c.name from sys.columns c where object_schema_name(c.object_id) = N'{0}' and object_name(c.object_id) = N'{1}') c
	                where roleprinc.type = 'R' and roleprinc.is_fixed_role = 0 and roleprinc.principal_id <> 0
	                and roleprinc.name + c.name + p.permission_name not in(select role_name + [column] + permission_name from @actual)
                    and roleprinc.name = '{2}'
                ) t order by [role_name], [column], [permission_name]
                ", DBObject.Schema.Name, DBObject.Name, Role.Name);
                this.column_permissions = this.DBObject.Connection.GetDataTable(column_permissions_sql).AsEnumerable().Select(x =>
                        new DBColumnPermission
                        {
                            DBColumn = (this.DBObject as IContainsColumns).Columns.Where(c => c.Name == x["column"].ToString()).FirstOrDefault(),
                            Role = Role, //roles.Where(r => r.Name == x["role_name"].ToString()).FirstOrDefault(),
                            PermissionName = (DBColumnPermission.PermissionNames)Enum.Parse(typeof(DBColumnPermission.PermissionNames), x["permission_name"].ToString().Replace(" ", "_"), true),
                            Grant = x["permission_state"].ToString().Equals("GRANT"),
                            Deny = x["permission_state"].ToString().Equals("DENY"),
                            State = EngineManager.DBObject.DBObjectState.Intact
                        }
                    ).ToList();


            }


            //columns select permissions
            dataGridView_columns_select.AutoGenerateColumns = false;
            dataGridView_columns_select.DataSource = new BindingSource(column_permissions.Where(p =>
                p.Role == listBox_roles.SelectedValue as DBRole
                && p.PermissionName == DBColumnPermission.PermissionNames.Select).ToList(), null);

            //columns update permissions
            dataGridView_columns_update.AutoGenerateColumns = false;
            dataGridView_columns_update.DataSource = new BindingSource(column_permissions.Where(p =>
                p.Role == listBox_roles.SelectedValue as DBRole
                && p.PermissionName == DBColumnPermission.PermissionNames.Update).ToList(), null);
        }

        private void listBox_roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
           // refresh_grids();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            //execute the changes
            foreach (var c in InitialRole.Connection.Project.Commands.Where(c => c.State == DBCommand.CommandState.New).ToList())
               DBProjectManager.Execute(c);

        }

        private void cmb_object_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            foreach (var c in DBObject.Connection.Project.Commands.Where(c=>c.State == DBCommand.CommandState.New).ToList())
                DBProjectManager.Execute(c);
        }

        private void listBox_roles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBox_roles.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                listBox_roles.SelectedIndex = index;
                mnu_role.Show(Cursor.Position);
                mnu_role.Visible = true;
            }
            else
            {
                mnu_role.Visible = false;
            }
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var role = listBox_roles.SelectedValue as DBRole;
            var form = new ManageRolesForm();
            form.Role = role;
            form.ShowDialog();
        }

        private void dataGridView_object_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView_object.SelectedCells.Count > 0)
            {
                clear_grids();
            }
        }

        private void clear_grids()
        {
            dataGridView_columns_select.DataSource = null;
            dataGridView_columns_update.DataSource = null;
        }

        private void btn_refresh_col_permissions_Click(object sender, EventArgs e)
        {
            refresh_grids();
        }
    }

    public class DBObjectPermissionDescription
    {

        public string original_schema { get; set; }
        
        public DBRole Role { get; set; }
        public DBSchemaObject Object { get; set; }

        public enum Permission { REVOKE, GRANT, DENY }


        private Permission vd;
        public Permission ViewDefinition { get { return vd; } set { vd = value; if (view_definition != null) view_definition.Grant = value == Permission.GRANT; } }

        private Permission sel;
        public Permission Select { get { return sel; } set { sel = value; if (select != null) select.Grant = value == Permission.GRANT; } }

        private Permission ins;
        public Permission Insert { get { return ins; } set { ins = value; if (insert != null) insert.Grant = value == Permission.GRANT; } }

        private Permission upd;
        public Permission Update { get { return upd; } set { upd = value; if (update != null) update.Grant = value == Permission.GRANT; } }

        private Permission del;
        public Permission Delete { get { return del; } set { del = value; if (delete != null) delete.Grant = value == Permission.GRANT; } }

        public DBObjectPermission view_definition { get; set; }
        public DBObjectPermission select { get; set; }
        public DBObjectPermission insert { get; set; }
        public DBObjectPermission update { get; set; }
        public DBObjectPermission delete { get; set; }

    }
}
