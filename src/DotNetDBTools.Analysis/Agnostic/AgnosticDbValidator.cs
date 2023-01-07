using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Agnostic;

internal class AgnosticDbValidator : DbValidator
{
    // TODO Convert to all posstible/supported dbms kinds and analyze them?

    protected override DbAnalysisContext BuildCurrentAnalysisContext(Database database)
    {
        return new DbAnalysisContext();
    }

    protected override bool DataTypeIsValid(DataType dataType, out string dataTypeErrorMessage)
    {
        if (dataType is not AgnosticVerbatimDataType && string.IsNullOrEmpty(dataType.Name))
            dataTypeErrorMessage = $"DataType is null or empty.";
        else if (dataType is AgnosticVerbatimDataType avdt && DataTypeNameIsNullOrEmptyForAllDbms(avdt))
            dataTypeErrorMessage = $"VerbatimDataType.Name is null or empty for all dbms types.";
        else
            dataTypeErrorMessage = null;

        return dataTypeErrorMessage is null;

        bool DataTypeNameIsNullOrEmptyForAllDbms(AgnosticVerbatimDataType avdt)
        {
            bool res = false;
            foreach (DatabaseKind dbKind in avdt.NameCodePiece.DbKindToCodeMap.Keys)
            {
                string specificDbmsDataTypeName = avdt.NameCodePiece.DbKindToCodeMap[dbKind];
                if (string.IsNullOrEmpty(specificDbmsDataTypeName))
                    res = true;
            }
            return res;
        }
    }
}
