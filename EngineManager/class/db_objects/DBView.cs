using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace EngineManager
{
    public class DBView : DBContainsColumns, IContainsColumns
    {
        public DBView() : base()
        {
            this.type = DB.DBObjectType.View;
            this.DefinitionSQL = "SELECT {0} FROM {1}";
            this.OnAlreadyExists = DBOnExists.DropBeforePerformingAction;
            this.Columns = new MyBindingList<DBColumn>(this);
            this.ExtendedProperties = new MyBindingList<DBExtendedProperty>(this);
        }
        public MyBindingList<DBColumn> Columns { get; set; }
        public MyBindingList<DBExtendedProperty> ExtendedProperties { get; set; }

        private DBColumn default_sort;
        [DataMember, Category("Data"), Description("Default Sort Column")]
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn DefaultSort
        {
            get { return default_sort; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "default_sort", this.default_sort == null ? "" : this.default_sort.Name, value == null ? "" : value.Name);
                }

                default_sort = value;
            }
        }
        public enum SortDirections { Ascending, Descending }

        private SortDirections sort_direction;
        [DataMember, Category("Data"), Description("Default Sort Column Order")]
        public SortDirections Direction
        {
            get { return sort_direction; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "sort_direction", this.sort_direction.ToString(), value.ToString());
                }

                sort_direction = value;
            }
        }


        public override string GetCommandDetailSyntax()
        {
            var syntax = "";
            //create/alter
            if (this.Action == DB.DBAction.Create || this.Action == DB.DBAction.Alter)
            {
                syntax = this.GetDefinitionSQL();
            }

            return syntax;
        }

        //public List<DBColumn> AppendColumns { get; set; }
        public override string GetCommandSyntax()
        {
            //correct the action
            if (this.Action == DB.DBAction.Add)
                this.Action = DB.DBAction.Create;

            if (Action == DB.DBAction.Drop)
                return base.GetCommandSyntax();

            return "";// base.GetCommandSyntax();
        }
    }
}
