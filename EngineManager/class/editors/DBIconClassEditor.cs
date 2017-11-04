using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EngineManager
{
    //editor custom for iconclass
    class DBIconClassEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            DBIconClass icon = value as DBIconClass;
            if (svc != null && icon != null)
            {
                using (IconClassForm form = new IconClassForm())
                {
                    form.Value = icon;
                    if (svc.ShowDialog(form) == DialogResult.OK)
                    {
                        icon = form.Value; // update object
                    }
                }
            }
            return icon; // can also replace the wrapper object here
        }
    }
}
