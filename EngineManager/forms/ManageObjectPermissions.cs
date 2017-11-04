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
    public partial class ManageObjectPermissions : Form
    {
        public ManageObjectPermissions()
        {
            InitializeComponent();
        }

        private DBSchemaObject db_object;
        public DBSchemaObject DBObject {
            get { return this.db_object; }
            set
            {
                this.db_object = value;
                if (!cmb_object.Items.Contains(this.db_object))
                {
                    cmb_object.Items.Add(this.db_object);
                }

                if (db_object is DBTable && (db_object as DBTable).SecureView != null)
                {
                    if (!cmb_object.Items.Contains((db_object as DBTable).SecureView))
                        cmb_object.Items.Add((db_object as DBTable).SecureView);
                    
                    cmb_object.SelectedItem = (db_object as DBTable).SecureView;
                }
                RefreshView();
                   
            }
        }

        private List<DBObjectPermission> object_permissions = new List<DBObjectPermission>();
        private List<DBColumnPermission> column_permissions = new List<DBColumnPermission>();

        public void RefreshView()
        {
            var last_selection = listBox_roles.SelectedIndex;
            var roles = DBObject.Schema.Database.Roles;
            listBox_roles.DataSource = roles;

            var object_permissions_sql = string.Format(@"
                declare @actual table (role_name nvarchar(100), [permission_name] nvarchar(100), [permission_state] nvarchar(100))

                insert @actual
	                SELECT    
		                [role_name] = roleprinc.[name] COLLATE SQL_Latin1_General_CP1_CI_AS,      
		                [permission_name] = perm.[permission_name] COLLATE SQL_Latin1_General_CP1_CI_AS,       
		                [permission_state] = perm.[state_desc] COLLATE SQL_Latin1_General_CP1_CI_AS
	                FROM    
		                sys.database_principals roleprinc
		                join sys.database_permissions perm ON perm.[grantee_principal_id] = roleprinc.[principal_id]
		                join sys.objects o on perm.major_id = o.object_id and perm.minor_id = 0
		                join sys.schemas s on o.schema_id = s.schema_id
	                where
		                s.name = N'{0}' and o.name = N'{1}'
				
                select * from 
                (
                select * from @actual

				union all

                select [role_name] = roleprinc.[name], p.permission_name, s.permission_state
	                from sys.database_principals roleprinc 
	                cross join (values('VIEW DEFINITION'), ('SELECT'), ('INSERT'), ('UPDATE'), ('DELETE')) p(permission_name)
	                cross join (values('REVOKE')) s(permission_state)
	                where roleprinc.type = 'R' and roleprinc.is_fixed_role = 0 
	                and roleprinc.name + p.permission_name not in(select role_name + permission_name from @actual)
                ) t order by [role_name], [permission_name]
                ", DBObject.Schema.Name, DBObject.Name);
            this.object_permissions = this.DBObject.Connection.GetDataTable(object_permissions_sql).AsEnumerable().Select(x =>
                new DBObjectPermission
                {
                    DBSchemaObject = this.DBObject,
                    Role = roles.Where(r => r.Name == x["role_name"].ToString()).FirstOrDefault(),
                    PermissionName = (DBObjectPermission.PermissionNames)Enum.Parse(typeof(DBObjectPermission.PermissionNames), x["permission_name"].ToString().Replace(" ", "_"), true),
                    Grant = x["permission_state"].ToString().Equals("GRANT"),
                    Deny = x["permission_state"].ToString().Equals("DENY"),
                    State = EngineManager.DBObject.DBObjectState.Intact
                }
            ).ToList();

            


            if (DBObject is IContainsColumns)
            {


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
                ) t order by [role_name], [column], [permission_name]
                ", DBObject.Schema.Name, DBObject.Name);
                this.column_permissions = this.DBObject.Connection.GetDataTable(column_permissions_sql).AsEnumerable().Select(x =>
                        new DBColumnPermission
                        {
                            DBColumn = (this.DBObject as IContainsColumns).Columns.Where(c => c.Name == x["column"].ToString()).FirstOrDefault(),
                            Role = roles.Where(r => r.Name == x["role_name"].ToString()).FirstOrDefault(),
                            PermissionName = (DBColumnPermission.PermissionNames)Enum.Parse(typeof(DBColumnPermission.PermissionNames), x["permission_name"].ToString().Replace(" ", "_"), true),
                            Grant = x["permission_state"].ToString().Equals("GRANT"),
                            Deny = x["permission_state"].ToString().Equals("DENY"),
                            State = EngineManager.DBObject.DBObjectState.Intact
                        }
                    ).ToList();


            }

            listBox_roles.SelectedIndex = last_selection;

            if(last_selection < 0)
            {
                for (int i = 0; i < listBox_roles.Items.Count; i++)
                {
                    var role = (listBox_roles.Items[i] as DBRole);
                    if (role.Name.Equals("Public User"))
                    {
                        listBox_roles.SelectedIndex = i;
                        break;
                    }
                }
            }

            refresh_grids();
        }

        private void refresh_grids()
        {
            //object permissions
            dataGridView_object.AutoGenerateColumns = false;
            dataGridView_object.DataSource = new BindingSource(object_permissions.Where(p => p.Role == listBox_roles.SelectedValue as DBRole).ToList(), null);

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
            refresh_grids();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            //execute the changes
            foreach (var c in DBObject.Connection.Project.Commands.Where(c => c.State == DBCommand.CommandState.New).ToList())
            	DBProjectManager.Execute(c);

        }

        private void cmb_object_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.db_object = cmb_object.SelectedItem as DBSchemaObject;
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
    }

    public class DBObjectPermission : DBObject
    {
        public DBRole Role { get; set; }
        public DBSchemaObject DBSchemaObject { get; set; }
        public enum PermissionNames { View_Definition, Select, Update, Delete, Insert }
        public PermissionNames PermissionName { get; set; }

        private bool Revoke { get { return this.grant == false && this.deny == false; } }

        private bool grant;
        public bool Grant
        {
            get { return grant; }
            set { grant = value; if (value) deny = !grant; if (this.State != DBObjectState.None) create_db_command(); }
        }

        private bool deny;
        public bool Deny
        {
            get { return deny; }
            set { deny = value; if (value) grant = !deny; if (this.State != DBObjectState.None) create_db_command(); }
        }
        
        private void create_db_command()
        {
            var cmd = new DBCommand
            {
                Sql = GetSQL(),
                Owner = DBSchemaObject,
                Description = "Permission Change"
            };

            DBSchemaObject.Connection.Project.Commands.Add(cmd);

            //if view definition on secure view
            if(PermissionName == PermissionNames.View_Definition && DBSchemaObject.Schema.Name == "secure")
            {
                var schema_name = this.DBSchemaObject.Name.Substring(0, this.DBSchemaObject.Name.IndexOf('_'));
                var table_name = this.DBSchemaObject.Name.Substring(this.DBSchemaObject.Name.IndexOf('_') + 1);

                var p = new DBObjectPermission
                {
                    DBSchemaObject = new DBTable
                              {
                                  Name = table_name,
                                  Schema = new DBSchema { Name = schema_name },
                                  Connection = this.DBSchemaObject.Connection
                              },
                    State = DBObjectState.New,
                    OnAlreadyExists = DBOnExists.PerformActionWithNoExistenceCheck,
                    PermissionName = PermissionNames.View_Definition,
                    Role = this.Role
                };

                p.Grant = this.Grant; //this will trigger command creation..
            }
        }

        public override string GetSQL()
        {
            var verb = this.Revoke ? "revoke" : this.grant ? "grant" : "deny";

            var sql = string.Format("{0} {1} on {2} to {3};", 
                    verb, 
                    this.PermissionName.ToString().Replace("_", " ").ToLower()
                    , this.DBSchemaObject.FullName
                    , this.Role.FullName);

            return sql;
        }
    }

    public class DBColumnPermission : DBObject
    {
        public DBRole Role { get; set; }
        public DBObject DBColumn { get; set; }
        public enum PermissionNames { Select, Update }
        public PermissionNames PermissionName { get; set; }

        private bool Revoke { get { return this.grant == false && this.deny == false; } }

        private bool grant;
        public bool Grant
        {
            get { return grant; }
            set { grant = value; if (value) deny = !grant; if (this.State != DBObjectState.None) create_db_command(); }
        }

        private bool deny;
        public bool Deny
        {
            get { return deny; }
            set { deny = value; if (value) grant = !deny; if (this.State != DBObjectState.None) create_db_command(); }
        }

        private void create_db_command()
        {
            var cmd = new DBCommand
            {
                Sql = GetSQL(),
                Owner = DBColumn,
                Description = "Column Permission Change"
            };

            DBColumn.Connection.Project.Commands.Add(cmd);
        }

        public string GetSQL()
        {
            var verb = this.Revoke ? "revoke" : this.grant ? "grant" : "deny";

            var sql = string.Format("{0} {1} on {2}({3}) to {4};",
                verb
                , this.PermissionName.ToString().ToLower()
                , this.DBColumn.Parent.FullName
                , this.DBColumn.FullName
                , this.Role.FullName
                );

            return sql;
        }
    }
}
