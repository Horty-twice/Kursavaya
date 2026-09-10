SET session_replication_role = 'replica';

-- ============================================================
-- БЛОК 1: ТАБЛИЦА LIBRARY (БИБЛИОТЕКИ)
-- ============================================================
INSERT INTO library (name, street, building_number, phone) 
SELECT 
    'Библиотека №' || i,
    CASE (i % 10)
        WHEN 0 THEN 'Ленина'
        WHEN 1 THEN 'Пушкина'
        WHEN 2 THEN 'Гагарина'
        WHEN 3 THEN 'Советская'
        WHEN 4 THEN 'Мира'
        WHEN 5 THEN 'Кирова'
        WHEN 6 THEN 'Чехова'
        WHEN 7 THEN 'Толстого'
        WHEN 8 THEN 'Горького'
        WHEN 9 THEN 'Центральная'
    END,
    (10 + (i % 90))::text,
    '+7910' || lpad((1000000 + i)::text, 7, '0')
FROM generate_series(1, 50) AS i;


-- ============================================================
-- БЛОК 2: ТАБЛИЦА GENRE (ЖАНРЫ)
-- ============================================================
INSERT INTO genre (name) VALUES 
    ('Фантастика'), ('Детектив'), ('Роман'), ('Научная литература'),
    ('Историческая проза'), ('Приключения'), ('Поэзия'), ('Драма'),
    ('Детская литература'), ('Биография');


-- ============================================================
-- БЛОК 3: ТАБЛИЦА POSITION (ДОЛЖНОСТИ)
-- ============================================================
INSERT INTO position (name) VALUES 
    ('Библиотекарь'), 
    ('Работник книжного фонда'),
    ('Менеджер по поставкам');


-- ============================================================
-- БЛОК 4: ТАБЛИЦА READER_STATUS (СТАТУСЫ ЧИТАТЕЛЯ)
-- ============================================================
INSERT INTO reader_status (name) VALUES 
    ('Активный'), 
    ('Заблокирован');


-- ============================================================
-- БЛОК 5: ТАБЛИЦА ISSUE_STATUS (СТАТУСЫ ВЫДАЧИ)
-- ============================================================
INSERT INTO issue_status (name) VALUES 
    ('Активна'), 
    ('Завершена'), 
    ('Просрочена'), 
    ('Отменена');


-- ============================================================
-- БЛОК 6: ТАБЛИЦА ISSUE_ITEM_STATUS (СТАТУСЫ ПОЗИЦИЙ)
-- ============================================================
INSERT INTO issue_item_status (name) VALUES 
    ('Выдана'), 
    ('Возвращена');


-- ============================================================
-- БЛОК 7: ТАБЛИЦА BOOK_COPY_STATUS (СТАТУСЫ ЭКЗЕМПЛЯРОВ)
-- ============================================================
INSERT INTO book_copy_status (name) VALUES 
    ('Доступна'), 
    ('Выдана'), 
    ('Списана');


-- ============================================================
-- БЛОК 8: ТАБЛИЦА PURCHASE_REQUEST_STATUS (СТАТУСЫ ЗАЯВОК)
-- ============================================================
INSERT INTO purchase_request_status (name) VALUES 
    ('Новая'), 
    ('Одобрена'), 
    ('Выполнена'), 
    ('Отклонена');


-- ============================================================
-- БЛОК 9: ТАБЛИЦА PURCHASE_REQUEST_ITEM_STATUS (СТАТУСЫ ПОЗ.)
-- ============================================================
INSERT INTO purchase_request_item_status (name) VALUES 
    ('Новая'), 
    ('Одобрена'), 
    ('Выполнена'), 
    ('Отклонена');


-- ============================================================
-- БЛОК 10: ТАБЛИЦА SUPPLY_STATUS (СТАТУСЫ ПОСТАВОК)
-- ============================================================
INSERT INTO supply_status (name) VALUES 
    ('Ожидает'), 
    ('Получена'), 
    ('Проверена');


-- ============================================================
-- БЛОК 11: ТАБЛИЦА SUPPLY_ITEM_STATUS (СТАТУСЫ ПОЗ. ПОСТАВОК)
-- ============================================================
INSERT INTO supply_item_status (name) VALUES 
    ('Получена'), 
    ('Проверена');


-- ============================================================
-- БЛОК 12: ТАБЛИЦА MANAGER_REQUEST_STATUS (СТАТУСЫ ЗАЯВОК МЕН.)
-- ============================================================
INSERT INTO manager_request_status (name) VALUES 
    ('Новая'), 
    ('Отменена'), 
    ('Выполнена');


-- ============================================================
-- БЛОК 13: ТАБЛИЦА FINE_STATUS (СТАТУСЫ ШТРАФОВ)
-- ============================================================
INSERT INTO fine_status (name) VALUES 
    ('Активен'), 
    ('Оплачен');


