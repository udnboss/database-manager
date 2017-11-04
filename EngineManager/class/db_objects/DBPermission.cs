namespace EngineManager
{
    public class DBPermission : DBObject
    {
        public DBPermission() : base()
        {
            this.type = DB.DBObjectType.Permission;
            this.State = DBObjectState.None;
            this.OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck;
        }
        
        public DBObject Owner {get;set;}

        public enum PermissionVerb { Revoke, Grant, Deny }

        //[ReadOnly(true)]
        public DBRole Grantee { get; set; }
        private PermissionVerb view_definition;
        public PermissionVerb ViewDefinition
        {
            get
            {
                return this.view_definition;
            }
            set
            {
                bool changed = this.view_definition != value;
                view_definition = value;

                if (this.State != DBObjectState.None && changed)//must not be initial
                {
                    DBObjectManager.CreatePermissionCommand(this); 

                    //if the owner is a secure view
                    var owner_schema_object = this.Owner as DBSchemaObject;
                    if(owner_schema_object is DBView && owner_schema_object.Schema.Name == "secure")
                    {
                        var schema_name = owner_schema_object.Name.Substring(0, this.Owner.Name.IndexOf('_') + 1);
                        var table_name = owner_schema_object.Name.Substring(this.Owner.Name.IndexOf('_') + 1);
                        var p = new DBPermission
                        {
                            Owner = new DBTable
                            {
                                Name = table_name,
                                Schema = new DBSchema { Name = schema_name }                                
                            },
                            ViewDefinition = value,
                            OnAlreadyExists = DBOnExists.PerformActionWithNoExistenceCheck,
                            Grantee = this.Grantee,
                            State = DBObjectState.Intact
                        };

                        DBObjectManager.CreatePermissionCommand(p); 

                        
//                        this.Connection.Project.Commands.Add(new DBCommand()
//                        {
//                            Owner = this,
//                            Sql = p.GetSQL(),
//                            Description = string.Format("Set Permissions on {0} to '{1}'", p.Owner.FullName, p.Grantee.FullName)
//                        });
                    }
                }
            }
        }
        private PermissionVerb select;
        public PermissionVerb Select
        {
            get
            {
                return this.select;
            }
            set
            {
                bool changed = this.select != value;
                select = value;

                if (this.State != DBObjectState.None && changed)//must not be initial
                {
                     DBObjectManager.CreatePermissionCommand(this);
                }
            }
        }
        private PermissionVerb insert;
        public PermissionVerb Insert
        {
            get
            {
                return this.insert;
            }
            set
            {
                bool changed =  this.insert != value;
                insert = value;

                if (this.State != DBObjectState.None && changed)//must not be initial
                {
                    DBObjectManager.CreatePermissionCommand(this);
                }
            }
        }
        private PermissionVerb update;
        public PermissionVerb Update
        {
            get
            {
                return this.update;
            }
            set
            {
                bool changed =  this.update != value;
                update = value;

                if (this.State != DBObjectState.None && changed)//must not be initial
                {
                    DBObjectManager.CreatePermissionCommand(this);
                }
            }
        }
        private PermissionVerb delete;
        public PermissionVerb Delete
        {
            get
            {
                return this.delete;
            }
            set
            {
                bool changed = this.delete != value;
                delete = value;

                if (this.State != DBObjectState.None && changed)//must not be initial
                {
                    DBObjectManager.CreatePermissionCommand(this);
                }
            }
        }      
        
        
        
        
        public override string GetCommandSyntax()
        {
            var cmd = "";
            if (this.Owner is DBColumn)
            {
                cmd = string.Format(@"revoke select, update on {0}({1}) to {2};", this.Owner.Parent.FullName, this.Owner.FullName, this.Grantee.FullName);

                cmd += string.Format(@"{0} select on {1}({2}) to {3};", this.Select.ToString(), this.Owner.Parent.FullName, this.Owner.FullName, this.Grantee.FullName);
                cmd += string.Format(@"{0} update on {1}({2}) to {3};", this.Update.ToString(), this.Owner.Parent.FullName, this.Owner.FullName, this.Grantee.FullName);          
            }
            else
            {
                cmd = string.Format(@"revoke view definition, select, insert, update, delete on {0} to {1};", this.Owner.FullName, this.Grantee.FullName);

                cmd += string.Format(@"{0} view definition on {1} to {2};", this.ViewDefinition.ToString(), this.Owner.FullName, this.Grantee.FullName);
                cmd += string.Format(@"{0} select on {1} to {2};", this.Select.ToString(), this.Owner.FullName, this.Grantee.FullName);
                cmd += string.Format(@"{0} insert on {1} to {2};", this.Insert.ToString(), this.Owner.FullName, this.Grantee.FullName);
                cmd += string.Format(@"{0} update on {1} to {2};", this.Update.ToString(), this.Owner.FullName, this.Grantee.FullName);
                cmd += string.Format(@"{0} delete on {1} to {2};", this.Delete.ToString(), this.Owner.FullName, this.Grantee.FullName);
            }
            
            return cmd;
        }
    }
}
