using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class SecurityPolicyQuestionDefinition
    {
        public string Table { get; set; }
        public string ID { get; set; }
        public string Question { get; set; }
        public string SourceColumn { get; set; }
        public string SourceComputedColumn { get; set; }
        public string Operator { get; set; }
        public string TargetColumn { get; set; }
        public string ConstantValue { get; set; }

        public SecurityPolicyQuestion GetObject(DBTable t)
        {
            var q = new SecurityPolicyQuestion
                {
                    ID = ID,
                    ConstantValue = ConstantValue,
                    Operator = (SecurityPolicyQuestion.SqlOperator)Enum.Parse(typeof(SecurityPolicyQuestion.SqlOperator), Operator),
                    Question = Question,
                    SourceColumn = t.AdditionalRelatedColumns.Where(c => c.Alias == SourceColumn).FirstOrDefault(),
                    SourceComputedColumn = t.AdditionalComputedColumns.Where(c => c.Alias == SourceComputedColumn).FirstOrDefault(),
                    Table = t,
                    TargetColumn = t.AdditionalRelatedColumns.Where(c => c.Alias == TargetColumn).FirstOrDefault()
                };

            return q;
        }
    }
}
