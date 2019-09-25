
CREATE TABLE contact (
	contact_id uuid NOT NULL,
  contact_type varchar NULL,
	first_name varchar NULL,
	last_name varchar NOT NULL,
  middle_name varchar NULL,
  title varchar NULL,
	company varchar NOT NULL,
	description varchar NULL,
  photo varchar NULL,
  email varchar NULL,
  mobile varchar NULL,
  work_phone varchar NULL,
  home_phone varchar NULL,
  mailing_street varchar NULL,
  mailing_country varchar NULL,
  mailing_city varchar NULL,
  mailing_zipcode varchar NULL,
  mailing_state varchar NULL,
	CONSTRAINT contact_pk PRIMARY KEY (contact_id)
);

INSERT INTO public.contact (contact_id,contact_type,first_name,last_name,middle_name,title,company,description,photo,email,mobile,work_phone,home_phone,mailing_street,mailing_country,mailing_city,mailing_zipcode,mailing_state) VALUES
('e92df611-e80c-48ee-94c2-77bbe72312ac','Contact','sunt i','ut labo','quis','commodo consequat. Duis aute','qui offi','enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi u','cupidatat non proident, sunt in culpa qui offi',NULL,'mollit anim id est laborum.Lorem ipsum dolor sit amet, consect','in','commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit es','enim ad minim veniam, quis nostrud exer','ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolor','sunt in culpa','ad minim ','aliqua. Ut enim ad minim veniam, qu')
,('c58f13d4-677d-49c6-8873-f35704e112f6','Contact','conse','adipis','ex ea c','consectetur adipiscing elit, ','dolor in reprehenderit in voluptate velit es','deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consect','consectetur a',NULL,'dolor sit amet, consectetur adipiscing elit, sed do eius','rum.Lorem ipsum dolor sit amet, consectet','Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id e','in voluptate velit esse cillum d','in culpa qu','Duis aute irure dolor in reprehenderit in voluptate ','cupidatat non proident, sunt in culpa qui officia deserun','aborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt u')
;
