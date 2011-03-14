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
            code.AppendLine("\tpublic static class EntityBase");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic static System.String ConnectionString { get; set; }");
            code.AppendLine();
            code.AppendLine("\t\tprivate static System.Data.SqlServerCe.SqlCeConnection connectionInstance = null;");
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
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\tpublic static System.Data.SqlServerCe.SqlCeCommand CreateCommand()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\treturn Connection.CreateCommand();");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
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
            GenerateIRepository();

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t#region " + table.TableName + " Repository");

                code.AppendLine("\tpublic interface I" + table.TableName + "Repository : IRepository<" + table.TableName + ">");
                code.AppendLine("\t{");
                foreach (var column in table.Columns)
                {
                    if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                        continue;

                    if (column.Value.ManagedType.IsValueType)
                    {
                        code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2});", table.TableName, column.Value.ManagedType, column.Value.Name);
                        code.AppendLine();
                        code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1}? {2}, int count);", table.TableName, column.Value.ManagedType, column.Value.Name);
                        code.AppendLine();
                    }
                    else
                    {
                        code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2});", table.TableName, column.Value.ManagedType, column.Value.Name);
                        code.AppendLine();
                        code.AppendFormat("\t\tSystem.Collections.Generic.List<{0}> SelectBy{2}({1} {2}, int count);", table.TableName, column.Value.ManagedType, column.Value.Name);
                        code.AppendLine();
                    }
                }
                code.AppendLine("\t}");
                code.AppendLine();

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

                code.AppendLine("\t}");
                code.AppendLine("\t#endregion");
                code.AppendLine();
            }

            code.AppendLine("}");
        }

        private void GenerateIRepository()
        {
            code.AppendLine("\t#region Repository Interface");
            code.AppendLine("\tpublic interface IRepository<T>");
            code.AppendLine("\t{");
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList();");
            code.AppendLine("\t\tSystem.Collections.Generic.List<T> ToList(int count);");
            code.AppendLine("\t\tT[] ToArray();");
            code.AppendLine("\t\tT[] ToArray(int count);");
            code.AppendLine("\t\tvoid Create(T item);");
            code.AppendLine("\t\tvoid Create(System.Collections.Generic.IEnumerable<T> items);");
            code.AppendLine("\t\tvoid Update(T item);");
            code.AppendLine("\t\tvoid Delete(T item);");
            code.AppendLine("\t\tvoid Purge();");
            code.AppendLine("\t}");
            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        #region Generate Entities

        private void GenerateEntity(Table table, EntityGeneratorOptions options)
        {
            code.AppendLine("\t#region " + table.TableName);

            code.AppendLine("\tpublic partial class " + table.TableName);
            code.AppendLine("\t{");

            foreach (var column in table.Columns)
            {
                code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.Name + ";");

                if (column.Value.MaxLength > 0 && column.Value.ManagedType.Equals(typeof(string)))
                    code.AppendLine("\t\tpublic const int " + column.Value.Name + "_MAX_LENGTH = " + column.Value.MaxLength + ";");

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

            code.AppendLine("\t#endregion");
            code.AppendLine();
        }

        #endregion
    }
}