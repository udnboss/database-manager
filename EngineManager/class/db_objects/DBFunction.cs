namespace EngineManager
{
    public class DBFunction : DBObject
    {
        public DBFunction()
            : base()
        {
            this.type = DB.DBObjectType.Function;
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
    }
}
