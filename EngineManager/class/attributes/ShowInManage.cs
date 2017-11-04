using System;

namespace EngineManager
{
    public class ShowInManage : Attribute
    {
        public ShowInManage(bool show)
        {
            this.Show = show;
        }

        public bool Show { get; set; }
    }
}
