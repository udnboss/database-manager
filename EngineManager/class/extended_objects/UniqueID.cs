using System;
using System.ComponentModel;

namespace EngineManager
{
    public class UniqueID
    {
        public UniqueID()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        [ReadOnly(true)]
        public string ID { get; set; }
    }
}
