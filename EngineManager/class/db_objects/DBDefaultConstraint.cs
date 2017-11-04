using System.ComponentModel;

namespace EngineManager
{
    public class DBDefaultConstraint : DBConstraint
    {
        public DBDefaultConstraint()
            : base()
        {
            this.type = DB.DBObjectType.Constraint;
            this.constraint_type = DBConstraintTypes.Default;
        }

        private DBColumn column;

        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn Column {
            get
            {
                return this.column;
            }
            set
            {
                if(this.State != DBObjectState.None && value != null)
                {
                    var sql = this.GetCommandSyntax();
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Sql = sql,
                        Description = "modify default constraint for " + value.Name
                    });
                }
                this.column = value;
            }
        }

        private string expression;
        public string Expression
        {
            get
            {
                return this.expression;
            }
            set
            {
                if (this.State != DBObjectState.None && value != null)
                {
                    this.Connection.Project.Commands.Add(new DBCommand
                    {
                        Owner = this,
                        Sql = this.GetCommandSyntax(),
                        Description = "modify default constraint expression"
                    });
                }
                this.expression = value;
            }
        }
        public override string GetCommandSyntax()
        {
            var sql = "";
            if(this.Action == DB.DBAction.Add || this.Column.Action == DB.DBAction.Add)
            {
                sql = string.Format(@"
                ALTER TABLE {0} 
	                ADD  CONSTRAINT {1}
	                DEFAULT ({2}) 
	                FOR {3}
                ;
                ", this.Parent.FullName, string.Format("DF_{0}_{1}_{2}", this.Owner.Schema.Name, this.Owner.Parent.Name, this.Owner.Name), this.Expression, this.Column.FullName);
            }
            else if(this.Action == DB.DBAction.Alter)
            {
                sql = string.Format(@"
                ALTER TABLE {0} 
	                ADD  CONSTRAINT {1}
	                DEFAULT ({2}) 
	                FOR {3}
                ;
                ", this.Parent.FullName, this.FullName, this.Expression, this.Column.FullName);
            }
            else if(this.Action == DB.DBAction.Drop)
            {
                sql = string.Format(@"
                    ALTER TABLE {0} 
	                    DROP CONSTRAINT {1}
                    ;
                    ", this.Parent.FullName, this.FullName);
            }

            return sql;
        }
    }
}
