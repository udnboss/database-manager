using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EngineManager
{
    //to select a column from the owner table (used by computedcolumn)
    public class ComputedColumnListTypeConverter : TypeConverter
    {
        private List<AdditionalComputedColumn> m_list = new List<AdditionalComputedColumn>(); //stores the list
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
                AdditionalComputedColumn newVal = m_list.Where(x => x.Alias.Equals(value.ToString())).FirstOrDefault();
                return newVal;
            }

            return base.ConvertFrom(context, culture, value);
        }

    }
}
