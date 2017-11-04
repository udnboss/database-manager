using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
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
                   DBManager.RefreshRoles(db); //a proper way is to ensure roles are pre loaded.. TODO
                m_list = (context.Instance as SecurityPolicyQuestion).Table.Database.Roles.ToList();
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
}
