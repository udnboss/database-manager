using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineManager
{
    public class MyBindingList<T> : BindingList<T>
    {
        public MyBindingList() : base()
        {

        }
        public MyBindingList(IList<T> list) : base(list)
        {
            
        }
        public MyBindingList(DBObject owner) : base()
        {
        	this._owner = owner;
        }
        
        public new void Add(T item)
	    {
	        base.Add(item);
	        MyBindingList_AddingNew(item);
	        
	    }


		void MyBindingList_AddingNew(object item)
		{
			var o = item as DBObject;
			var so = item as DBSchemaObject;


            if (o is DBFKConstraint)
                if ((o as DBFKConstraint).Parent != _owner)
                    return; //special case for FK sub relations

			if(o!=null && _owner != null)
			{
				o.Parent = _owner; //own whatever added as sub object.
           		o.Connection = _owner.Connection;
            	o.Database = _owner.Database;
            	
            	var db_schema_owner = _owner as DBSchemaObject;
            	if(so != null)
            	{
            		if(db_schema_owner != null)
            	   	{
            			//schema object owned by another under schema object
            			so.Schema = db_schema_owner.Schema;
            			so.Owner = db_schema_owner;
            		}
            		else //owned by a schema
	            	{
	            		var schema = _owner as DBSchema;
	            		so.Schema = schema;
	            	}
            	}
            	
			}
            
		}
        protected DBObject _owner;
        
        protected override void SetItem(int index, T item)
        {
            T old_item = this.Items[index];
            T new_item = item;

            if (ItemChanging != null)
            {
                ItemChanging(old_item, new_item);
            }

            base.SetItem(index, item);
        }

        protected override void RemoveItem(int itemIndex)
        {
            T deletedItem = this.Items[itemIndex];

            if (ItemRemoving != null)
            {
                ItemRemoving(deletedItem);
            }

            base.RemoveItem(itemIndex);
        }

        public delegate void DelegateItemChanged(T oldItem, T newItem);
        public event DelegateItemChanged ItemChanging;

        public delegate void DelegateBeforeRemove(T deletedItem);
        public event DelegateBeforeRemove ItemRemoving;
    }
}
