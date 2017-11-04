using System.ComponentModel;

namespace EngineManager
{
    public class DBCheckConstraint : DBConstraint
    {
        public DBCheckConstraint()
            : base()
        {
            this.type = DB.DBObjectType.Constraint;
            this.constraint_type = DBConstraintTypes.Check;
        }
        private string fail_message;
        [Description("message to display when the expression fails")]
        public string FailMessage
        {
            get
            {
                return this.fail_message;
            }
            set
            {
                if (this.State != DBObjectState.None && value != null)
                {
                    DBObjectManager.CreateExtendedProperty(this, "MS_Description", this.fail_message, value);
                }

                this.fail_message = value;
            }
        }

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if(this.State != DBObjectState.None)
                {
                    var sql = string.Format(@"ALTER TABLE {0} {1}CHECK CONSTRAINT {2}", this.Parent.FullName, value ? "" : "NO", this.FullName);
                    this.Connection.Project.Commands.Add(new DBCommand { Owner = this, Sql = sql, Description = "Modify check constraint enabled" });
                }
                enabled = value;
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
                if(this.State != DBObjectState.None && value != null)
                {
                    this.Connection.Project.Commands.Add(new DBCommand
                        {
                            Owner = this,
                            Sql = this.GetCommandSyntax(),
                            Description = "modify check constraint expression"
                        });
                }
                this.expression = value;
            }
        }

        public override string GetCommandSyntax()
        {
            var sql = "";

            if (this.Action == DB.DBAction.Add)
            {
                sql = string.Format(@"
                    ALTER TABLE {0}  
	                    WITH NOCHECK 
	                    ADD  CONSTRAINT {1}
	                    CHECK  ({2})
                    ;

                    ALTER TABLE {0}   
	                    {3}CHECK CONSTRAINT {1}
                    ;
                    ", 
                     this.Parent.FullName,
                     this.FullName,
                     this.Expression,
                     this.Enabled ? "" : "NO"
                     );
            }
            else if(this.Action == DB.DBAction.Drop)
            {
                sql = string.Format(@"
                    ALTER TABLE {0}  
	                    DROP  CONSTRAINT {1}
                    ",
                     this.Parent.FullName,
                     this.FullName
                     );
            }
            else if(this.Action ==DB.DBAction.Alter)
            {
                sql = string.Format(@"
                    ALTER TABLE {0}  
	                    DROP  CONSTRAINT {1}
                    ;
                    ALTER TABLE {0}  
	                    WITH NOCHECK 
	                    ADD  CONSTRAINT {1}
	                    CHECK  ({2})
                    ;

                    ALTER TABLE {0}   
	                    {3}CHECK CONSTRAINT {1}
                    ;
                    ",
                     this.Parent.FullName,
                     this.FullName,
                     this.Expression,
                     this.Enabled ? "" : "NO"
                     );
            }
            return sql;
        }
    }
}
