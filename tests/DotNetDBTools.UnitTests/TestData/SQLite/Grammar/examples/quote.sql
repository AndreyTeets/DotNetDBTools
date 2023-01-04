-- ===quote.test===
SELECT '@abc'.'!pqr', '@abc'.'#xyz'+5 FROM '@abc';

UPDATE '@abc' SET '#xyz'=11;

SELECT '@abc'.'!pqr', '@abc'.'#xyz'+5 FROM '@abc';

DROP TABLE '@abc'