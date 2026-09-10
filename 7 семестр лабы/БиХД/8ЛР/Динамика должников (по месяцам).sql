EXPLAIN ANALYZE
SELECT 
    TO_CHAR(i.planned_return_date, 'YYYY-MM') AS "Месяц возврата",
    COUNT(DISTINCT i.readerid) AS "Количество должников",
    COUNT(ii.id) AS "Невозвращенные книги (шт.)"
FROM issue i
JOIN issue_item ii ON ii.issueid = i.id
WHERE i.planned_return_date < CURRENT_DATE 
  AND ii.actual_return_date IS NULL 
  AND i.planned_return_date >= (CURRENT_DATE - INTERVAL '1 year')
GROUP BY TO_CHAR(i.planned_return_date, 'YYYY-MM')
ORDER BY "Месяц возврата" DESC;
