namespace EngineManager
{
    public class WorkflowState
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }


}
