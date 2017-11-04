using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EngineManager
{
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
}
