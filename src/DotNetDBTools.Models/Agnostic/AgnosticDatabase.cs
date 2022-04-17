using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic;

public class AgnosticDatabase : Database
{
    public AgnosticDatabase()
    {
        Kind = DatabaseKind.Agnostic;
        Tables = new List<AgnosticTable>();
        Views = new List<AgnosticView>();
    }
}
