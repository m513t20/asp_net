DROP TABLE IF EXISTS journal;

CREATE TABLE IF NOT EXISTS "journal" (
	"id" serial NOT NULL UNIQUE,
	"reciept_number" varchar(20) NOT NULL,
	"employee_id" bigint,
    "employee_name" varchar(255) NOT NULL,
	"nomenclature_id" bigint NOT NULL,
    "nomenclature_name" varchar(255) NOT NULL,
	"description" varchar(255) NOT NULL,
	"category_id" bigint NOT NULL,
    "category_name" varchar(255) NOT NULL,
	"transaction_id" bigint NOT NULL,
	"transaction_date" timestamp without time zone NOT NULL,
	"amount" bigint NOT NULL,
	"total" bigint NOT NULL,
	"discount" bigint NOT NULL,
	PRIMARY KEY ("id")
);

ALTER TABLE "journal" ADD CONSTRAINT "journal_fk2" FOREIGN KEY ("employee_id") REFERENCES "employers"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk3" FOREIGN KEY ("nomenclature_id") REFERENCES "nomenclature"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk5" FOREIGN KEY ("category_id") REFERENCES "categories"("id");
ALTER TABLE "journal" ADD CONSTRAINT "journal_fk6" FOREIGN KEY ("transaction_id") REFERENCES "transactions"("id");
