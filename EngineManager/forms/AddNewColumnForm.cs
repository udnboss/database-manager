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
    public partial class AddNewColumnForm : Form
    {
        public AddNewColumnForm()
        {
            InitializeComponent();
        }
        private DBTable _table;
        private DBColumn column = new DBColumn();
        public DBTable Table
        {
            get { return _table; }
            set
            {
                _table = value;
                this.column = new DBColumn
                {
                    Parent = value,
                    Schema = value.Schema,
                    Name = "NewColumn1",
                    State = DBObject.DBObjectState.None,
                    Action = DB.DBAction.Add,
                    Connection = value.Connection,
                    DataType = DB.DBDataType.nvarchar,
                    Length = 50,
                    Nullable = true
                };
                propertyGrid1.SelectedObject = column;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            column.State = DBObject.DBObjectState.New;
            Table.Connection.Project.Commands.Add(new DBCommand { Description = "Add New Column", Owner = Table, Sql = column.GetSQL() });
        }
    }
}
