namespace EngineManager
{
    public class DBUserDataType : DBObject
    {
        public DBUserDataType() : base()
        {
            this.type = DB.DBObjectType.Type;
            this.OnAlreadyExists = DBOnExists.AlterElsePerformAction;
        }

        public DB.DBDataType BaseType { get; set; }

        public bool IsNullable { get; set; }
        public int Length { get; set; }
        public override string GetCommandDetailSyntax()
        {
            var syntax = "";

            if (this.Action == DB.DBAction.Create || this.Action == DB.DBAction.Alter)
            {
                syntax = string.Format("FROM {1} {2} NULL", this.FullName, this.BaseType, DB.GetLengthSyntax(this.BaseType, null, this.Length), this.IsNullable ? "" : "NOT");
            }

            return syntax;
        }

        public override string GetCommandSyntax()
        {
            if (this.Action == DB.DBAction.Add)
                this.Action = DB.DBAction.Create;
            return base.GetCommandSyntax();
        }
    }
}
