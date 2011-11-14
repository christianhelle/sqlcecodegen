using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
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
                code.AppendLine();

                GenerateXmlDoc(code, 1, "Represents the " + table.DisplayName + " table in the database");
                code.AppendLine("\t[Table]");
                code.AppendLine("\tpublic partial class " + table.ClassName + " : INotifyPropertyChanged, INotifyPropertyChanging");
                code.AppendLine("\t{");

                foreach (var column in table.Columns)
                    code.AppendLine("\t\tprivate " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? _" : " _") + column.Value.FieldName + ";");
                code.AppendLine();

                code.AppendLine("\t\t[Column(IsVersion = true)]");
                code.AppendLine("\t\tprivate Binary version;");
                code.AppendLine();

                GenerateXmlDoc(code, 2, "Notifies clients that a property value is changing.");
                code.AppendLine("\t\tpublic event PropertyChangingEventHandler PropertyChanging;");

                GenerateXmlDoc(code, 2, "Notifies clients that a property value has changed.");
                code.AppendLine("\t\tpublic event PropertyChangedEventHandler PropertyChanged;");
                
                code.AppendLine();

                foreach (var column in table.Columns)
                {
                    GenerateXmlDoc(code, 2, "Gets or sets the value of " + column.Value.Name);
                    code.Append("\t\t[Column(Name = \"" + column.Value.DisplayName + "\"");
                    if (column.Value.IsPrimaryKey)
                        code.Append(", IsPrimaryKey = true");
                    if (column.Value.AutoIncrement.HasValue)
                        code.Append(", IsDbGenerated = true");
                    if (column.Value.AllowsNull)
                        code.Append(", CanBeNull = true");
                    if (code.ToString().EndsWith(", "))
                        code.Remove(code.Length - 2, 2);
                    code.Append(")]");
                    code.AppendLine();
                    code.AppendLine("\t\tpublic " + column.Value.ManagedType + (column.Value.ManagedType.IsValueType ? "? " : " ") + column.Value.FieldName);
                    code.AppendLine("\t\t{");
                    code.AppendLine("\t\t\tget { return _" + column.Value.FieldName + "; }");
                    code.AppendLine("\t\t\tset");
                    code.AppendLine("\t\t\t{");
                    code.AppendLine("\t\t\t\tif (_" + column.Value.FieldName + " != value)");
                    code.AppendLine("\t\t\t\t{");
                    code.AppendLine("\t\t\t\t\tif (PropertyChanging != null)");
                    code.AppendLine("\t\t\t\t\t\tPropertyChanging.Invoke(this, new PropertyChangingEventArgs(\"" + column.Value.FieldName + "\"));");
                    code.AppendLine("\t\t\t\t\t_" + column.Value.FieldName + " = value;");
                    code.AppendLine("\t\t\t\t\tif (PropertyChanged != null)");
                    code.AppendLine("\t\t\t\t\t\tPropertyChanged.Invoke(this, new PropertyChangedEventArgs(\"" + column.Value.FieldName + "\"));");
                    code.AppendLine("\t\t\t\t}");
                    code.AppendLine("\t\t\t}");
                    code.AppendLine("\t\t}");
                    code.AppendLine();
                }

                code.AppendLine("\t}");
                code.AppendLine("}");

                AppendCode(table.DisplayName, code);
            }
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");
            code.AppendLine("\tusing System.Data.Linq;");
            code.AppendLine();

            var connStr = new SqlCeConnectionStringBuilder(Database.ConnectionString);
            var dataSource = new FileInfo(connStr.DataSource);
            var className = dataSource.Name.Trim(' ').Replace(dataSource.Extension, string.Empty);

            GenerateXmlDoc(code, 1, "Represents the " + className + " data context");
            code.AppendLine("\tpublic partial class " + className + "DataContext : DataContext");
            code.AppendLine("\t{");

            GenerateXmlDoc(code, 2, "Global Connection String");
            code.AppendLine("\t\tpublic static string ConnectionString = \"Data Source=isostore:/" + dataSource.Name + "\";");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of the " + className + " data context");
            code.AppendLine("\t\tpublic " + className + "DataContext () : this(ConnectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t}");
            code.AppendLine();

            GenerateXmlDoc(code, 2, "Creates an instance of the " + className + " data context", new KeyValuePair<string, string>("connectionString", "connection string to be used for this instance"));
            code.AppendLine("\t\tpublic " + className + "DataContext (string connectionString) : base(connectionString)");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tif (!DatabaseExists())");
            code.AppendLine("\t\t\t\tCreateDatabase();");
            code.AppendLine("\t\t}");

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
