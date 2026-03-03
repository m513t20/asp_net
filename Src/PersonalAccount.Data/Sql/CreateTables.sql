CREATE TABLE IF NOT EXISTS "load_settings" (
	"id" uuid NOT NULL UNIQUE DEFAULT gen_random_uuid(),
	"pack_size" bigint NOT NULL,
	"load_data_type" bigint NOT NULL,
	"organisation_id" uuid NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "organisations" (
	"id" uuid NOT NULL UNIQUE DEFAULT gen_random_uuid(),
	"name" varchar(255) NOT NULL,
	"adress" varchar(255) NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "categories" (
	"id" serial NOT NULL UNIQUE,
	"name" varchar(255) NOT NULL,
	"organisation_id" uuid NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "employers" (
	"id" serial NOT NULL UNIQUE,
	"name" varchar(255) NOT NULL,
	"phone" varchar(11),
	"organisation_id" uuid NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "nomenclature" (
	"id" serial NOT NULL UNIQUE,
	"category_id" bigint NOT NULL,
	"measure_unit" varchar(100) NOT NULL,
	"name" varchar(255) NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "transactions" (
	"id" serial NOT NULL UNIQUE,
	"type" bigint,
	"employee_id" bigint,
	"organisation_id" uuid,
	"opened" timestamp without time zone NOT NULL,
	"closed" timestamp without time zone,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "journal" (
	"id" serial NOT NULL UNIQUE,
	"reciept_number" varchar(20) NOT NULL,
	"employee_id" bigint,
	"nomenclature_id" bigint NOT NULL,
	"description" varchar(255) NOT NULL,
	"category_id" bigint NOT NULL,
	"transaction_id" bigint NOT NULL,
	"transaction_date" timestamp without time zone NOT NULL,
	"amount" bigint NOT NULL,
	"total" bigint NOT NULL,
	"discount" bigint NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "load_types" (
	"id" serial NOT NULL UNIQUE,
	"name" varchar(255) NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "transaction_types" (
	"id" serial NOT NULL UNIQUE,
	"name" varchar(255) NOT NULL,
	PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "users" (
	"id" uuid NOT NULL UNIQUE DEFAULT gen_random_uuid(),
	"login" varchar(255) NOT NULL,
	"password" varchar(255) NOT NULL,
	PRIMARY KEY ("id")
);


CREATE TABLE IF NOT EXISTS "connections" (
	"id" uuid NOT NULL UNIQUE DEFAULT gen_random_uuid(),
	"user_id" uuid NOT NULL,
	"organisation_id" uuid NOT NULL,

	PRIMARY KEY ("id")
);


ALTER TABLE "load_settings" ADD CONSTRAINT "load_settings_fk2" FOREIGN KEY ("load_data_type") REFERENCES "load_types"("id");
ALTER TABLE "load_settings" ADD CONSTRAINT "load_settings_fk3" FOREIGN KEY ("organisation_id") REFERENCES "organisations"("id");

ALTER TABLE "categories" ADD CONSTRAINT "categories_fk2" FOREIGN KEY ("organisation_id") REFERENCES "organisations"("id");

ALTER TABLE "employers" ADD CONSTRAINT "employers_fk3" FOREIGN KEY ("organisation_id") REFERENCES "organisations"("id");

ALTER TABLE "nomenclature" ADD CONSTRAINT "nomenclature_fk1" FOREIGN KEY ("category_id") REFERENCES "categories"("id");

ALTER TABLE "transactions" ADD CONSTRAINT "transactions_fk1" FOREIGN KEY ("type") REFERENCES "transaction_types"("id");
ALTER TABLE "transactions" ADD CONSTRAINT "transactions_fk3" FOREIGN KEY ("employee_id") REFERENCES "employers"("id");
ALTER TABLE "transactions" ADD CONSTRAINT "transactions_fk4" FOREIGN KEY ("organisation_id") REFERENCES "organisations"("id");

ALTER TABLE "journal" ADD CONSTRAINT "journal_fk2" FOREIGN KEY ("employee_id") REFERENCES "employers"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk3" FOREIGN KEY ("nomenclature_id") REFERENCES "nomenclature"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk5" FOREIGN KEY ("category_id") REFERENCES "categories"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk6" FOREIGN KEY ("transaction_id") REFERENCES "transactions"("id");

ALTER TABLE "connections" ADD CONSTRAINT "connections_fk1" FOREIGN KEY ("user_id") REFERENCES "users"("id");
ALTER TABLE "connections" ADD CONSTRAINT "connections_fk2" FOREIGN KEY ("organisation_id") REFERENCES "organisations"("id");

