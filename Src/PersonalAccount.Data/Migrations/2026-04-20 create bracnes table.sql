-- Скрипт миграции
-- 2026-04-20

-- Создаем новую структуру
create table if not exists branches
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    company_id uuid NOT NULL,
    name text COLLATE pg_catalog."default",
    load_options jsonb,
    CONSTRAINT branches_pkey PRIMARY KEY (id)
);

alter table branches
add constraint branches_company_id_fk FOREIGN KEY (company_id)
references companies (id)
on update no action
on delete no action;

-- Копируем данные
insert into branches(company_id, name, load_options)
select id, name, null as load_options from companies;

-- Меняем структуру
truncate table journal;
alter table transactions add branch_id uuid not null;
update transactions set company_id = null;
update transactions set branch_id = (select id from branches limit 1);
alter table transactions drop column company_id;

alter table journal add branch_id uuid not null;
