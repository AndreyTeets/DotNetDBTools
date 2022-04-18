using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal interface IDefaultValueConverter
{
    public CodePiece Convert(CSharpDefaultValue defaultValue);
}
