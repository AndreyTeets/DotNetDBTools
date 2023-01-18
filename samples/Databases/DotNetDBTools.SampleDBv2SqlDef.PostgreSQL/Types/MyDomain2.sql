--ID:#{2200D040-A892-43B5-9B5E-DB9F6458187F}#
CREATE DOMAIN "MyDomain2" AS "MyCompositeType1"
    NOT NULL
    DEFAULT '("some string", "{42.78, -4, 0}")';