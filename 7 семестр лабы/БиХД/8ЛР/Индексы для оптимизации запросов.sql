--статус читателя
-- Ускоряет поиск выдач конкретного читателя (вместо перебора 20,000 строк)
CREATE INDEX idx_issue_reader_id ON issue(readerid);

-- Ускоряет поиск книг внутри конкретной выдачи (вместо перебора 40,000 строк)
CREATE INDEX idx_issue_item_issue_id ON issue_item(issueid);

-- Ускоряет проверку штрафов. 
-- WHERE payment_date IS NULL делает индекс крошечным и очень быстрым (частичный индекс).
CREATE INDEX idx_fine_reader_payment ON fine(readerid) WHERE payment_date IS NULL;

--Для жанров

-- Ускоряет соединение таблицы книг с жанрами
CREATE INDEX idx_book_genre_id ON book(genreid);

-- Ускоряет соединение позиций выдачи с книгами (самая большая таблица issue_item)
CREATE INDEX idx_issue_item_book_id ON issue_item(bookid);

-- Ускоряет фильтрацию выдач по дате (у вас там условие request_date BETWEEN...)
CREATE INDEX idx_issue_request_date ON issue(request_date);

--Для должников
-- Ускоряет поиск штрафов по дате назначения (для группировки по месяцам)
CREATE INDEX idx_fine_imposition_date ON fine(imposition_date);

