using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class DataAccessLayerGenerator : IDataAccessLayerGenerator
    {
        protected StringBuilder code;

        protected DataAccessLayerGenerator(StringBuilder code)
        {
            this.code = code;
        }

        protected void GetReaderValues(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (column.Value.IsValueType)
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = (" + column.Value + "?) (stream[\"" + column.Key + "\"] is System.DBNull ? null : stream[\"" + column.Key + "\"]);");
                else
                    code.AppendLine("\t\t\t\t\t\titem." + column.Key + " = stream[\"" + column.Key + "\"] as " + column.Value + ";");
            }
        }

        public abstract void GenerateSelectAll(Table table);
        public abstract void GenerateSelectTop(Table table);
        public abstract void GenerateCreateIgnoringPrimaryKey(Table table);
        public abstract void GenerateCreateUsingAllColumns(Table table);
        public abstract void GenerateDelete(Table table);
        public abstract void GenerateDeleteAll(Table table);
        public abstract void GenerateSaveChanges(Table table);
    }
}
