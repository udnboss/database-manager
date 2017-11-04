using System.ComponentModel;

namespace EngineManager
{
    public interface IContainsColumns
    {       
        BindingList<DBColumn> Columns { get; set; }
    }
}
