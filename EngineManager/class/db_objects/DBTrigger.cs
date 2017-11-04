namespace EngineManager
{
    public class DBTrigger : DBSchemaObject
    {
        public DBTrigger() : base()
        {
            this.type = DB.DBObjectType.Trigger;
            this.ExecutionOrder = DBTriggerExecutionOrder.Undefined;
            this.TriggerAfterInsert = true;
            this.TriggerAfterDelete = true;
            this.TriggerAfterUpdate = true;
            this.OnAlreadyExists = DBOnExists.DropBeforePerformingAction;
            //this.WrapInExec = true;
            //this.AutoSchema = true; //always in same schema as its table schema
        }
        public enum DBTriggerExecutionOrder { First, Undefined, Last }

        public DBTriggerExecutionOrder ExecutionOrder { get; set; } //TODO

        public string TableName { get; set; }
        public bool TriggerAfterDelete { get; set; }
        public bool TriggerAfterInsert { get; set; }

        public bool TriggerAfterUpdate { get; set; }
        public override string GetCommandDetailSyntax()
        {
            var syntax = "";

            if (this.Action == DB.DBAction.Create || this.Action == DB.DBAction.Alter)
            {
                return this.GetDefinitionSQL();
                //var afters = new List<string>();
                //if (this.TriggerAfterInsert) 
                //    afters.Add("INSERT");
                //if (this.TriggerAfterDelete) 
                //    afters.Add("DELETE");
                //if (this.TriggerAfterUpdate) 
                //    afters.Add("UPDATE");

                //var onTable = "<reference_table>";
                //if(this.GenerateFrom != null)
                //    onTable = this.GenerateFrom.FullName;

                //syntax = string.Format("ON {0} AFTER {1} AS\r\n\tBEGIN\r\n\t\t{2}\r\n\tEND",  onTable, string.Join(",", afters), this.GetDefinitionSQL().Replace("\r\n","\r\n\t\t"));
            }

            return syntax;
        }

        public override string GetCommandSyntax()
        {
            if (this.Action == DB.DBAction.Add) //triggers do not have add action, use create instead.
                this.Action = DB.DBAction.Create;

            if (this.Action == DB.DBAction.Create || this.Action == DB.DBAction.Alter)
                return "";

            return base.GetCommandSyntax();
        }
    }
}
