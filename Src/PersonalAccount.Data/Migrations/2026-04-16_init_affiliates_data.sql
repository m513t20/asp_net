insert into affiliates(id, company_id, name, inn, address, load_options)
values('aae54725-0efc-42b8-a27d-a84f9a7257c5', '14e54725-0efc-42b8-a27d-a84f9a7257c5','TEST', '1234567890', 'г Москва, ул Тестовая, д 1', '{}');


-- Заполнения id филиалов

-- номенклатура
UPDATE "public"."categories" t1
SET "affiliate_id" = t2."id"
FROM "public"."affiliates" t2
WHERE t1."company_id" = t2."company_id";

-- сотрудники
UPDATE "public"."emploees" t1
SET "affiliate_id" = t2."id"
FROM "public"."affiliates" t2
WHERE t1."company_id" = t2."company_id";

-- транзакция
UPDATE "public"."transactions" t1
SET "affiliate_id" = t2."id"
FROM "public"."affiliates" t2
WHERE t1."company_id" = t2."company_id";

-- удаление колонки 
ALTER TABLE "public"."categories" 
DROP COLUMN "company_id";

ALTER TABLE "public"."emploees" 
DROP COLUMN "company_id";

ALTER TABLE "public"."transactions" 
DROP COLUMN "company_id";
