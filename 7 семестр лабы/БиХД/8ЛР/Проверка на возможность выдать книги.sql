EXPLAIN ANALYZE
SELECT 
    r.id AS "ID Читателя",
    r.last_name || ' ' || r.first_name AS "ФИО",
    CASE 
        WHEN (
            -- 1. ЕСТЬ ЛИ ПРОСРОЧКИ?
            -- (Книга не возвращена AND плановая дата возврата уже прошла)
            COUNT(DISTINCT CASE 
                WHEN ii.actual_return_date IS NULL 
                     AND i.planned_return_date < CURRENT_DATE 
                THEN ii.id END) > 0
            OR 
            -- 2. ЕСТЬ ЛИ НЕОПЛАЧЕННЫЕ ШТРАФЫ?
            -- (Сумма штрафов, где дата оплаты NULL, больше 0)
            COALESCE(SUM(f.amount), 0) > 0
            OR
            -- 3. ПРЕВЫШЕН ЛИ ЛИМИТ?
            -- (Более 3 активных выдач на руках)
            COUNT(DISTINCT CASE 
                WHEN ii.actual_return_date IS NULL 
                THEN i.id END) >= 3
        ) THEN 'ОТКАЗ'
        ELSE 'ОДОБРЕНО'
    END AS "Решение"
FROM reader r
LEFT JOIN issue i ON i.readerid = r.id
LEFT JOIN issue_item ii ON ii.issueid = i.id
LEFT JOIN fine f ON f.readerid = r.id AND f.payment_date IS NULL
WHERE r.id = 1  -- Проверяем конкретного читателя
GROUP BY r.id, r.last_name, r.first_name;