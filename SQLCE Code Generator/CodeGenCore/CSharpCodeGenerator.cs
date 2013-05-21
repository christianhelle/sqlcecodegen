using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpCodeGenerator : CodeGenerator
    {
        public CSharpCodeGenerator(ISqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        private void GenerateEntityBase()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Base class for all data access repositories");
            code.AppendLine("\tpublic static class EntityBase");
            code.AppendLine("\t{");
            code.AppendLine("\t\tprivate static System.Data.SqlServerCe.SqlCeConnection connectionInstance;");
            code.AppendLine("\t\tprivate static readonly object syncLock = new object();");
            code.AppendLine();

            var connStr = new SqlCeConnectionStringBuilder(Database.ConnectionString);
            var dataSource = new FileInfo(connStr.DataSource);

            code.AppendLine("\t\tstatic EntityBase()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tConnectionString = \"Data Source='" + dataSource.Name + "'\";");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Gets or sets the global SQL CE Connection String to be used");
            code.AppendLine("\t\tpublic static System.String ConnectionString { get; set; }");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Gets or sets the global SQL CE Connection instance");
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeConnection Connection");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tget");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tlock (syncLock)");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\tif (connectionInstance == null)");
            code.AppendLine("\t\t\t\t\t\tconnectionInstance = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);");
            code.AppendLine("\t\t\t\t\tif (connectionInstance.State != System.Data.ConnectionState.Open)");
            code.AppendLine("\t\t\t\t\t\tconnectionInstance.Open();");
            code.AppendLine("\t\t\t\t\treturn connectionInstance;");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tset");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tlock (syncLock)");
            code.AppendLine("\t\t\t\t\tconnectionInstance = value;");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Create a SqlCeCommand instance using the global SQL CE Conection instance");
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeCommand CreateCommand()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\treturn CreateCommand(null);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Create a SqlCeCommand instance using the global SQL CE Conection instance and associate this with a transaction", new KeyValuePair<string, string>("transaction", "SqlCeTransaction to be used for the SqlCeCommand"));
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeCommand CreateCommand(System.Data.SqlServerCe.SqlCeTransaction transaction)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar command = Connection.CreateCommand();");
            code.AppendLine("\t\t\tcommand.Transaction = transaction;");
            code.AppendLine("\t\t\treturn command;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("EntityBase", code);
        }

        private void GenerateCreateDatabase()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Helper class for generating the database file in runtime");
            code.AppendLine("\tpublic static class DatabaseFile");
            code.AppendLine("\t{");
            GenerateXmlDoc(code, 2, "Creates the database");
            code.AppendLine("\t\tpublic static int CreateDatabase()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tint resultCount = 0;");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var engine = new System.Data.SqlServerCe.SqlCeEngine(EntityBase.ConnectionString))");
            code.AppendLine("\t\t\t\tengine.CreateDatabase();");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var transaction = EntityBase.Connection.BeginTransaction())");
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand(transaction))");
            code.AppendLine("\t\t\t{");
            foreach (var table in Database.Tables)
            {
                code.Append("\t\t\t\tcommand.CommandText = ");
                code.Append("\"CREATE TABLE " + table.Name);
                code.Append("(");
                foreach (var column in table.Columns)
                {
                    code.AppendFormat("{0} {1}", column.Value.FieldName, column.Value.DatabaseType.ToUpper());
                    if (String.Compare(column.Value.DatabaseType, "ntext", StringComparison.OrdinalIgnoreCase) == 0 ||
                        String.Compare(column.Value.DatabaseType, "image", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        code.Append(", ");
                        continue;
                    }
                    if (column.Value.ManagedType == typeof(string))
                        code.Append("(" + column.Value.MaxLength + ")");
                    if (column.Value.AutoIncrement.HasValue)
                        code.AppendFormat(" IDENTITY({0},{1})", column.Value.AutoIncrementSeed, column.Value.AutoIncrement);
                    if (column.Value.IsPrimaryKey)
                        code.Append(" PRIMARY KEY");
                    if (!column.Value.AllowsNull)
                        code.Append(" NOT NULL");
                    code.Append(", ");
                }
                code.Remove(code.Length - 2, 2);
                code.Append(")\";");
                code.AppendLine();
                code.AppendLine("\t\t\t\tresultCount += command.ExecuteNonQuery();");
                code.AppendLine();
            }
            code.Remove(code.Length - 1, 1);
            code.AppendLine("\t\t\t\ttransaction.Commit();");
            code.AppendLine("\t\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t\treturn resultCount;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("DatabaseFile", code);
        }

        public override void GenerateEntities()
        {
            GenerateEntities(new EntityGeneratorOptions());
        }

        public override void GenerateEntities(EntityGeneratorOptions options)
        {
            //Code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            //Code.AppendLine("{");

            foreach (var table in Database.Tables)
                GenerateEntity(table, options);

            //Code.AppendLine("}");
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            GenerateEntityBase();
            GenerateCreateDatabase();

            var repositoryPatternGenerator = new RepositoryPatternGenerator(Database, true);
            repositoryPatternGenerator.GenerateIRepository();
            repositoryPatternGenerator.GenerateIDataRepository();
            repositoryPatternGenerator.GenerateDataRepository();

            foreach (var table in Database.Tables)
            {
                repositoryPatternGenerator.GenerateITableRepository(table);
                repositoryPatternGenerator.GenerateTableRepository<CSharpDataAccessLayerGenerator>(table);
            }

            foreach (var codeFile in repositoryPatternGenerator.CodeFiles)
                AppendCode(codeFile.Key, codeFile.Value);
        }

/*
        private void GenerateTableRepository(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Default I" + table.ClassName + "Repository implementation ");
            code.AppendLine("\tpublic partial class " + table.ClassName + "Repository : I" + table.ClassName + "Repository");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic System.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }");
            code.AppendLine();

            DataAccessLayerGenerator generator = new CSharpDataAccessLayerGenerator(code, table);
            generator.GenerateSelectAll();
            generator.GenerateSelectWithTop();
            generator.GenerateSelectBy();
            generator.GenerateSelectByWithTop();
            generator.GenerateCreate();
            generator.GenerateCreateIgnoringPrimaryKey();
            generator.GenerateCreateUsingAllColumns();
            generator.GeneratePopulate();
            generator.GenerateDelete();
            generator.GenerateDeleteBy();
            generator.GenerateDeleteAll();
            generator.GenerateUpdate();
            generator.GenerateCount();

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode(table.ClassName + "Repository", code);
        }
*/

/*
        private void GenerateITableRepository(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Represents the " + table.ClassName + " repository");
            code.AppendLine("\tpublic partial interface I" + table.ClassName + "Repository : IRepository<" + table.ClassName + ">");
            code.AppendLine("\t{");
            GenerateXmlDoc(code, 2, "Transaction instance created from <see cref=\"IDataRepository\" />");
            code.AppendLine("\t\tSystem.Data.SqlServerCe.SqlCeTransaction Transaction { get; set; }");
            code.AppendLine();

            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                if (column.Value.ManagedType.IsValueType)
                {
                    GenerateXmlDoc(code, 2, "Retrieves a collection of items by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2});", table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                    code.AppendLine("\n");
                    GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count by " + column.Value.FieldName,
                        new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count);", table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                }
                else
                {
                    GenerateXmlDoc(code, 2, "Retrieves a collection of items by " + column.Value.FieldName, new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2});", table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                    code.AppendLine("\n");
                    GenerateXmlDoc(code, 2, "Retrieves the first set of items specified by count by " + column.Value.FieldName,
                        new KeyValuePair<string, string>(column.Value.FieldName, column.Value.FieldName + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count);", table.ClassName, column.Value.ManagedType, column.Value.FieldName);
                }
                code.AppendLine("\n");
            }

            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
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
*/

/*
        private void GenerateIDataRepository()
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
            GenerateXmlDoc(code, 2, "Starts a SqlCeTransaction using the global SQL CE Conection instance");
            code.AppendLine("\t\tSystem.Data.SqlServerCe.SqlCeTransaction BeginTransaction();");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Commits the transaction");
            code.AppendLine("\t\tvoid Commit();");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Rollbacks the transaction");
            code.AppendLine("\t\tvoid Rollback();");
            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("IDataRepository", code);
        }
*/

/*
        private void GenerateDataRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Main Data Repository implementation containing all default table repositories implementations");
            code.AppendLine("\tpublic partial class DataRepository : IDataRepository");
            code.AppendLine("\t{");
            code.AppendLine("\t\tprivate System.Data.SqlServerCe.SqlCeTransaction transaction = null;");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of DataRepository");
            code.AppendLine("\t\tpublic DataRepository()");
            code.AppendLine("\t\t{");
            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t\t\t" + table.ClassName + " = new " + table.ClassName + "Repository();");
            }
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of DataRepository",
                           new KeyValuePair<string, string>("connectionString", "Connection string to use"));
            code.AppendLine("\t\tpublic DataRepository(string connectionString) : this()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tEntityBase.ConnectionString = connectionString;");
            code.AppendLine("\t\t}");
            code.AppendLine();

            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(code, 2, "Gets an instance of the I" + table.ClassName + "Repository");
                code.AppendLine("\t\tpublic I" + table.ClassName + "Repository " + table.ClassName + " { get; private set; }");
                code.AppendLine();
            }

            GenerateXmlDoc(code, 2, "Starts a SqlCeTransaction using the global SQL CE Conection instance");
            code.AppendLine("\t\tpublic System.Data.SqlServerCe.SqlCeTransaction BeginTransaction()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (transaction != null)");
            code.AppendLine(
                "\t\t\t\tthrow new System.InvalidOperationException(\"A transaction has already been started. Only one transaction is allowed\");");
            code.AppendLine("\t\t\ttransaction = EntityBase.Connection.BeginTransaction();");
            foreach (var table in Database.Tables)
                code.AppendLine("\t\t\t" + table.ClassName + ".Transaction = transaction;");
            code.AppendLine("\t\t\treturn transaction;");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Commits the transaction");
            code.AppendLine("\t\tpublic void Commit()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (transaction == null)");
            code.AppendLine("\t\t\t\tthrow new System.InvalidOperationException(\"No transaction has been started\");");
            code.AppendLine("\t\t\ttransaction.Commit();");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Rollbacks the transaction");
            code.AppendLine("\t\tpublic void Rollback()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (transaction == null)");
            code.AppendLine("\t\t\t\tthrow new System.InvalidOperationException(\"No transaction has been started\");");
            code.AppendLine("\t\t\ttransaction.Rollback();");
            code.AppendLine("\t\t}");
            code.AppendLine();

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
            code.AppendLine("\t\t\t\tif (transaction != null)");
            code.AppendLine("\t\t\t\t{");
            code.AppendLine("\t\t\t\t\ttransaction.Dispose();");
            code.AppendLine("\t\t\t\t\ttransaction = null;");
            code.AppendLine("\t\t\t\t}");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tdisposed = true;");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\tprivate bool disposed = false;");
            code.AppendLine();

            code.AppendLine("\t\t~DataRepository()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tDispose(false);");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("DataRepository", code);
        }
*/

/*
        private void GenerateIRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Base Repository interface defining the basic and commonly used data access methods");
            code.AppendLine("\tpublic partial interface IRepository<T>");
            code.AppendLine("\t{");
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
*/

        #region Generate Entities

        private void GenerateEntity(Table table, EntityGeneratorOptions options)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Represents the " + table.ClassName + " table");
            code.AppendLine("\tpublic partial class " + table.ClassName);
            code.AppendLine("\t{");

            // Create backing fields if the type of the field is a string
            foreach (var column in table.Columns)
            {
                if (options.AutoPropertiesOnly)
                    continue;
                
                if (!(column.Value.MaxLength > 0) || column.Value.ManagedType != typeof (string)) 
                    continue;
                
                code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.FieldName + ";");
            }
            code.AppendLine();

            // Create public constants describing the max length allowed for each string field
            foreach (var column in table.Columns)
            {
                if (options.AutoPropertiesOnly)
                    continue;

                if (!(column.Value.MaxLength > 0) || column.Value.ManagedType != typeof(string))
                    continue;

                GenerateXmlDoc(code, 2, "The Maximum Length the " + column.Value.FieldName + " field allows");
                code.AppendLine("\t\tpublic const int " + column.Value.FieldName + "_Max_Length = " + column.Value.MaxLength + ";");
                code.AppendLine();
            }

            foreach (var column in table.Columns)
            {
                GenerateXmlDoc(code, 2, "Gets or sets the value of " + column.Value.FieldName);
                
                if (!options.AutoPropertiesOnly && column.Value.MaxLength > 0 && column.Value.ManagedType == typeof (string))
                {
                    // Property with backing field
                    code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.FieldName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _" + column.Value.FieldName + "; }");
                    code.AppendLine("\t\t\tset");
                    code.AppendLine("\t\t\t{");
                    code.AppendLine("\t\t\t\t_" + column.Value.FieldName + " = value;");

                    if (options.ThrowExceptions)
                    {
                        code.AppendLine("\t\t\t\tif (_" + column.Value.FieldName + " != null && " + column.Value.FieldName + ".Length > " + column.Value.FieldName + "_Max_Length)");
                        code.AppendLine("\t\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.FieldName + " is " + column.Value.MaxLength + "\");"); 
                    }
                    
                    code.AppendLine("\t\t\t}");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }
                else
                {
                    // Auto property
                    code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.FieldName + " { get; set; }");
                    code.AppendLine();
                }
            }

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode(table.ClassName, code);
        }

        #endregion
    }
}