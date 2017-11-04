using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class RowActionDefinition
    {
        public string ActionName { get; set; }
        public string IconClass { get; set; }
        public string NewValue { get; set; }
        public string ActionMessage { get; set; }
        public bool CloseAfterAction { get; set; }
        public string ActionMessageType { get; set; }
        public string AvailabilityPolicy { get; set; }
        public string Procedure { get; set; }
        public string BasicProcedure { get; set; }

        public RowAction GetObject(DBTable t)
        {
            var action = new RowAction
            {
                ActionMessage = ActionMessage,
                ActionMessageType = (RowAction.ActionMessageTypes)Enum.Parse(typeof(RowAction.ActionMessageTypes), ActionMessageType),
                ActionName = ActionName,
                AvailabilityPolicy = t.AvailablePolicies.Where(x => x.ID == AvailabilityPolicy).FirstOrDefault(),
                CloseAfterAction = CloseAfterAction,
                Procedure = Procedure,
                Table = t
            };

            return action;
        }
    }
}
