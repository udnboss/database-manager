using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class DBSchema : DBObject
    {
        public DBSchema() : base()
        {
            this.type = DB.DBObjectType.Schema;
            this.OnAlreadyExists = DBOnExists.DoNothingElsePerformAction;
            
            this.Tables = new MyBindingList<DBTable>(this);                        
            this.Views = new MyBindingList<DBView>(this);                        
            this.TableFunctions = new MyBindingList<DBFunction>(this);                                    
            this.ScalarFunctions = new MyBindingList<DBFunction>(this);                       
            this.StoredProcedures = new MyBindingList<DBStoredProcedure>(this);
						
            PrepareSubCollections();
            //this.WrapInExec = true; //execute in own batch?
        }
       
        [Browsable(false)]
        public MyBindingList<DBFunction> ScalarFunctions { get; set; }

        public DBSchema Schema { get { return this; } }
        [Browsable(false)]
        public DBSchemaTypes SchemaType { get; set; }
        public enum DBSchemaTypes { User, Engine }


        public MyBindingList<DBStoredProcedure> StoredProcedures { get; set; }

        [Browsable(false)]
        public MyBindingList<DBFunction> TableFunctions { get; set; }


        public MyBindingList<DBTable> Tables { get; set; }


        public MyBindingList<DBView> Views { get; set; }


        public override string GetCommandDetailSyntax()
        {
            if (Action == DB.DBAction.Drop)
                return "";
            return "AUTHORIZATION [dbo]";
        }


    }
}
