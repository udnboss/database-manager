using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    //BasicProcedure type converter
    public class BasicProcedureListTypeConverter : TypeConverter
    {
        private List<BasicProcedure> m_list = new List<BasicProcedure>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is RowAction)
            {
                var ra = context.Instance as RowAction;
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
}
