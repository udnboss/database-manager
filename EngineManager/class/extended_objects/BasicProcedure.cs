using System;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    public class BasicProcedure : UniqueID
    {
        public BasicProcedure()
        {
            ID = Guid.NewGuid().ToString();
            this.Statements = new BindingList<BasicProcedureStatement>();
            this.Statements.ListChanged += Statements_ListChanged;
        }

        private void Statements_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                Statements[e.NewIndex].Table = Table;
            }
        }

        public string Name { get; set; }
        public DBTable Table { get; set; }
        
        public BindingList<BasicProcedureStatement> Statements { get; set; }

        public string GetSyntax()
        {
            var sql_proc = string.Format(@"
if object_id('[{0}].[{1}]') is not null 
    exec('drop procedure [{0}].[{1}]')

create procedure [{0}].[{1}]
(
    @pk_value nvarchar(max)
)
as

{2}", Table.Schema.Name, Name, string.Join(";\r\n\r\n", Statements.Select(s=> s.Statement != null ? s.Statement.GetSyntax() : "").ToList()));
        
        return sql_proc;
        }

        public string GetDefinition()
        {
            var def = new BasicProcedureDefinition
            {
                ID = ID,
                Name = Name

            };
            //foreach(var s in Statements)
            //    s.

            return ""; //todo..
        }
    }
}
