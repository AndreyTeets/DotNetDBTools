﻿using System;

namespace DotNetDBTools.Definition
{
    public abstract class BaseColumn : IDbObject
    {
        private readonly Guid _id;
        protected BaseColumn(string id)
        {
            _id = new Guid(id);
        }

        public Guid ID => _id;
        public IDbType Type { get; set; }
        public bool Nullable { get; set; }
        public bool Unique { get; set; }
        public bool Identity { get; set; }
        public object Default { get; set; }
    }
}