SELECT  
	[id] = db_name() + '/SCHEMA/' + s.name + '/' + t.type_desc collate SQL_Latin1_General_CP1_CI_AS + '/' + t.name + '/COLUMN/'  + c.name,     
	s.name as [schema_name], 
	t.name as [table_name], 
	c.name as [column_name], 
	st.name as [datatype], 
	ut.name as [usertype], 
	CASE 
		WHEN c.max_length = 1 THEN 1
		WHEN c.is_ansi_padded = 1 THEN c.max_length / 2 
		ELSE c.max_length 
	END as [length],
	c.is_computed, 
	c.is_nullable, 
	c.is_identity, 
	ISNULL(i.is_primary_key, 0) AS is_primary_key, 
	c.column_id as [column_order], 
	cc.definition [computed_value], 
	d.definition [default_value],
    d.name [default_constraint_name],
	title = cast(ep.value as nvarchar(max)),
	displayorder = isnull(cast(ep_o.value as nvarchar(max)), cast(c.column_id as nvarchar(max))),
    sortorder = cast(isnull(cast(ep_o.value as nvarchar(max)), cast(c.column_id as nvarchar(max))) as int),
	iconclass = cast(ep_i.value as nvarchar(max))

FROM            
	sys.indexes AS i INNER JOIN
	sys.index_columns AS ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id and i.is_primary_key = 1 RIGHT OUTER JOIN
	sys.columns AS c INNER JOIN
	sys.objects AS t ON c.object_id = t.object_id ON ic.column_id = c.column_id AND ic.object_id = t.object_id INNER JOIN 
	sys.systypes st ON c.system_type_id = st.xusertype INNER JOIN
	sys.systypes ut ON c.user_type_id = ut.xusertype INNER JOIN
	sys.schemas s ON s.schema_id = t.schema_id LEFT JOIN
	sys.computed_columns cc ON cc.object_id = t.object_id AND cc.column_id = c.column_id left join 
	sys.default_constraints d on c.column_id = d.parent_column_id and c.object_id = d.parent_object_id
	left join sys.extended_properties ep on ep.major_id = t.object_id and ep.minor_id = c.column_id and ep.name = 'title'
	left join sys.extended_properties ep_o on ep_o.major_id = t.object_id and ep_o.minor_id = c.column_id and ep_o.name = 'displayorder'
	left join sys.extended_properties ep_i on ep_i.major_id = t.object_id and ep_i.minor_id = c.column_id and ep_i.name = 'iconclass'


    --WHERE        t.name = '{0}' and s.name = '{1}'
    order by sortorder