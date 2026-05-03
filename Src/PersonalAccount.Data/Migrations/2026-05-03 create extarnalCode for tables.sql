-- Скрипт миграции
-- 2026-05-03

-- Добавляем связующий код
alter table "public"."categories" add external_code bigint;
alter table "public"."emploees" add external_code bigint;
alter table "public"."nomenclatures" add external_code bigint;

-- Добавляем индексы
create index categories_external_code_ix on "public"."categories"(external_code);
create index emploees_external_code_ix on "public"."emploees"(external_code);
create index nomenclatures_external_code_ix on "public"."nomenclatures"(external_code);