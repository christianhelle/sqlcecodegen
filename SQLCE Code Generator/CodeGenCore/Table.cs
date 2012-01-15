using System;
using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
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
