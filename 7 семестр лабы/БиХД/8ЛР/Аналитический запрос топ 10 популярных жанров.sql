EXPLAIN ANALYZE
SELECT 
    g.name AS "Жанр",
    COUNT(ii.id) AS "Количество выдач",
    ROUND(COUNT(ii.id) * 100.0 / SUM(COUNT(ii.id)) OVER(), 2) AS "Доля спроса (%)"
FROM issue_item ii
JOIN book b ON ii.bookid = b.id
JOIN genre g ON b.genreid = g.id
JOIN issue i ON ii.issueid = i.id
WHERE i.request_date BETWEEN '2024-01-01' AND '2025-12-31' 
GROUP BY g.name
ORDER BY "Количество выдач" DESC
LIMIT 10;
