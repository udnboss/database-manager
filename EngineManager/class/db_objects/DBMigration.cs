using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineManager
{
    class DBMigration : DBObject
    {
        public DBMigration() {
            this.StructureSource = MigrationSource.Source;
            this.DataSource = MigrationSource.Source;
        }

        public enum MigrationSource { None, Source, Target, SourceAndTarget, Query }


        private DBObject obj;
        public DBObject Object 
        {
            get { return obj; }
            set { obj = value; }
        }


        private MigrationSource data_source;
        public MigrationSource DataSource
        {
            get { return this.data_source; }
            set
            {
                if (this.State != DBObjectState.None)
                {
                   // this.DBObjectManager.CreateExtendedProperty(this, )
                }
                this.data_source = value;
            }
        }


        private MigrationSource structure_source;
        public MigrationSource StructureSource 
        {
            get { return this.structure_source; }
            set { this.structure_source = value; }
        }


        private string data_query;
        public string DataQuery
        {
            get { return data_query; }
            set { data_query = value; }
        }


        private string structure_query;
        public string StructureQuery
        {
            get { return structure_query; }
            set { structure_query = value; }
        }

        public override string GetCommandSyntax()
        {
            return "";
        }
    }
}
