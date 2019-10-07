
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

INSERT INTO public.contact (contact_id,contact_type,first_name,last_name,middle_name,title,
  company,description,photo,email,mobile,work_phone,home_phone,mailing_street,mailing_country,
  mailing_city,mailing_zipcode,mailing_state) VALUES
('e92df611-e80c-48ee-94c2-77bbe72312ac','Contact','Mathilde','Russell',null,null,'MS',null,null,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)
,('c58f13d4-677d-49c6-8873-f35704e112f6','Contact','Walsh','Jetta',null,null,'Github',null,null,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)
;
