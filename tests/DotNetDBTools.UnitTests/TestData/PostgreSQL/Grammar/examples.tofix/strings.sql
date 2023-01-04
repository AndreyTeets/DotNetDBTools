SELECT 'tricky' AS U&"\" UESCAPE '!';

SELECT U&'wrong: +0061' UESCAPE +;
SELECT U&'wrong: +0061' UESCAPE '+';

SELECT 'tricky' AS U&"\" UESCAPE '!';

SELECT U&'wrong: +0061' UESCAPE '+';

select 'a\\bcd' as f1, 'a\\b\'cd' as f2, 'a\\b\'''cd' as f3, 'abcd\\'   as f4, 'ab\\\'cd' as f5, '\\\\' as f6;

select 'a\\bcd' as f1, 'a\\b\'cd' as f2, 'a\\b\'''cd' as f3, 'abcd\\'   as f4, 'ab\\\'cd' as f5, '\\\\' as f6;


SELECT U&'d\0061t\+000061' AS U&"d\0061t\+000061";
SELECT U&'d!0061t\+000061' UESCAPE '!' AS U&"d*0061t\+000061" UESCAPE '*';
SELECT U&'a\\b' AS "a\b";

SELECT U&' \' UESCAPE '!' AS "tricky";



SELECT U&'d\0061t\+000061' AS U&"d\0061t\+000061";
SELECT U&'d!0061t\+000061' UESCAPE '!' AS U&"d*0061t\+000061" UESCAPE '*';

SELECT U&' \' UESCAPE '!' AS "tricky";

