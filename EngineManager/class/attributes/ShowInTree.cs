using System;

namespace EngineManager
{
    public class ShowInTree : Attribute
    {
        public ShowInTree(bool show)
        {
            this.Show = show;
        }

        public bool Show { get; set; }
    }
}
