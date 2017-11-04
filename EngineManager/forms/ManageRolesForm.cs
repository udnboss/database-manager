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
    public partial class ManageRolesForm : Form
    {
        public ManageRolesForm()
        {
            InitializeComponent();
            
        }

        private DBRole role;
        public DBRole Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
                RefreshView();
            }
        }

        public void RefreshView()
        {
            //available roles
            var sql = @"select p.name 
                        from sys.database_principals p 
                        where p.type = 'R' and p.is_fixed_role = 0
                    order by
	                    p.name";

            var roles = Role.Connection.GetDataTable(sql).AsEnumerable().Select(x => new DBRole 
            { 
                Name = x["name"].ToString(), 
                Connection = Role.Connection
            }).ToList();

            listBox_roles.DataSource = roles;

            //available users
            sql = @"select p.name 
	                    , friendlyname = concat(isnull(u.name, p.name), ' (', p.name ,')')
                        from sys.database_principals p 
                            left join hr.Persons u on u.loginname = p.name
                        where p.type <> 'R'
                    order by
	                    p.name";

            var users = Role.Connection.GetDataTable(sql).AsEnumerable().Select(x => new DBUser
            {
                Name = x["name"].ToString(),
                Connection = Role.Connection,
                FriendlyName = x["friendlyname"].ToString()
            }).ToList();

            listBox_users.DataSource = users;

            //pre select
            if (Role != null)
            {
                for (int i = 0; i < listBox_roles.Items.Count; i++)
                    if ((listBox_roles.Items[i] as DBRole).Name == Role.Name)
                    {
                        listBox_roles.SelectedIndex = i;
                        break;
                    }
            }

        }
        public DBRole SelectedRole
        {
            get
            {
                return listBox_roles.SelectedValue as DBRole;
            }            
        }
        private void listBox_roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedRole.Members.Count == 0)
            {
                var sql = string.Format(@"select 
	                                        username = p.name
	                                        , rolename = pr.name 
	                                        , type = p.type
	                                        , friendlyname = concat(isnull(u.name, p.name), ' (', p.name ,')')

                                        from sys.database_principals p 
	                                        join sys.database_role_members r on p.principal_id = r.member_principal_id
		                                        join sys.database_principals pr on pr.principal_id = r.role_principal_id
			                                        left join hr.Persons u on u.loginname = p.name
                                        where 
	                                        pr.is_fixed_role = 0
	                                        and pr.name = '{0}'

                                        order by
	                                        p.name", SelectedRole.Name);

                var users = Role.Connection.GetDataTable(sql).AsEnumerable().Select(x => new DBUser
                {
                    Name = x["username"].ToString(),
                    Connection = Role.Connection,
                    FriendlyName = x["friendlyname"].ToString()
                }).ToList();

                

                SelectedRole.Members = new BindingList<DBUser>(users);
            }

            listBox_members.DataSource = SelectedRole.Members;
        }

        private void listBox_members_DragDrop(object sender, DragEventArgs e)
        {
            var user = e.Data.GetData(typeof(DBUser)) as DBUser;
            if (user != null)
            {
                if (SelectedRole.Members.Where(m => m.Name == user.Name).Count() == 0)
                {
                    var sql = string.Format("ALTER ROLE [{0}] ADD MEMBER [{1}]", SelectedRole.Name, user.Name);
                    var cmd = new DBCommand { Owner = SelectedRole, Description = "Add role member", Sql = sql };
                    SelectedRole.Connection.Project.Commands.Add(cmd);
                    SelectedRole.Members.Add(user);
                }
            }
        }

        private void listBox_users_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox_users.Items.Count == 0)
            {
                return;
            }

            int index = listBox_users.IndexFromPoint(e.X, e.Y);
            var s = listBox_users.Items[index];
            DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.All);
        }

        private void listBox_members_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listBox_members_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox_members.Items.Count == 0)
            {
                return;
            }

            int index = listBox_members.IndexFromPoint(e.X, e.Y);
            if (index > -1)
            {
                var s = listBox_members.Items[index];
                DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.All);
            }

        }

        private void listBox_users_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listBox_users_DragDrop(object sender, DragEventArgs e)
        {
            var user = e.Data.GetData(typeof(DBUser)) as DBUser;
            if (user != null)
            {
                if (SelectedRole.Members.Where(m => m.Name == user.Name).Count() == 1)
                {
                    var sql = string.Format("ALTER ROLE [{0}] DROP MEMBER [{1}]", SelectedRole.Name, user.Name);
                    var cmd = new DBCommand { Owner = SelectedRole, Description = "Drop role member", Sql = sql };
                    SelectedRole.Connection.Project.Commands.Add(cmd);
                    SelectedRole.Members.Remove(user);
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Role.Connection.Project.Commands.Clear();               
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            foreach (var c in Role.Connection.Project.Commands)
                DBProjectManager.Execute(c);
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            foreach (var c in Role.Connection.Project.Commands)
                DBProjectManager.Execute(c);
        }


    }
}
