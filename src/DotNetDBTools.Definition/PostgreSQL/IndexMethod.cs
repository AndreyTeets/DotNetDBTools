namespace DotNetDBTools.Definition.PostgreSQL;

public enum IndexMethod
{
    BTree,
    Hash,
    GiST,
    SPGiST,
    GIN,
    BRIN,
}
