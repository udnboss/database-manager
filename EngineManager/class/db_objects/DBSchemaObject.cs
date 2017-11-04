using System;
using System.ComponentModel;

namespace EngineManager
{
	/// <summary>
	/// Description of DBSchemaObject.
	/// </summary>
	public abstract class DBSchemaObject : DBObject
	{	
		[Browsable(false)]
        public DBSchemaObject Owner { get; set; }
		
		[Browsable(false)]
        [ReadOnly(true)]
        public override string FullName
        {
            get
            {
                var name = this.Name;
                var schema = this.Schema != null ? this.Schema.Name : "";

                return types_not_needing_fullname.Contains(Type) ? string.Format("[{0}]", name) : string.Format("[{0}].[{1}]", schema, name);
            }
        }
		
        [Browsable(false)]
        public override string DisplayName { get { return types_not_needing_fullname.Contains(Type) ? string.Format("{0}", this.Name) : string.Format("{0}.{1}", this.Schema != null ? this.Schema.Name : "", this.Name); } }

        
		[ReadOnly(true)]
        public DBSchema Schema { get; set; }
	}
}
