using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Definition;

internal static class CSharpDefinitionGenerationHelper
{
    public static string DeclareDataType(DataType dataType)
    {
        // TODO declare as CSharp types where possible (including user defined)
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
