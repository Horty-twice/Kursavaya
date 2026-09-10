WITH table_list AS (
    SELECT * FROM (VALUES 
        ('library'), ('reader'), ('book'), ('author'), ('genre'), ('book_author'),
        ('book_copy'), ('employee'), ('issue'), ('issue_item'), ('purchase_request'),
        ('purchase_request_item'), ('supply'), ('supply_item'), ('fine'), ('manager_request'),
        ('reader_status'), ('issue_status'), ('issue_item_status'), ('book_copy_status'),
        ('purchase_request_status'), ('purchase_request_item_status'), ('supply_status'),
        ('supply_item_status'), ('manager_request_status'), ('fine_status'), ('fine_reason'),
        ('position'), ('supplier')
    ) AS t(tablename)
),
table_stats AS (
    SELECT 
        tablename,
        CASE tablename
            WHEN 'library' THEN (SELECT COUNT(*) FROM library)
            WHEN 'reader' THEN (SELECT COUNT(*) FROM reader)
            WHEN 'book' THEN (SELECT COUNT(*) FROM book)
            WHEN 'author' THEN (SELECT COUNT(*) FROM author)
            WHEN 'genre' THEN (SELECT COUNT(*) FROM genre)
            WHEN 'book_author' THEN (SELECT COUNT(*) FROM book_author)
            WHEN 'book_copy' THEN (SELECT COUNT(*) FROM book_copy)
            WHEN 'employee' THEN (SELECT COUNT(*) FROM employee)
            WHEN 'issue' THEN (SELECT COUNT(*) FROM issue)
            WHEN 'issue_item' THEN (SELECT COUNT(*) FROM issue_item)
            WHEN 'purchase_request' THEN (SELECT COUNT(*) FROM purchase_request)
            WHEN 'purchase_request_item' THEN (SELECT COUNT(*) FROM purchase_request_item)
            WHEN 'supply' THEN (SELECT COUNT(*) FROM supply)
            WHEN 'supply_item' THEN (SELECT COUNT(*) FROM supply_item)
            WHEN 'fine' THEN (SELECT COUNT(*) FROM fine)
            WHEN 'manager_request' THEN (SELECT COUNT(*) FROM manager_request)
            WHEN 'reader_status' THEN (SELECT COUNT(*) FROM reader_status)
            WHEN 'issue_status' THEN (SELECT COUNT(*) FROM issue_status)
            WHEN 'issue_item_status' THEN (SELECT COUNT(*) FROM issue_item_status)
            WHEN 'book_copy_status' THEN (SELECT COUNT(*) FROM book_copy_status)
            WHEN 'purchase_request_status' THEN (SELECT COUNT(*) FROM purchase_request_status)
            WHEN 'purchase_request_item_status' THEN (SELECT COUNT(*) FROM purchase_request_item_status)
            WHEN 'supply_status' THEN (SELECT COUNT(*) FROM supply_status)
            WHEN 'supply_item_status' THEN (SELECT COUNT(*) FROM supply_item_status)
            WHEN 'manager_request_status' THEN (SELECT COUNT(*) FROM manager_request_status)
            WHEN 'fine_status' THEN (SELECT COUNT(*) FROM fine_status)
            WHEN 'fine_reason' THEN (SELECT COUNT(*) FROM fine_reason)
            WHEN 'position' THEN (SELECT COUNT(*) FROM position)
            WHEN 'supplier' THEN (SELECT COUNT(*) FROM supplier)
            ELSE 0
        END AS record_count,
        pg_total_relation_size(tablename::regclass) AS total_bytes,
        pg_relation_size(tablename::regclass) AS data_bytes,
        pg_indexes_size(tablename::regclass) AS indexes_bytes
    FROM table_list
)
SELECT 
    "Таблица",
    "Кол-во записей",
    "Размер строки (байт)",
    "Данные (МБ)",
    "Индексы (МБ)",
    "Всего (МБ)"
FROM (
    -- Таблицы
    SELECT 
        1 AS sort_order,
        tablename AS "Таблица",
        record_count AS "Кол-во записей",
        CASE WHEN record_count > 0 
             THEN ROUND(data_bytes::numeric / record_count, 2)
             ELSE 0 
        END AS "Размер строки (байт)",
        ROUND(data_bytes / 1048576.0, 2) AS "Данные (МБ)",
        ROUND(indexes_bytes / 1048576.0, 2) AS "Индексы (МБ)",
        ROUND(total_bytes / 1048576.0, 2) AS "Всего (МБ)"
    FROM table_stats
    
    UNION ALL
    
    -- Итоговая строка (с правильными расчетами)
    SELECT 
        2 AS sort_order,
        'ИТОГО' AS "Таблица",
        SUM(record_count) AS "Кол-во записей",
        NULL AS "Размер строки (байт)",  -- Пусто для итогов
        ROUND(SUM(data_bytes) / 1048576.0, 2) AS "Данные (МБ)",
        ROUND(SUM(indexes_bytes) / 1048576.0, 2) AS "Индексы (МБ)",
        ROUND(SUM(data_bytes + indexes_bytes) / 1048576.0, 2) AS "Всего (МБ)"
    FROM table_stats
) AS combined
ORDER BY sort_order, "Всего (МБ)" DESC;