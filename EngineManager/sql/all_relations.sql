select
                parent_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/FOREIGN_KEY_CONSTRAINT',
                constraint_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/FOREIGN_KEY_CONSTRAINT/' + t1.fk_name,
                column_id = db_name() + '/SCHEMA/' + t1.fk_schema + '/USER_TABLE/' + t1.fk_table + '/COLUMN/' + t1.fk_column,
                referenced_column_id = db_name() + '/SCHEMA/' + t1.pk_schema + '/USER_TABLE/' + t1.pk_table + '/COLUMN/' + t1.pk_column,
                ConstraintName = t1.fk_name,
                SchemaName = t1.fk_schema,
                TableName = t1.fk_table, 
                ColumnName = t1.fk_column, 
                referencedSchema = t1.pk_schema,
                referencedTable = t1.pk_table, 
                referencedColumn = t1.pk_column, 
                t1.update_action,
                t1.delete_action,
		        respect_read = cast(case when respect like '%R%' then 1 else 0 end as bit),
		        respect_create = cast(case when respect like '%C%' then 1 else 0 end as bit),
		        respect_update = cast(case when respect like '%U%' then 1 else 0 end as bit),
		        respect_delete = cast(case when respect like '%D%' then 1 else 0 end as bit),
		        cascade_lookup = cast(case when respect like '%L%' then 1 else 0 end as bit),
		        refresh_parents_on_change = cast(case when respect like '%S%' then 1 else 0 end as bit),
		        respect,
                title,
		        is_constraint = cast(1 as bit),
		        is_disabled,
		        relation_type = 'constraint',
		        for_finding_child = cast(1 as bit),
		        for_finding_parent = cast(1 as bit)
		
	        from 
		        (
			        SELECT   
				        fk_name = kc.name,    
				        pk_column_id = concat(pk_schema.name,'.',pktable.name, '.', pkcolumn.name),
				        fk_column_id = concat(fk_schema.name,'.',fktable.name, '.', fkcolumn.name),
				        fk_schema.name as [fk_schema]
				        , fktable.name AS fk_table
				        , fkcolumn.name AS fk_column
				        , pk_schema.name as [pk_schema]
				        , pktable.name AS pk_table
				        , pkcolumn.name AS pk_column
				        , kc.delete_referential_action_desc [delete_action]
				        , kc.update_referential_action_desc [update_action]			
				        , respect = cast(ep.value as nvarchar(max))
				        , title = cast(ept.value as nvarchar(max))
				        , kc.is_disabled
			        FROM 
				        sys.foreign_key_columns AS fkc INNER JOIN
				        sys.tables AS fktable ON fkc.parent_object_id = fktable.object_id INNER JOIN
				        sys.columns AS fkcolumn ON fkc.parent_column_id = fkcolumn.column_id AND fktable.object_id = fkcolumn.object_id INNER JOIN
				        sys.tables AS pktable ON fkc.referenced_object_id = pktable.object_id INNER JOIN
				        sys.columns AS pkcolumn ON fkc.referenced_column_id = pkcolumn.column_id  AND pktable.object_id = pkcolumn.object_id
				        INNER JOIN sys.schemas fk_schema ON fktable.schema_id = fk_schema.schema_id
				        INNER JOIN sys.schemas pk_schema ON pktable.schema_id = pk_schema.schema_id
				        INNER JOIN sys.foreign_keys kc on fkc.constraint_object_id = kc.object_id
				        left join sys.extended_properties ep on ep.major_id = fkc.constraint_object_id and ep.name = 'MS_Description'
				        left join sys.extended_properties ept on ept.major_id = fkc.constraint_object_id and ept.name = 'title'
				 
		        ) as t1