INSERT INTO load_types (id,name) VALUES
(1,'CSV'),
(2,'Json');

INSERT INTO transaction_types (id, name) VALUES
(101, 'PLU sale'),
(111, 'Write Off'),
(211, 'Cash'),
(216, 'Visa'),
(301, 'Login'),
(386, 'Job Start'),
(387, 'Job Finish'),
(501, 'Total');

INSERT INTO organisations (id, name, adress, load_options) VALUES
('5eefff0e-b7dc-4c07-9b71-9115873bb8f3','Булочная холодный прием', 'ул. Ленина, 10', '{}'),
('3da36fae-98fd-41ce-8c70-f6e80e23c6ff','Кафе у Вадимыча', 'пр. Мира, 15', '{}');

INSERT INTO categories (id, name, organisation_id) VALUES
(1, 'Напитки', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(2, 'Горячие блюда', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(3, 'Напитки', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
(4, 'Десерты', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
(5, 'Салаты', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');

-- Сотрудники (organisation_id integer)
INSERT INTO employers (id, name, phone, organisation_id) VALUES
(1, 'Иванов Иван', '88005553535', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(2, 'Петров Петр', '84444444444', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
(3, 'Сидорова Анна', '84445444444', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff'),
(4, 'Козлова Елена', '84444444544', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');

INSERT INTO nomenclature (id, category_id, measure_unit, name) VALUES
(1, 1, 'шт', 'Чай черный'),
(2, 1, 'шт', 'Кофе американо'),
(3, 2, 'шт', 'Борщ'),
(4, 2, 'шт', 'Котлета с пюре'),
(5, 3, 'шт', 'Сок апельсиновый'),
(6, 3, 'шт', 'Минеральная вода'),
(7, 4, 'шт', 'Тирамису'),
(8, 4, 'шт', 'Чизкейк'),
(9, 5, 'шт', 'Цезарь'),
(10, 5, 'шт', 'Греческий');

INSERT INTO transactions (id, type, employee_id, organisation_id, opened, closed) VALUES
(1, 211, 1, '5eefff0e-b7dc-4c07-9b71-9115873bb8f3', '2025-02-01 10:30:00', '2025-02-01 10:35:00'),
(2, 211, 2, '5eefff0e-b7dc-4c07-9b71-9115873bb8f3', '2025-02-01 12:15:00', '2025-02-01 12:20:00'),
(3, 211, 3, '3da36fae-98fd-41ce-8c70-f6e80e23c6ff', '2025-02-01 13:00:00', '2025-02-01 13:05:00'),
(4, 216, 4, '3da36fae-98fd-41ce-8c70-f6e80e23c6ff', '2025-02-01 14:00:00', '2025-02-01 14:02:00');

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

INSERT INTO users (id, login, password) VALUES
('6eefff0e-b7dc-4c07-9b71-9115873bb8f3', 'admin', 'admin'),
('7da36fae-98fd-41ce-8c70-f6e80e23c6ff', 'user', 'user');

INSERT INTO connections (user_id, organisation_id) VALUES
('6eefff0e-b7dc-4c07-9b71-9115873bb8f3', '5eefff0e-b7dc-4c07-9b71-9115873bb8f3'),
('7da36fae-98fd-41ce-8c70-f6e80e23c6ff', '3da36fae-98fd-41ce-8c70-f6e80e23c6ff');
