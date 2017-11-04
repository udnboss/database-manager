using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Glee.Drawing;
namespace EngineManager
{
    public class Workflow
    {
        public string Name { get; set; }

        [ReadOnly(true)]
        public DBTable Table { get; set; }

        private DBColumn state_col;
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn StateColumn { get { return state_col; } set { state_col = value; RefreshStates(); } }

        private DBColumn state_name_col;
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn StateNameColumn { get { return state_name_col; } set { state_name_col = value; RefreshStates(); } }

        public List<WorkflowState> States { get; set; }

        public Workflow()
        {
            States = new List<WorkflowState>();
            Transitions = new BindingList<WorkflowTransition>();
            Transitions.ListChanged += Transitions_ListChanged;
        }

        private void Transitions_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                Transitions[e.NewIndex].Workflow = this;
            }
        }

        public void RefreshStates()
        {
            if (Table != null && StateColumn != null)
            {
                var referenced_column = Table.ForeignKeys.Where(f => f.Column == StateColumn).FirstOrDefault().ReferenceColumn;
                var select = string.Format("select [ID] = {0}, [Name] = {1} from {2}", referenced_column.FullName, StateNameColumn != null ? StateNameColumn.FullName : referenced_column.FullName, referenced_column.Parent.FullName);
                States = Table.Connection.GetDataTable(select).AsEnumerable().Select(x => new WorkflowState { ID = x[0].ToString(), Name = x[1].ToString() }).ToList();
            }            
        }
        public BindingList<WorkflowTransition> Transitions { get; set; }

        public void RefreshTransitions()
        {
            //load saved transitions..
        }

        public Graph DrawGraph()
        {
            Graph g = new Graph(Name) { Directed = true, BuildNodeHierarchy = true, Cluster = true };
            g.GraphAttr.OptimizeLabelPositions = true;

            foreach (var s in States)
            {
                Node n = g.AddNode(s.Name);
                n.UserData = s;
                n.Attr.Shape = Shape.Circle;
            }

            foreach(var t in Transitions)
            {
                if(t.From != null && t.To != null)
                {
                    var policy_name = t.AvailabilityPolicy != null ? t.AvailabilityPolicy.ID + "\r\nto " + t.Action + "?" : "";

                    if(policy_name != "")
                    {
                        //diamond
                        Node pn = g.AddNode(policy_name);
                        pn.UserData = t.AvailabilityPolicy;
                        pn.Attr.Shape = Shape.Diamond;

                        //arrow to diamond
                        Edge e = g.AddEdge(t.From.Name, policy_name);
                        e.UserData = t;
                        e.Attr.ArrowHeadAtTarget = ArrowStyle.Normal;
                    }


                    Edge e2 = g.AddEdge(policy_name != "" ? policy_name : t.From.Name, t.Action, t.To.Name);
                    e2.UserData = t;
                    e2.Attr.ArrowHeadAtTarget = ArrowStyle.Normal;
                    
                }                
            }

            return g;
        }
    }


}
