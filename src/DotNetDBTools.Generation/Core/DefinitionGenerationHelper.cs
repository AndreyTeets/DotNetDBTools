using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core;

internal static class DefinitionGenerationHelper
{
    public static string DeclareDataType(DataType dataType)
    {
        if (dataType.IsUserDefined)
            return $@"new Types.{dataType.Name}()";
        else
            return $@"new VerbatimDataType({DeclareString(dataType.Name)})";
    }

    public static string DeclareDefaultValue(CodePiece codePiece)
    {
        if (codePiece.Code is not null)
            return $@"new VerbatimDefaultValue({DeclareString(codePiece.Code)})";
        else
            return "null";
    }

    public static string DeclareString(string val)
    {
        if (val is not null)
            return $"@\"{val.Replace("\"", "\"\"")}\"";
        else
            return "null";
    }

    public static string DeclareBool(bool val)
    {
        return val.ToString().ToLower();
    }
}
