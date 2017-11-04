declare @schema_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @schema_folders
	select distinct 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_name] = o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

declare @object_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_folders
	select distinct --constraints etc
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_name] = o.type_desc collate SQL_Latin1_General_CP1_CI_AS
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.objects po on o.parent_object_id = po.object_id
	where
		s.schema_id between 5 and 16383
		and o.parent_object_id <> 0

-------------------------------------------------------------------------------------------------------------------------------

declare @object_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_property_folders
	select distinct 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
	where
		s.schema_id between 5 and 16383
-------------------------------------------------------------------------------------------------------------------------------

declare @sub_object_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @sub_object_property_folders
	select distinct --constraints etc
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name  + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.objects po on o.parent_object_id = po.object_id
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.class = 1
	where
		s.schema_id between 5 and 16383
		and o.parent_object_id <> 0

-------------------------------------------------------------------------------------------------------------------------------

declare @object_column_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_column_folders
	select distinct --columns
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN'
		, [object_name] = 'COLUMN'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.columns c on c.object_id = o.object_id
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

declare @object_column_property_folders table (id nvarchar(200), object_name nvarchar(200), object_type nvarchar(50), parent_id nvarchar(200))
insert @object_column_property_folders
	select distinct --columns
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/' + c.name + '/PROPERTY'
		, [object_name] = 'PROPERTY'
		, [object_type] = 'FOLDER'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/' + c.name
	from 
		sys.schemas s
		join sys.objects o on o.schema_id = s.schema_id
		join sys.columns c on c.object_id = o.object_id
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = c.column_id
	where
		s.schema_id between 5 and 16383

-------------------------------------------------------------------------------------------------------------------------------

select 
	[id] = cast([id] as nvarchar(200))
	, [object_name] = cast([object_name] as nvarchar(200))
	, [object_type] = cast([object_type] as nvarchar(200))
	, [parent_id] = cast([parent_id] as nvarchar(200))
	, [definition] = cast([definition] as nvarchar(max))
from
(
select	--database
	[id] = db_name()
	, [object_name] = db_name()
	, [object_type] = 'DATABASE' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = null
	, [definition] = null

union all

select --database folders
	[id] = db_name() + '/SCHEMA'
	, [object_name] = 'SCHEMA'
	, [object_type] = 'FOLDER' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name()
	, [definition] = null

union all

select --schemas
	[id] = db_name() + '/SCHEMA/' + s.name
	, [object_name] = s.name
	, [object_type] = 'SCHEMA' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA' collate SQL_Latin1_General_CP1_CI_AS	
	, [definition] = null
from 
	sys.schemas s
where
	s.schema_id between 5 and 16383

union all

	select --schema folders
		*, [definition] = null 
	from @schema_folders

union all

select --tables etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [object_name] = o.name
	, [object_type] = o.type_desc
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
	, [definition] = OBJECT_DEFINITION(o.object_id)
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
where
	s.schema_id between 5 and 16383
	and o.parent_object_id = 0

union all

	select 
		*, [definition] = null 
	from @object_folders

union all

	select 
		*, [definition] = null 
	from @object_column_folders

union all

	select 
		*, [definition] = null 
	from @object_property_folders

union all

	select --table properties etc 
		[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY/' + ep.name
		, [object_name] = ep.name
		, [object_type] = 'PROPERTY'
		, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
		, [definition] = cast(ep.value as varchar(max))
	from 
		sys.objects o 
		join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
		join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
	where
		s.schema_id between 5 and 16383

union all

select --special case for history
	[id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT'
	, [object_name] = o.name
	, [object_type] = 'FOLDER'
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [definition] = OBJECT_DEFINITION(o.object_id)
from
	sys.objects o 
	join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
where
	s.name = 'history' and o.name = 'EventLog'

union all

select --special case for history fk
	[id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT/' + 'FK_history_eventlog_' + s.name + '_' + o.name
	, [object_name] = 'FK_history_eventlog_' + s.name + '_' + o.name
	, [object_type] = 'FOREIGN_KEY_CONSTRAINT'
	, [parent_id] = db_name() + '/SCHEMA/history/USER_TABLE/EventLog/FOREIGN_KEY_CONSTRAINT'
	, [definition] = null
from
	sys.objects o 
	join sys.schemas s on o.schema_id = s.schema_id and o.parent_object_id = 0
where
	object_id('history.EventLog') is not null
	and exists(select 1 from sys.extended_properties ep where ep.major_id = o.object_id and ep.minor_id = 0 and ep.name = 'use_history' and ep.value = 'true')

union all

select --constraints etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name
	, [object_name] = o.name
	, [object_type] = o.type_desc
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS
	, [definition] = OBJECT_DEFINITION(o.object_id)
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.objects po on o.parent_object_id = po.object_id
where
	s.schema_id between 5 and 16383
	and o.parent_object_id <> 0

union all

select --properties of constraints etc
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY/' + ep.name
	, [object_name] = ep.name
	, [object_type] = 'PROPERTY'
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + po.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + po.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/PROPERTY'
	, [definition] = cast(ep.value as varchar(max))
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.objects po on o.parent_object_id = po.object_id
	join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = 0
where
	s.schema_id between 5 and 16383
	and o.parent_object_id <> 0

union all

	select 
		*, [definition] = null 
	from @sub_object_property_folders

union all

select --columns
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/'  + c.name
	, [object_name] = c.name
	, [object_type] = 'COLUMN' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name  + '/COLUMN'
	, [definition] = null
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.columns c on c.object_id = o.object_id
where
	s.schema_id between 5 and 16383

union all

	select --property folders
		*, [definition] = null
	from @object_column_property_folders

union all

select --columns properties
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name + '/COLUMN/'  + c.name + '/PROPERTY/' + ep.name
	, [object_name] = ep.name
	, [object_type] = 'PROPERTY' collate SQL_Latin1_General_CP1_CI_AS
	, [parent_id] = db_name() + '/SCHEMA/' + s.name + '/' + o.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + o.name  + '/COLUMN/' + c.name + '/PROPERTY'
	, [definition] = cast(ep.value as varchar(max))
from 
	sys.schemas s
	join sys.objects o on o.schema_id = s.schema_id
	join sys.columns c on c.object_id = o.object_id
	join sys.extended_properties ep on ep.major_id = o.object_id and ep.minor_id = c.column_id
where
	s.schema_id between 5 and 16383

) t
order by id