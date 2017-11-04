using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

namespace EngineManager
{
    //this class allows having a custom list of values in property grid drop down
    //the property needs this attribute in its class definition
    //[TypeConverter(typeof(ListTypeConverter))]
    public class ListTypeConverter : TypeConverter
    {
        public ListTypeConverter()
        {
            m_list.Add("Demo.TestClass.Created");
            m_list.Add("Demo.TestClass.Name");
            m_list.Add("Demo.TestClass.Description");
        }
        private List<string> m_list = new List<string>();
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        private StandardValuesCollection GetValues()
        {
            return new StandardValuesCollection(m_list);
        }
        protected void SetList(List<string> list)
        {
            m_list = list;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return GetValues();
        }
    }

    //to select a column from the owner table
    public class ColumnListTypeConverter : TypeConverter
    {
        private List<DBColumn> m_list = new List<DBColumn>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {    
            if (context.Instance is DBTable)
            {
                
                var table = context.Instance as DBTable;
                m_list = table.Columns.ToList();
                if(table.SecureView != null)
                {
                    if(table.SecureView.Columns.Count == 0)
                    {
                        table.SecureView.Refresh();
                    }
                    m_list = table.SecureView.Columns.ToList();
                }
            }
            else if (context.Instance is DBView)
            {
                var view = context.Instance as DBView;
                m_list = view.Columns.ToList();
            }
            else if (context.Instance is DBColumn && (context.Instance as DBObject).Owner is IContainsColumns)
            {
                var table = (context.Instance as DBObject).Owner as IContainsColumns;
                if(table != null)
                m_list = table.Columns.ToList();              
            }
            else if (context.Instance is DBRowAction && (context.Instance as DBRowAction).Table is IContainsColumns)
            {
                var table = (context.Instance as DBRowAction).Table as IContainsColumns;
                if (table != null)
                    m_list = table.Columns.ToList();
            }
            else if (context.Instance is SecurityPolicyQuestion)
            {
                var question = context.Instance as SecurityPolicyQuestion;
                var additional_columns = question.Table.AdditionalRelatedColumns.Select(rc=> rc.Column).ToList();
                m_list = additional_columns;
            }
            else if(context.Instance is DBRelatedColumn)
            {
                var db_rel_col = context.Instance as DBRelatedColumn;
                if(db_rel_col.Constraint == null)
                    m_list = db_rel_col.CurrentTable.Columns.ToList();
                else if (db_rel_col.Constraint.Owner == db_rel_col.CurrentTable)
                    m_list = (db_rel_col.Constraint.ReferenceColumn.Owner as DBTable).CombinedColumns.ToList();
                else
                    m_list = (db_rel_col.Constraint.Owner as DBTable).CombinedColumns.ToList();

            }
            else if(context.Instance is Workflow)
            {

                var wf = context.Instance as Workflow;
                if(wf.Table != null && context.PropertyDescriptor.Name == "StateColumn")
                    m_list = wf.Table.ForeignKeys.Select(x => x.Column).Distinct().ToList();
                else if(wf.Table != null && context.PropertyDescriptor.Name == "StateNameColumn" && wf.StateColumn != null)
                {
                    m_list = (wf.Table.ForeignKeys.Where(x => x.Column == wf.StateColumn).FirstOrDefault().ReferenceColumn.Owner as DBTable).Columns.Distinct().ToList();
                }
            }
            else if (context.Instance is ColumnValuePair)
            {
                var cvp = context.Instance as ColumnValuePair;
                if (cvp.Table != null)
                    m_list = cvp.Table.Columns.ToList();
            }
            else if (context.Instance is InsertColumnValuePair)
            {
                var cvp = context.Instance as InsertColumnValuePair;
                if (cvp.Table != null)
                {
                    if(context.PropertyDescriptor.Name == "SourceColumn")
                    {
                        m_list = cvp.Table.Columns.ToList();
                    }
                    else if(cvp.TargetTable != null && context.PropertyDescriptor.Name == "TargetColumn")
                    {
                        m_list = cvp.TargetTable.Columns.ToList();
                    }
                }
                    
            }
            else if (context.Instance is SQLStatement)
            {
                var cvp = context.Instance as SQLStatement;
                if (cvp.Table != null)
                    m_list = cvp.Table.Columns.ToList();
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                DBColumn newVal = m_list.Where(x => x.Name.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }

    //to select a state from the owner table
    public class WorkflowStateListTypeConverter : TypeConverter
    {
        private List<WorkflowState> m_list = new List<WorkflowState>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is WorkflowTransition)
            {
                var t = context.Instance as WorkflowTransition;
                m_list = t.Workflow.States.ToList();
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                WorkflowState newVal = m_list.Where(x => x.Name.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
    //to select a column from the owner table (used by dbrelatedcolumn/computed)
    public class AdditionalColumnListTypeConverter : TypeConverter
    {
        private List<DBRelatedColumn> m_list = new List<DBRelatedColumn>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is SecurityPolicyQuestion)
            {
                var question = context.Instance as SecurityPolicyQuestion;
                var additional_columns = question.Table.AdditionalRelatedColumns.Select(rc => rc).ToList();
                m_list = additional_columns;
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                DBRelatedColumn newVal = m_list.Where(x => x.Alias.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }

    //to select a column from the owner table (used by computedcolumn)
    public class ComputedColumnListTypeConverter : TypeConverter
    {
        private List<DBComputedColumn> m_list = new List<DBComputedColumn>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is SecurityPolicyQuestion)
            {
                var question = context.Instance as SecurityPolicyQuestion;
                var additional_columns = question.Table.AdditionalComputedColumns.Select(cc => cc).ToList();
                m_list = additional_columns;
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                DBComputedColumn newVal = m_list.Where(x => x.Alias.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }

    //to select a role
    public class RoleListTypeConverter : TypeConverter
    {
        private List<DBRole> m_list = new List<DBRole>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is SecurityPolicyQuestion)
            {
                var db = (context.Instance as SecurityPolicyQuestion).Table.Database;
                if (db.Roles.Count == 0)
                    db.Refresh(); //a proper way is to ensure roles are pre loaded.. TODO
                m_list = (context.Instance as SecurityPolicyQuestion).Table.Database.Roles;
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                DBRole newVal = m_list.Where(x => x.Name.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
    //security policy type converter...
    public class SecurityPolicyListTypeConverter : TypeConverter
    {
        private List<SecurityPolicy> m_list = new List<SecurityPolicy>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is DBRowAction)
            {
                var ra = context.Instance as DBRowAction;
                m_list = ra.Table.AvailablePolicies.ToList();
            }
            else if (context.Instance is WorkflowTransition)
            {
                var t = context.Instance as WorkflowTransition;
                m_list = t.Workflow.Table.AvailablePolicies.ToList();
            }
            else if (context.Instance is Validator)
            {
                var t = context.Instance as Validator;
                m_list = t.Table.AvailablePolicies.ToList();
            }
            else if (context.Instance is SQLStatement)
            {
                var t = context.Instance as SQLStatement;
                m_list = t.Table.AvailablePolicies.ToList();
            }
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                SecurityPolicy newVal = m_list.Where(x => x.ID.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }

    //BasicProcedure type converter
    public class BasicProcedureListTypeConverter : TypeConverter
    {
        private List<BasicProcedure> m_list = new List<BasicProcedure>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is DBRowAction)
            {
                var ra = context.Instance as DBRowAction;
                m_list = ra.Table.BasicProcedures.ToList();
            }
            
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the one having selected name
                BasicProcedure newVal = m_list.Where(x => x.ID.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }

    //IStatement type converter
    public class IStatementListTypeConverter : TypeConverter
    {
        private List<SQLStatement> m_list = new List<SQLStatement>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is ProcedureStatement)
            {
                var ps = context.Instance as ProcedureStatement;
                m_list = ps.Table.UpdateCommands.Cast<SQLStatement>().ToList().Concat(ps.Table.DeleteCommands.Cast<SQLStatement>().ToList()).Concat(ps.Table.InsertCommands.Cast<SQLStatement>().ToList()).ToList();
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the one having selected name
                var newVal = m_list.Where(x => x.Name.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }


    //to multi-select columns from the owner table


    //to select a string
    public class StringListTypeConverter : TypeConverter
    {
        private List<string> m_list = new List<string>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            var sql = @"
                        select 
	                        [schema] = s.name, 
                            [table] = t.name,
                            [column] = c.name,
                            [full_name] = concat(s.name, '.', t.name, '.', c.name)
                        from 
	                        sys.schemas s
	                        join sys.objects t on t.schema_id = s.schema_id
	                        join sys.columns c on c.object_id = t.object_id
                        where 
	                        t.type in('U', 'V')
	                        and s.schema_id between 5 and 16000
	                        and s.name not in('views', 'computed', 'secure', 'select', 'update', 'delete', 'cache', 'db', 'engine', 'email', 'extensions','filters','history','migration','survey','security','workflow')
                        order by [full_name]
                        ";

            //populate the list here
            if (context.Instance is DBObject)
            {
                //get a global list
                m_list = (context.Instance as DBObject).Connection.GetDataTable(sql).AsEnumerable().Select(x => x["full_name"].ToString()).ToList();
            }

            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_list);
        }
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                // find the column having selected name
                string newVal = m_list.Where(x => x.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }



    //editor custom for iconclass
    class DBIconClassEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            DBIconClass icon = value as DBIconClass;
            if (svc != null && icon != null)
            {
                using (IconClassForm form = new IconClassForm())
                {
                    form.Value = icon;
                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        icon = form.Value; // update object
                    }
                }
            }
            return icon; // can also replace the wrapper object here
        }
    }

    class MultiSelectEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<string> values = value as List<string>;
            if (svc != null && values != null)
            {
                using (MultiSelectForm form = new MultiSelectForm())
                {
                    form.DBObject = context.Instance as DBObject;
                    form.Value = values;
                    form.populate_list();

                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        values = form.Value; // update object
                    }
                }
            }
            return values; // can also replace the wrapper object here
        }
    }

    class SQLEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            
            if (svc != null)
            {
                using (SQLEditorForm form = new SQLEditorForm())
                {
                    form.SQL = value != null ? value.ToString() : "";

                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        value = form.SQL; // update object
                    }
                }
            }
            return value; // can also replace the wrapper object here
        }
    }
}
