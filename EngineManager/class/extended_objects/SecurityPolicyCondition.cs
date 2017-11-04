using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace EngineManager
{
    public class SecurityPolicyCondition : UniqueID, ISecurityPolicyExpression
    {
        public SecurityPolicyCondition() : base()
        {
            this.SubConditions = new BindingList<SecurityPolicyCondition>();
            this.SubConditions.ListChanged += SubConditions_ListChanged;
        }

        private void SubConditions_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                this.SubConditions[e.NewIndex].Parent = this;               
            }

            //reorder
            foreach(var sc in this.SubConditions)
            {
                sc.Order = this.SubConditions.IndexOf(sc);
            }
        }

        public SecurityPolicyQuestion Question { get; set; }
        public SecurityPolicyCondition Parent { get; set; }
        public BindingList<SecurityPolicyCondition> SubConditions { get; set; }

        public string WhereExpression { get { return GetExpression(); } } 
        public string CaseExpression { get { return GetExpression("", true); } }

        private string GetExpression(string prefix = "", bool case_expression = false)
        {
            if (Question == null) return "";

            var expression = prefix + (case_expression ? this.Question.CaseExpression : this.Question.WhereExpression);

            if (this.SubConditions.Count > 0) //there are sub-conditions
            {
                expression += "\r\nAND\r\n\t(";

                prefix += "\t";
                foreach (var sc in this.SubConditions)
                {
                    expression += (this.SubConditions.IndexOf(sc) == 0 ? "" : "\r\nOR\r\n\t") + "(" + sc.GetExpression(prefix, case_expression) + "\r\n\t)";
                }

                expression += "\r\n\t)";
            }

            return expression;
        }

        public int Order { get; internal set; }

        public override string ToString()
        {
            return this.Question != null ? this.Question.ToString() : "?";
        }

        public SecurityPolicyConditionDefinition GetDefinition()
        {
            return new SecurityPolicyConditionDefinition { ID = ID, Order = Order, Question = Question.ID, SubConditions = GetDefinitionTree(this) };
        }
        private List<SecurityPolicyConditionDefinition> GetDefinitionTree(SecurityPolicyCondition c)
        {
            var output = new List<SecurityPolicyConditionDefinition>();

            foreach (var sc in c.SubConditions)
                output.Add(sc.GetDefinition());

            return output;
        }

        public TreeNode GetTree()
        {
            var node = new TreeNode(this.ToString()) { Tag = this };

            foreach(var c in this.SubConditions)
            {
                node.Nodes.Add(c.GetTree());
            }

            return node;
        }
    }
}
