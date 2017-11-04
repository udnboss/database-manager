using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineManager
{
    public partial class WorkflowForm : Form
    {
        public WorkflowForm()
        {
            InitializeComponent();
        }
        private DBTable table;
        
        public DBTable Table {
            get { return table; }
            set { table = value; RefreshView(); }
        }

        private void RefreshView()
        {
            if (table.Workflow == null)
                table.Workflow = new Workflow { Name = "New Workflow", Table = table };

            table.Workflow.RefreshStates();
            table.Workflow.RefreshTransitions();

            propertyGrid_workflow.SelectedObject = table.Workflow;

            gViewer1.Graph = table.Workflow.DrawGraph();
        }

        private void propertyGrid_workflow_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            gViewer1.Graph = table.Workflow.DrawGraph();
            propertyGrid_workflow.SelectedObject = table.Workflow;
        }

        private void toolStripButton_refresh_Click(object sender, EventArgs e)
        {
            gViewer1.Graph = table.Workflow.DrawGraph();
        }

        private void propertyGrid_workflow_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            gViewer1.Graph = table.Workflow.DrawGraph();
        }
    }
}
