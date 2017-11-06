using System.ComponentModel;

namespace EngineManager
{
    public interface IContainsColumns
    {
        MyBindingList<DBColumn> Columns { get; set; }
    }
}
