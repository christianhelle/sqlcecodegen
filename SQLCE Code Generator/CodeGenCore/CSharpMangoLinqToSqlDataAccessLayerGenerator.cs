using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoLinqToSqlDataAccessLayerGenerator : DataAccessLayerGenerator
    {
        public CSharpMangoLinqToSqlDataAccessLayerGenerator(StringBuilder code, Table table) : base(code, table)
        {
        }

        #region Overrides of DataAccessLayerGenerator

        public override void GenerateSelectAll()
        {
            Code.AppendLine("\t\t#region SELECT *");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves all items as a generic collection");
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\treturn DataContext." + Table.ClassName + ".ToList();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\tpublic " + Table.ClassName + "[] ToArray()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = ToList();");
            Code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
            foreach (var column in Table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                Code.AppendLine("\t\t#region SELECT .... WHERE " + column.Value.FieldName + "=?");
                Code.AppendLine();

                GenerateXmlDoc(2, "Retrieves a collection of items by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                Code.AppendFormat(
                    column.Value.ManagedType.IsValueType
                        ? "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2})"
                        : "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2})",
                    Table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                Code.AppendLine();
                Code.AppendLine("\t\t{");
                Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
                Code.AppendLine("\t\t#endregion");
                Code.AppendLine();
            }
        }

        public override void GenerateSelectWithTop()
        {
            Code.AppendLine("\t\t#region SELECT TOP()");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves the first set of items specified by count as a generic collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\tpublic " + Table.ClassName + "[] ToArray(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = ToList(count);");
            Code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in Table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                Code.AppendLine("\t\t#region SELECT TOP(?).... WHERE " + column.Value.FieldName + "=?");
                Code.AppendLine();

                GenerateXmlDoc(2, "Retrieves the first set of items specified by count by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"), new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
                Code.AppendFormat(
                    column.Value.ManagedType.IsValueType
                        ? "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count)"
                        : "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count)",
                    Table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                Code.AppendLine();
                Code.AppendLine("\t\t{");
                Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
                Code.AppendLine("\t\t#endregion");
                Code.AppendLine();
            }
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
            if (string.IsNullOrEmpty(Table.PrimaryKeyColumnName))
                return;

            Code.AppendLine("\t\t#region INSERT Ignoring Primary Key");
            Code.AppendLine();

            GenerateXmlDoc(2, "Inserts a new record to the table without specifying the primary key", (from column in Table.Columns where column.Value.Name != Table.PrimaryKeyColumnName select new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value")).ToArray());
            Code.Append("\t\tpublic void Create(");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                if (column.Value.ManagedType.IsValueType)
                    Code.Append(column.Value + "? " + column.Value.FieldName + ", ");
                else
                    Code.Append(column.Value + " " + column.Value.FieldName + ", ");
            }
            Code.Remove(Code.Length - 2, 2);
            Code.Append(")");
            Code.AppendLine();
            Code.AppendLine("\t\t{");

            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                Code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + column.Value.MaxLength + ")");
                Code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            Code.AppendLine();

            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");

            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateCreateUsingAllColumns()
        {
            Code.AppendLine("\t\t#region INSERT " + Table.Name + " by fields");
            Code.AppendLine();

            GenerateXmlDoc(2, "Inserts a new record to the table specifying all fields", Table.Columns.Select(column => new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value")).ToArray());
            Code.Append("\t\tpublic void Create(");
            foreach (var column in Table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    Code.Append(column.Value + "? " + column.Value.FieldName + ", ");
                else
                    Code.Append(column.Value + " " + column.Value.FieldName + ", ");
            }
            Code.Remove(Code.Length - 2, 2);
            Code.Append(")\n");
            Code.AppendLine("\t\t{");

            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                Code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + Table.ClassName + "." + column.Value.FieldName + "_Max_Length)");
                Code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            Code.AppendLine();

            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
           
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateDelete()
        {
            Code.AppendLine("\t\t#region DELETE");
            Code.AppendLine();
            GenerateXmlDoc(2, "Deletes the item", new KeyValuePair<string, string>("item", "Item to delete"));
            Code.AppendLine("\t\tpublic void Delete(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();

            GenerateDeleteMany();
        }

        private void GenerateDeleteMany()
        {
            Code.AppendLine("\t\t#region DELETE MANY");
            Code.AppendLine();
            GenerateXmlDoc(2, "Deletes a collection of item", new KeyValuePair<string, string>("items", "Items to delete"));
            Code.AppendLine("\t\tpublic void Delete(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in Table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                Code.AppendLine("\t\t#region DELETE BY " + column.Value.FieldName);
                Code.AppendLine();
                GenerateXmlDoc(2, "Delete records by " + column.Value.FieldName,
                               new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                Code.AppendFormat("\t\tpublic int DeleteBy{1}({0}{2} {1})", column.Value.ManagedType, column.Value.FieldName, column.Value.ManagedType.IsValueType ? "?" : string.Empty);
                Code.AppendLine("\n\t\t{");
                Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
                Code.AppendLine("\t\t#endregion");
                Code.AppendLine();
            }
        }

        public override void GenerateDeleteAll()
        {
            Code.AppendLine("\t\t#region Purge");
            Code.AppendLine();
            GenerateXmlDoc(2, "Purges the contents of the table");
            Code.AppendLine("\t\tpublic int Purge()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateUpdate()
        {
            Code.AppendLine("\t\t#region UPDATE");
            Code.AppendLine();
            GenerateXmlDoc(2, "Updates the item", new KeyValuePair<string, string>("item", "Item to update"));
            Code.AppendLine("\t\tpublic void Update(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();

            GenerateUpdateMany();
        }

        private void GenerateUpdateMany()
        {
            Code.AppendLine("\t\t#region UPDATE MANY");
            Code.AppendLine();
            GenerateXmlDoc(2, "Updates a collection of items", new KeyValuePair<string, string>("items", "Items to update"));
            Code.AppendLine("\t\tpublic void Update(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GeneratePopulate()
        {
            Code.AppendLine("\t\t#region INSERT MANY");
            Code.AppendLine();
            GenerateXmlDoc(2, "Populates the table with a collection of items");
            Code.AppendLine("\t\tpublic void Create(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateCreate()
        {
            Code.AppendLine("\t\t#region INSERT " + Table.Name);
            Code.AppendLine();
            GenerateXmlDoc(2, "Inserts the item to the table", new KeyValuePair<string, string>("item", "Item to insert to the database"));
            Code.AppendLine("\t\tpublic void Create(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateCount()
        {
            Code.AppendLine("\t\t#region COUNT " + Table.Name);
            Code.AppendLine();
            GenerateXmlDoc(2, "Gets the number of records in the table");
            Code.AppendLine("\t\tpublic int Count()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tthrow new System.NotImplementedException();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        #endregion
    }
}
