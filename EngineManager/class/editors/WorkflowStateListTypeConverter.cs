using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
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
}
