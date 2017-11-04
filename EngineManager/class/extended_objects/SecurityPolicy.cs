using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EngineManager
{
    public class SecurityPolicy : UniqueID, ISecurityPolicyExpression
    {
        public string Name { get; set; }
        [Browsable(false)]
        public DBTable Table { get; set; }
        public SecurityPolicy Self { get { return this; } }
        public List<SecurityPolicyCondition> Conditions { get; set; }

        public SecurityPolicy() : base()
        {
            this.Name = "New Policy";
            this.Conditions = new List<SecurityPolicyCondition>();
        }

        internal TreeNode GetTree()
        {
            var root = new TreeNode(ID) { Tag = this };
            foreach (var c in Conditions)
                root.Nodes.Add(c.GetTree());

            return root;
        }

        //where expression
        public string WhereExpression
        {
            get
            {
                return GetExpression();
            }
        }
        public string CaseExpression
        {
            get
            {
                return GetExpression(true);
            }
        }
        private string GetExpression(bool case_expression = false)
        {
            string xp = "";
            foreach (var c in Conditions)
            {
                xp += (Conditions.IndexOf(c) > 0 ? "\r\nOR\r\n" : "") + "(" + (case_expression ? c.CaseExpression : c.WhereExpression) + ")";
            }
            return xp;
        }

        public string GetQuery(bool return_key_only = false, bool join_only = false)
        {
            var used_questions = get_used_questions();//todo from within only

            //get all involved columns
            var used_columns = used_questions.Select(q => q.SourceColumn).Distinct().ToList();
            //ensure their tables are joined
            var join_list = new List<string>();
            var joins = "";
            used_columns.ForEach(x =>
            {
                if (x != null)
                {
                    var join = x.GetJoinSyntax(true);
                    if (!join_list.Contains(join))
                    {
                        joins += join;
                        join_list.Add(join);
                    }
                }

            });

            var pk = Table.PrimaryKey.Columns.First().FullName;
            var sql_select = string.Format("select [tbl].{0} /* {1} */", return_key_only ? pk : "*", pk);
            var sql_join = string.Format("\r\n\tfrom {0} as [tbl] {1}", Table.FullName, joins);
            var sql_where = string.Format("\r\n\twhere {0}", WhereExpression);

            var final_query = (join_only ? "" : sql_select) + sql_join + sql_where;
            return final_query;
        }

        private List<SecurityPolicyQuestion> get_used_questions()
        {
            var used_questions = new List<SecurityPolicyQuestion>();

            foreach (var condition in Conditions)
            {
                used_questions.AddRange(get_condition_question(condition));
            }

            return used_questions.Distinct().ToList();
        }

        private List<SecurityPolicyQuestion> get_condition_question(SecurityPolicyCondition c)
        {
            var qs = new List<SecurityPolicyQuestion>();
            qs.Add(c.Question);
            foreach (var sc in c.SubConditions)
                qs.AddRange(get_condition_question(sc));
            return qs;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) == false ? Name : ID;
        }

        internal SecurityPolicyDefinition GetDefinition()
        {
            return new SecurityPolicyDefinition { ID = ID, Name = Name, Conditions = Conditions.Select(c => c.GetDefinition()).ToList() };
        }
    }
}
