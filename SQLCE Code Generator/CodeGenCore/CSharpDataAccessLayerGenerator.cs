﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpDataAccessLayerGenerator : DataAccessLayerGenerator
    {
        public CSharpDataAccessLayerGenerator(StringBuilder code, Table table)
            : base(code, table)
        {
        }

        public override void GenerateSelectAll()
        {
            code.AppendLine("\t\t#region SELECT *");
            code.AppendLine("\t\tpublic static System.Collections.Generic.List<" + table.TableName + "> ToList()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"SELECT * FROM " + table.TableName + "\";");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
            GetReaderValues(table);
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic static " + table.TableName + "[] ToArray()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList();");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectWithTop()
        {
            code.AppendLine("\t\t#region SELECT TOP()");
            code.AppendLine("\t\tpublic static System.Collections.Generic.List<" + table.TableName + "> ToList(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = string.Format(\"SELECT TOP({0}) * FROM " + table.TableName + "\", count);");
            code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            code.AppendLine("\t\t\t\t\t{");
            code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
            GetReaderValues(table);
            code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
            code.AppendLine("\t\t\t\t\t}");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic static " + table.TableName + "[] ToArray(int count)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar list = ToList(count);");
            code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
            foreach (var column in table.Columns)
            {
                code.AppendLine("\t\t#region SELECT .... WHERE " + column.Value.Name + "=?");
                code.AppendFormat("\n\t\tpublic static System.Collections.Generic.List<{0}> SelectBy{2}({1} {2})", table.TableName, column.Value.ManagedType, column.Value.Name);
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT * FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.Name);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
                GetReaderValues(table);
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in table.Columns)
            {
                code.AppendLine("\t\t#region SELECT TOP(?).... WHERE " + column.Value.Name + "=?");
                code.AppendFormat("\n\t\tpublic static System.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count)", table.TableName, column.Value.ManagedType, column.Value.Name);
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + table.TableName + ">();");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.Name);
                code.AppendLine();
                code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                code.AppendLine("\t\t\t\t\t{");
                code.AppendLine("\t\t\t\t\t\tvar item = new " + table.TableName + "();");
                GetReaderValues(table);
                code.AppendLine("\t\t\t\t\t\tlist.Add(item);");
                code.AppendLine("\t\t\t\t\t}");
                code.AppendLine("\t\t\t\t}");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
            code.AppendLine("\t\t#region INSERT");
            code.Append("\t\tpublic static " + table.TableName + " Create(");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Key + ", ");
                else
                    code.Append(column.Value + " " + column.Key + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")\n");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.TableName + " (");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append("@" + column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");
            }
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine();
            code.AppendLine("\t\t\t\tvar item = new " + table.TableName + "();");
            code.AppendLine();
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                {
                    query.Remove(0, query.Length);
                    query.Append("\"");
                    query.Append("SELECT TOP(1) " + column.Key + " FROM " + table.TableName + " ");
                    query.Append("ORDER BY " + column.Key + " DESC");
                    query.Append("\";");
                    code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
                    code.AppendLine("\t\t\t\tcommand.Parameters.Clear();");
                    code.AppendLine("\t\t\t\tvar value = command.ExecuteScalar();");
                    code.AppendLine();
                    code.Append("\t\t\t\titem." + column.Key + " = value as " + column.Value);
                    if (column.Value.ManagedType.IsValueType)
                        code.Append("?;");
                    else
                        code.Append(";");
                    code.AppendLine();
                }
                else
                    code.AppendLine("\t\t\t\titem." + column.Key + " = " + column.Key + ";");
            }
            code.AppendLine("\t\t\t\treturn item;");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateCreateUsingAllColumns()
        {
            code.AppendLine("\t\t#region INSERT");
            code.Append("\t\tpublic static " + table.TableName + " Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Key + ", ");
                else
                    code.Append(column.Value + " " + column.Key + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(")\n");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + table.TableName + " (");
            foreach (var column in table.Columns)
                query.Append(column.Key + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in table.Columns)
                query.Append("@" + column.Key + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine();
            code.AppendLine("\t\t\t\tvar item = new " + table.TableName + "();");
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\titem." + column.Key + " = " + column.Key + ";");
            code.AppendLine("\t\t\t\treturn item;");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDelete()
        {
            code.AppendLine("\t\t#region DELETE");
            code.AppendLine("\t\tpublic void Delete()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + table.TableName + " WHERE ");
            foreach (var column in table.Columns)
                query.Append(column.Key + " = @" + column.Key + " AND ");
            query.Remove(query.Length - 5, 5);
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine();

            foreach (var column in table.Columns)
                code.AppendLine("\t\t\tthis." + column.Key + " = null;");

            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in table.Columns)
            {
                code.AppendLine("\t\t#region DELETE BY " + column.Value.Name);
                code.AppendFormat("\n\t\tpublic static int DeleteBy{1}({0}{2} {1})", column.Value.ManagedType, column.Value.Name, column.Value.ManagedType.IsValueType ? "?" : string.Empty);
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
                code.AppendLine("\t\t\t{");
                code.AppendFormat("\n\t\t\t\tcommand.CommandText = \"DELETE FROM {0} WHERE {1}=@{1}\";", table.TableName, column.Value.Name);
                code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0} != null ? (object){0} : System.DBNull.Value);", column.Value.Name);
                code.AppendLine("\n\t\t\t\treturn command.ExecuteNonQuery();");
                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t}");
                code.AppendLine("\t\t#endregion");
                code.AppendLine();
            }
        }

        public override void GenerateDeleteAll()
        {
            code.AppendLine("\t\t#region Purge");
            code.AppendLine("\t\tpublic static void Purge()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tcommand.CommandText = \"DELETE FROM " + table.TableName + "\";");
            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }

        public override void GenerateSaveChanges()
        {
            code.AppendLine("\t\t#region UPDATE");
            code.AppendLine("\t\tpublic void SaveChanges()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + table.TableName + " SET ");
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Key + " = @" + column.Key + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in table.Columns)
            {
                if (column.Key == table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Key + " = @" + column.Key);
                    break;
                }
            }
            query.Append("\";");

            code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in table.Columns)
                code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Key + "\", " + column.Key + " != null ? (object)" + column.Key + " : System.DBNull.Value);");

            code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine("\t\t#endregion");
            code.AppendLine();
        }
    }
}
