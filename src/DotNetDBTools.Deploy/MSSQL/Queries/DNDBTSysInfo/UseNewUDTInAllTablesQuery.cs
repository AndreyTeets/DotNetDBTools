using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class UseNewUDTInAllTablesQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public UseNewUDTInAllTablesQuery(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            _sql = GetSql(userDefinedTypeDiff);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            string query =
$@"DECLARE @SqlText NVARCHAR(MAX) =
(
    SELECT STUFF ((
        SELECT
            CHAR(10) + CHAR(10) + t.AlterStatement
        FROM
        (
            SELECT
                cci.DropConstraintStatement + CHAR(10) +
                N'ALTER TABLE ' + QUOTENAME(cci.TABLE_NAME) + ' ALTER COLUMN ' + QUOTENAME(cci.COLUMN_NAME) + ' ' + '[{userDefinedTypeDiff.NewUserDefinedType.Name}]' + ';' + CHAR(10) +
                cci.AddConstraintStatement AS AlterStatement
            FROM
            (
                SELECT
                    c.TABLE_NAME,
                    c.COLUMN_NAME,
                    ccu.CONSTRAINT_NAME,
                    tc.CONSTRAINT_TYPE,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N'ALTER TABLE ' + QUOTENAME(c.TABLE_NAME) + ' DROP CONSTRAINT ' + QUOTENAME(ccu.CONSTRAINT_NAME) + ';' + CHAR(10)
                    ELSE
                        ''
                    END AS DropConstraintStatement,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N'ALTER TABLE ' + QUOTENAME(c.TABLE_NAME) + ' ADD CONSTRAINT ' + QUOTENAME(ccu.CONSTRAINT_NAME) + ' ' + tc.CONSTRAINT_TYPE + '(' +
                        (SELECT STUFF ((
                            SELECT
                                ', ' + t.COLUMN_NAME
                            FROM
                            (
                                SELECT
                                    ccu2.COLUMN_NAME
                                FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu2
                                WHERE ccu2.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                            ) t FOR XML PATH('')), 1, 2, '')) +
                        ')' + ';'
                    ELSE
                        ''
                    END AS ADDConstraintStatement
                FROM INFORMATION_SCHEMA.COLUMNS c
                LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
                    ON ccu.COLUMN_NAME = c.COLUMN_NAME
                        AND ccu.TABLE_NAME = c.TABLE_NAME
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                    ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                        AND tc.CONSTRAINT_TYPE IN ('UNIQUE', 'PRIMARY KEY')
                WHERE c.DOMAIN_NAME = '{userDefinedTypeDiff.OldUserDefinedType.Name}'
            ) cci
        ) t FOR XML PATH('')), 1, 2, '')
);
EXEC (@SqlText);";

            return query;
        }
    }
}
