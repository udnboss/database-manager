namespace EngineManager
{
    public interface IRefreshableObject
    {
        DBConnection Connection { get; set; }
        //void Refresh();
    }
}
