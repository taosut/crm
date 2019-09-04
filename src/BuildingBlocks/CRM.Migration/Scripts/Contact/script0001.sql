
CREATE TABLE contact (
	contact_id uuid NOT NULL,
	first_name varchar NULL,
	last_name varchar NOT NULL,
    middle_name varchar NOT NULL,
	company varchar NOT NULL,
	email varchar NULL,
	description varchar NULL,
	CONSTRAINT contact_pk PRIMARY KEY (contact_id)
);

INSERT INTO public.contact (contact_id,first_name,last_name,middle_name,company,email,description) VALUES 
('6c7b66fb-d8cb-493b-a2fe-96963f81e43f','Ut enim ad minim veniam, quis nos','Duis aute irure dolor in reprehenderit in vo','ipsum dolor sit amet, c','nisi ut aliquip ex ea commodo consequa','veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo con','m')
,('78d616a1-9e0c-4c07-a089-9d64ea551dfb','dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul','laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incid','consequat. Duis aute irure dolor in reprehender','sunt in culpa qui officia deserunt','adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna ali','consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magn')
,('0e579d7a-9508-4ec8-931a-fe0d81009333','proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lor','ex ea commodo consequat. Duis aute irure dolor ','Duis aute irure dolor in reprehenderit ','anim id est laborum.Lorem ipsum dolor si','minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. D','aliquip ex')
,('ea9cdfd4-8d0f-4a1a-b781-45099266d18c','in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat','dolore magna aliqua. U','consectetur adipiscing elit, sed do eiusmod tempor ','Duis aute irure dolor in reprehenderit in voluptate velit esse ci','velit esse cillum ','quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea')
,('3f25d96b-39f2-4593-9b61-97b4e266d240','dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercit','ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis ','ullamco laboris nisi ut aliquip ex ea commodo consequat. ','cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est ','ut labore et dolore magna aliqua. Ut enim ','sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ips')
,('3ca5fa4e-33be-4cda-8b5c-e747ccacde9d','sed do eiusmod tempor incididunt ut labore et dolore magna aliqu','labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco la','aute irure dolor in reprehenderit in voluptate velit esse cillum dolore ','tempor incididunt ut labore et dolore magna aliqua. ','do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut','sit amet, consectetur adipiscing elit, sed do ')
,('9c8d239f-d3be-4c16-ba54-be56d0d215ba','in reprehende','tempor incididu','non proident, sunt in','ip','par','incididunt ')
,('7ee639b2-c09f-40f9-b05c-fccf2dd39d2b','ex ea commodo conse','qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consect','cupidatat n','pariatur. Excepteur sint occaecat cupidatat non ','dolore magna aliqua. Ut enim ad minim veniam, quis nostrud','ex ea commodo consequat. Duis aute irure dolor in reprehenderit in vo')
,('4865a75d-9b2b-4b23-85fa-4f3c0a17db2d','Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolor','ea c','id est lab','dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolo','eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad min','nulla paria')
,('e7312d28-b5da-426a-ae81-b3dcc8daf49c','consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna','adipiscing elit, sed do eiusmod tem','tempor incididunt ut labore et dolore magna aliqua. Ut enim ad','qui offic','magna aliqua. Ut enim ad minim veniam, quis nost','sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a')
;