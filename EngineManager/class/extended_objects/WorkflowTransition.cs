using System.ComponentModel;
namespace EngineManager
{
    public class WorkflowTransition
    {
        public string Name { get; set; }
        public Workflow Workflow { get; set; }

        [TypeConverter(typeof(WorkflowStateListTypeConverter))]
        public WorkflowState From { get; set; }
        [TypeConverter(typeof(WorkflowStateListTypeConverter))]
        public WorkflowState To { get; set; }
        public string Action { get; set; }

        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy AvailabilityPolicy { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} -> {2}", Action, From != null ? From.Name : "", To != null ? To.Name : "");
        }
    }


}