-- ============================================================
-- БЛОК 14: ТАБЛИЦА FINE_REASON (ПРИЧИНЫ ШТРАФОВ)
-- ============================================================
INSERT INTO fine_reason (name) VALUES 
    ('Просрочка возврата'), 
    ('Порча книги');


-- ============================================================
-- БЛОК 15: ТАБЛИЦА SUPPLIER (ПОСТАВЩИКИ)
-- ============================================================
INSERT INTO supplier (name, city, street, building_number, phone, rating)
SELECT 
    'Поставщик ' || i,
    CASE (i % 5)
        WHEN 0 THEN 'Москва'
        WHEN 1 THEN 'Санкт-Петербург'
        WHEN 2 THEN 'Пенза'
        WHEN 3 THEN 'Самара'
        WHEN 4 THEN 'Казань'
    END,
    'Улица Поставщиков',
    (i % 100)::text,
    '+7800' || lpad((1000000 + i)::text, 7, '0'),
    (random() * 2 + 3)::int
FROM generate_series(1, 100) AS i;


-- ============================================================
-- БЛОК 16: ТАБЛИЦА AUTHOR (АВТОРЫ)
-- ============================================================
INSERT INTO author (last_name, first_name, middle_name)
SELECT 
    'Авторов' || (i % 100),
    CASE (i % 10) WHEN 0 THEN 'Александр' WHEN 1 THEN 'Алексей' WHEN 2 THEN 'Андрей' WHEN 3 THEN 'Дмитрий' WHEN 4 THEN 'Сергей' WHEN 5 THEN 'Иван' WHEN 6 THEN 'Михаил' WHEN 7 THEN 'Владимир' WHEN 8 THEN 'Николай' WHEN 9 THEN 'Евгений' END,
    CASE (i % 10) WHEN 0 THEN 'Александрович' WHEN 1 THEN 'Алексеевич' WHEN 2 THEN 'Андреевич' WHEN 3 THEN 'Дмитриевич' WHEN 4 THEN 'Сергеевич' WHEN 5 THEN 'Иванович' WHEN 6 THEN 'Михайлович' WHEN 7 THEN 'Владимирович' WHEN 8 THEN 'Николаевич' WHEN 9 THEN 'Евгеньевич' END
FROM generate_series(1, 20000) AS i;


-- ============================================================
-- БЛОК 17: ТАБЛИЦА BOOK (КНИГИ)
-- ============================================================
INSERT INTO book (title, total_quantity, available_quantity, genreid)
SELECT 
    'Книга ' || i,
    (random() * 20 + 5)::int,
    (random() * 15 + 1)::int,
    (random() * 9 + 1)::int
FROM generate_series(1, 50000) AS i;


-- ============================================================
-- БЛОК 18: ТАБЛИЦА EMPLOYEE (СОТРУДНИКИ)
-- ============================================================
INSERT INTO employee (last_name, first_name, middle_name, employment_date, positionid, libraryid)
SELECT 
    'Сотрудников' || (i % 50),
    CASE (i % 10) WHEN 0 THEN 'Анна' WHEN 1 THEN 'Елена' WHEN 2 THEN 'Ольга' WHEN 3 THEN 'Татьяна' WHEN 4 THEN 'Ирина' WHEN 5 THEN 'Наталья' WHEN 6 THEN 'Мария' WHEN 7 THEN 'Светлана' WHEN 8 THEN 'Юлия' WHEN 9 THEN 'Екатерина' END,
    'Ивановна',
    CURRENT_DATE - (random() * 3650)::int,
    (random() * 2 + 1)::int,
    (random() * 49 + 1)::int
FROM generate_series(1, 500) AS i;


-- ============================================================
-- БЛОК 19: ТАБЛИЦА READER (ЧИТАТЕЛИ)
-- ============================================================
INSERT INTO reader (last_name, first_name, middle_name, phone, password, registration_date, reader_statusid, libraryid)
SELECT 
    'Читателев' || (i % 100),
    'Алексей', 'Петрович',
    '+7916' || lpad((1000000 + i)::text, 7, '0'),
    md5(random()::text),
    CURRENT_DATE - (random() * 365 * 3)::int,
    1,
    (random() * 49 + 1)::int
FROM generate_series(1, 10000) AS i;


-- ============================================================
-- БЛОК 20: ТАБЛИЦА BOOK_COPY (ЭКЗЕМПЛЯРЫ КНИГ)
-- ============================================================
INSERT INTO book_copy (inventory_number, arrival_date, bookid, book_copy_statusid)
SELECT 
    'INV-' || lpad(i::text, 8, '0'),
    CURRENT_DATE - (random() * 365 * 5)::int,
    (random() * 49999 + 1)::int,
    CASE (random() * 100)::int WHEN 0 THEN 3 WHEN 1 THEN 2 ELSE 1 END
