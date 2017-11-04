using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace EngineManager
{
    public class DBConnection : IGenerateTreeNode
    {
        public BindingSource bindingSource;
        public DataTable dataTable;
        //tracks for PositionChanged event last row
        public DataRow LastDataRow = null;

        public List<string> sql_messages = new List<string>();
        public SqlCommandBuilder sqlCommandBuilder;
        public SqlConnection sqlConnection;
        public SqlDataAdapter sqlDataAdapter;
        public DBConnection()
        {
            this.Databases = new BindingList<DB>();
			this.Databases.AddingNew += (sender, e) => {
					(e.NewObject as DB).Connection = this;
			};
        }

        public DBObject OwnedBy { get; set; }

        public DBConnection Connection { get { return this; } }
        public string InitialDatabase { get; set; }

        public BindingList<DB> Databases { get; set;}

        public string Name { get; set; }

        public string Password { get; set; }

        public DBProject Project { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public DataTable GetDataTable(String query)
        {

            dataTable = new DataTable();
            if (query != null)
            {
                var connString = String.Format("Server = {0}; Database = {1}; User Id = {2}; Password = {3}",
            	                               Server, OwnedBy != null && OwnedBy.Database != null ? OwnedBy.Database.Name : InitialDatabase, User, Password);
                sqlConnection = new SqlConnection(connString);
                var cmd = new SqlCommand(query, sqlConnection);

				sqlConnection.InfoMessage += (sender, e) => sql_messages.Add(e.Message);

                sqlConnection.Open();

                // Create a new data adapter based on the specified query.
                sqlDataAdapter = new SqlDataAdapter(cmd);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                try
                {
                    sqlDataAdapter.Fill(dataTable);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    dataTable = null;
                }


                // Populate a new data table and bind it to the BindingSource.
                bindingSource = new BindingSource();
                bindingSource.DataSource = dataTable;

                //when position changes, check for updates etc..
                bindingSource.PositionChanged += bindingSource_PositionChanged;
                bindingSource.ListChanged += BindingSource_ListChanged;

                //bindingSource.

                sqlConnection.Close();
                //sqlDataAdapter.Dispose();                  

            }
            return dataTable;
        }

        readonly Dictionary<BindingSource, SqlDataAdapter> adapters = new Dictionary<BindingSource, SqlDataAdapter>();

        public BindingSource GetBoundSource(String query)
        {

            var data_table = new DataTable();
            var binding_source = new BindingSource();

            if (!string.IsNullOrEmpty(query))
            {
                var connString = String.Format("Server = {0}; Database = {1}; User Id = {2}; Password = {3}",
            	                               Server, OwnedBy != null ? OwnedBy.Database.Name : InitialDatabase, User, Password);
                var conn = new SqlConnection(connString);
                var cmd = new SqlCommand(query, conn);

				conn.InfoMessage += (sender, e) => sql_messages.Add(e.Message);

                conn.Open();

                // Create a new data adapter based on the specified query.
                var adapter = new SqlDataAdapter(cmd);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                var command_builder = new SqlCommandBuilder(adapter);
                try
                {
                    adapter.Fill(data_table);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    conn.Close();
                    adapter.Dispose();
                    return null;
                }


                // Populate a new data table and bind it to the BindingSource.
                
                binding_source.DataSource = data_table;

                //when position changes, check for updates etc..
				binding_source.PositionChanged += bindingSource_PositionChanged;
                binding_source.ListChanged += BindingSource_ListChanged;

                //bindingSource.

                conn.Close();
                //sqlDataAdapter.Dispose();                  

                adapters.Add(binding_source, adapter);
            }
            return binding_source;
        }

        void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemDeleted)
            {
                try
                {
	                if(adapters.ContainsKey(sender as BindingSource))
	                    adapters[(sender as BindingSource)].Update(((sender as BindingSource).DataSource as DataTable));
	            }
                catch
                {
                    
                }

            }

        }

        public TreeNode GetTree()
        {
            var root = new TreeNode(string.Format("{0}: {1}, {2}", this.User, this.Server, this.InitialDatabase)) { Tag = this };
            root.Expand();
            //existing databases
            var dbsNode = new TreeNode("Databases");
            
            //dbsNode.Expand();
            root.Nodes.Add(dbsNode);
            dbsNode.Expand();

            foreach (var db in this.Databases)
            {
                if (db.Name == this.InitialDatabase)
                {
                    //get children and ensure they are added
                    var db_node = db.GetTree();
                    dbsNode.Nodes.Add(db_node);
                    db_node.Expand();

                }
                else
                {
                    var db_node = new TreeNode(db.Name) { Tag = db }; //simple unexpanded
                    dbsNode.Nodes.Add(db_node);
                }
            }

            return root;

        }

        void bindingSource_PositionChanged(object sender, EventArgs e)
        {
            // if the user moves to a new row, check if the 
            // last row was changed

            var thisBindingSource = (BindingSource)sender;
            DataRow ThisDataRow = ((DataRowView)thisBindingSource.Current).Row;
            if (ThisDataRow == LastDataRow)
            {
                // we need to avoid to write a datarow to the 
                // database when it is still processed. Otherwise
                // we get a problem with the event handling of 
                //the DataTable.
               // throw new ApplicationException("It seems the" +
               //   " PositionChanged event was fired twice for" +
               //   " the same row");

            }

            UpdateRowToDatabase(sender as BindingSource);

            // track the current row for next 
            // PositionChanged event
            LastDataRow = ThisDataRow;
        }
        void UpdateRowToDatabase(BindingSource binding_source)
        {
            if (LastDataRow != null)
            {
                switch (LastDataRow.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Deleted:
                    case DataRowState.Modified:
                        try
                        {
                            if (adapters.ContainsKey(binding_source))
                                adapters[binding_source].Update((DataTable)binding_source.DataSource);
                        }
                        catch
                        {
                            if (LastDataRow.HasErrors)
                            {
                                MessageBox.Show(LastDataRow.RowError);
                            }
                        }
                        break;
                }
            }
        }
   }
}
