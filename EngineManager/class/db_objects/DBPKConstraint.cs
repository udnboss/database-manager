using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DBPKConstraint : DBConstraint
    {
        public DBPKConstraint() : base()
        {
            this.Action = DB.DBAction.Create;
            this.type = DB.DBObjectType.Constraint;
            this.OnAlreadyExists = DBOnExists.PerformActionElseDoNothing;
            this.constraint_type = DBConstraintTypes.PrimaryKey;
            this.Columns = new MyBindingList<DBColumn>();
            //this.Columns.ListChanged += Columns_ListChanged;
        }

        void Columns_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted)
                if (this.State != DBObjectState.None)
                    this.Connection.Project.Commands.Add(new DBCommand {
                        Sql = this.GetCommandSyntax(),
                        Description = "primary key modification on " + this.Parent.FullName,
                        Owner = this
                    });
        }

        private MyBindingList<DBColumn> columns;
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        public MyBindingList<DBColumn> Columns { get { return this.columns; } set { this.columns = value; this.columns.ListChanged += Columns_ListChanged; } }

        public override string GetCommandSyntax()
        {
            var sql = "";
            if(this.Action == DB.DBAction.Add)
            {
                sql = string.Format("ALTER TABLE {0}\r\n\tADD CONSTRAINT {1}", this.Parent.FullName, this.FullName);
                sql += string.Format("PRIMARY KEY CLUSTERED ({0});", string.Join(",", this.Columns.Select(c=>c.FullName).ToList()));
            }
            else if(this.Action == DB.DBAction.Alter)
            {
                sql = string.Format("ALTER TABLE {0}\r\n\tDROP CONSTRAINT {1};", this.Parent.FullName, this.FullName);
                sql += string.Format("ALTER TABLE {0}\r\n\tADD CONSTRAINT {1}", this.Parent.FullName, this.FullName);
                sql += string.Format("PRIMARY KEY CLUSTERED ({0});", string.Join(",", this.Columns.Select(c => c.FullName).ToList()));
            }
            else if(this.Action == DB.DBAction.Drop)
            {
                sql = string.Format("ALTER TABLE {0}\r\n\tDROP CONSTRAINT {1};", this.Parent.FullName, this.FullName);
            }
            return sql;
        }

        protected override string GetCheckExistSyntax()
        {
            var syntax = "";
            if (this.Action != DB.DBAction.None)
                syntax = string.Format("IF NOT EXISTS(select name from sys.key_constraints where name = '{0}')\r\n", Name);
            return syntax;
        }
    }
}