FROM generate_series(1, 150000) AS i;


-- ============================================================
-- БЛОК 21: ТАБЛИЦА BOOK_AUTHOR (СВЯЗЬ КНИГА-АВТОР)
-- ============================================================
INSERT INTO book_author (bookid, authorid)
SELECT 
    (1 + floor(random() * 50000)::integer), 
    (1 + floor(random() * 20000)::integer)
FROM generate_series(1, 80000) ON CONFLICT DO NOTHING;


-- ============================================================
-- БЛОК 22: ТАБЛИЦА ISSUE (ВЫДАЧИ)
-- ============================================================
INSERT INTO issue (request_date, planned_return_date, readerid, issue_statusid, employeeid)
SELECT 
    CURRENT_DATE - (random() * 365)::int,
    CURRENT_DATE - (random() * 300)::int + 14,
    (random() * 9999 + 1)::int,
    1,
    (random() * 499 + 1)::int
FROM generate_series(1, 20000) AS i;


-- ============================================================
-- БЛОК 23: ТАБЛИЦА ISSUE_ITEM (ПОЗИЦИИ ВЫДАЧИ)
-- ВАЖНО: Реализация логики 80/20. Только 20% читателей нарушают.
-- ============================================================
DO $$
DECLARE
    book_ids int[];
BEGIN
    SELECT ARRAY(SELECT DISTINCT bookid FROM book_copy) INTO book_ids;

    INSERT INTO issue_item (actual_return_date, issueid, issue_item_statusid, bookid)
    SELECT 
        CASE 
            WHEN (((SELECT readerid FROM issue WHERE id = sub.issue_id_gen) % 5) = 0) AND (random() > 0.3) 
            THEN NULL -- "Плохой" читатель (20% базы)
            ELSE CURRENT_DATE - 1 -- "Хороший" читатель (80% базы)
        END,
        sub.issue_id_gen,
        CASE 
             WHEN (((SELECT readerid FROM issue WHERE id = sub.issue_id_gen) % 5) = 0) AND (random() > 0.3) 
             THEN 1 
             ELSE 2 
        END,
        book_ids[floor(random() * array_length(book_ids, 1) + 1)]
    FROM (
        SELECT (floor(random()*20000)+1)::int as issue_id_gen 
        FROM generate_series(1, 40000)
    ) as sub;
END $$;


-- ============================================================
-- БЛОК 24: ТАБЛИЦА FINE (ШТРАФЫ)
-- ВАЖНО: Штрафы назначаются только 20% читателей (ID кратно 5).
-- ============================================================
INSERT INTO fine (amount, imposition_date, payment_date, readerid, issue_itemid, fine_statusid, fine_reasonid)
SELECT 
    (random() * 5000 + 100)::numeric(10,2),
    CURRENT_DATE - 10,
    NULL, -- Не оплачен
    i * 5, -- ID: 5, 10, 15... (Только 20% базы)
    (floor(random()*40000)+1)::int,
    1, 
    1
FROM generate_series(1, 2000) AS i;


-- ============================================================
-- БЛОК 25: ТАБЛИЦА PURCHASE_REQUEST (ЗАЯВКИ НА ЗАКУПКУ)
-- ============================================================
INSERT INTO purchase_request (creation_date, supplierid, purchase_request_statusid)
SELECT 
    CURRENT_DATE - (random() * 365 * 2)::int,
    (random() * 99 + 1)::int,
    (random() * 3 + 1)::int
FROM generate_series(1, 5000) AS i;


-- ============================================================
-- БЛОК 26: ТАБЛИЦА PURCHASE_REQUEST_ITEM (ПОЗИЦИИ ЗАЯВОК)
-- ============================================================
INSERT INTO purchase_request_item (quantity, purchase_request_item_statusid, purchase_requestid, bookid)
SELECT 
    (random() * 20 + 1)::int,
    (random() * 3 + 1)::int,
    (random() * 4999 + 1)::int,
    (random() * 49999 + 1)::int
FROM generate_series(1, 15000) AS i;


-- ============================================================
-- БЛОК 27: ТАБЛИЦА SUPPLY (ПОСТАВКИ)
-- ============================================================
INSERT INTO supply (order_date, delivery_date, total_cost, supplierid, supply_statusid)
SELECT 
    CURRENT_DATE - (random() * 365 * 2)::int,
    CURRENT_DATE - (random() * 365 * 2 - 30)::int,
    (random() * 100000 + 1000)::numeric(12,2),
    (random() * 99 + 1)::int,
    CASE (random() * 100)::int WHEN 0 THEN 3 WHEN 1 THEN 1 ELSE 2 END
FROM generate_series(1, 3000) AS i;


