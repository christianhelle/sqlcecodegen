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
using System.Text;
using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class RepositoryPatternGenerator : CodeGenerator
    {
        private readonly DataAccessLayerGeneratorOptions options;
        private readonly bool supportSqlCeTransactions;
        private readonly bool usesLinqToSql;

        public RepositoryPatternGenerator(ISqlCeDatabase database, DataAccessLayerGeneratorOptions options, bool supportSqlCeTransactions = true, bool usesLinqToSql = false)
            : base(database)
        {
            this.options = options;
            this.supportSqlCeTransactions = supportSqlCeTransactions;
            this.usesLinqToSql = usesLinqToSql;
        }

        public void GenerateTableRepository<T>(Table table) where T : DataAccessLayerGenerator
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            //code.AppendLine("\tusing System.Linq;");
            //code.AppendLine("\tusing System.Data.Linq;");
            code.AppendLine();
            GenerateXmlDoc(code, 1, "Default I" + table.ClassName + "Repository implementation ");
            code.AppendLine("\tpublic partial class " + table.ClassName + "Repository : I" + table.ClassName + "Repository");
            code.AppendLine("\t{");

            if (usesLinqToSql)
            {
                GenerateXmlDoc(code, 2, "Creates an instance of " + table.ClassName + "Repository");
                code.AppendLine("\t\tpublic " + table.ClassName + "Repository() : this(new EntityDataContext())");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t}");
                code.AppendLine();

                GenerateXmlDoc(code, 2,
                               "Creates an instance of " + table.ClassName +
                               "Repository using the specified DataContext",
                               new KeyValuePair<string, string>("context", "The data context in use"));
                code.AppendLine("\t\tpublic " + table.ClassName + "Repository(EntityDataContext context)");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tDataContext = context;");
                code.AppendLine("\t\t}");
            }

            if (supportSqlCeTransactions)
            {
                code.AppendLine("\t\tpublic System.Data.IDbTransaction Transaction { get; set; }");
                code.AppendLine();
            }

            if (usesLinqToSql)
            {
                GenerateXmlDoc(code, 2, "Gets the DataContext in use");
                code.AppendLine("\t\tpublic EntityDataContext DataContext { get; private set; }");
                code.AppendLine();
            }

            var generator = (DataAccessLayerGenerator)Activator.CreateInstance(typeof(T), code, table);
            generator.GenerateCreateEntity();

            if (options.GenerateSelectAll)
                generator.GenerateSelectAll();

            if (options.GenerateSelectAllWithTop)
                generator.GenerateSelectWithTop();

            if (options.GenerateSelectBy)
                generator.GenerateSelectBy();

            if (options.GenerateSelectByWithTop)
                generator.GenerateSelectByWithTop();

            if (options.GenerateSelectByTwoColumns)
                generator.SelectByTwoColumns();

            if (options.GenerateSelectByThreeColumns)
                generator.SelectByThreeColumns();

            if (options.GenerateCreate)
                generator.GenerateCreate();

            if (options.GenerateCreateIgnoringPrimaryKey)
                generator.GenerateCreateIgnoringPrimaryKey();

            if (options.GenerateCreateUsingAllColumns)
                generator.GenerateCreateUsingAllColumns();

            if (options.GeneratePopulate)
                generator.GeneratePopulate();

            if (options.GenerateDelete)
                generator.GenerateDelete();

            if (options.GenerateDeleteBy)
                generator.GenerateDeleteBy();

            if (options.GenerateDeleteAll)
                generator.GenerateDeleteAll();

            if (options.GenerateUpdate)
                generator.GenerateUpdate();

            if (options.GenerateCount)
                generator.GenerateCount();

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode(table.ClassName + "Repository", code);
        }

        public void GenerateITableRepository(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Represents the " + table.ClassName + " repository");
            code.AppendLine("\tpublic partial interface I" + table.ClassName + "Repository : IRepository<" + table.ClassName + ">");
            code.AppendLine("\t{");

            if (supportSqlCeTransactions)
            {
                GenerateXmlDoc(code, 2, "Transaction instance created from <see cref=\"IDataRepository\" />");
                code.AppendLine("\t\tSystem.Data.IDbTransaction Transaction { get; set; }");
                code.AppendLine();
            }

            if (usesLinqToSql)
            {
                GenerateXmlDoc(code, 2, "Gets the DataContext in use");
                code.AppendLine("\t\tEntityDataContext DataContext { get; }");
                code.AppendLine();
            }

            foreach (var firstColumn in table.Columns)
            {
                if (String.Compare(firstColumn.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(firstColumn.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                if (firstColumn.Value.ManagedType.IsValueType)
                {
                    GenerateXmlDoc(code, 2, "Retrieves a collection of items by " + firstColumn.Value.FieldName, new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2});", table.ClassName, firstColumn.Value.ManagedType, firstColumn.Value.FieldName);
                    code.AppendLine("\n");
                    GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count by " + firstColumn.Value.FieldName,
                        new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count);", table.ClassName, firstColumn.Value.ManagedType, firstColumn.Value.FieldName);
                }
                else
                {
                    GenerateXmlDoc(code, 2, "Retrieves a collection of items by " + firstColumn.Value.FieldName, new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2});", table.ClassName, firstColumn.Value.ManagedType, firstColumn.Value.FieldName);
                    code.AppendLine("\n");
                    GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count by " + firstColumn.Value.FieldName,
                        new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count);", table.ClassName, firstColumn.Value.ManagedType, firstColumn.Value.FieldName);
                }
                code.AppendLine("\n");

                if (options.GenerateSelectByTwoColumns)
                {
                    foreach (var secondColumn in table.Columns)
                    {
                        if (secondColumn.Equals(firstColumn))
                            continue;

                        GenerateXmlDoc(code, 2,
                                       "Retrieves a collection of items by " + firstColumn.Value.FieldName + " and " + secondColumn.Value.FieldName,
                                       new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                                       new KeyValuePair<string, string>(secondColumn.Value.FieldName, secondColumn.Value.FieldName + " value"));

                        code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{3}And{6}({1}{2} {3}, {4}{5} {6});",
                                          table.ClassName,
                                          firstColumn.Value.ManagedType,
                                          firstColumn.Value.ManagedType.IsValueType ? "?" : "",
                                          firstColumn.Value.FieldName,
                                          secondColumn.Value.ManagedType,
                                          secondColumn.Value.ManagedType.IsValueType ? "?" : "",
                                          secondColumn.Value.FieldName);
                        code.AppendLine("\n");
                    }
                }

                if (options.GenerateSelectByThreeColumns)
                {
                    foreach (var secondColumn in table.Columns)
                    {
                        if (secondColumn.Equals(firstColumn))
                            continue;

                        foreach (var thirdColumn in table.Columns)
                        {
                            if (thirdColumn.Equals(firstColumn) || thirdColumn.Equals(secondColumn))
                                continue;

                            GenerateXmlDoc(code, 2,
                                           "Retrieves a collection of items by " + firstColumn.Value.FieldName + " and " + secondColumn.Value.FieldName + " and " + thirdColumn.Value.FieldName,
                                           new KeyValuePair<string, string>(firstColumn.Value.FieldName, firstColumn.Value.FieldName + " value"),
                                           new KeyValuePair<string, string>(secondColumn.Value.FieldName, secondColumn.Value.FieldName + " value"),
                                           new KeyValuePair<string, string>(thirdColumn.Value.FieldName, thirdColumn.Value.FieldName + " value"));

                            code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{3}And{6}And{9}({1}{2} {3}, {4}{5} {6}, {7}{8} {9});",
                                              table.ClassName,
                                              firstColumn.Value.ManagedType,
                                              firstColumn.Value.ManagedType.IsValueType ? "?" : "",
                                              firstColumn.Value.FieldName,
                                              secondColumn.Value.ManagedType,
                                              secondColumn.Value.ManagedType.IsValueType ? "?" : "",
                                              secondColumn.Value.FieldName,
                                              thirdColumn.Value.ManagedType,
                                              thirdColumn.Value.ManagedType.IsValueType ? "?" : "",
                                              thirdColumn.Value.FieldName);
                            code.AppendLine("\n");
                        }
                    }
                }
            }

            foreach (var column in table.Columns)
            {
                if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(column.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                GenerateXmlDoc(code, 2, "Delete records by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                if (column.Value.ManagedType.IsValueType)
                {
                    code.AppendFormat("\t\tint DeleteBy{1}({0}? {1});", column.Value.ManagedType, column.Value.FieldName);
                    code.AppendLine();
                }
                else
                {
                    code.AppendFormat("\t\tint DeleteBy{1}({0} {1});", column.Value.ManagedType, column.Value.FieldName);
                    code.AppendLine();
                }
                code.AppendLine();
            }

            if (!string.IsNullOrEmpty(table.PrimaryKeyColumnName))
            {
                GenerateXmlDoc(code, 2, "Create new record without specifying a primary key");
                code.Append("\t\tvoid Create(");
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
                code.Append(");\n");
                code.AppendLine();
            }

            GenerateXmlDoc(code, 2, "Create new record specifying all fields");
            code.Append("\t\tvoid Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Value.FieldName + ", ");
                else
                    code.Append(column.Value + " " + column.Value.FieldName + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(");\n");

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("I" + table.ClassName + "Repository", code);
        }

        public void GenerateDataRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Main Data Repository implementation containing all default table repositories implementations");
            code.AppendLine("\tpublic partial class DataRepository : IDataRepository");
            code.AppendLine("\t{");

            if (supportSqlCeTransactions)
            {
                code.AppendLine("\t\tprivate System.Data.IDbTransaction transaction;");
                code.AppendLine();
            }

            if (usesLinqToSql)
            {
                code.AppendLine("\t\tprivate EntityDataContext context;");
                code.AppendLine();
            }

            GenerateXmlDoc(code, 2, "Creates an instance of DataRepository");
            code.AppendLine("\t\tpublic DataRepository()");
            code.AppendLine("\t\t{");
            if (usesLinqToSql)
                code.AppendLine("\t\t\tcontext = new EntityDataContext();");
            foreach (var table in Database.Tables)
            {
                if (usesLinqToSql)
                    code.AppendLine("\t\t\t" + table.ClassName + " = new " + table.ClassName + "Repository(context);");
                else
                    code.AppendLine("\t\t\t" + table.ClassName + " = new " + table.ClassName + "Repository();");
            }
            code.AppendLine("\t\t}");
            code.AppendLine();

            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(code, 2, "Gets an instance of the I" + table.ClassName + "Repository");
                code.AppendLine("\t\tpublic I" + table.ClassName + "Repository " + table.ClassName + " { get; private set; }");
                code.AppendLine();
            }

            if (supportSqlCeTransactions)
            {
                GenerateXmlDoc(code, 2, "Starts a SqlCeTransaction using the global SQL CE Conection instance");
                code.AppendLine("\t\tpublic System.Data.IDbTransaction BeginTransaction()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tif (transaction != null)");
                code.AppendLine(
                    "\t\t\t\tthrow new System.InvalidOperationException(\"A transaction has already been started. Only one transaction is allowed\");");
                code.AppendLine("\t\t\ttransaction = Database.Connection.BeginTransaction();");
                foreach (var table in Database.Tables)
                    code.AppendLine("\t\t\t" + table.ClassName + ".Transaction = transaction;");
                code.AppendLine("\t\t\treturn transaction;");
                code.AppendLine("\t\t}");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Commits the transaction");
                code.AppendLine("\t\tpublic void Commit()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tif (transaction == null)");
                code.AppendLine(
                    "\t\t\t\tthrow new System.InvalidOperationException(\"No transaction has been started\");");
                code.AppendLine("\t\t\ttransaction.Commit();");
                code.AppendLine("\t\t\ttransaction = null;");
                foreach (var table in Database.Tables)
                    code.AppendLine("\t\t\t" + table.ClassName + ".Transaction = transaction;");
                code.AppendLine("\t\t}");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Rollbacks the transaction");
                code.AppendLine("\t\tpublic void Rollback()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tif (transaction == null)");
                code.AppendLine(
                    "\t\t\t\tthrow new System.InvalidOperationException(\"No transaction has been started\");");
                code.AppendLine("\t\t\ttransaction.Rollback();");
                code.AppendLine("\t\t\ttransaction = null;");
                foreach (var table in Database.Tables)
                    code.AppendLine("\t\t\t" + table.ClassName + ".Transaction = transaction;");
                code.AppendLine("\t\t}");
                code.AppendLine();
            }

            if (usesLinqToSql)
            {
                GenerateXmlDoc(code, 2, "Persists the pending changes to the database");
                code.AppendLine("\t\tpublic void SubmitChanges()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tcontext.SubmitChanges();");
                code.AppendLine("\t\t}");
                code.AppendLine();
            }

            GenerateXmlDoc(code, 2, "Releases the resources used. All uncommitted transactions are rolled back");
            code.AppendLine("\t\tpublic void Dispose()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tDispose(true);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\tprotected void Dispose(bool disposing)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (disposed) return;");
            code.AppendLine("\t\t\tif (disposing)");
            code.AppendLine("\t\t\t{");
            if (usesLinqToSql)
            {
                code.AppendLine("\t\t\t\tcontext.Dispose();");
                code.AppendLine("\t\t\t\tcontext = null;");
            }
            if (supportSqlCeTransactions)
            {
                code.AppendLine("\t\t\t\tif (transaction != null)");
                code.AppendLine("\t\t\t\t{");
                code.AppendLine("\t\t\t\t\ttransaction.Dispose();");
                code.AppendLine("\t\t\t\t\ttransaction = null;");
                code.AppendLine("\t\t\t\t}");
            }
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tdisposed = true;");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\tprivate bool disposed;");
            code.AppendLine();

            code.AppendLine("\t\t~DataRepository()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tDispose(false);");
            code.AppendLine("\t\t}");

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("DataRepository", code);
        }

        public void GenerateIDataRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Main Data Repository interface containing all table repositories");
            code.AppendLine("\tpublic partial interface IDataRepository : System.IDisposable");
            code.AppendLine("\t{");

            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(code, 2, "Gets an instance of the I" + table.ClassName + "Repository");
                code.AppendLine("\t\tI" + table.ClassName + "Repository " + table.ClassName + " { get; }");
                code.AppendLine();
            }

            if (supportSqlCeTransactions)
            {
                GenerateXmlDoc(code, 2, "Starts a SqlCeTransaction using the global SQL CE Conection instance");
                code.AppendLine("\t\tSystem.Data.IDbTransaction BeginTransaction();");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Commits the transaction");
                code.AppendLine("\t\tvoid Commit();");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Rollbacks the transaction");
                code.AppendLine("\t\tvoid Rollback();");
            }

            if (usesLinqToSql)
            {
                GenerateXmlDoc(code, 2, "Persists the pending changes to the database");
                code.AppendLine("\t\tvoid SubmitChanges();");
                code.AppendLine();
            }

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("IDataRepository", code);
        }

        public void GenerateIRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Base Repository interface defining the basic and commonly used data access methods");
            code.AppendLine("\tpublic partial interface IRepository<T>");
            code.AppendLine("\t{");
            GenerateXmlDoc(code, 2, "Retrieves all items as an IEnumerable collection");
            code.AppendLine("\t\tSystem.Collections.Generic.IEnumerable<T> ToEnumerable();");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count as an IEnumerable collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tSystem.Collections.Generic.IEnumerable<T> ToEnumerable(int count);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Retrieves all items as a generic collection");
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList();");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count as a generic collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList(int count);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Retrieves all items as an array of T");
            code.AppendLine("\t\tT[] ToArray();");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Retrieves the first set of items specific by count as an array of T", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tT[] ToArray(int count);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Inserts the item to the table", new KeyValuePair<string, string>("item", "Item to be inserted to the database"));
            code.AppendLine("\t\tvoid Create(T item);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Populates the table with a collection of items", new KeyValuePair<string, string>("items", "Items to be inserted to the database"));
            code.AppendLine("\t\tvoid Create(System.Collections.Generic.IEnumerable<T> items);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Updates the item", new KeyValuePair<string, string>("item", "Item to be updated on the database"));
            code.AppendLine("\t\tvoid Update(T item);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Updates a collection items", new KeyValuePair<string, string>("items", "Items to be updated on the database"));
            code.AppendLine("\t\tvoid Update(System.Collections.Generic.IEnumerable<T> items);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Deletes the item", new KeyValuePair<string, string>("item", "Item to be deleted from the database"));
            code.AppendLine("\t\tvoid Delete(T item);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Deletes a collection of item", new KeyValuePair<string, string>("items", "Items to be deleted from the database"));
            code.AppendLine("\t\tvoid Delete(System.Collections.Generic.IEnumerable<T> items);");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Purges the contents of the table");
            code.AppendLine("\t\tint Purge();");
            code.AppendLine();
            GenerateXmlDoc(code, 2, "Gets the number of records in the table");
            code.AppendLine("\t\tint Count();");
            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("IRepository", code);
        }
    }
}
