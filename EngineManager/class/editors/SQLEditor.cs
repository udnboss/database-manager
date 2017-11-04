using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

namespace EngineManager
{

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
