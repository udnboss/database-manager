using System.ComponentModel;

namespace EngineManager
{
    public class RowAction
    { 
        [Description("Column Name to create for this Action")]
        public string ActionName { get; set; }        
        [Description("The action message to show")]
        public string ActionMessage { get; set; }
        [Description("Close the screen after the action is performed")]
        public bool CloseAfterAction { get; set; }
        public enum ActionMessageTypes {Info, Success, Warning, Danger}
        [Description("The action message type to use")]
        public ActionMessageTypes ActionMessageType { get; set; }

        [Description("The custom procedure to execute, specify columns to pass using brackets. eg. [ID]")]
        public string Procedure { get; set; }

        //todo type converter to lookup commands available..
        [Description("The basic procedure to execute instead of the custom procedure")]
        [TypeConverter(typeof(BasicProcedureListTypeConverter))]
        public BasicProcedure BasicProcedure { get; set; }

        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy AvailabilityPolicy { get; set; }

        [Browsable(false)]
        public DBTable Table { get; set; }

        internal RowActionDefinition GetDefinition()
        {
            return new RowActionDefinition
            {
                ActionMessage = ActionMessage,
                ActionMessageType = ActionMessageType.ToString(),
                ActionName = ActionName,
                AvailabilityPolicy = AvailabilityPolicy != null ? AvailabilityPolicy.ID : null,
                CloseAfterAction = CloseAfterAction,
                Procedure = Procedure,
                BasicProcedure = BasicProcedure != null ? BasicProcedure.ID : null
            };
        }

        internal string GetOuterSelectSyntax()
        {
            var sql_outer_select = "\r\n\t, [" + ActionName + "] = cast(case when (" + (AvailabilityPolicy == null ? "1 = 1" : AvailabilityPolicy.CaseExpression) + ") then 0 else 1 end as bit)";
            return sql_outer_select;
        }
    }
}
