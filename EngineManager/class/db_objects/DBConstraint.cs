namespace EngineManager
{
    public abstract class DBConstraint : DBSchemaObject
    {
        protected DBConstraintTypes constraint_type;

        public enum DBConstraintTypes { PrimaryKey, Unique, ForeignKey, Check, Default, Trigger }
        public DBConstraintTypes ConstraintType { get { return this.constraint_type; } }
        protected override string GetCheckExistSyntax()
        {
            return base.GetCheckExistSyntax();
        }
    }
}
