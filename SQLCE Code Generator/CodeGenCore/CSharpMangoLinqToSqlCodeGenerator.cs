using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoLinqToSqlCodeGenerator : CodeGenerator
    {
        public CSharpMangoLinqToSqlCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        public override void GenerateEntities()
        {
            GenerateEntities(new EntityGeneratorOptions());
        }

        public override void GenerateEntities(EntityGeneratorOptions options)
        {
            foreach (var table in Database.Tables)
            {
                var code = new StringBuilder();

                code.AppendLine("namespace " + Database.Namespace);
                code.AppendLine("{");
                code.AppendLine("\tusing System.Data.Linq;");
                code.AppendLine("\tusing System.Data.Linq.Mapping;");
                code.AppendLine("\tusing System.ComponentModel;");
                code.AppendLine("\tusing Microsoft.Phone.Data.Linq.Mapping;");
                code.AppendLine();

                GenerateXmlDoc(code, 1, "Represents the " + table.DisplayName + " table in the database");
                code.AppendLine("\t[Table]");
                foreach (var index in table.Indexes)
                    code.AppendLine("\t[Index(Columns = \"" + index.Column.Name + "\", IsUnique = " + index.Unique.ToString(CultureInfo.InvariantCulture).ToLower() + ", Name = \"" + index.Name + "\")]");
                code.AppendLine("\tpublic partial class " + table.ClassName + " : INotifyPropertyChanged, INotifyPropertyChanging");
                code.AppendLine("\t{");

                foreach (var column in table.Columns)
                    code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.FieldName + ";");

                foreach (var reference in table.References)
                    code.AppendLine("\t\tprivate EntityRef<" + reference.ReferenceTable.ClassName + "> _" + reference.ReferenceTable.ClassName + ";");

                foreach (var referencedBy in table.ReferencedBy)
                    code.AppendLine("\t\tprivate readonly EntitySet<" + referencedBy.ReferenceTable.ClassName + "> _Associated" + referencedBy.ReferenceTable.ClassName + " = new EntitySet<" + referencedBy.ReferenceTable.ClassName + ">();");

                code.AppendLine();

                //foreach (var column in table.Columns.Where(column => column.Value.MaxLength > 0 && column.Value.ManagedType.Equals(typeof(string))))
                //{
                //    GenerateXmlDoc(code, 2, "The Maximum Length the " + column.Value.FieldName + " field allows");
                //    code.AppendLine("\t\tpublic const int " + column.Value.FieldName + "_Max_Length = " + column.Value.MaxLength + ";");
                //    code.AppendLine();
                //}

                code.AppendLine("\t\t#pragma warning disable");
                code.AppendLine("\t\t[Column(IsVersion = true)]");
                code.AppendLine("\t\tinternal Binary Version;");
                code.AppendLine("\t\t#pragma warning restore");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Notifies clients that a property value is changing.");
                code.AppendLine("\t\tpublic event PropertyChangingEventHandler PropertyChanging;");
                code.AppendLine();
                GenerateXmlDoc(code, 2, "Notifies clients that a property value has changed.");
                code.AppendLine("\t\tpublic event PropertyChangedEventHandler PropertyChanged;");

                code.AppendLine();

                code.AppendLine("\t\t#region Fields");
                code.AppendLine();

                foreach (var column in table.Columns)
                {
                    GenerateXmlDoc(code, 2, "Gets or sets the value of " + column.Value.Name);

                    code.Append("\t\t[Column(Name = \"" + column.Value.Name + "\"");
                    if (column.Value.IsPrimaryKey)
                        code.Append(", IsPrimaryKey = true");
                    if (column.Value.AutoIncrement.HasValue)
                        code.Append(", IsDbGenerated = true");
                    code.Append(", CanBeNull = " + column.Value.AllowsNull.ToString(CultureInfo.InvariantCulture).ToLower());
                    code.Append(")]");

                    code.AppendLine();
                    code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.FieldName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _" + column.Value.FieldName + "; }");
                    code.AppendLine("\t\t\tset");
                    code.AppendLine("\t\t\t{");
                    code.AppendLine("\t\t\t\tif (_" + column.Value.FieldName + " == value) return;");
                    code.AppendLine("\t\t\t\tif (PropertyChanging != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanging.Invoke(this, new PropertyChangingEventArgs(\"" + column.Value.FieldName + "\"));");
                    code.AppendLine("\t\t\t\t_" + column.Value.FieldName + " = value;");
                    code.AppendLine("\t\t\t\tif (PropertyChanged != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanged.Invoke(this, new PropertyChangedEventArgs(\"" + column.Value.FieldName + "\"));");
                    code.AppendLine("\t\t\t}");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }

                code.AppendLine("\t\t#endregion");
                code.AppendLine();

                code.AppendLine("\t\t#region References");
                code.AppendLine();

                foreach (var foreignKeyConstraint in table.References)
                {
                    code.AppendLine("\t\t[Association(ThisKey = \"" + foreignKeyConstraint.Column.FieldName +
                                    "\", OtherKey = \"" + foreignKeyConstraint.ReferenceColumn.FieldName +
                                    "\", Storage = \"_" + foreignKeyConstraint.ReferenceTable.ClassName + "\")]");
                    code.AppendLine("\t\tpublic " + foreignKeyConstraint.ReferenceTable.ClassName + " " + foreignKeyConstraint.ReferenceTable.ClassName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _" + foreignKeyConstraint.ReferenceTable.ClassName + ".Entity; }");
                    code.AppendLine("\t\t\tset");
                    code.AppendLine("\t\t\t{");
                    code.AppendLine("\t\t\t\tif (_" + foreignKeyConstraint.ReferenceTable.ClassName + ".Entity == value) return;");
                    code.AppendLine("\t\t\t\tif (PropertyChanging != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanging.Invoke(this, new PropertyChangingEventArgs(\"" + foreignKeyConstraint.ReferenceTable.ClassName + "\"));");
                    code.AppendLine("\t\t\t\tif (PropertyChanging != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanging.Invoke(this, new PropertyChangingEventArgs(\"" + foreignKeyConstraint.ReferenceColumn.FieldName + "\"));");
                    code.AppendLine();
                    code.AppendLine("\t\t\t\t_" + foreignKeyConstraint.ReferenceTable.ClassName + ".Entity = value;");
                    code.AppendLine("\t\t\t\t_" + foreignKeyConstraint.Column.FieldName + " = value." + foreignKeyConstraint.ReferenceTable.Columns.First(c => c.Value.IsPrimaryKey).Value.FieldName + ";");
                    code.AppendLine();
                    code.AppendLine("\t\t\t\tif (PropertyChanged != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanged.Invoke(this, new PropertyChangedEventArgs(\"" + foreignKeyConstraint.ReferenceTable.ClassName + "\"));");
                    code.AppendLine("\t\t\t\tif (PropertyChanged != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanged.Invoke(this, new PropertyChangedEventArgs(\"" + foreignKeyConstraint.ReferenceColumn.FieldName + "\"));");
                    code.AppendLine("\t\t\t}");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }

                foreach (var foreignKeyConstraint in table.ReferencedBy)
                {
                    code.AppendLine("\t\t[Association(ThisKey = \"" + table.PrimaryKeyColumnName +
                                    "\", OtherKey = \"" + foreignKeyConstraint.ReferenceColumn.Name +
                                    "\", Storage = \"_Associated" + foreignKeyConstraint.ReferenceTable.ClassName + "\")]");
                    code.AppendLine("\t\tpublic EntitySet<" + foreignKeyConstraint.ReferenceTable.ClassName + "> Associated" + foreignKeyConstraint.ReferenceTable.ClassName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _Associated" + foreignKeyConstraint.ReferenceTable.ClassName + "; }");
                    code.AppendLine("\t\t\tset { _Associated" + foreignKeyConstraint.ReferenceTable.ClassName + ".Assign(value); }");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }

                code.AppendLine("\t\t#endregion");
                code.AppendLine();

                code.AppendLine("\t}");
                code.AppendLine("}");
                code.AppendLine();

                AppendCode(table.DisplayName, code);
            }
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            GenerateDataContext();

            //var repositoryPatternGenerator = new RepositoryPatternGenerator(Database, false, true);
            //repositoryPatternGenerator.GenerateIRepository();
            //repositoryPatternGenerator.GenerateIDataRepository();
            //repositoryPatternGenerator.GenerateDataRepository();

            //foreach (var table in Database.Tables)
            //{
            //    repositoryPatternGenerator.GenerateITableRepository(table);
            //    repositoryPatternGenerator.GenerateTableRepository<CSharpMangoLinqToSqlDataAccessLayerGenerator>(table);
            //}

            //foreach (var codeFile in repositoryPatternGenerator.CodeFiles)
            //    AppendCode(codeFile.Key, codeFile.Value);
        }

        private void GenerateDataContext()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");
            code.AppendLine("\tusing System.Data.Linq;");
            code.AppendLine();

            var connStr = new SqlCeConnectionStringBuilder(Database.ConnectionString);
            var dataSource = new FileInfo(connStr.DataSource);
            var className = dataSource.Name.Trim(' ').Replace(dataSource.Extension, string.Empty);

            GenerateXmlDoc(code, 1, "Represents the entity data context");
            code.AppendLine("\tpublic partial class EntityDataContext : DataContext");
            code.AppendLine("\t{");

            GenerateXmlDoc(code, 2, "Global Connection String");
            code.AppendLine("\t\tpublic static string ConnectionString = \"Data Source=isostore:/" + dataSource.Name + "\";");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of the " + className + " data context");
            code.AppendLine("\t\tpublic EntityDataContext() : this(ConnectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of the entity data context",
                           new KeyValuePair<string, string>("connectionString", "connection string to be used for this instance"));
            code.AppendLine("\t\tpublic EntityDataContext(string connectionString) : base(connectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (!DatabaseExists())");
            code.AppendLine("\t\t\t\tCreateDatabase();");
            code.AppendLine("\t\t}");

            //GenerateXmlDoc(code, 2, "Factory method that creates an instance of the entity data context");
            //code.AppendLine("\t\tpublic static EntityDataContext Create()");
            //code.AppendLine("\t\t{");
            //code.AppendLine("\t\t\treturn Create(ConnectionString);");
            //code.AppendLine("\t\t}");

            //GenerateXmlDoc(code, 2, "Factory method that creates an instance of the entity data context",
            //               new KeyValuePair<string, string>("connectionString", "connection string to be used for this instance"));
            //code.AppendLine("\t\tpublic static EntityDataContext Create(string connectionString)");
            //code.AppendLine("\t\t{");
            //code.AppendLine("\t\t\treturn new EntityDataContext(connectionString);");
            //code.AppendLine("\t\t}");

            foreach (var table in Database.Tables)
            {
                code.AppendLine();
                GenerateXmlDoc(code, 2, "Represents the " + table.DisplayName + " table");
                code.AppendLine("\t\tpublic Table<" + table.ClassName + "> " + table.ClassName + ";");
            }

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode(className + "DataContext", code);
        }
    }
}
