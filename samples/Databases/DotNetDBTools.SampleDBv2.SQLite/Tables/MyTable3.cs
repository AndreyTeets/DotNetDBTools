﻿using System;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DataTypes;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable3 : ITable
    {
        public Guid ID => new("474CD761-2522-4529-9D20-2B94115F9626");

        public Column MyColumn1 = new("726F503A-D944-46EE-A0FF-6A2C2FAAB46E")
        {
            DataType = new IntDataType(),
            Nullable = false,
            Default = 333,
        };

        public Column MyColumn2 = new("169824E1-8B74-4B60-AF17-99656D6DBBEE")
        {
            DataType = new ByteDataType() { Length = 22 },
        };

        public UniqueConstraint UQ_MyTable3_MyColumns12 = new("FD288E38-35BA-4BB1-ACE3-597C99EF26C7")
        {
            Columns = new string[] { nameof(MyColumn1), nameof(MyColumn2) },
        };
    }
}
