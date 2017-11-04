namespace EngineManager
{
    public class DBStoredProcedure : DBSchemaObject
    {
        public DBStoredProcedure() : base()
        {
            this.type = DB.DBObjectType.Procedure;
            this.OnAlreadyExists = DBOnExists.DropBeforePerformingAction;
            //this.WrapInExec = true;
        }
        public override string GetCommandDetailSyntax()
        {
            if (Action == DB.DBAction.Drop)
                return "";
            return GetDefinitionSQL();
        }

        public override string GetCommandSyntax()
        {
            if (Action == DB.DBAction.Drop)
                return base.GetCommandSyntax();
            return "";
        }

        protected override string GetCheckExistSyntax()
        {
            var syntax = "";
            if (this.Action != DB.DBAction.None)
                syntax = string.Format("IF {0} EXISTS(select name from sys.procedures where name = '{1}')\r\n", OnAlreadyExists == DBOnExists.DoNothingElsePerformAction ? "NOT" : "", Name);
            return syntax;
        }
    }
}
