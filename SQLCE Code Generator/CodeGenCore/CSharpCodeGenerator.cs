using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpCodeGenerator : CodeGenerator
    {
        public CSharpCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        private void GenerateEntityBase()
        {
            code.AppendLine("\t#region EntityBase");
            code.AppendLine();
            GenerateXmlDoc(1, "Base class for all data access repositories");
            code.AppendLine("\tpublic static class EntityBase");
            code.AppendLine("\t{");
            GenerateXmlDoc(2, "Gets or sets the global SQL CE Connection String to be used");
            code.AppendLine("\t\tpublic static System.String ConnectionString { get; set; }");
            code.AppendLine();

            code.AppendLine("\t\tprivate static System.Data.SqlServerCe.SqlCeConnection connectionInstance = null;");
            GenerateXmlDoc(2, "Gets or sets the global SQL CE Connection instance");
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeConnection Connection");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tget");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tif (connectionInstance == null)");
            code.AppendLine("\t\t\t\t\tconnectionInstance = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);");
            code.AppendLine("\t\t\t\tif (connectionInstance.State != System.Data.ConnectionState.Open)");
            code.AppendLine("\t\t\t\t\tconnectionInstance.Open();");
            code.AppendLine("\t\t\t\treturn connectionInstance;");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tset { connectionInstance = value; }");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(2, "Create a SqlCeCommand instance using the global SQL CE Conection instance");
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeCommand CreateCommand()");
            code.AppendLine("\t\t{");
            //code.AppendLine("\t\t\tif (Connection.State != System.Data.ConnectionState.Open)");
            //code.AppendLine("\t\t\t\tConnection.Open();");
            code.AppendLine("\t\t\treturn Connection.CreateCommand();");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine();
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        private void GenerateCreateDatabase()
        {
            code.AppendLine("\t#region DatabaseFile");
            code.AppendLine();
            GenerateXmlDoc(1, "Helper class for generating the database file in runtime");
            code.AppendLine("\tpublic static class DatabaseFile");
            code.AppendLine("\t{");
            GenerateXmlDoc(2, "Creates the database");
            code.AppendLine("\t\tpublic static int CreateDatabase()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tint resultCount = 0;");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var engine = new System.Data.SqlServerCe.SqlCeEngine(EntityBase.ConnectionString))");
            code.AppendLine("\t\t\t\tengine.CreateDatabase();");
            code.AppendLine();
            code.AppendLine("\t\t\tusing (var command = EntityBase.CreateCommand())");
            code.AppendLine("\t\t\t{");
            foreach (var table in Database.Tables)
            {
                code.Append("\t\t\t\tcommand.CommandText = ");
                code.Append("\"CREATE TABLE " + table.TableName);
                code.Append("(");
                foreach (var column in table.Columns)
                {
                    code.AppendFormat("{0} {1}", column.Key, column.Value.DatabaseType.ToUpper());
                    if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 ||
                        string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    {
                        code.Append(", ");
                        continue;
                    }
                    if (column.Value.ManagedType == typeof(string))
                        code.Append("(" + column.Value.MaxLength + ")");
                    if (!column.Value.AllowsNull)
                        code.Append(" NOT NULL");
                    if (column.Value.IsPrimaryKey)
                        code.Append(" PRIMARY KEY");
                    code.Append(", ");
                }
                code.Remove(code.Length - 2, 2);
                code.Append(")\";");
                code.AppendLine();
                code.AppendLine("\t\t\t\tresultCount += command.ExecuteNonQuery();");
                code.AppendLine();
            }
            code.AppendLine("\t\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t\treturn resultCount;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine();
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        public override void GenerateEntities()
        {
            GenerateEntities(new EntityGeneratorOptions());
        }

        public override void GenerateEntities(EntityGeneratorOptions options)
        {
            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            foreach (var table in Database.Tables)
                GenerateEntity(table, options);

            code.AppendLine("}");
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            GenerateEntityBase();
            GenerateCreateDatabase();
            GenerateIRepository();
            GenerateIDataRepository();

            foreach (var table in Database.Tables)
            {
                GenerateITableRepository(table);
                GenerateTableRepository(table);
            }

            code.AppendLine("}");
        }

        private void GenerateTableRepository(Table table)
        {
            code.AppendLine("\t#region " + table.TableName + " Repository");
            code.AppendLine("\tpublic partial class " + table.TableName + "Repository : I" + table.TableName + "Repository");
            code.AppendLine("\t{");

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
            generator.GenerateSaveChanges();
            generator.GenerateCount();

            code.AppendLine("\t}");
            code.AppendLine();
            code.AppendLine("\t#endregion");
        }

        private void GenerateITableRepository(Table table)
        {
            code.AppendLine("\t#region I" + table.TableName + " Repository");
            code.AppendLine();

            GenerateXmlDoc(1, "Default I" + table.TableName + "Repository implementation ");
            code.AppendLine("\tpublic partial interface I" + table.TableName + "Repository : IRepository<" + table.TableName + ">");
            code.AppendLine("\t{");

            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                if (column.Value.ManagedType.IsValueType)
                {
                    GenerateXmlDoc(2, "Retrieves a collection of items by " + column.Value.Name, new KeyValuePair<string, string>(column.Value.Name, column.Value.Name + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2});", table.TableName, column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                    GenerateXmlDoc(2, "Retrieves the first set of items specified by count by " + column.Value.Name,
                        new KeyValuePair<string, string>(column.Value.Name, column.Value.Name + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count);", table.TableName, column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                }
                else
                {
                    GenerateXmlDoc(2, "Retrieves a collection of items by " + column.Value.Name, new KeyValuePair<string, string>(column.Value.Name, column.Value.Name + " value"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2});", table.TableName, column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                    GenerateXmlDoc(2, "Retrieves the first set of items specified by count by " + column.Value.Name,
                        new KeyValuePair<string, string>(column.Value.Name, column.Value.Name + " value"),
                        new KeyValuePair<string, string>("count", "the number of records to be retrieved"));
                    code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count);", table.TableName, column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                }
            }

            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                GenerateXmlDoc(2, "Delete records by " + column.Value.Name, new KeyValuePair<string, string>(column.Value.Name, column.Value.Name + " value"));
                if (column.Value.ManagedType.IsValueType)
                {
                    code.AppendFormat("\t\tint DeleteBy{1}({0}? {1});", column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                }
                else
                {
                    code.AppendFormat("\t\tint DeleteBy{1}({0} {1});", column.Value.ManagedType, column.Value.Name);
                    code.AppendLine();
                }
            }

            GenerateXmlDoc(2, "Create new record without specifying a primary key");
            code.Append("\t\tvoid Create(");
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
            code.Append(");\n");

            GenerateXmlDoc(2, "Create new record specifying all fields");
            code.Append("\t\tvoid Create(");
            foreach (var column in table.Columns)
            {
                if (column.Value.ManagedType.IsValueType)
                    code.Append(column.Value + "? " + column.Key + ", ");
                else
                    code.Append(column.Value + " " + column.Key + ", ");
            }
            code.Remove(code.Length - 2, 2);
            code.Append(");\n");

            code.AppendLine("\t}");
            code.AppendLine();
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        private void GenerateIDataRepository()
        {
            code.AppendLine("\t#region IDataRepository");
            code.AppendLine();
            GenerateXmlDoc(1, "Main Data Repository interface containing all table repositories");
            code.AppendLine("\tpublic partial interface IDataRepository");
            code.AppendLine("\t{");
            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(2, "Gets an instance of the I" + table.TableName + "Repository");
                code.AppendLine("\t\tI" + table.TableName + "Repository " + table.TableName + " { get; }");
            }
            code.AppendLine("\t}");
            code.AppendLine("\t#endregion");
            code.AppendLine();

            code.AppendLine("\t#region DataRepository");
            code.AppendLine();
            GenerateXmlDoc(1, "Main Data Repository implementation containing all default table repositories implementations");
            code.AppendLine("\tpublic partial class DataRepository : IDataRepository");
            code.AppendLine("\t{");
            GenerateXmlDoc(2, "Creates an instance of DataRepository");
            code.AppendLine("\t\tpublic DataRepository()");
            code.AppendLine("\t\t{");
            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t\t\t" + table.TableName + " = new " + table.TableName + "Repository();");
            }
            code.AppendLine("\t\t}");
            code.AppendLine();
            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(2, "Gets an instance of the I" + table.TableName + "Repository");
                code.AppendLine("\t\tpublic I" + table.TableName + "Repository " + table.TableName + " { get; private set; }");
            }
            code.AppendLine("\t}");
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        private void GenerateIRepository()
        {
            code.AppendLine("\t#region Repository Interface");
            code.AppendLine();
            GenerateXmlDoc(1, "Base Repository interface defining the basic and commonly used data access methods");
            code.AppendLine("\tpublic partial interface IRepository<T>");
            code.AppendLine("\t{");
            GenerateXmlDoc(2, "Retrieves all items as a generic collection");
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList();");
            GenerateXmlDoc(2, "Retrieves the first set of items specified by count as a generic collection", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList(int count);");
            GenerateXmlDoc(2, "Retrieves all items as an array of T");
            code.AppendLine("\t\tT[] ToArray();");
            GenerateXmlDoc(2, "Retrieves the first set of items specific by count as an array of T", new KeyValuePair<string, string>("count", "Number of records to be retrieved"));
            code.AppendLine("\t\tT[] ToArray(int count);");
            GenerateXmlDoc(2, "Inserts the item to the table", new KeyValuePair<string, string>("item", "Item to be inserted to the database"));
            code.AppendLine("\t\tvoid Create(T item);");
            GenerateXmlDoc(2, "Populates the table with a collection of items", new KeyValuePair<string, string>("items", "Items to be inserted to the database"));
            code.AppendLine("\t\tvoid Create(System.Collections.Generic.IEnumerable<T> items);");
            GenerateXmlDoc(2, "Updates the item", new KeyValuePair<string, string>("item", "Item to be updated on the database"));
            code.AppendLine("\t\tvoid Update(T item);");
            GenerateXmlDoc(2, "Deletes the item", new KeyValuePair<string, string>("item", "Item to be deleted from the database"));
            code.AppendLine("\t\tvoid Delete(T item);");
            GenerateXmlDoc(2, "Purges the contents of the table");
            code.AppendLine("\t\tint Purge();");
            GenerateXmlDoc(2, "Gets the number of records in the table");
            code.AppendLine("\t\tint Count();");
            code.AppendLine("\t}");
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        #region Generate Entities

        private void GenerateEntity(Table table, EntityGeneratorOptions options)
        {
            code.AppendLine("\t#region " + table.TableName);
            code.AppendLine();

            GenerateXmlDoc(1, "Represents the " + table.TableName + " table");
            code.AppendLine("\tpublic partial class " + table.TableName);
            code.AppendLine("\t{");

            foreach (var column in table.Columns)
            {
                code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.Name + ";");

                if (column.Value.MaxLength > 0 && column.Value.ManagedType.Equals(typeof(string)))
                {
                    GenerateXmlDoc(2, "The Maximum Length the " + column.Value.Name + " field allows");
                    code.AppendLine("\t\tpublic const int " + column.Value.Name + "_MAX_LENGTH = " + column.Value.MaxLength + ";");
                }

                GenerateXmlDoc(2, "Gets or sets the value of " + column.Value.Name);
                code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.Name);
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\t get { return _" + column.Value.Name + "; }");
                code.AppendLine("\t\t\t set");
                code.AppendLine("\t\t\t{");
                code.AppendLine("\t\t\t\t_" + column.Value.Name + " = value;");

                if (column.Value.MaxLength > 0 && column.Value.ManagedType.Equals(typeof(string)))
                {
                    code.AppendLine("\t\t\t\tif (_" + column.Value.Name + " != null && " + column.Value.Name + ".Length > " + column.Value.Name + "_MAX_LENGTH)");
                    code.AppendLine("\t\t\t\t\tthrow new System.ArgumentException(\"Max length for " + column.Value.Name + " is " + column.Value.MaxLength + "\");");
                }

                code.AppendLine("\t\t\t}");
                code.AppendLine("\t\t}");
            }
            code.AppendLine("\t}");
            code.AppendLine();

            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        #endregion
    }
}