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

        private void GenerateDatabaseClass()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Base class for all data access repositories");
            code.AppendLine("\tpublic static class Database");
            code.AppendLine("\t{");
            code.AppendLine("\t\tprivate static System.Data.IDbConnection connectionInstance;");
            code.AppendLine("\t\tprivate static readonly object syncLock = new object();");
            code.AppendLine();

            var connStr = new SqlCeConnectionStringBuilder(Database.ConnectionString);
            var dataSource = new FileInfo(connStr.DataSource);

            code.AppendLine("\t\tstatic Database()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tConnectionString = \"Data Source='" + dataSource.Name + "'\";");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Gets or sets the global SQL CE Connection String to be used");
            code.AppendLine("\t\tpublic static System.String ConnectionString { get; set; }");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Gets or sets the global SQL CE Connection instance");
            code.AppendLine("\t\tpublic static System.Data.IDbConnection Connection");
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
            code.AppendLine("\t\tpublic static System.Data.IDbCommand CreateCommand()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\treturn CreateCommand(null);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Create a SqlCeCommand instance using the global SQL CE Conection instance and associate this with a transaction", new KeyValuePair<string, string>("transaction", "SqlCeTransaction to be used for the SqlCeCommand"));
            code.AppendLine("\t\tpublic static System.Data.IDbCommand CreateCommand(System.Data.IDbTransaction transaction)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar command = Connection.CreateCommand();");
            code.AppendLine("\t\t\tcommand.Transaction = transaction;");
            code.AppendLine("\t\t\treturn command;");
            code.AppendLine("\t\t}");
            
            GenerateXmlDoc(code, 2, "Create a DbParameter instance using the global SQL CE Conection instance", new KeyValuePair<string, string>("name", "Name of the parameter"), new KeyValuePair<string, string>("type", "The database type"), new KeyValuePair<string, string>("value", "The actual value to set the parameter to"));
            code.AppendLine("\t\tpublic static System.Data.Common.DbParameter CreateParameter(string name, System.Data.SqlDbType type, object value)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\treturn new System.Data.SqlServerCe.SqlCeParameter(name, type) { Value = value };");
            code.AppendLine("\t\t}");

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("Database", code);
        }

        private void GenerateCreateDatabase()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            GenerateXmlDoc(code, 1, "Helper class for generating the database file in runtime");
            code.AppendLine("\tpublic class DatabaseFile");
            code.AppendLine("\t{");
            GenerateXmlDoc(code, 2, "Creates the database");
            code.AppendLine("\t\tpublic virtual int CreateDatabase()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tint resultCount = 0;");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var engine = new System.Data.SqlServerCe.SqlCeEngine(Database.ConnectionString))");
            code.AppendLine("\t\t\t\tengine.CreateDatabase();");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var transaction = Database.Connection.BeginTransaction())");
            code.AppendLine("\t\t\tusing (var command = Database.CreateCommand(transaction))");
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
            foreach (var table in Database.Tables)
                GenerateEntity(table, options);
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            GenerateDatabaseClass();
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