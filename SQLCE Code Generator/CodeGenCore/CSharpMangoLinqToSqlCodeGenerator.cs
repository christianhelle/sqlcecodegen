
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
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
            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            foreach (var table in Database.Tables)
            {
                GenerateXmlDoc(1, "Represents the " + table.DisplayName + " table in the database");
                code.AppendLine("\t[System.Data.Linq.Mapping.Table]");
                code.AppendLine("\tpublic partial class " + table.ClassName + " : System.ComponentModel.INotifyPropertyChanged");
                code.AppendLine("\t{");

                foreach (var column in table.Columns)
                    code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.FieldName + ";");
                code.AppendLine();

                code.AppendLine("\t\tpublic event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
                code.AppendLine();

                foreach (var column in table.Columns)
                {
                    GenerateXmlDoc(2, "Gets or sets the value of " + column.Value.Name);
                    code.Append("\t\t[System.Data.Linq.Mapping.Column(");
                    if (column.Value.IsPrimaryKey)
                        code.Append("IsPrimaryKey = true, ");
                    if (column.Value.AutoIncrement.HasValue)
                        code.Append("IsDbGenerated = true, ");
                    if (column.Value.AllowsNull)
                        code.Append("CanBeNull = true, ");
                    if (code.ToString().EndsWith(", "))
                        code.Remove(code.Length - 2, 2);
                    code.Append(")]");
                    code.AppendLine();
                    code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.FieldName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _" + column.Value.FieldName + "; }");
                    code.AppendLine("\t\t\tset");
                    code.AppendLine("\t\t\t{");
                    code.AppendLine("\t\t\t\t_" + column.Value.FieldName + " = value;");
                    code.AppendLine("\t\t\t\tif (PropertyChanged != null)");
                    code.AppendLine("\t\t\t\t\tPropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(\"" + column.Value.FieldName + "\"));");
                    code.AppendLine("\t\t\t}");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }

                code.AppendLine("\t}");
                code.AppendLine();
            }

            code.AppendLine("}");
            code.AppendLine();
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            var connStr = new SqlCeConnectionStringBuilder(Database.ConnectionString);
            var dataSource = new FileInfo(connStr.DataSource);
            var className = dataSource.Name.Trim(' ').Replace(dataSource.Extension, string.Empty);

            GenerateXmlDoc(1, "Represents the " + className + " data context");
            code.AppendLine("\tpublic partial class " + className + "DataContext : System.Data.Linq.DataContext");
            code.AppendLine("\t{");

            GenerateXmlDoc(2, "Global Connection String");
            code.AppendLine("\t\tpublic static string ConnectionString = \"Data Source=isostore:/" + dataSource.Name + "\";");
            code.AppendLine();

            GenerateXmlDoc(2, "Creates an instance of the " + className + " data context");
            code.AppendLine("\t\tpublic " + className + "DataContext () : this(ConnectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(2, "Creates an instance of the " + className + " data context", new KeyValuePair<string, string>("connectionString", "connection string to be used for this instance"));
            code.AppendLine("\t\tpublic " + className + "DataContext (string connectionString) : base(connectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (!DatabaseExists())");
            code.AppendLine("\t\t\t\tCreateDatabase();");
            code.AppendLine("\t\t}");

            foreach (var table in Database.Tables)
            {
                code.AppendLine();
                GenerateXmlDoc(2, "Represents the " + table.DisplayName + " table");
                code.AppendLine("\t\tpublic System.Data.Linq.Table<" + table.ClassName + "> " + table.ClassName + ";");
            }

            code.AppendLine("\t}");
            code.AppendLine("}");
        }
    }
}
