using System.Collections.Generic;
using System.Text;
using System.Data;
using System;
using System.Data.SqlClient;
using System.ComponentModel;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpDataAccessLayerGenerator : DataAccessLayerGenerator
    {
        public CSharpDataAccessLayerGenerator(StringBuilder code, Table table)
            : base(code, table)
        {
        }

        private void GetReaderValues()
        {
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.AppendLine("\t\t\t\t\t\titem." + column.Value.FieldName + " = (" + column.Value.ManagedType + "?) (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"]);");
                else
                    code.AppendLine("\t\t\t\t\t\titem." + column.Value.FieldName + " = (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"] as " + column.Value.ManagedType + ");");
            }
        }

        public override void GenerateSelectAll()
        {
            code.AppendLine("\t\t#region SELECT *");
            code.AppendLine();
            GenerateXmlDoc(2, "Retrieves all items as a generic collection");
            code.AppendLine("\t\tpublic System.Collections.Generic.List<" + table.ClassName + "> ToList()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.ClassName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"SELECT * FROM " + table.ClassName + "\";");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.ClassName + "();");
            GetReaderValues();
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic " + table.ClassName + "[] ToArray()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList();");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectWithTop()
        {
            code.AppendLine("\t\t#region SELECT TOP()");
            code.AppendLine();
            GenerateXmlDoc(2, "Retrieves the first set of items specified by count as a generic collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tpublic System.Collections.Generic.List<" + table.ClassName + "> ToList(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.ClassName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = string.Format(\"SELECT TOP({0}) * FROM " + table.ClassName + "\", count);");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.ClassName + "();");
            GetReaderValues();
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic " + table.ClassName + "[] ToArray(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList(count);");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region SELECT .... WHERE " + column.Value.FieldName + "=?");
                code.AppendLine();

                GenerateXmlDoc(2, "Retrieves a collection of items by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                if (column.Value.ManagedType.IsValueType)
                    code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2})",
                                      table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                else
                    code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2})",
                                      table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                code.AppendLine();
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.ClassName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
                code.AppendLine("\t\t\t{");
                code.AppendLine("\t\t\t\tif (" + column.Value.FieldName + " != null)");
                code.AppendLine("\t\t\t\t{");
                code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM {0} WHERE {1}=@{1}\";", table.ClassName, column.Value.FieldName);
                code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.FieldName);
                code.AppendLine();
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t\telse");
                code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM {0} WHERE {1} IS NULL\";", table.ClassName, column.Value.FieldName);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.ClassName + "();");
                GetReaderValues();
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine();
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region SELECT TOP(?).... WHERE " + column.Value.FieldName + "=?");
                code.AppendLine();

                GenerateXmlDoc(2, "Retrieves the first set of items specified by count by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"), new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
                if (column.Value.ManagedType.IsValueType)
                    code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count)", table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                else
                    code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count)", table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                code.AppendLine();
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.ClassName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
                code.AppendLine("\t\t\t{");
                code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null)");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM {0} WHERE {1}=@{1}\";", table.ClassName, column.Value.FieldName);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.FieldName);
                code.AppendLine();
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\telse");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM {0} WHERE {1} IS NULL\";", table.ClassName, column.Value.FieldName);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.ClassName + "();");
                GetReaderValues();
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine();
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateCount()
        {
            code.AppendLine("\t\t#region COUNT " + table.Name);
            code.AppendLine();
            GenerateXmlDoc(2, "Gets the number of records in the table");
            code.AppendLine("\t\tpublic int Count()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");
            code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT COUNT(*) FROM {0}\";", table.ClassName);
            code.AppendLine();
            code.AppendFormat("\t\t\t\treturn (int)command.ExecuteScalar();");
            code.AppendLine();
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreate()
        {
            code.AppendLine("\t\t#region INSERT " + table.Name);
            code.AppendLine();
            GenerateXmlDoc(2, "Inserts the item to the table", new KeyValuePair<string, string>("item", "Item to insert to the database"));
            code.AppendLine("\t\tpublic void Create(" + table.ClassName + " item)");
            code.AppendLine("\t\t{");
            code.Append("\t\t\tCreate(");
            foreach (var column in table.Columns)
            {
                if (column.Value.IsPrimaryKey && column.Value.AutoIncrement.HasValue)
                    continue;
                code.Append("item." + column.Value.FieldName + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(");");
            code.AppendLine();
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
            if (string.IsNullOrEmpty(table.PrimaryKeyColumnName))
                return;

            code.AppendLine("\t\t#region INSERT Ignoring Primary Key");
            code.AppendLine();

            var list = new List<KeyValuePair<string, string>>();
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                list.Add(new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
            }

            GenerateXmlDoc(2, "Inserts a new record to the table without specifying the primary key", list.ToArray());
            code.Append("\t\tpublic void Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Value.FieldName + ", ");
                else
                    code.Append(column.Value + " " + column.Value.FieldName + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")");
            code.AppendLine();
            code.AppendLine("\t\t{");

            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + column.Value.MaxLength + ")");
                code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            code.AppendLine();

            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.Name + " (");
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                query.Append("@" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", " + column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value);");
            }
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreateUsingAllColumns()
        {
            code.AppendLine("\t\t#region INSERT " + table.Name + " by fields");
            code.AppendLine();

            var list = new List<KeyValuePair<string, string>>();
            foreach (var column in table.Columns)
                list.Add(new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));

            GenerateXmlDoc(2, "Inserts a new record to the table specifying all fields", list.ToArray());
            code.Append("\t\tpublic void Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Value.FieldName + ", ");
                else
                    code.Append(column.Value + " " + column.Value.FieldName + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")\n");
            code.AppendLine("\t\t{");

            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                if (!column.Value.ManagedType.Equals(typeof(string)))
                    continue;
                code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + table.ClassName + "." + column.Value.FieldName + "_Max_Length)");
                code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            code.AppendLine();

            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.Name + " (");
            foreach (var column in table.Columns)
                query.Append(column.Value.FieldName + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
                query.Append("@" + column.Value.FieldName + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", " + column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value);");
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GeneratePopulate()
        {
            code.AppendLine("\t\t#region INSERT MANY");
            code.AppendLine();
            GenerateXmlDoc(2, "Populates the table with a collection of items");
            code.AppendLine("\t\tpublic void Create(System.Collections.Generic.IEnumerable<" + table.ClassName + "> items)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandType = System.Data.CommandType.TableDirect;");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"" + table.ClassName + "\";");
            code.AppendLine();
            code.AppendLine("\t\t\t\tusing (var resultSet = command.ExecuteResultSet(System.Data.SqlServerCe.ResultSetOptions.Updatable))");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\tvar record = resultSet.CreateRecord();");
            code.AppendLine("\t\t\t\t\tforeach (var item in items)");
            code.AppendLine("\t\t\t\t\t{");
            foreach (var column in table.Columns.Values)
            {
                if (column.AutoIncrement.HasValue)
                    continue;
                code.AppendFormat("\t\t\t\t\t\trecord.SetValue({0}, item.{1});", column.Ordinal - 1, column.FieldName);
                code.AppendLine();
            }
            code.AppendLine("\t\t\t\t\t\tresultSet.Insert(record);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDelete()
        {
            code.AppendLine("\t\t#region DELETE");
            code.AppendLine();
            GenerateXmlDoc(2, "Deletes the item", new KeyValuePair<string, string>("item", "Item to delete"));
            code.AppendLine("\t\tpublic void Delete(" + table.ClassName + " item)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + table.Name + " WHERE ");

            var hasPrimaryKey = false;
            foreach (var column in table.Columns)
            {
                if (!column.Value.IsPrimaryKey) continue;
                hasPrimaryKey = true;
                query.Append(column.Value.FieldName + " = @" + column.Value.FieldName);
                break;
            }
            if (!hasPrimaryKey)
            {
                foreach (var column in table.Columns)
                    query.Append(column.Value.FieldName + " = @" + column.Value.FieldName + " AND ");
                query.Remove(query.Length - 5, 5);
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (!hasPrimaryKey)
                    code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");
                else if (column.Value.IsPrimaryKey)
                {
                    code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");
                    break;
                }
            }

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();

            GenerateDeleteMany();
        }

        private void GenerateDeleteMany()
        {
            code.AppendLine("\t\t#region DELETE MANY");
            code.AppendLine();
            GenerateXmlDoc(2, "Deletes a collection of item", new KeyValuePair<string, string>("items", "Items to delete"));
            code.AppendLine("\t\tpublic void Delete(System.Collections.Generic.IEnumerable<" + table.ClassName + "> items)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + table.Name + " WHERE ");

            var hasPrimaryKey = false;
            foreach (var column in table.Columns)
            {
                if (!column.Value.IsPrimaryKey) continue;
                hasPrimaryKey = true;
                query.Append(column.Value.FieldName + " = @" + column.Value.FieldName);
                break;
            }
            if (!hasPrimaryKey)
            {
                foreach (var column in table.Columns)
                    query.Append(column.Value.FieldName + " = @" + column.Value.FieldName + " AND ");
                query.Remove(query.Length - 5, 5);
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (!hasPrimaryKey)
                    //code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", typeof( " + column.Value.ManagedType + "));");
                    code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.SqlDbType." + GetSqlDbType(column.Value.ManagedType) + ");");
                else if (column.Value.IsPrimaryKey)
                {
                    //code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", typeof( " + column.Value.ManagedType + "));");
                    code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.SqlDbType." + GetSqlDbType(column.Value.ManagedType) + ");");
                    break;
                }
            }
            code.AppendLine("\t\t\t\tcommand.Prepare();");
            code.AppendLine();

            code.AppendLine("\t\t\t\tforeach (var item in items)");
            code.AppendLine("\t\t\t\t{");
            foreach (var column in table.Columns)
            {
                if (!hasPrimaryKey)
                    code.AppendLine("\t\t\t\t\t\tcommand.Parameters[\"@" + column.Value.FieldName + "\"].Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
                else if (column.Value.IsPrimaryKey)
                {
                    code.AppendLine("\t\t\t\t\t\tcommand.Parameters[\"@" + column.Value.FieldName + "\"].Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
                    break;
                }
                code.AppendLine("\t\t\t\t\tcommand.ExecuteNonQuery();");
            }
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t#region DELETE BY " + column.Value.FieldName);
                code.AppendLine();
                GenerateXmlDoc(2, "Delete records by " + column.Value.FieldName,
                               new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                code.AppendFormat("\t\tpublic int DeleteBy{1}({0}{2} {1})", column.Value.ManagedType, column.Value.FieldName, column.Value.ManagedType.IsValueType ? "?" : string.Empty);
                code.AppendLine("\n\t\t{");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"DELETE FROM {0} WHERE {1}=@{1}\";", table.Name, column.Value.FieldName);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0} != null ? (object){0} : System.DBNull.Value);", column.Value.FieldName);
                code.AppendLine("\n\t\t\t\treturn command.ExecuteNonQuery();");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t}");
                code.AppendLine();
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateDeleteAll()
        {
            code.AppendLine("\t\t#region Purge");
            code.AppendLine();
            GenerateXmlDoc(2, "Purges the contents of the table");
            code.AppendLine("\t\tpublic int Purge()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"DELETE FROM " + table.Name + "\";");
            code.AppendLine("\t\t\t\treturn command.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateUpdate()
        {
            code.AppendLine("\t\t#region UPDATE");
            code.AppendLine();
            GenerateXmlDoc(2, "Updates the item", new KeyValuePair<string, string>("item", "Item to update"));
            code.AppendLine("\t\tpublic void Update(" + table.ClassName + " item)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + table.Name + " SET ");
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.FieldName + " = @" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Value.FieldName + " = @" + column.Value.FieldName);
                    break;
                }
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();

            GenerateUpdateMany();
        }

        private void GenerateUpdateMany()
        {
            code.AppendLine("\t\t#region UPDATE MANY");
            code.AppendLine();
            GenerateXmlDoc(2, "Updates a collection of items", new KeyValuePair<string, string>("items", "Items to update"));
            code.AppendLine("\t\tpublic void Update(System.Collections.Generic.IEnumerable<" + table.ClassName + "> items)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(Transaction))");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + table.Name + " SET ");
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.FieldName + " = @" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in table.Columns)
            {
                if (column.Value.Name == table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Value.FieldName + " = @" + column.Value.FieldName);
                    break;
                }
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                //code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", typeof(" + column.Value.ManagedType + "));");
                code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.SqlDbType." + GetSqlDbType(column.Value.ManagedType) + ");");
            code.AppendLine("\t\t\t\tcommand.Prepare();");
            code.AppendLine();

            code.AppendLine("\t\t\t\tforeach (var item in items)");
            code.AppendLine("\t\t\t\t{");
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\t\tcommand.Parameters[\"@" + column.Value.FieldName + "\"].Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
            code.AppendLine("\t\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t\t}");

            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        private static SqlDbType GetSqlDbType(Type type)
        {
            if (type == typeof(byte[]))
                return SqlDbType.Image;

            var parameter = new SqlParameter();
            var typeConverter = TypeDescriptor.GetConverter(parameter.DbType);
            parameter.DbType = (DbType)typeConverter.ConvertFrom(type.Name);
            return parameter.SqlDbType;
        }
    }
}
