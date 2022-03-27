namespace DotNetDBTools.Definition.Core.CSharpDataTypes;

/// <summary>
/// Column is declared with a guid type appropriate to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'UNIQUEIDENTIFIER'.</item>
/// <item><term>MySQL</term> Column is declared as 'BINARY(16)'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'UUID'.</item>
/// <item><term>SQLite</term> Column is declared with 'BLOB' affinity.</item>
/// </list>
/// </summary>
public class GuidDataType : IDataType
{
}
