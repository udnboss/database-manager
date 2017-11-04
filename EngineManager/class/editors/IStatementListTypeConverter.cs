using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EngineManager
{
    //IStatement type converter
    public class IStatementListTypeConverter : TypeConverter
    {
        private List<SQLStatement> m_list = new List<SQLStatement>(); //stores the list
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (context.Instance is BasicProcedureStatement)
            {
                var ps = context.Instance as BasicProcedureStatement;
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
}
