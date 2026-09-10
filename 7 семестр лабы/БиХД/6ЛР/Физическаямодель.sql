-- Справочные таблицы
create table library (
  id              serial not null, 
  name            varchar(255) not null unique, 
  street          varchar(100) not null, 
  building_number varchar(10) not null, 
  phone           varchar(20) not null unique, 
  primary key (id)
);

create table genre (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table position (
  id   serial not null, 
  name varchar(100) not null unique, 
  primary key (id)
);

create table author (
  id          serial not null, 
  last_name   varchar(100) not null, 
  first_name  varchar(100) not null, 
  middle_name varchar(100), 
  primary key (id)
);

create table supplier (
  id              serial not null, 
  name            varchar(200) not null, 
  city            varchar(50) not null, 
  street          varchar(100) not null, 
  building_number varchar(10) not null, 
  phone           varchar(20) not null unique, 
  rating          int4 default 3 not null, 
  primary key (id)
);

-- Таблицы статусов
create table reader_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table issue_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table issue_item_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table book_copy_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table purchase_request_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table purchase_request_item_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table supply_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table supply_item_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table manager_request_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table fine_status (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

create table fine_reason (
  id   serial not null, 
  name varchar(50) not null unique, 
  primary key (id)
);

-- Основные таблицы
create table reader (
  id                serial not null, 
  last_name         varchar(100) not null, 
  first_name        varchar(100) not null, 
  middle_name       varchar(100), 
  phone             varchar(20) not null unique, 
  password          varchar(255) not null, 
  registration_date date default CURRENT_DATE not null, 
  reader_statusid   int4 not null, 
  libraryid         int4 not null, 
  primary key (id)
);

create table book (
  id                 serial not null, 
  title              varchar(255) not null unique, 
  total_quantity     int4 not null check(total_quantity >= 0), 
  available_quantity int4 not null check(available_quantity >= 0), 
  genreid            int4 not null, 
  primary key (id)
);

create table book_copy (
  id                 serial not null, 
  inventory_number   varchar(50) not null unique, 
  arrival_date       date default CURRENT_DATE not null, 
  bookid             int4 not null, 
  book_copy_statusid int4 not null, 
  primary key (id)
);

create table employee (
  id              serial not null, 
  last_name       varchar(100) not null, 
  first_name      varchar(100) not null, 
  middle_name     varchar(100) not null, 
  employment_date date not null, 
  positionid      int4 not null, 
  libraryid       int4 not null, 
  primary key (id)
);

create table issue (
  id             serial not null, 
  request_date   date default CURRENT_DATE not null, 
  planned_return_date date, 
  readerid       int4 not null, 
  issue_statusid int4 not null, 
  employeeid     int4 not null, 
  primary key (id)
);

create table issue_item (
  id                  serial not null, 
  actual_return_date  date, 
  issueid             int4 not null, 
  issue_item_statusid int4 not null, 
  bookid              int4 not null, 
  primary key (id)
);

create table purchase_request (
  id                        serial not null, 
  creation_date             date default CURRENT_DATE not null, 
  supplierid                int4 not null, 
  purchase_request_statusid int4 not null, 
  primary key (id)
);

create table purchase_request_item (
  id                             serial not null, 
  quantity                       int4 not null check(quantity > 0), 
  purchase_request_item_statusid int4 not null, 
  purchase_requestid             int4 not null, 
  bookid                         int4 not null, 
  primary key (id)
);

create table supply (
  id              serial not null, 
  order_date      date default CURRENT_DATE not null, 
  delivery_date   date not null, 
  total_cost      numeric(12, 2) not null, 
  supplierid      int4 not null, 
  supply_statusid int4 not null, 
  primary key (id)
);

create table supply_item (
  id                      serial not null, 
  quantity                int4 not null check(quantity >= 0), 
  price_per_unit          numeric(10, 2) not null check(price_per_unit >= 0), 
  supplyid                int4 not null, 
  supply_item_statusid    int4 not null, 
  bookid                  int4 not null, 
  purchase_request_itemid int4 not null, 
  primary key (id)
);

create table fine (
  id              serial not null, 
  amount          numeric(10, 2) not null check(amount >= 0), 
  imposition_date date default CURRENT_DATE not null, 
  payment_date    date, 
  readerid        int4 not null, 
  issue_itemid    int4 not null, 
  fine_statusid   int4 not null, 
  fine_reasonid   int4 not null, 
  primary key (id)
);

create table manager_request (
  id                       serial not null, 
  creation_date            date default CURRENT_DATE not null, 
  employeeid               int4 not null, 
  manager_request_statusid int4 not null, 
  bookid                   int4 not null, 
  primary key (id)
);

create table book_author (
  bookid   int4 not null, 
  authorid int4 not null, 
  primary key (bookid, authorid)
);

-- Внешние ключи с явными стратегиями ссылочной целостности
alter table reader 
  add constraint FKreader264946 
  foreign key (reader_statusid) 
  references reader_status (id) 
  on delete restrict 
  on update cascade;

alter table reader 
  add constraint FKreader5851 
  foreign key (libraryid) 
  references library (id) 
  on delete restrict 
  on update cascade;

alter table book 
  add constraint FKbook500794 
  foreign key (genreid) 
  references genre (id) 
  on delete restrict 
  on update cascade;

alter table book_copy 
  add constraint FKbook_copy961686 
  foreign key (bookid) 
  references book (id) 
  on delete restrict 
  on update cascade;

alter table book_copy 
  add constraint FKbook_copy587427 
  foreign key (book_copy_statusid) 
  references book_copy_status (id) 
  on delete restrict 
  on update cascade;

alter table employee 
  add constraint FKemployee650198 
  foreign key (positionid) 
  references position (id) 
  on delete restrict 
  on update cascade;

alter table employee 
  add constraint FKemployee514608 
  foreign key (libraryid) 
  references library (id) 
  on delete restrict 
  on update cascade;

alter table issue 
  add constraint FKissue492432 
  foreign key (readerid) 
  references reader (id) 
  on delete restrict 
  on update cascade;

alter table issue 
  add constraint FKissue236550 
  foreign key (issue_statusid) 
  references issue_status (id) 
  on delete restrict 
  on update cascade;

alter table issue 
  add constraint FKissue18721 
  foreign key (employeeid) 
  references employee (id) 
  on delete restrict 
  on update cascade;

alter table issue_item 
  add constraint FKissue_item657321 
  foreign key (issueid) 
  references issue (id) 
  on delete cascade 
  on update cascade;

alter table issue_item 
  add constraint FKissue_item139245 
  foreign key (issue_item_statusid) 
  references issue_item_status (id) 
  on delete restrict 
  on update cascade;

alter table issue_item 
  add constraint FKissue_item694591 
  foreign key (bookid) 
  references book (id) 
  on delete restrict 
  on update cascade;

alter table purchase_request 
  add constraint FKpurchase_r909394 
  foreign key (supplierid) 
  references supplier (id) 
  on delete restrict 
  on update cascade;

alter table purchase_request 
  add constraint FKpurchase_r536362 
  foreign key (purchase_request_statusid) 
  references purchase_request_status (id) 
  on delete restrict 
  on update cascade;

alter table purchase_request_item 
  add constraint FKpurchase_r643340 
  foreign key (purchase_request_item_statusid) 
  references purchase_request_item_status (id) 
  on delete restrict 
  on update cascade;

alter table purchase_request_item 
  add constraint FKpurchase_r25238 
  foreign key (purchase_requestid) 
  references purchase_request (id) 
  on delete cascade 
  on update cascade;

alter table purchase_request_item 
  add constraint FKpurchase_r396518 
  foreign key (bookid) 
  references book (id) 
  on delete restrict 
  on update cascade;

alter table supply 
  add constraint FKsupply26053 
  foreign key (supplierid) 
  references supplier (id) 
  on delete restrict 
  on update cascade;

alter table supply 
  add constraint FKsupply23249 
  foreign key (supply_statusid) 
  references supply_status (id) 
  on delete restrict 
  on update cascade;

alter table supply_item 
  add constraint FKsupply_ite394142 
  foreign key (supplyid) 
  references supply (id) 
  on delete cascade 
  on update cascade;

alter table supply_item 
  add constraint FKsupply_ite653301 
  foreign key (supply_item_statusid) 
  references supply_item_status (id) 
  on delete restrict 
  on update cascade;

alter table supply_item 
  add constraint FKsupply_ite528158 
  foreign key (bookid) 
  references book (id) 
  on delete restrict 
  on update cascade;

alter table supply_item 
  add constraint FKsupply_ite368767 
  foreign key (purchase_request_itemid) 
  references purchase_request_item (id) 
  on delete restrict 
  on update cascade;

alter table fine 
  add constraint FKfine859344 
  foreign key (readerid) 
  references reader (id) 
  on delete restrict 
  on update cascade;

alter table fine 
  add constraint FKfine6543 
  foreign key (issue_itemid) 
  references issue_item (id) 
  on delete restrict 
  on update cascade;

alter table fine 
  add constraint FKfine160352 
  foreign key (fine_statusid) 
  references fine_status (id) 
  on delete restrict 
  on update cascade;

alter table fine 
  add constraint FKfine244827 
  foreign key (fine_reasonid) 
  references fine_reason (id) 
  on delete restrict 
  on update cascade;

alter table manager_request 
  add constraint FKmanager_re940456 
  foreign key (employeeid) 
  references employee (id) 
  on delete restrict 
  on update cascade;

alter table manager_request 
  add constraint FKmanager_re173053 
  foreign key (manager_request_statusid) 
  references manager_request_status (id) 
  on delete restrict 
  on update cascade;

alter table manager_request 
  add constraint FKmanager_re167454 
  foreign key (bookid) 
  references book (id) 
  on delete restrict 
  on update cascade;

alter table book_author 
  add constraint FKbook_autho786924 
  foreign key (bookid) 
  references book (id) 
  on delete cascade 
  on update cascade;

alter table book_author 
  add constraint FKbook_autho683369 
  foreign key (authorid) 
  references author (id) 
  on delete cascade 
  on update cascade;

-- Индексы для внешних ключей
create index idx_reader_statusid on reader(reader_statusid);
create index idx_reader_libraryid on reader(libraryid);
create index idx_book_genreid on book(genreid);
create index idx_book_copy_bookid on book_copy(bookid);
create index idx_book_copy_statusid on book_copy(book_copy_statusid);
create index idx_employee_positionid on employee(positionid);
create index idx_employee_libraryid on employee(libraryid);
create index idx_issue_readerid on issue(readerid);
create index idx_issue_statusid on issue(issue_statusid);
create index idx_issue_employeeid on issue(employeeid);
create index idx_issue_item_issueid on issue_item(issueid);
create index idx_issue_item_statusid on issue_item(issue_item_statusid);
create index idx_issue_item_bookid on issue_item(bookid);
create index idx_purchase_request_supplierid on purchase_request(supplierid);
create index idx_purchase_request_statusid on purchase_request(purchase_request_statusid);
create index idx_purchase_request_item_statusid on purchase_request_item(purchase_request_item_statusid);
create index idx_purchase_request_item_requestid on purchase_request_item(purchase_requestid);
create index idx_purchase_request_item_bookid on purchase_request_item(bookid);
create index idx_supply_supplierid on supply(supplierid);
create index idx_supply_statusid on supply(supply_statusid);
create index idx_supply_item_supplyid on supply_item(supplyid);
create index idx_supply_item_statusid on supply_item(supply_item_statusid);
create index idx_supply_item_bookid on supply_item(bookid);
create index idx_supply_item_request_itemid on supply_item(purchase_request_itemid);
create index idx_fine_readerid on fine(readerid);
create index idx_fine_issue_itemid on fine(issue_itemid);
create index idx_fine_statusid on fine(fine_statusid);
create index idx_fine_reasonid on fine(fine_reasonid);
create index idx_manager_request_employeeid on manager_request(employeeid);
create index idx_manager_request_statusid on manager_request(manager_request_statusid);
create index idx_manager_request_bookid on manager_request(bookid);
create index idx_book_author_authorid on book_author(authorid);

-- Индексы для часто используемых полей
create index idx_reader_phone on reader(phone);
create index idx_reader_last_name on reader(last_name);
create index idx_reader_registration_date on reader(registration_date);
create index idx_book_title on book(title);
create index idx_author_last_name on author(last_name);
create index idx_employee_last_name on employee(last_name);
create index idx_issue_request_date on issue(request_date);
create index idx_issue_planned_return_date on issue(planned_return_date);
create index idx_issue_item_actual_return_date on issue_item(actual_return_date);
create index idx_purchase_request_creation_date on purchase_request(creation_date);
create index idx_supply_order_date on supply(order_date);
create index idx_supply_delivery_date on supply(delivery_date);
create index idx_fine_imposition_date on fine(imposition_date);
create index idx_fine_payment_date on fine(payment_date);
create index idx_manager_request_creation_date on manager_request(creation_date);
create index idx_supplier_phone on supplier(phone);
create index idx_supplier_name on supplier(name);
create index idx_library_phone on library(phone);

-- Составные индексы для сложных запросов
create index idx_reader_name_search on reader(last_name, first_name);
create index idx_employee_name_search on employee(last_name, first_name);
create index idx_issue_dates on issue(request_date, planned_return_date);
create index idx_fine_dates on fine(imposition_date, payment_date);