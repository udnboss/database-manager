/* get_schemas */

	select name 
		from sys.schemas 
	where 
		schema_id > 4 order by name

/* get_schemas_end */

/* get_schema_tables_and_views */

	select name 
		from sys.objects 
	where 
		type in('U','V') 
		and schema_id = schema_id('{0}') 
	order by name

/* get_schema_tables_and_views_end */

/* get_object_columns */

	select name 
		from sys.columns 
	where 
		object_id = object_id('{0}.{1}') 
	order by column_id

/* get_object_columns_end */

/* get_impersonation */

	select *
	from
	(
		select [impersonator] = u.name, [impersonated] = impersonated.name 
			from sys.database_permissions sp 
				join sys.database_principals impersonated on sp.major_id = impersonated.principal_id
				join sys.database_principals impersonator on sp.grantee_principal_id = impersonator.principal_id
				join sys.database_role_members m on m.role_principal_id = impersonator.principal_id
				join sys.database_principals u on u.principal_id = m.member_principal_id
			where permission_name = 'IMPERSONATE' and sp.state = 'G' and u.name = original_login()
	union all
		SELECT [impersonator] =original_login(), [impersonated] = l.name
				FROM sys.sql_logins l LEFT JOIN sys.database_principals u ON l.sid = u.sid
				WHERE l.name NOT like 'NT %' AND l.name NOT like '#%' AND l.is_policy_checked = 0 
				and is_srvrolemember('sysadmin', system_user) = 1
	) t
	order by t.[impersonated]

/* get_impersonation_end */
