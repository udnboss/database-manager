using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    //security policy type converter...
    public class SecurityPolicyListTypeConverter : TypeConverter
    {
        private List<SecurityPolicy> m_list = new List<SecurityPolicy>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is RowAction)
            {
                var ra = context.Instance as RowAction;
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
}
