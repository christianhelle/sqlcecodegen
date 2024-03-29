﻿#region License
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
using System.Collections.Generic;
using System.Linq;
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

        //private void GetReaderValues()
        //{
        //    foreach (var column in Table.Columns)
        //    {
        //        if (column.Value.ManagedType.IsValueType)
        //            Code.AppendLine("\t\t\t\t\t\titem." + column.Value.FieldName + " = (" + column.Value.ManagedType + "?) (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"]);");
        //        else
        //            Code.AppendLine("\t\t\t\t\t\titem." + column.Value.FieldName + " = (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"] as " + column.Value.ManagedType + ");");
        //    }
        //}

        public override void GenerateCreateEntity()
        {
            Code.AppendLine("\t\t#region Create" + Table.ClassName);
            Code.AppendLine();
            GenerateXmlDoc(2, "Maps an entity to the current reader");
            Code.AppendLine("\t\tpublic static " + Table.ClassName + " CreateEntity(System.Data.IDataReader reader)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar item = new " + Table.ClassName + "();");
            foreach (var column in Table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    Code.AppendLine("\t\t\titem." + column.Value.FieldName + " = (" + column.Value.ManagedType + "?) (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"]);");
                else
                    Code.AppendLine("\t\t\titem." + column.Value.FieldName + " = (reader.IsDBNull(" + (column.Value.Ordinal - 1) + ") ? null : reader[\"" + column.Value.FieldName + "\"] as " + column.Value.ManagedType + ");");
            }
            Code.AppendLine("\t\t\treturn item;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateSelectAll()
        {
            Code.AppendLine("\t\t#region SELECT *");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves all items as an IEnumerable collection");
            Code.AppendLine("\t\tpublic System.Collections.Generic.IEnumerable<" + Table.ClassName + "> ToEnumerable()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandText = \"SELECT * FROM [" + Table.ClassName + "]\";");
            Code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            Code.AppendLine("\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            Code.AppendLine("\t\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\t\tyield return CreateEntity(reader);");
            Code.AppendLine("\t\t\t\t\t}");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves all items as a generic collection");
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandText = \"SELECT * FROM [" + Table.ClassName + "]\";");
            Code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            Code.AppendLine("\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            Code.AppendLine("\t\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
            Code.AppendLine("\t\t\t\t\t}");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
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

        public override void GenerateSelectWithTop()
        {
            Code.AppendLine("\t\t#region SELECT TOP()");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves the first set of items specified by count as an IEnumerable collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            Code.AppendLine("\t\tpublic System.Collections.Generic.IEnumerable<" + Table.ClassName + "> ToEnumerable(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandText = string.Format(\"SELECT TOP({0}) * FROM [" + Table.ClassName + "]\", count);");
            Code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            Code.AppendLine("\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            Code.AppendLine("\t\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\t\tyield return CreateEntity(reader);");
            Code.AppendLine("\t\t\t\t\t}");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves the first set of items specified by count as a generic collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandText = string.Format(\"SELECT TOP({0}) * FROM [" + Table.ClassName + "]\", count);");
            Code.AppendLine("\t\t\t\tusing (var reader = command.ExecuteReader())");
            Code.AppendLine("\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
            Code.AppendLine("\t\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
            Code.AppendLine("\t\t\t\t\t}");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
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

        public override void GenerateSelectBy()
        {
            foreach (var column in Table.Columns)
            {
                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(column.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
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
                Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
                Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
                Code.AppendLine("\t\t\t{");
                Code.AppendLine("\t\t\t\tif (" + column.Value.FieldName + " != null)");
                Code.AppendLine("\t\t\t\t{");
                Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1}\";", Table.ClassName, column.Value.FieldName);
                Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = {1};", column.Value.FieldName, "(object)" + column.Value.FieldName);
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.FieldName);
                Code.AppendLine();
                Code.AppendLine("\t\t\t\t}");
                Code.AppendLine("\t\t\t\telse");
                Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL\";", Table.ClassName, column.Value.FieldName);
                Code.AppendLine();
                Code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                Code.AppendLine("\t\t\t\t{");
                Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                Code.AppendLine("\t\t\t\t\t{");
                Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
                Code.AppendLine("\t\t\t\t\t}");
                Code.AppendLine("\t\t\t\t}");
                Code.AppendLine("\t\t\t}");
                Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
                Code.AppendLine("\t\t#endregion");
                Code.AppendLine();
            }
        }

        public override void SelectByThreeColumns()
        {
            foreach (var firstColumn in Table.Columns)
            {
                if (String.Compare(firstColumn.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(firstColumn.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                foreach (var secondColumn in Table.Columns)
                {
                    if (secondColumn.Equals(firstColumn))
                        continue;

                    foreach (var thirdColumn in Table.Columns)
                    {
                        if (thirdColumn.Equals(firstColumn) || thirdColumn.Equals(secondColumn))
                            continue;

                        Code.AppendLine("\t\t#region SELECT .... WHERE " + firstColumn.Value.FieldName + "=?");
                        Code.AppendLine();

                        GenerateXmlDoc(2,
                                       "Retrieves a collection of items by " + firstColumn.Value.FieldName + " and " + secondColumn.Value.FieldName + " and " + thirdColumn.Value.FieldName,
                                       new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                                       new KeyValuePair<string, string>(secondColumn.Value.FieldName, secondColumn.Value.FieldName + " value"),
                                       new KeyValuePair<string, string>(thirdColumn.Value.FieldName, thirdColumn.Value.FieldName + " value"));

                        Code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{3}And{6}And{9}({1}{2} {3}, {4}{5} {6}, {7}{8} {9})",
                                          Table.ClassName,
                                          firstColumn.Value.ManagedType,
                                          firstColumn.Value.ManagedType.IsValueType ? "?" : "",
                                          firstColumn.Value.FieldName,
                                          secondColumn.Value.ManagedType,
                                          secondColumn.Value.ManagedType.IsValueType ? "?" : "",
                                          secondColumn.Value.FieldName,
                                          thirdColumn.Value.ManagedType,
                                          thirdColumn.Value.ManagedType.IsValueType ? "?" : "",
                                          thirdColumn.Value.FieldName);

                        Code.AppendLine();
                        Code.AppendLine("\t\t{");
                        Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
                        Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
                        Code.AppendLine("\t\t\t{");
                        Code.AppendLine("\t\t\t\tif (" + firstColumn.Value.FieldName + " != null && " + secondColumn.Value.FieldName + " != null && " + thirdColumn.Value.FieldName + " != null)");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1} AND {2}=@{2}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName);
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", firstColumn.Value.FieldName, GetSqlDbType(firstColumn.Value.ManagedType));
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", secondColumn.Value.FieldName, GetSqlDbType(secondColumn.Value.ManagedType));
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", thirdColumn.Value.FieldName, GetSqlDbType(thirdColumn.Value.ManagedType));
                        Code.AppendLine();
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " == null && " + secondColumn.Value.FieldName + " != null && " + thirdColumn.Value.FieldName + " != null)");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL AND {2}=@{2} AND {3}=@{3}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName, thirdColumn.Value.FieldName);
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", secondColumn.Value.FieldName, GetSqlDbType(thirdColumn.Value.ManagedType));
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", thirdColumn.Value.FieldName, GetSqlDbType(thirdColumn.Value.ManagedType));
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " == null && " + secondColumn.Value.FieldName + " == null && " + thirdColumn.Value.FieldName + " != null)");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL AND {2} IS NULL AND {3}=@{3}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName, thirdColumn.Value.FieldName);
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", thirdColumn.Value.FieldName, GetSqlDbType(thirdColumn.Value.ManagedType));
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " != null && " + secondColumn.Value.FieldName + " == null && " + thirdColumn.Value.FieldName + " != null)");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1} AND {2} IS NULL AND {3}=@{3}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName, thirdColumn.Value.FieldName);
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", firstColumn.Value.FieldName, GetSqlDbType(firstColumn.Value.ManagedType));
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", thirdColumn.Value.FieldName, GetSqlDbType(thirdColumn.Value.ManagedType));
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " != null && " + secondColumn.Value.FieldName + " != null && " + thirdColumn.Value.FieldName + " == null)");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1} AND {2}=@{2} AND {3} IS NULL\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName, thirdColumn.Value.FieldName);
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", firstColumn.Value.FieldName, GetSqlDbType(firstColumn.Value.ManagedType));
                        Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", secondColumn.Value.FieldName, GetSqlDbType(secondColumn.Value.ManagedType));
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t\telse");
                        Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL AND {2} IS NULL AND {3} IS NULL\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName, thirdColumn.Value.FieldName);
                        Code.AppendLine();
                        Code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                        Code.AppendLine("\t\t\t\t{");
                        Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                        Code.AppendLine("\t\t\t\t\t{");
                        Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
                        Code.AppendLine("\t\t\t\t\t}");
                        Code.AppendLine("\t\t\t\t}");
                        Code.AppendLine("\t\t\t}");
                        Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                        Code.AppendLine("\t\t}");
                        Code.AppendLine();
                        Code.AppendLine("\t\t#endregion");
                        Code.AppendLine();
                    }
                }
            }
        }

        public override void SelectByTwoColumns()
        {
            foreach (var firstColumn in Table.Columns)
            {
                if (String.Compare(firstColumn.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(firstColumn.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                foreach (var secondColumn in Table.Columns)
                {
                    if (secondColumn.Equals(firstColumn))
                        continue;

                    Code.AppendLine("\t\t#region SELECT .... WHERE " + firstColumn.Value.FieldName + "=?");
                    Code.AppendLine();

                    GenerateXmlDoc(2,
                                   "Retrieves a collection of items by " + firstColumn.Value.FieldName + " and " + secondColumn.Value.FieldName,
                                   new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                                   new KeyValuePair<string, string>(secondColumn.Value.FieldName, secondColumn.Value.FieldName + " value"));

                    Code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{3}And{6}({1}{2} {3}, {4}{5} {6})",
                                      Table.ClassName,
                                      firstColumn.Value.ManagedType,
                                      firstColumn.Value.ManagedType.IsValueType ? "?" : "",
                                      firstColumn.Value.FieldName,
                                      secondColumn.Value.ManagedType,
                                      secondColumn.Value.ManagedType.IsValueType ? "?" : "",
                                      secondColumn.Value.FieldName);

                    Code.AppendLine();
                    Code.AppendLine("\t\t{");
                    Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
                    Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
                    Code.AppendLine("\t\t\t{");
                    Code.AppendLine("\t\t\t\tif (" + firstColumn.Value.FieldName + " != null && " + secondColumn.Value.FieldName + " != null)");
                    Code.AppendLine("\t\t\t\t{");
                    Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1} AND {2}=@{2}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName);
                    Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", firstColumn.Value.FieldName, GetSqlDbType(firstColumn.Value.ManagedType));
                    Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", secondColumn.Value.FieldName, GetSqlDbType(secondColumn.Value.ManagedType));
                    Code.AppendLine();
                    Code.AppendLine("\t\t\t\t}");
                    Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " == null && " + secondColumn.Value.FieldName + " != null)");
                    Code.AppendLine("\t\t\t\t{");
                    Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL AND {2}=@{2}\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName);
                    Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", secondColumn.Value.FieldName, GetSqlDbType(secondColumn.Value.ManagedType));
                    Code.AppendLine("\t\t\t\t}");
                    Code.AppendLine("\t\t\t\telse if (" + firstColumn.Value.FieldName + " != null && " + secondColumn.Value.FieldName + " == null)");
                    Code.AppendLine("\t\t\t\t{");
                    Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1}=@{1} AND {2} IS NULL\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName);
                    Code.AppendFormat("\n\t\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", firstColumn.Value.FieldName, GetSqlDbType(firstColumn.Value.ManagedType));
                    Code.AppendLine("\t\t\t\t}");
                    Code.AppendLine("\t\t\t\telse");
                    Code.AppendFormat("\t\t\t\t\tcommand.CommandText = \"SELECT * FROM [{0}] WHERE {1} IS NULL AND {2} IS NULL\";", Table.ClassName, firstColumn.Value.FieldName, secondColumn.Value.FieldName);
                    Code.AppendLine();
                    Code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                    Code.AppendLine("\t\t\t\t{");
                    Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                    Code.AppendLine("\t\t\t\t\t{");
                    Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
                    Code.AppendLine("\t\t\t\t\t}");
                    Code.AppendLine("\t\t\t\t}");
                    Code.AppendLine("\t\t\t}");
                    Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                    Code.AppendLine("\t\t}");
                    Code.AppendLine();
                    Code.AppendLine("\t\t#endregion");
                    Code.AppendLine();
                }
            }
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in Table.Columns)
            {
                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(column.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
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
                Code.AppendLine("\t\t\tvar list = new System.Collections.Generic.List<" + Table.ClassName + ">();");
                Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
                Code.AppendLine("\t\t\t{");
                Code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null)");
                Code.AppendLine("\t\t\t{");
                Code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM [{0}] WHERE {1}=@{1}\";", Table.ClassName, column.Value.FieldName);
                Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {0}));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = {1};", column.Value.FieldName, "(object)" + column.Value.FieldName);
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0});", column.Value.FieldName);
                Code.AppendLine();
                Code.AppendLine("\t\t\t}");
                Code.AppendLine("\t\t\telse");
                Code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT TOP(\" + count + \") * FROM [{0}] WHERE {1} IS NULL\";", Table.ClassName, column.Value.FieldName);
                Code.AppendLine();
                Code.AppendLine("\n\t\t\t\tusing (var reader = command.ExecuteReader())");
                Code.AppendLine("\t\t\t\t{");
                Code.AppendLine("\t\t\t\t\twhile (reader.Read())");
                Code.AppendLine("\t\t\t\t\t{");
                Code.AppendLine("\t\t\t\t\t\tlist.Add(CreateEntity(reader));");
                Code.AppendLine("\t\t\t\t\t}");
                Code.AppendLine("\t\t\t\t}");
                Code.AppendLine("\t\t\t}");
                Code.AppendLine("\t\t\treturn list.Count > 0 ? list : null;");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
                Code.AppendLine("\t\t#endregion");
                Code.AppendLine();
            }
        }

        public override void GenerateCount()
        {
            Code.AppendLine("\t\t#region COUNT " + Table.Name);
            Code.AppendLine();
            GenerateXmlDoc(2, "Gets the number of records in the table");
            Code.AppendLine("\t\tpublic int Count()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendFormat("\t\t\t\tcommand.CommandText = \"SELECT COUNT(*) FROM [{0}]\";", Table.ClassName);
            Code.AppendLine();
            Code.AppendFormat("\t\t\t\treturn (int)command.ExecuteScalar();");
            Code.AppendLine();
            Code.AppendLine("\t\t\t}");
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
            Code.Append("\t\t\tCreate(");
            foreach (var column in Table.Columns)
            {
                if (column.Value.IsPrimaryKey && column.Value.AutoIncrement.HasValue)
                    continue;
                Code.Append("item." + column.Value.FieldName + ", ");
            }
            Code.Remove(Code.Length - 2, 2);
            Code.Append(");");
            Code.AppendLine();
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
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
                if (column.Value.ManagedType != typeof(string))
                    continue;
                Code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + column.Value.MaxLength + ")");
                Code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            Code.AppendLine();

            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + Table.Name + " (");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.Name + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                query.Append("@" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, null));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                Code.AppendFormat("\n\t\t\t\t((System.Data.Common.DbParameter)command.Parameters[\"@{0}\"]).Value = {1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value");
                //code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", " + column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value);");
            }
            Code.AppendLine();
            Code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t}");
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
                if (column.Value.ManagedType != typeof(string))
                    continue;
                Code.AppendLine("\t\t\tif (" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + Table.ClassName + "." + column.Value.FieldName + "_Max_Length)");
                Code.AppendLine("\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");");
            }
            Code.AppendLine();

            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"INSERT INTO " + Table.Name + " (");
            foreach (var column in Table.Columns)
                query.Append(column.Value.Name + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(") ");
            query.Append(" VALUES (");
            foreach (var column in Table.Columns)
                query.Append("@" + column.Value.FieldName + ", ");
            query.Remove(query.Length - 2, 2);
            query.Append(")\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {2}));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType), column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value");
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = {1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value");
                //code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", " + column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value);");
            }
            Code.AppendLine();
            Code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandType = System.Data.CommandType.TableDirect;");
            Code.AppendLine("\t\t\t\tcommand.CommandText = \"" + Table.ClassName + "\";");
            Code.AppendLine();
            Code.AppendLine("\t\t\t\tusing (var resultSet = ((System.Data.SqlServerCe.SqlCeCommand)command).ExecuteResultSet(System.Data.SqlServerCe.ResultSetOptions.Updatable))");
            Code.AppendLine("\t\t\t\t{");
            Code.AppendLine("\t\t\t\t\tvar record = resultSet.CreateRecord();");
            Code.AppendLine("\t\t\t\t\tforeach (var item in items)");
            Code.AppendLine("\t\t\t\t\t{");
            foreach (var column in Table.Columns.Values)
            {
                if (column.AutoIncrement.HasValue)
                    continue;
                Code.AppendFormat("\t\t\t\t\t\trecord.SetValue({0}, item.{1});", column.Ordinal - 1, column.FieldName);
                Code.AppendLine();
            }
            Code.AppendLine("\t\t\t\t\t\tresultSet.Insert(record);");
            Code.AppendLine("\t\t\t\t\t}");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + Table.Name + " WHERE ");

            var hasPrimaryKey = false;
            foreach (var column in Table.Columns)
            {
                if (!column.Value.IsPrimaryKey) continue;
                hasPrimaryKey = true;
                query.Append(column.Value.Name + " = @" + column.Value.FieldName);
                break;
            }
            if (!hasPrimaryKey)
            {
                foreach (var column in Table.Columns)
                    query.Append(column.Value.Name + " = @" + column.Value.FieldName + " AND ");
                query.Remove(query.Length - 5, 5);
            }
            query.Append("\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                if (!hasPrimaryKey)
                {
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                    //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                    //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = item.{1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value");
                    //code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");
                }
                else if (column.Value.IsPrimaryKey)
                {
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                    //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                    //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = item.{1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value");
                    //code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");
                    break;
                }
            }

            Code.AppendLine();
            Code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"DELETE FROM " + Table.Name + " WHERE ");

            var hasPrimaryKey = false;
            foreach (var column in Table.Columns)
            {
                if (!column.Value.IsPrimaryKey) continue;
                hasPrimaryKey = true;
                query.Append(column.Value.Name + " = @" + column.Value.FieldName);
                break;
            }
            if (!hasPrimaryKey)
            {
                foreach (var column in Table.Columns)
                    query.Append(column.Value.Name + " = @" + column.Value.FieldName + " AND ");
                query.Remove(query.Length - 5, 5);
            }
            query.Append("\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                if (!hasPrimaryKey)
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, null));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.DbType." + GetSqlDbType(column.Value.ManagedType) + ");");
                else if (column.Value.IsPrimaryKey)
                {
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, null));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                    //Code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.DbType." + GetSqlDbType(column.Value.ManagedType) + ");");
                    break;
                }
            }
            Code.AppendLine("\t\t\t\tcommand.Prepare();");
            Code.AppendLine();

            Code.AppendLine("\t\t\t\tforeach (var item in items)");
            Code.AppendLine("\t\t\t\t{");
            foreach (var column in Table.Columns)
            {
                if (!hasPrimaryKey)
                    Code.AppendLine("\t\t\t\t\t((System.Data.Common.DbParameter)command.Parameters[\"@" + column.Value.FieldName + "\"]).Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
                else if (column.Value.IsPrimaryKey)
                {
                    Code.AppendLine("\t\t\t\t\t((System.Data.Common.DbParameter)command.Parameters[\"@" + column.Value.FieldName + "\"]).Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
                    break;
                }
            }
            Code.AppendLine();
            Code.AppendLine("\t\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t\t}");
            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in Table.Columns)
            {
                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(column.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                Code.AppendLine("\t\t#region DELETE BY " + column.Value.FieldName);
                Code.AppendLine();
                GenerateXmlDoc(2, "Delete records by " + column.Value.FieldName,
                               new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                Code.AppendFormat("\t\tpublic int DeleteBy{1}({0}{2} {1})", column.Value.ManagedType, column.Value.FieldName, column.Value.ManagedType.IsValueType ? "?" : string.Empty);
                Code.AppendLine("\n\t\t{");
                Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
                Code.AppendLine("\t\t\t{");
                Code.AppendFormat("\t\t\t\tcommand.CommandText = \"DELETE FROM {0} WHERE {1}=@{2}\";", Table.Name, column.Value.Name, column.Value.FieldName);
                Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, {2}));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType), column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value");
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters[\"@{0}\"].Value = {1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)" + column.Value.FieldName + " : System.DBNull.Value");
                //code.AppendFormat("\n\t\t\t\tcommand.Parameters.AddWithValue(\"@{0}\", {0} != null ? (object){0} : System.DBNull.Value);", column.Value.FieldName);
                Code.AppendLine();
                Code.AppendLine("\n\t\t\t\treturn command.ExecuteNonQuery();");
                Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");
            Code.AppendLine("\t\t\t\tcommand.CommandText = \"DELETE FROM " + Table.Name + "\";");
            Code.AppendLine("\t\t\t\treturn command.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + Table.Name + " SET ");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.Name + " = @" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Value.Name + " = @" + column.Value.FieldName);
                    break;
                }
            }
            query.Append("\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0)
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.String, null));", column.Value.FieldName);
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.String);", column.Value.FieldName);
                else
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, null));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                Code.AppendFormat("\n\t\t\t\t((System.Data.Common.DbParameter)command.Parameters[\"@{0}\"]).Value = item.{1};", column.Value.FieldName, column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value");
                //code.AppendLine("\t\t\t\tcommand.Parameters.AddWithValue(\"@" + column.Value.FieldName + "\", item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value);");
            }
            Code.AppendLine();

            Code.AppendLine("\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t}");
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
            Code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(Transaction))");
            Code.AppendLine("\t\t\t{");

            var query = new StringBuilder();
            query.Append("\"UPDATE " + Table.Name + " SET ");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                query.Append(column.Value.FieldName + " = @" + column.Value.FieldName + ", ");
            }
            query.Remove(query.Length - 2, 2);
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                {
                    query.Append(" WHERE " + column.Value.FieldName + " = @" + column.Value.FieldName);
                    break;
                }
            }
            query.Append("\";");

            Code.AppendLine("\t\t\t\tcommand.CommandText = " + query);
            foreach (var column in Table.Columns)
            {
                //Code.AppendLine("\t\t\t\tcommand.Parameters.Add(\"@" + column.Value.FieldName + "\", System.Data.DbType." + GetSqlDbType(column.Value.ManagedType) + ");");

                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0)
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.String, null));", column.Value.FieldName);
                //Code.AppendFormat("\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.String);", column.Value.FieldName);
                else
                    Code.AppendFormat("\n\t\t\t\tcommand.Parameters.Add(Database.CreateParameter(\"@{0}\", System.Data.DbType.{1}, null));", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));
                //Code.AppendFormat("\t\t\t\tcommand.Parameters.Add(\"@{0}\", System.Data.DbType.{1});", column.Value.FieldName, GetSqlDbType(column.Value.ManagedType));

                Code.AppendLine();
            }
            Code.AppendLine("\t\t\t\tcommand.Prepare();");
            Code.AppendLine();

            Code.AppendLine("\t\t\t\tforeach (var item in items)");
            Code.AppendLine("\t\t\t\t{");
            foreach (var column in Table.Columns)
                Code.AppendLine("\t\t\t\t\t((System.Data.Common.DbParameter)command.Parameters[\"@" + column.Value.FieldName + "\"]).Value = item." + column.Value.FieldName + " != null ? (object)item." + column.Value.FieldName + " : System.DBNull.Value;");
            Code.AppendLine("\t\t\t\t\tcommand.ExecuteNonQuery();");
            Code.AppendLine("\t\t\t\t}");

            Code.AppendLine("\t\t\t}");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        private static DbType GetSqlDbType(Type type)
        {
            if (type == typeof(byte[]))
                return DbType.Binary;

            var parameter = new SqlParameter();
            var typeConverter = TypeDescriptor.GetConverter(parameter.DbType);
            var convertFrom = typeConverter.ConvertFrom(type.Name);
            if (convertFrom != null)
                parameter.DbType = (DbType)convertFrom;
            return parameter.DbType;
        }
    }
}
