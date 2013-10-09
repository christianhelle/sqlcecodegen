﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMockDataAccessLayerCodeGenerator : DataAccessLayerGenerator
    {
        public CSharpMockDataAccessLayerCodeGenerator(StringBuilder code, Table table, bool supportSqlCeTransactions = true)
            : base(code, table)
        {
            Code.AppendLine("\t\tprivate readonly System.Collections.Generic.List<" + Table.ClassName + "> mockDataSource = new System.Collections.Generic.List<" + Table.ClassName + ">();");
            Code.AppendLine();

            if (supportSqlCeTransactions)
            {
                Code.AppendLine("\t\tpublic System.Data.IDbTransaction Transaction { get; set; }");
                Code.AppendLine();
            }
        }

        #region Overrides of DataAccessLayerGenerator

        public override void GenerateSelectAll()
        {
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\treturn mockDataSource;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();

            Code.AppendLine("\t\tpublic " + Table.ClassName + "[] ToArray()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = ToList();");
            Code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
            foreach (var column in Table.Columns)
            {
                if (System.String.Compare(column.Value.DatabaseType, "ntext", System.StringComparison.OrdinalIgnoreCase) == 0 ||
                    System.String.Compare(column.Value.DatabaseType, "image", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                Code.AppendFormat(
                    column.Value.ManagedType.IsValueType
                        ? "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2})"
                        : "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2})",
                    Table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                Code.AppendLine();
                Code.AppendLine("\t\t{");
                Code.AppendLine("\t\t\tif (!mockDataSource.Any(c => c." + column.Value.FieldName + " == " + column.Value.FieldName + ")) return null;");
                Code.AppendLine("\t\t\treturn mockDataSource.Where(c => c." + column.Value.FieldName + " == " + column.Value.FieldName + ").ToList();");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
            }
        }

        public override void GenerateSelectWithTop()
        {
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tif (!mockDataSource.Any()) return null;");
            Code.AppendLine("\t\t\treturn mockDataSource.Take(count).ToList();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();

            Code.AppendLine("\t\tpublic " + Table.ClassName + "[] ToArray(int count)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = ToList(count);");
            Code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateSelectByWithTop()
        {
            foreach (var column in Table.Columns)
            {
                if (System.String.Compare(column.Value.DatabaseType, "ntext", System.StringComparison.OrdinalIgnoreCase) == 0 ||
                    System.String.Compare(column.Value.DatabaseType, "image", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                Code.AppendFormat(
                    column.Value.ManagedType.IsValueType
                        ? "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count)"
                        : "\t\tpublic System.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count)",
                    Table.ClassName, column.Value.ManagedType, column.Value.FieldName);

                Code.AppendLine();
                Code.AppendLine("\t\t{");
                Code.AppendLine("\t\t\tif (!mockDataSource.Any(c => c." + column.Value.FieldName + " == " + column.Value.FieldName + ")) return null;");
                Code.AppendLine("\t\t\treturn mockDataSource.Where(c => c." + column.Value.FieldName + " == " + column.Value.FieldName + ").Take(count).ToList();");
                Code.AppendLine("\t\t}");
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
                        Code.AppendLine("\t\t{"); Code.AppendLine("\t\t\tif (!mockDataSource.Any(c => c." + firstColumn.Value.FieldName + " == " + firstColumn.Value.FieldName + " && c." + secondColumn.Value.FieldName + " == " + secondColumn.Value.FieldName + " && c." + thirdColumn.Value.FieldName + " == " + thirdColumn.Value.FieldName + ")) return null;");
                        Code.AppendLine("\t\t\treturn mockDataSource.Where(c => c." + firstColumn.Value.FieldName + " == " + firstColumn.Value.FieldName + " && c." + secondColumn.Value.FieldName + " == " + secondColumn.Value.FieldName + " && c." + thirdColumn.Value.FieldName + " == " + thirdColumn.Value.FieldName + ").ToList();");
                        Code.AppendLine("\t\t}");
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

                    Code.AppendFormat("\t\tpublic System.Collections.Generic.List<{0}> SelectBy{3}And{6}({1}{2} {3}, {4}{5} {6})",
                                   Table.ClassName,
                                   firstColumn.Value.ManagedType,
                                   firstColumn.Value.ManagedType.IsValueType ? "?" : "",
                                   firstColumn.Value.FieldName,
                                   secondColumn.Value.ManagedType,
                                   secondColumn.Value.ManagedType.IsValueType ? "?" : "",
                                   secondColumn.Value.FieldName);

                    Code.AppendLine();
                    Code.AppendLine("\t\t{"); Code.AppendLine("\t\t\tif (!mockDataSource.Any(c => c." + firstColumn.Value.FieldName + " == " + firstColumn.Value.FieldName + " && c." + secondColumn.Value.FieldName + " == " + secondColumn.Value.FieldName + ")) return null;");
                    Code.AppendLine("\t\t\treturn mockDataSource.Where(c => c." + firstColumn.Value.FieldName + " == " + firstColumn.Value.FieldName + " && c." + secondColumn.Value.FieldName + " == " + secondColumn.Value.FieldName + ").ToList();");
                    Code.AppendLine("\t\t}");
                    Code.AppendLine();
                }
            }
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
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
            Code.AppendLine("\t\t\tCreate(new " + Table.ClassName);
            Code.AppendLine("\t\t\t{");
            foreach (var column in Table.Columns)
            {
                if (column.Value.Name == Table.PrimaryKeyColumnName)
                    continue;
                Code.AppendLine("\t\t\t\t" + column.Value.FieldName + " = " + column.Value.FieldName + ", ");
            }
            Code.Remove(Code.Length - 2, 2);
            Code.AppendLine("\n\t\t\t});");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateCreateUsingAllColumns()
        {
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
            Code.AppendLine("\t\t\tCreate(new " + Table.ClassName);
            Code.AppendLine("\t\t\t{");
            foreach (var column in Table.Columns)
                Code.AppendLine("\t\t\t\t" + column.Value.FieldName + " = " + column.Value.FieldName + ", ");
            Code.Remove(Code.Length - 2, 2);
            Code.AppendLine("\n\t\t\t});");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateDelete()
        {
            Code.AppendLine("\t\tpublic void Delete(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tmockDataSource.Remove(item);");
            Code.AppendLine("\t\t}");
            Code.AppendLine();

            Code.AppendLine("\t\tpublic void Delete(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tforeach (var item in items) mockDataSource.Remove(item);");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateDeleteBy()
        {
            foreach (var column in Table.Columns)
            {
                if (System.String.Compare(column.Value.DatabaseType, "ntext", System.StringComparison.OrdinalIgnoreCase) == 0 ||
                    System.String.Compare(column.Value.DatabaseType, "image", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                Code.AppendFormat("\t\tpublic int DeleteBy{1}({0}{2} {1})\n", column.Value.ManagedType, column.Value.FieldName,
                                  column.Value.ManagedType.IsValueType ? "?" : string.Empty);

                Code.AppendLine("\t\t{");
                Code.AppendLine("\t\t\tvar items = mockDataSource.Where(c => c." + column.Value.FieldName + " == " + column.Value.FieldName + ");");
                Code.AppendLine("\t\t\tvar count = 0;");
                Code.AppendLine("\t\t\tforeach (var item in new System.Collections.Generic.List<" + Table.ClassName + ">(items))");
                Code.AppendLine("\t\t\t{");
                Code.AppendLine("\t\t\t\tmockDataSource.Remove(item);");
                Code.AppendLine("\t\t\t\tcount++;");
                Code.AppendLine("\t\t\t}");
                Code.AppendLine("\t\t\treturn count;");
                Code.AppendLine("\t\t}");
                Code.AppendLine();
            }
        }

        public override void GenerateDeleteAll()
        {
            Code.AppendLine("\t\tpublic int Purge()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar returnValue = mockDataSource.Count;");
            Code.AppendLine("\t\t\tmockDataSource.Clear();");
            Code.AppendLine("\t\t\treturn returnValue;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateUpdate()
        {
            Code.AppendLine("\t\tpublic void Update(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            if (!string.IsNullOrEmpty(Table.PrimaryKeyColumnName))
            {
                Code.AppendLine("\t\t\tfor (int i = 0; i < mockDataSource.Count; i++)");
                Code.AppendLine("\t\t\t{");
                var column = Table.Columns.Values.First(c => c.IsPrimaryKey);
                Code.AppendLine("\t\t\t\tif (mockDataSource[i]." + column.FieldName + " == item." + column.FieldName + ")");
                Code.AppendLine("\t\t\t\t\tmockDataSource[i] = item;");
                Code.AppendLine("\t\t\t}");
            }
            Code.AppendLine("\t\t}");
            Code.AppendLine();

            Code.AppendLine("\t\tpublic void Update(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tforeach (var item in items) Update(item);");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GeneratePopulate()
        {
            Code.AppendLine("\t\tpublic void Create(System.Collections.Generic.IEnumerable<" + Table.ClassName + "> items)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tmockDataSource.AddRange(items);");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateCreate()
        {
            Code.AppendLine("\t\tpublic void Create(" + Table.ClassName + " item)");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tmockDataSource.Add(item);");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        public override void GenerateCount()
        {
            Code.AppendLine("\t\tpublic int Count()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\treturn mockDataSource.Count;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
        }

        #endregion
    }
}
