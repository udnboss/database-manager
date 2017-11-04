using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EngineManager
{
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
                       DBObjectManager.RefreshColumns(table.SecureView);
                    }
                    m_list = table.SecureView.Columns.ToList();
                }
            }
            else if (context.Instance is DBView)
            {
                var view = context.Instance as DBView;
                m_list = view.Columns.ToList();
            }
            else if (context.Instance is DBColumn && (context.Instance as DBObject).Parent is IContainsColumns)
            {
                var table = (context.Instance as DBObject).Parent as IContainsColumns;
                if(table != null)
                m_list = table.Columns.ToList();              
            }
            else if (context.Instance is RowAction && (context.Instance as RowAction).Table is IContainsColumns)
            {
                var table = (context.Instance as RowAction).Table as IContainsColumns;
                if (table != null)
                    m_list = table.Columns.ToList();
            }
            else if (context.Instance is SecurityPolicyQuestion)
            {
                var question = context.Instance as SecurityPolicyQuestion;
                var additional_columns = question.Table.AdditionalRelatedColumns.Select(rc=> rc.Column).ToList();
                m_list = additional_columns;
            }
            else if(context.Instance is AdditionalColumn)
            {
                var db_rel_col = context.Instance as AdditionalColumn;
                if(db_rel_col.Constraint == null)
                    m_list = db_rel_col.CurrentTable.Columns.ToList();
                else if (db_rel_col.Constraint.Parent == db_rel_col.CurrentTable)
                    m_list = (db_rel_col.Constraint.ReferenceColumn.Parent as DBTable).CombinedColumns.ToList();
                else
                    m_list = (db_rel_col.Constraint.Parent as DBTable).CombinedColumns.ToList();

            }
            else if(context.Instance is Workflow)
            {

                var wf = context.Instance as Workflow;
                if(wf.Table != null && context.PropertyDescriptor.Name == "StateColumn")
                    m_list = wf.Table.ForeignKeys.Select(x => x.Column).Distinct().ToList();
                else if(wf.Table != null && context.PropertyDescriptor.Name == "StateNameColumn" && wf.StateColumn != null)
                {
                    m_list = (wf.Table.ForeignKeys.Where(x => x.Column == wf.StateColumn).FirstOrDefault().ReferenceColumn.Parent as DBTable).Columns.Distinct().ToList();
                }
            }
            else if (context.Instance is SQLUpdateColumnValuePair)
            {
                var cvp = context.Instance as SQLUpdateColumnValuePair;
                if (cvp.Table != null)
                    m_list = cvp.Table.Columns.ToList();
            }
            else if (context.Instance is SQLInsertColumnValuePair)
            {
                var cvp = context.Instance as SQLInsertColumnValuePair;
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
}
