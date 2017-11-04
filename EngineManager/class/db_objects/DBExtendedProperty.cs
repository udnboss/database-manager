using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace EngineManager
{
    public class DBExtendedProperty : DBObject
    {
    	public DBObject Owner {get;set;}
    	
        public DBExtendedProperty()
            : base()
        {
        	this.OnAlreadyExists = DBOnExists.PerformActionWithNoExistenceCheck;
            this.type = DB.DBObjectType.Property;
            this.PropertyChanged += DBExtendedProperty_PropertyChanged;
        }

        void DBExtendedProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.State == DBObjectState.Modified && this.Owner != null && this.Owner.Connection.Project != null)
            {
            	DBObjectManager.CreateExtendedProperty(this.Owner, this.Name, "", this.Value);
            }
        }

        private string val;
        public string Value { get { return val; } set { if(val != value) SetField(ref val, value); } }

        protected override string GetCheckExistSyntax()
        {
            return "";
        }
        public override string GetCommandSyntax()
        {
        	var schema_level_owner = Owner as DBSchemaObject;
        	
            var name = this.Name;
            var value = this.Action == DB.DBAction.Create && Value != null ? this.Value.Replace("'", "''") : "";
                         
            var level0type = schema_level_owner != null ? "N'SCHEMA'" : "NULL"; //todo for users/roles etc which do not have schema
            var level0name = schema_level_owner != null ? "N'" + schema_level_owner.Schema.Name + "'" : "NULL";
                         
            var level1type = schema_level_owner != null ? this.Owner is DBColumn || this.Owner is DBConstraint ? "N'" + schema_level_owner.Owner.Type.ToString().ToUpper() + "'" : "N'" + this.Owner.Type.ToString().ToUpper() + "'" : "NULL";
            var level1name = schema_level_owner != null ? this.Owner is DBColumn || this.Owner is DBConstraint ? "N'" + schema_level_owner.Owner.Name + "'" : "N'" + this.Owner.Name + "'" : "NULL";

            var level2type = schema_level_owner != null ? this.Owner is DBColumn || this.Owner is DBConstraint ? "N'" + this.Owner.Type.ToString().ToUpper() + "'" : "NULL" : "NULL";
            var level2name = schema_level_owner != null ? this.Owner is DBColumn || this.Owner is DBConstraint ? "N'" + this.Owner.Name + "'" : "NULL" : "NULL";





            var sql = string.Format(@"
declare
	@name nvarchar(100)		  = N'{0}',
	@value sql_variant		  = '{1}',
	@level0type nvarchar(100) = {2}, --schema
	@level0name nvarchar(100) = {3},
	@level1type nvarchar(100) = {4}, --table/view
	@level1name nvarchar(100) = {5},
	@level2type nvarchar(100) = {6}, --column/constraint
	@level2name nvarchar(100) = {7}

declare @major_id int = 0, @minor_id int = 0;

if @level0type = 'SCHEMA'
	select @major_id = o.object_id from sys.objects o join sys.schemas s on o.schema_id = s.schema_id where s.name = @level0name and o.name = @level1name

if @level2type = 'CONSTRAINT'
	select @major_id = so.object_id from sys.objects so join sys.objects o on so.parent_object_id = o.object_id join sys.schemas s on o.schema_id = s.schema_id 
		where s.name = @level0name and o.name = @level1name and so.name = @level2name
else if @level2type = 'COLUMN'
	select @minor_id = c.column_id from sys.columns c join sys.objects o on c.object_id = o.object_id join sys.schemas s on o.schema_id = s.schema_id 
		where s.name = @level0name and o.name = @level1name and c.name = @level2name


if not exists(select 1 from sys.extended_properties ep where ep.major_id = @major_id and ep.minor_id = @minor_id and ep.name = @name)
	begin
		print 'property does not exist'
		if len(cast(@value as nvarchar(max))) > 0
			begin
				execute sp_addextendedproperty @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name
				print 'created'
			end
	end
else --exists
	begin
		if len(cast(@value as nvarchar(max))) = 0 --empty? delete it
			begin
				execute sp_dropextendedproperty @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name
				print 'deleted'
			end
		else --update it
			begin
				execute sp_updateextendedproperty @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name
				print 'updated'
			end
		
	end


                        ",
                         name, value, level0type, level0name, level1type, level1name, level2type, level2name
                         );

            return sql;
        }

        public object GetObject(Type target_type)
        {
            if (!string.IsNullOrEmpty(val))
                return JsonConvert.DeserializeObject(val, target_type);

            return null;
        }
    }
}
