using System.Collections.Generic;
using System.ComponentModel;

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
}
