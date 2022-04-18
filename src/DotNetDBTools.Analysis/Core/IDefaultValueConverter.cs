using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public interface IDefaultValueConverter
{
    CodePiece Convert(CSharpDefaultValue defaultValue);
}
