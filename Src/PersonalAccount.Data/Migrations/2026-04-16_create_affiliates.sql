-- создание таблицы филиалов
 create table affiliates
(
    id uuid not null primary key DEFAULT gen_random_uuid(),
    company_id uuid,
    inn text,
    name text,
    address text, 
    load_options jsonb
);

alter table affiliates
add constraint affiliates_company_id_fk 
foreign key (company_id)
references companies(id);

-- добавление полей для филиалов в транзакции

-- номенклатура
alter table "public"."categories" 
add column affiliate_id uuid;

alter table categories
add constraint categories_affiliate_id_fk 
foreign key (affiliate_id)
references affiliates(id);

-- сотрудники
alter table "public"."emploees" 
add column affiliate_id uuid;

alter table emploees
add constraint emploees_affiliate_id_fk 
foreign key (affiliate_id)
references affiliates(id);

-- транзакция
alter table "public"."transactions" 
add column affiliate_id uuid;

alter table transactions
add constraint transactions_affiliate_id_fk
foreign key (affiliate_id)
references affiliates(id);