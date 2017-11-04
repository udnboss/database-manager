namespace EngineManager
{
    public class DBUniqueConstraint : DBConstraint
    {
        public DBUniqueConstraint()
            : base()
        {
            this.type = DB.DBObjectType.Constraint;
            this.constraint_type = DBConstraintTypes.Unique;
        }
    }
}
