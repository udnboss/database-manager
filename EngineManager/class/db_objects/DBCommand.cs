using System.ComponentModel;
using System.Data;

namespace EngineManager
{
    public class DBCommand : INotifyPropertyChanged
    {
        private string sql = "";
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public DBCommand()
        {
            this.State = CommandState.New;
        }
        public enum CommandState { New, Executed, Failed }

        public DBObject Owner { get; set; }
        public string Description { get; set; }
        protected CommandState _state = CommandState.New;
		public CommandState State {
			get{ return _state; }
			set {
				_state = value;
				Notify("State");
			}
		}
        public string Sql
        {
            get
            {
                return this.sql;
            }
            set
            {
                sql = value;
                this.State = CommandState.New;
            }
        }
        
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public string PropertyName { get; set; }
    }
}
