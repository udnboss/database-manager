using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EngineManager
{
    class MultiSelectEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<string> values = value as List<string>;
            if (svc != null && values != null)
            {
                using (MultiSelectForm form = new MultiSelectForm())
                {
                    form.DBObject = context.Instance as DBObject;
                    form.Value = values;
                    form.populate_list();

                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        values = form.Value; // update object
                    }
                }
            }
            return values; // can also replace the wrapper object here
        }
    }
}
