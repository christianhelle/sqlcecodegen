#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    public class Table
    {
        public Table()
        {
            Columns = new Dictionary<string, Column>();
            Indexes = new List<Index>();
            References = new List<ForeignKeyConstraint>();
            ReferencedBy = new List<ForeignKeyConstraint>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ClassName { get; set; }
        public Dictionary<string, Column> Columns { get; set; }
        public List<Index> Indexes { get; set; }
        public List<ForeignKeyConstraint> References { get; set; }
        public List<ForeignKeyConstraint> ReferencedBy { get; set; }
        public string PrimaryKeyColumnName { get; set; }
        public string PrimaryKeyColumnFieldName { get; set; }
        public string PrimaryKeyColumnDisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string FieldName { get; set; }
        public int? MaxLength { get; set; }
        public Type ManagedType { get; set; }
        public string DatabaseType { get; set; }
        public bool AllowsNull { get; set; }
        public bool IsPrimaryKey { get; set; }
        public long? AutoIncrement { get; set; }
        public long? AutoIncrementSeed { get; set; }
        public bool IsForeignKey { get; set; }
        public int Ordinal { get; set; }

        public override string ToString()
        {
            return ManagedType.ToString();
        }
    }

    public class Index
    {
        public Column Column { get; set; }
        public string Name { get; set; }
        public bool Unique { get; set; }
        public bool Clustered { get; set; }
    }

    public class ForeignKeyConstraint
    {
        public Column Column { get; set; }
        public string Name { get; set; }
        public Table ReferenceTable { get; set; }
        public Column ReferenceColumn { get; set; }
    }
}
