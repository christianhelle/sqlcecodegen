using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class SqlCeDatabase
    {
        public SqlCeDatabase(string connectionString)
            : this("SqlCeCodeGen", connectionString)
        {
        }

        public SqlCeDatabase(string defaultNamespace, string connectionString)
        {
            Namespace = defaultNamespace;
            AnalyzeDatabase(connectionString);
        }

        public string Namespace { get; set; }
        public List<Table> Tables { get; set; }

        private void AnalyzeDatabase(string connectionString)
        {
            var engine = new SqlCeEngine(connectionString);
            var tables = engine.GetTables();
            if (tables == null) return;

            var tableList = GetTableInformation(connectionString, tables);
            AnalyzeTables(tableList, connectionString);
        }

        private void AnalyzeTables(Dictionary<string, Table> tableList, string connectionString)
        {
            Tables = new List<Table>(tableList.Values);
            foreach (var table in Tables)
            {                
                using (var conn = new SqlCeConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME=@Name AND PRIMARY_KEY=1";
                    cmd.Parameters.AddWithValue("@Name", table.TableName);
                    var primaryKeyColumnName = cmd.ExecuteScalar();
                    if (primaryKeyColumnName != null)
                        table.PrimaryKeyColumnName = primaryKeyColumnName.ToString();
                }
            }
        }

        private static Dictionary<string, Table> GetTableInformation(string connectionString, ICollection<string> tables)
        {
            var tableList = new Dictionary<string, Table>(tables.Count);
            foreach (var table in tables)
            {
                var schema = new DataTable(table);

                using (var connection = new SqlCeConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + table + "'";
                    using (var adapter = new SqlCeDataAdapter(command))
                        adapter.Fill(schema);

                    var columnDescriptions = new DataTable();
                    command.CommandText = @"SELECT * FROM " + table;
                    using (var adapter = new SqlCeDataAdapter(command))
                        adapter.Fill(columnDescriptions);

                    var item = new Table
                    {
                        TableName = table,
                        Columns = new Dictionary<string, Column>(schema.Rows.Count)
                    };

                    foreach (DataRow row in schema.Rows)
                    {
                        var name = row.Field<string>("COLUMN_NAME");
                        item.Columns.Add(name, new Column
                        {
                            Name = name,
                            DatabaseType = row.Field<string>("DATA_TYPE"),
                            MaxLength = row.Field<int?>("CHARACTER_MAXIMUM_LENGTH"),
                            ManagedType = columnDescriptions.Columns[name].DataType
                        });
                    }

                    tableList.Add(table, item);
                }
            }
            return tableList;
        }
    }

    public class Table
    {
        public string TableName { get; set; }
        public Dictionary<string, Column> Columns { get; set; }
        public string PrimaryKeyColumnName { get; set; }

        public override string ToString()
        {
            return TableName;
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public int? MaxLength { get; set; }
        public Type ManagedType { get; set; }
        public string DatabaseType { get; set; }

        public override string ToString()
        {
            return ManagedType.ToString();
        }
    }
}
