using System.Collections.Generic;
using System.ComponentModel;

namespace EngineManager
{
    public class SecurityPolicyQuestion : UniqueID, ISecurityPolicyExpression, ITableOwned
    {
        [ReadOnly(true)]
        [Browsable(false)]
        public DBTable Table { get; set; }

        [Category("Appearance")]
        public string Question { get; set; }

        [Category("Compare Column")]
        [Description("The source column of interest")]
        [TypeConverter(typeof(AdditionalColumnListTypeConverter))]
        public AdditionalColumn SourceColumn { get; set; }

        [Category("Compare Column")]
        [TypeConverter(typeof(ComputedColumnListTypeConverter))]
        public AdditionalComputedColumn SourceComputedColumn { get; set; }

        [Category("Current User")]
        [TypeConverter(typeof(RoleListTypeConverter))]
        public DBRole IsMemberOf { get; set; }

        private Dictionary<SqlOperator, string> Operators = new Dictionary<SqlOperator, string> {
            { SqlOperator.Equals, "="},
            { SqlOperator.GreaterThan, ">"},
            { SqlOperator.LessThan, "<"},
            { SqlOperator.NotEquals, "<>"},
            { SqlOperator.GreaterThanOrEquals, ">="},
            { SqlOperator.LessThanOrEquals, "<="},
            { SqlOperator.IsNull, "is null"},
            { SqlOperator.IsNotNull, "is not null"},
            { SqlOperator.Like, "like" }
        };

        public enum SqlOperator { Equals, GreaterThan, GreaterThanOrEquals, LessThan, LessThanOrEquals, NotEquals, IsNull, IsNotNull, Like}
        [Category("To")]
        public SqlOperator Operator { get; set; }

        [Category("To")]
        [Description("The source column's value will be compared to this column's value")]
        [TypeConverter(typeof(AdditionalColumnListTypeConverter))]
        public AdditionalColumn TargetColumn { get; set; }

        [Category("To")]
        [Description("The source column's value will be compared to this value")]
        public string ConstantValue { get; set; }

        [Category("Xpression"), Description("Security Policy Expression -- WHERE statement")]
        [ReadOnly(true)]
        public string WhereExpression
        {
            get
            {
                return GetExpression(false);
            }
        }
        [Category("Xpression"), Description("Expression -- CASE statement")]
        [ReadOnly(true)]
        public string CaseExpression
        {
            get
            {
                return GetExpression(true);
            }
        }

        private string GetExpression(bool case_expression)
        {
            var source = "";

            if (SourceColumn != null)
            {
                source = case_expression ? "[t].[" + SourceColumn.Alias + "]" : SourceColumn.Column.Parent.FullName + "." + SourceColumn.Column.FullName;

                if (SourceColumn.FromThisTable && !case_expression)
                    source = string.Format("[tbl].{0}", SourceColumn.Column.FullName);
            }

            if (IsMemberOf != null) //role test
                return string.Format("IS_MEMBER('{0}') = 1 /* {1} */", IsMemberOf, Question);

            return string.Format("{0} {1} {2} /* {3} */", source, Operators[Operator], ConstantValue, Question);
        }

        public override string ToString()
        {
            return string.Format("{0}?", WhereExpression);
        }

        public SecurityPolicyQuestionDefinition GetDefinition()
        {
            return new SecurityPolicyQuestionDefinition
            {
                ID = ID,
                ConstantValue = ConstantValue,
                Operator = Operator.ToString(),
                Question = Question,
                SourceColumn = SourceColumn != null ? SourceColumn.Alias : null,
                SourceComputedColumn = SourceComputedColumn != null ? SourceComputedColumn.Alias : null,
                Table = Table.Name,
                TargetColumn = TargetColumn != null ? TargetColumn.Alias : null
            };
        }
    }
}
