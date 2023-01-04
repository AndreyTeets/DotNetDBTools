-- ===e_fts3.test===
SELECT * FROM docs WHERE hit;

SELECT optimize(t4) FROM t4 LIMIT 1;

SELECT optimize(t4) FROM t4 LIMIT 1