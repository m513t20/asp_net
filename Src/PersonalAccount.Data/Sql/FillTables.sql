INSERT INTO load_types (name) VALUES
('CSV'),
('Json');

INSERT INTO transaction_types (id, name) VALUES
(101, 'PLU sale'),
(111, 'Write Off'),
(211, 'Cash'),
(216, 'Visa'),
(301, 'Login'),
(386, 'Job Start'),
(387, 'Job Finish'),
(501, 'Total');

INSERT INTO organisations (id, name, adress) VALUES
('5eefff0e-b7dc-4c07-9b71-9115873bb8f3','Булочная холодный прием', 'ул. Ленина, 10'),
('3da36fae-98fd-41ce-8c70-f6e80e23c6ff','Кафе у Вадимыча', 'пр. Мира, 15');

INSERT INTO categories (name, organisation_id) VALUES
('Напитки', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
('Горячие блюда', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
('Напитки', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
('Десерты', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
('Салаты', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');

-- Сотрудники (organisation_id integer)
INSERT INTO employers (id, name, phone, organisation_id) VALUES
(1, 'Иванов Иван', '88005553535', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(2, 'Петров Петр', '84444444444', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(3, 'Сидорова Анна', '84445444444', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
(4, 'Козлова Елена', '84444444544', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');

INSERT INTO nomenclature (category_id, measure_unit, name) VALUES
(1, 'шт', 'Чай черный'),
(1, 'шт', 'Кофе американо'),
(2, 'шт', 'Борщ'),
(2, 'шт', 'Котлета с пюре'),
(3, 'шт', 'Сок апельсиновый'),
(3, 'шт', 'Минеральная вода'),
(4, 'шт', 'Тирамису'),
(4, 'шт', 'Чизкейк'),
(5, 'шт', 'Цезарь'),
( 5, 'шт', 'Греческий');

INSERT INTO transactions (type, employee_id, organisation_id, opened, closed) VALUES
(211, 1, '5eefff0e-b7dc-4c07-9b71-9115873bb8f3', '2025-02-01 10:30:00', '2025-02-01 10:35:00'),
(211, 2, '5eefff0e-b7dc-4c07-9b71-9115873bb8f3', '2025-02-01 12:15:00', '2025-02-01 12:20:00'),
(211, 3, '3da36fae-98fd-41ce-8c70-f6e80e23c6ff', '2025-02-01 13:00:00', '2025-02-01 13:05:00'),
(216, 4, '3da36fae-98fd-41ce-8c70-f6e80e23c6ff', '2025-02-01 14:00:00', '2025-02-01 14:02:00');

INSERT INTO journal (reciept_number, employee_id, nomenclature_id, description, category_id, transaction_id, transaction_date, amount, total, discount) VALUES
('ЧК001', 1, 1, 'Чай черный', 1, 1, '2025-02-01 10:30:00', 2, 100, 0),
('ЧК001', 1, 2, 'Кофе американо', 1, 1, '2025-02-01 10:30:00', 1, 150, 0),
('ЧК002', 2, 3, 'Борщ', 2, 2, '2025-02-01 12:15:00', 2, 300, 0),
('ЧК002', 2, 4, 'Котлета с пюре', 2, 2, '2025-02-01 12:15:00', 1, 250, 0),
('ЧК003', 3, 5, 'Сок', 3, 3, '2025-02-01 13:00:00', 2, 120, 0),
('ЧК003', 3, 7, 'Тирамису', 4, 3, '2025-02-01 13:00:00', 1, 200, 0),
('ВОЗ001', 4, 6, 'Возврат воды', 3, 4, '2025-02-01 14:00:00', -1, -50, 0);

INSERT INTO load_settings (pack_size, load_data_type, organisation_id) VALUES
(100, 1, '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(50, 2, '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');

