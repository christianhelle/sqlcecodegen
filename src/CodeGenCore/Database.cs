using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class Database
    {
        public Database(string connectionString)
            : this("SqlCeCodeGen", connectionString)
        {
        }

        public Database(string defaultNamespace, string connectionString)
        {
            Namespace = defaultNamespace;
            AnalyzeDatabase(connectionString);
        }

        public string Namespace { get; set; }
        public List<Table> Tables { get; set; }

        private void AnalyzeDatabase(string connectionString)
        {
            // Read schema information
            var engine = new SqlCeEngine(connectionString);
            var tables = engine.GetTables();
            if (tables == null) return;

            // Retrieve table information
            var tableList = GetTableInformation(connectionString, tables);

            // Analyze table information (columns and data type)
            AnalyzeTables(tableList, connectionString);
        }

        private void AnalyzeTables(ICollection<KeyValuePair<string, DataTable>> tableList, string connectionString)
        {
            Tables = new List<Table>(tableList.Count);
            foreach (var table in tableList)
            {
                var currentTable = new Table();
                currentTable.TableName = table.Key;

                var columns = new Dictionary<string, Column>(table.Value.Columns.Count);
                foreach (DataColumn column in table.Value.Columns)
                    columns.Add(column.ColumnName,
                        new Column
                        {
                            Name = column.ColumnName,
                            ManagedType = column.DataType,
                            MaxLength = column.MaxLength
                        });
                currentTable.Columns = columns;

                using (var conn = new SqlCeConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME=@Name AND PRIMARY_KEY=1";
                    cmd.Parameters.AddWithValue("@Name", table.Key);
                    var primaryKeyColumnName = cmd.ExecuteScalar();
                    if (primaryKeyColumnName != null)
                        currentTable.PrimaryKeyColumnName = primaryKeyColumnName.ToString();
                }

                Tables.Add(currentTable);
            }
        }

        private static Dictionary<string, DataTable> GetTableInformation(string connectionString, ICollection<string> tables)
        {
            var tableList = new Dictionary<string, DataTable>(tables.Count);
            foreach (var table in tables)
            {
                using (var connection = new SqlCeConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"SELECT * FROM " + table;
                    //using (var reader = command.ExecuteReader())
                    //{
                    //    var schema = new DataTable(table);
                    //    for (var i = 0; i < reader.FieldCount; i++)
                    //        schema.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    //    tableList.Add(table, schema);
                    //}
                    using (var adapter = new SqlCeDataAdapter(command))
                    {
                        var schema = new DataTable(table);
                        adapter.Fill(schema);
                        tableList.Add(table, schema);
                    }
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
        public int MaxLength { get; set; }
        public Type ManagedType { get; set; }
        
        public override string ToString()
        {
            return ManagedType.ToString();
        }
    }
}