-- ============================================================
-- БЛОК 28: ТАБЛИЦА SUPPLY_ITEM (ПОЗИЦИИ ПОСТАВОК)
-- ============================================================
INSERT INTO supply_item (quantity, price_per_unit, supplyid, supply_item_statusid, bookid, purchase_request_itemid)
SELECT 
    (random() * 50 + 1)::int,
    (random() * 1000 + 100)::numeric(10,2),
    (random() * 2999 + 1)::int,
    CASE (random() * 100)::int WHEN 0 THEN 1 ELSE 2 END,
    (random() * 49999 + 1)::int,
    (random() * 14999 + 1)::int
FROM generate_series(1, 10000) AS i;


-- ============================================================
-- БЛОК 29: ТАБЛИЦА MANAGER_REQUEST (ЗАЯВКИ МЕНЕДЖЕРА)
-- ============================================================
INSERT INTO manager_request (creation_date, employeeid, manager_request_statusid, bookid)
SELECT 
    CURRENT_DATE - (random() * 180)::int,
    (random() * 499 + 1)::int,
    (random() * 2 + 1)::int,
    (random() * 49999 + 1)::int
FROM generate_series(1, 2000) AS i;


SET session_replication_role = 'origin';

-- ЗАВЕРШЕНИЕ
DO $$
BEGIN
    RAISE NOTICE 'Все 29 таблиц успешно заполнены!';
END $$;


-- 16. ПРОВЕРКА И СТАТИСТИКА
----------------------------------------------------------------
-- Статистика по количеству записей в основных таблицах
DO $$
BEGIN
    RAISE NOTICE '================================================';
    RAISE NOTICE 'СТАТИСТИКА ЗАПОЛНЕНИЯ БАЗЫ ДАННЫХ';
    RAISE NOTICE '================================================';
    RAISE NOTICE 'Библиотеки: %', (SELECT COUNT(*) FROM library);
    RAISE NOTICE 'Читатели: %', (SELECT COUNT(*) FROM reader);
    RAISE NOTICE 'Книги (каталог): %', (SELECT COUNT(*) FROM book);
    RAISE NOTICE 'Авторы: %', (SELECT COUNT(*) FROM author);
    RAISE NOTICE 'Экземпляры книг: %', (SELECT COUNT(*) FROM book_copy);
    RAISE NOTICE 'Сотрудники: %', (SELECT COUNT(*) FROM employee);
    RAISE NOTICE 'Выдачи: %', (SELECT COUNT(*) FROM issue);
    RAISE NOTICE 'Позиции выдач: %', (SELECT COUNT(*) FROM issue_item);
    RAISE NOTICE 'Связи книги-авторы: %', (SELECT COUNT(*) FROM book_author);
    RAISE NOTICE 'Заявки на закупку: %', (SELECT COUNT(*) FROM purchase_request);
    RAISE NOTICE 'Позиции заявок: %', (SELECT COUNT(*) FROM purchase_request_item);
    RAISE NOTICE 'Поставки: %', (SELECT COUNT(*) FROM supply);
    RAISE NOTICE 'Позиции поставок: %', (SELECT COUNT(*) FROM supply_item);
    RAISE NOTICE 'Штрафы: %', (SELECT COUNT(*) FROM fine);
    RAISE NOTICE 'Заявки менеджера: %', (SELECT COUNT(*) FROM manager_request);
    RAISE NOTICE '================================================';
END $$;

-- Проверка целостности данных (простые проверки)
DO $$
BEGIN
    RAISE NOTICE '================================================';
    RAISE NOTICE 'ПРОВЕРКА ЦЕЛОСТНОСТИ ДАННЫХ';
    RAISE NOTICE '================================================';
    
    -- Проверка читателей без библиотек
    IF EXISTS (SELECT 1 FROM reader r LEFT JOIN library l ON r.libraryid = l.id WHERE l.id IS NULL) THEN
        RAISE NOTICE '✗ Найдены читатели без библиотек';
    ELSE
        RAISE NOTICE '✓ Все читатели имеют библиотеки';
    END IF;
    
    -- Проверка выданных книг без экземпляров
    IF EXISTS (SELECT 1 FROM issue_item ii LEFT JOIN book_copy bc ON ii.bookid = bc.bookid WHERE bc.id IS NULL) THEN
        RAISE NOTICE '✗ Найдены выданные книги без экземпляров';
    ELSE
        RAISE NOTICE '✓ Все выданные книги имеют экземпляры';
    END IF;
    
    -- Проверка штрафов без читателей
    IF EXISTS (SELECT 1 FROM fine f LEFT JOIN reader r ON f.readerid = r.id WHERE r.id IS NULL) THEN
        RAISE NOTICE '✗ Найдены штрафы без читателей';
    ELSE
        RAISE NOTICE '✓ Все штрафы имеют читателей';
    END IF;
    
    RAISE NOTICE '================================================';
END $$;

