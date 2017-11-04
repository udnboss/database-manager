using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EngineManager
{
    public partial class Migration : Form
    {
        private BindingList<MigrationProfile> profiles = new BindingList<MigrationProfile>();
        public Migration()
        {
            InitializeComponent();
            syntaxEditor1.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);
            syntaxEditor2.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);

            //load profiles
            var profiles_read = Util.ReadData(Util.AppData.Migration_Profiles) as BindingList<MigrationProfile>;
            if (profiles_read != null)
                this.profiles = profiles_read;

            cmb_profiles.DataSource = new BindingSource(this.profiles, null);
            //cmb_profiles.DisplayMember = "Name";


        }
        private DBConnection connection;
        public DBConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; RefreshView(); }
        }

        DB current_database;
        public DB CurrentDatabase { get { return current_database; } }

        private void RefreshView()
        {
            if(this.connection == null)
            {
                return;
            }
            current_database = this.connection.Databases.Where(d => d.Name == this.connection.InitialDatabase).FirstOrDefault(); 

            //cmb_data_sources.Items.AddRange(Enum.GetValues(typeof(DBTable.MigrationDataSources)).Cast<object>().ToList().ToArray());
            cmb_structure_sources.Items.AddRange(Enum.GetValues(typeof(DBTable.MigrationDataSources)).Cast<object>().ToList().ToArray());

            var tables_data = this.connection.GetDataTable(@"
                select 
	                [schema]
	                , [table]
	                , migration_data_source = isnull(p.migration_data_source, 'Source')
	                , migration_data_query = p.migration_data_query
	                , migration_structure_source = isnull(p.migration_structure_source, 'Source')
	                , migration_structure_query = p.migration_structure_query
                from
                (
	                select 
		                [schema] = s.name
		                , [table] = t.name
		                , ep.name
		                , value = cast(ep.value as nvarchar(max))
	                from 
		                sys.extended_properties ep 
		                right join sys.tables t on t.object_id = ep.major_id and ep.minor_id = 0
		                join sys.schemas s on t.schema_id = s.schema_id
	                where
		                ep.name in('migration_data_source', 'migration_data_query', 'migration_structure_source', 'migration_structure_query')
		                or ep.name is null
                ) d
                pivot
                (
	                max(value)
	                for name in([migration_data_source], [migration_data_query], [migration_structure_source], [migration_structure_query])
                ) p

                order by [schema], [table]            
            ");

            var servers_data = this.connection.GetDataTable(@"
                select 
	                [server] = s.name 
                from 
	                sys.servers s join sys.linked_logins ll on ll.server_id = s.server_id
                where
	                ll.local_principal_id = 0
                ");

            var pre_migration_scripts = Util.ReadData(Util.AppData.Migration_Database_Pre_Scripts) as List<DBMigrationScript>;
            if (pre_migration_scripts != null)
                CurrentDatabase.PreMigrationScripts = pre_migration_scripts;
            
            var post_migration_scripts = Util.ReadData(Util.AppData.Migration_Database_Post_Scripts) as List<DBMigrationScript>;
            if (post_migration_scripts != null)
                CurrentDatabase.PostMigrationScripts = post_migration_scripts;

            var production_server = Util.ReadData(Util.AppData.Migration_Database_Production_Server) as MigrationServerDetail;
            if (production_server != null) 
                CurrentDatabase.ProductionServer = production_server;

            var test_server = Util.ReadData(Util.AppData.Migration_Database_Test_Server) as MigrationServerDetail;
            if (test_server != null)
                CurrentDatabase.TestServer = test_server;

            var development_server = Util.ReadData(Util.AppData.Migration_Database_Development_Server) as MigrationServerDetail;
            if (development_server != null)
                CurrentDatabase.DevelopmentServer = development_server;


            cmb_target_server.DataSource = new List<string> { "Test", "Production" };

            //var snapshot = new DBSnapshot();
            //snapshot.get_db_basic_data(this.connection);
            //var all_objects = snapshot.db_objects;

            var tables_with_migration_spec = tables_data.AsEnumerable().Select(row => new DBTable
            {
                Name = row["table"].ToString(),
                Schema = new DBSchema { Name = row["schema"].ToString() },
                MigrationDataSource = !string.IsNullOrEmpty(row["migration_data_source"].ToString()) ? (DBTable.MigrationDataSources)Enum.Parse(typeof(DBTable.MigrationDataSources), row["migration_data_source"].ToString()) : DBTable.MigrationDataSources.Development,
                //MigrationStructureSource = !string.IsNullOrEmpty(row["migration_structure_source"].ToString()) ? (DBTable.MigrationDataSources)Enum.Parse(typeof(DBTable.MigrationDataSources), row["migration_structure_source"].ToString()) : DBTable.MigrationDataSources.Development,              
                MigrationDataQuery = row["migration_data_query"].ToString(),
                //MigrationStructureQuery = row["migration_structure_query"].ToString(),
                State = DBObject.DBObjectState.Intact,
                Connection = this.connection
            }).ToList();


            //update them in the snapshot -- todo

            //
            this.MigrationTables = new BindingList<DBTable>(tables_with_migration_spec);

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[]{
                new DataGridViewTextBoxColumn { HeaderText = "Schema", DataPropertyName = "Schema", ReadOnly = true},
                new DataGridViewTextBoxColumn { HeaderText = "Table", DataPropertyName = "Name", ReadOnly = true},
                new DataGridViewComboBoxColumn { FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox, HeaderText = "Data Source", DataPropertyName = "MigrationDataSource", DataSource = Enum.GetValues(typeof(DBTable.MigrationDataSources)) },
                new DataGridViewTextBoxColumn { HeaderText = "Data Query", DataPropertyName = "MigrationDataQuery"},
                //new DataGridViewComboBoxColumn { FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox, HeaderText = "Structure Source", DataPropertyName = "MigrationStructureSource", DataSource = Enum.GetValues(typeof(DBTable.MigrationDataSources)) },
                //new DataGridViewTextBoxColumn { HeaderText = "Structure Query", DataPropertyName = "MigrationStructureQuery"}
            });

            dataGridView1.DataSource = new BindingSource(tables_with_migration_spec, null);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            propertyGrid1.SelectedObject = this.CurrentDatabase;

        }

        public DBConnection target_connection { get; set; }

        private void cmb_structure_sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var data = (dataGridView1.DataSource as BindingSource);
            //foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            //    (data[row.Index] as DBTable).MigrationStructureSource = (DBTable.MigrationDataSources)cmb_structure_sources.SelectedItem;
        }

        private void cmb_data_sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var data = (dataGridView1.DataSource as BindingSource);
            //foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            //    (data[row.Index] as DBTable).MigrationDataSource = (DBTable.MigrationDataSources)cmb_data_sources.SelectedItem;
        }

        public BindingList<DBTable> MigrationTables { get; set; }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Util.SaveData(Util.AppData.Migration_Profiles, this.profiles);
            //Util.SaveData(Util.AppData.Connections, this.connection.Project.Connections); --need simple connection definition

            //Util.SaveData(Util.AppData.Migration_Database_Development_Server, this.current_database.DevelopmentServer);
            //Util.SaveData(Util.AppData.Migration_Database_Test_Server, this.current_database.TestServer);
            //Util.SaveData(Util.AppData.Migration_Database_Production_Server, this.current_database.ProductionServer);

            //Util.SaveData(Util.AppData.Migration_Database_Pre_Scripts, this.current_database.PreMigrationScripts);
            //Util.SaveData(Util.AppData.Migration_Database_Post_Scripts, this.current_database.PostMigrationScripts);

            //store the list of migration
            //var pre_ep = new DBExtendedProperty {Action = DB.DBAction.Create, Owner = CurrentDatabase, Name = "pre_migration_scripts", Value = JsonConvert.SerializeObject(this.CurrentDatabase.PreMigrationScripts, Newtonsoft.Json.Formatting.Indented) };
            //var post_ep = new DBExtendedProperty {Action = DB.DBAction.Create, Owner = CurrentDatabase, Name = "post_migration_scripts", Value = JsonConvert.SerializeObject(this.CurrentDatabase.PostMigrationScripts, Newtonsoft.Json.Formatting.Indented) };

            //var production_server = new DBExtendedProperty {Action = DB.DBAction.Create, Owner = CurrentDatabase, Name = "production_server", Value = JsonConvert.SerializeObject(this.CurrentDatabase.ProductionServer) };
            //var test_server = new DBExtendedProperty { Action = DB.DBAction.Create, Owner = CurrentDatabase, Name = "test_server", Value = JsonConvert.SerializeObject(this.CurrentDatabase.TestServer) };
            //var development_server = new DBExtendedProperty { Action = DB.DBAction.Create, Owner = CurrentDatabase, Name = "development_server", Value = JsonConvert.SerializeObject(this.CurrentDatabase.DevelopmentServer) };            

            //var cmd_pre = pre_ep.GetSQL();
            //var cmd_post = pre_ep.GetSQL();

            //var cmd_prod = production_server.GetSQL();
            //var cmd_test = test_server.GetSQL();
            //var cmd_dev = test_server.GetSQL();

            //var c = new DBCommand { Sql = cmd_pre, Owner = CurrentDatabase, Description = "pre_migration_scripts" };
            //var c2 = new DBCommand { Sql = cmd_post, Owner = CurrentDatabase, Description = "post_migration_scripts" };
            //var c3 = new DBCommand { Sql = cmd_prod, Owner = CurrentDatabase, Description = "production_server" };
            //var c4 = new DBCommand { Sql = cmd_test, Owner = CurrentDatabase, Description = "test_server" };
            //var c5 = new DBCommand { Sql = cmd_dev, Owner = CurrentDatabase, Description = "development_server" };

            //c.Execute();
            //c2.Execute();
            //c3.Execute();
            //c4.Execute();
            //c5.Execute();

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            var pre_script = string.Join("\r\n\r\n-------------------------------------------------------\r\n\r\n",
                    CurrentDatabase.PreMigrationScripts.Select(x => string.Format("/*** {0} ***/\r\n{1}\r\n/***********/\r\n--{2}\r\n", x.Title, x.SQLScript, x.SystemCommand)).ToList());

            pre_script = prepare_script(pre_script);

            syntaxEditor1.Text = pre_script;

            var post_script = string.Join("\r\n\r\n-------------------------------------------------------\r\n\r\n",
                    CurrentDatabase.PostMigrationScripts.Select(x => string.Format("/*** {0} ***/\r\n{1}\r\n/***********/\r\n--{2}\r\n", x.Title, x.SQLScript, x.SystemCommand)).ToList());

            post_script = prepare_script(post_script);

            syntaxEditor2.Text = post_script;

            foreach(var table in this.MigrationTables)
            {
                var status = new MigrationStatus
                {
                    Schema = table.Schema.Name,
                    Table = table.Name,
                    MigrationRule = table.MigrationDataSource.ToString(),
                    Status = "In Progress"
                };

                switch(table.MigrationDataSource)
                {
                    case DBTable.MigrationDataSources.NoMigration:
                        //drop all constraints referencing it
                        var cmd_drop_constraints = string.Format(@"
DECLARE @SchemaName nvarchar(100),
		@TableName nvarchar(100),

declare @cmd_drop_constraints nvarchar(max);
set @cmd_drop_constraints = null
select @cmd_drop_constraints = coalesce(@cmd_drop_constraints + char(13), '') 
	+ concat('alter table [', s.name, '].[', t.name, '] drop constraint [', k.name, '];')
from 
	sys.foreign_keys k 
		join sys.tables t on k.parent_object_id = t.object_id
		join sys.schemas s on s.schema_id = t.schema_id

		join sys.tables pt on k.referenced_object_id = pt.object_id
		join sys.schemas ps on ps.schema_id = pt.schema_id
	where ps.name = @SchemaName and pt.name = @TableName
", table.Schema.Name, table.Name);
                        //drop it

                        break;
                    case DBTable.MigrationDataSources.Development:
                        //do nothing
                        break;
                    case DBTable.MigrationDataSources.Production:
                        //clear it

                        //find common columns

                        //enable id insert if any

                        //copy data

                        //disable id insert if any
                        break;
                    case DBTable.MigrationDataSources.Clear:
                        //clear it

                        break;
                }
            }
        }

        private string prepare_script(string s)
        {
            var script = s;

            script = script.Replace("{development_database}", CurrentDatabase.DevelopmentServer.Database);
            script = script.Replace("{development_backup_file}", CurrentDatabase.DevelopmentServer.BackupFolder + @"\" + CurrentDatabase.DevelopmentServer.Database + ".bak");
            script = script.Replace("{development_backup_shared_folder}", CurrentDatabase.DevelopmentServer.BackupSharedFolder);

            script = script.Replace("{migration_database}", CurrentDatabase.DevelopmentServer.Database + "_migration");
            script = script.Replace("{migration_backup_file}", CurrentDatabase.DevelopmentServer.BackupFolder + @"\" + CurrentDatabase.DevelopmentServer.Database + "_migration.bak");

            script = script.Replace("{test_server}", CurrentDatabase.TestServer.Server);
            script = script.Replace("{test_database}", CurrentDatabase.TestServer.Database);
            script = script.Replace("{test_backup_file}", CurrentDatabase.TestServer.BackupFolder + @"\" + CurrentDatabase.TestServer.Database + ".bak");
            script = script.Replace("{test_backup_shared_folder}", CurrentDatabase.TestServer.BackupSharedFolder);
            script = script.Replace("{test_user}", CurrentDatabase.TestServer.Username);
            script = script.Replace("{test_password}", CurrentDatabase.TestServer.Password);

            script = script.Replace("{production_server}", CurrentDatabase.ProductionServer.Server);
            script = script.Replace("{production_database}", CurrentDatabase.ProductionServer.Database);
            script = script.Replace("{production_backup_file}", CurrentDatabase.ProductionServer.BackupFolder + @"\" + CurrentDatabase.ProductionServer.Database + ".bak");
            script = script.Replace("{production_backup_shared_folder}", CurrentDatabase.ProductionServer.BackupSharedFolder);
            script = script.Replace("{production_user}", CurrentDatabase.ProductionServer.Username);
            script = script.Replace("{production_password}", CurrentDatabase.ProductionServer.Password);

            return script;
        }

        private void btn_new_profile_Click(object sender, EventArgs e)
        {
            this.profiles.Add(new MigrationProfile { Name = "New Profile" });
            //cmb_profiles.DataSource = this.profiles;
            //cmb_profiles.DisplayMember = "Name";
            //cmb_profiles.Update();
            
        }

        private void btn_delete_profile_Click(object sender, EventArgs e)
        {
            this.profiles.Remove(cmb_profiles.SelectedItem as MigrationProfile);
        }

        private void cmb_profiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = cmb_profiles.SelectedItem;
        }
    }

    public class MigrationProfile
    {
        [Description("The migration profile name")]
        public string Name { get; set; }
        [Description("The development server where the database will be used for migration")]
        public MigrationServerDetail DevelopmentServer { get; set; }
        [Description("The test server where the result of the migration (between development and production) will be deployed for testing purposes")]
        public MigrationServerDetail TestServer { get; set; }
        [Description("The production server to use for production data and where the result of the migration will be deployed for final production usage")]
        public MigrationServerDetail ProductionServer { get; set; }

        [Description("Scripts to execute before processing tables")]
        public List<DBMigrationScript> PreMigrationScripts { get; set; }
        [Description("Scripts to execute after processing tables")]
        public List<DBMigrationScript> PostMigrationScripts { get; set; }

        [Description("Scripts to execute when a table's migration action is set as 'NoMigration' i.e. drop the table")]
        public List<DBMigrationScript> ScriptsForNoMigration { get; set; }
        [Description("Scripts to execute when a table's migration action is set as 'Development'. i.e. keep development data")]
        public List<DBMigrationScript> ScriptsForMigrationWithDevelopmentData { get; set; }
        [Description("Scripts to execute when a table's migration action is set as 'Production' i.e. keep production data")]
        public List<DBMigrationScript> ScriptsForMigrationWithProductionData { get; set; }
        [Description("Scripts to execute when a table's migration action is set as 'Clear' i.e. clear all data")]
        public List<DBMigrationScript> ScriptsForMigrationWithClearData { get; set; }

        [Description("Scripts to execute after migration of the database into 'test' server")]
        public List<DBMigrationScript> ScriptsForTestServer { get; set; }
        [Description("Scripts to execute after migration of the database into 'production' server")]
        public List<DBMigrationScript> ScriptsForProductionServer { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class MigrationStatus
    {
        public string Schema { get; set; }
        public string Table { get; set; }
        public string MigrationRule { get; set; }
        public string Status { get; set; }
    }

    public class MigrationServerDetail
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string BackupFolder { get; set; }
        public string BackupSharedFolder { get; set; }
        public string DataFolder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return Server + " (" + Database + ")";
        }
    }

}
