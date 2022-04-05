using System;
using System.Collections.Generic;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.PostgreSQL.Types
{
    public class MyEnumType1 : IEnumType
    {
        public Guid ID => new("9286CC1D-F0A5-4046-ADC0-B9AE298C6F91");
        public IEnumerable<string> AllowedValues => new[]
        {
            "Label1",
            "Label2",
        };
    }
}
