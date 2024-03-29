﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

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
            ConnectionString = connectionString;

            Verify();
            AnalyzeDatabase();
        }

        public bool VerifyConnectionStringPassword()
        {
            try
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                    conn.Open();
                return true;
            }
            catch (SqlCeInvalidDatabaseFormatException)
            {
                try
                {
                    Upgrade();
                    return VerifyConnectionStringPassword();
                }
                catch (SqlCeException e)
                {
                    return HandleInvalidPassword(e);
                }
            }
            catch (SqlCeException e)
            {
                return HandleInvalidPassword(e);
            }
        }

        private static bool HandleInvalidPassword(SqlCeException e)
        {
            if (e.NativeError == 25028 ||
                e.NativeError == 25140 ||
                e.Message.ToLower().Contains("password"))
                return false;
            throw e;
        }

        public void Verify()
        {
            try
            {
                using (var engine = new SqlCeEngine(ConnectionString))
                    engine.Verify();
                using (var connection = new SqlCeConnection(ConnectionString))
                    connection.Open();
            }
            catch (SqlCeInvalidDatabaseFormatException)
            {
                Upgrade();
            }
        }

        public void Upgrade()
        {
            var file = new FileInfo(new SqlConnectionStringBuilder(ConnectionString).DataSource);
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLCE Code Generator");
            if (!Directory.Exists(appData))
                Directory.CreateDirectory(appData);
            var newFile = file.CopyTo(Path.Combine(appData, file.Name), true);

            var newConnString = new SqlConnectionStringBuilder(ConnectionString) { DataSource = newFile.FullName };

            var firstIdx = newConnString.ToString().IndexOf("\"", 0, StringComparison.Ordinal);
            var lastIdx = newConnString.ToString().LastIndexOf("\"", StringComparison.Ordinal);
            var connStr = new StringBuilder(newConnString.ToString());
            connStr[firstIdx] = '\'';
            connStr[lastIdx] = '\'';

            ConnectionString = connStr.ToString();
            using (var engine = new SqlCeEngine(ConnectionString))
                engine.Upgrade();

            using (var connection = new SqlCeConnection(ConnectionString))
                connection.Open();
        }

        public object GetTableData(Table table)
        {
            using (var conn = new SqlCeConnection(ConnectionString))
            using (var adapter = new SqlCeDataAdapter("SELECT * FROM " + table.Name, conn))
            {
                var dataTable = new DataTable(table.DisplayName);
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public object GetTableData(string tableName, string columnName)
        {
            using (var conn = new SqlCeConnection(ConnectionString))
            using (var adapter = new SqlCeDataAdapter(string.Format("SELECT {0} FROM {1}", columnName, tableName), conn))
            {
                var dataTable = new DataTable(tableName);
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public string Namespace { get; set; }
        public List<Table> Tables { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseFilename { get; set; }

        public void AnalyzeDatabase()
        {
            var engine = new SqlCeEngine(ConnectionString);
            var tables = engine.GetTables();
            if (tables == null) return;

            var tableList = GetTableInformation(ConnectionString, tables);
            Tables = new List<Table>(tableList.Values);
            FetchPrimaryKeys();
            FetchIndexes();
            FetchReferences();
            FetchReferencedBy();
        }

        private void FetchReferences()
        {
            foreach (var table in Tables)
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT CONSTRAINT_NAME, CONSTRAINT_TABLE_NAME, UNIQUE_CONSTRAINT_TABLE_NAME, UNIQUE_CONSTRAINT_NAME
                                        FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
                                        WHERE CONSTRAINT_TABLE_NAME='" + table.DisplayName + "'";

                    var dataTable = new DataTable();
                    using (var adapter = new SqlCeDataAdapter(cmd))
                        adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                        continue;

                    table.References = new List<ForeignKeyConstraint>(dataTable.Rows.Count);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var foreignKeyConstraint = new ForeignKeyConstraint
                        {
                            Name = row.Field<string>("CONSTRAINT_NAME"),
                            ReferenceTable = Tables.FirstOrDefault(c => c.DisplayName == row.Field<string>("UNIQUE_CONSTRAINT_TABLE_NAME")),
                        };

                        cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME='" + foreignKeyConstraint.Name + "'";
                        foreignKeyConstraint.Column = table.Columns.Values.FirstOrDefault(c => c.DisplayName == cmd.ExecuteScalar() as string);
                        foreignKeyConstraint.ReferenceColumn = foreignKeyConstraint.ReferenceTable.Columns.Values.FirstOrDefault(c => c.IsPrimaryKey);

                        table.References.Add(foreignKeyConstraint);
                    }
                }
            }
        }

        private void FetchReferencedBy()
        {
            foreach (var table in Tables)
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT CONSTRAINT_NAME, CONSTRAINT_TABLE_NAME, UNIQUE_CONSTRAINT_TABLE_NAME, UNIQUE_CONSTRAINT_NAME
                                        FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
                                        WHERE UNIQUE_CONSTRAINT_TABLE_NAME='" + table.DisplayName + "'";

                    var dataTable = new DataTable();
                    using (var adapter = new SqlCeDataAdapter(cmd))
                        adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                        continue;

                    table.References = new List<ForeignKeyConstraint>(dataTable.Rows.Count);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var foreignKeyConstraint = new ForeignKeyConstraint
                        {
                            Name = row.Field<string>("CONSTRAINT_NAME"),
                            ReferenceTable = Tables.FirstOrDefault(c => c.DisplayName == row.Field<string>("CONSTRAINT_TABLE_NAME")),
                        };

                        cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME='" + foreignKeyConstraint.Name + "'";
                        foreignKeyConstraint.Column = table.Columns.Values.FirstOrDefault(c => c.DisplayName == cmd.ExecuteScalar() as string);
                        foreignKeyConstraint.ReferenceColumn = foreignKeyConstraint.ReferenceTable.Columns.Values.FirstOrDefault(c => c.IsPrimaryKey);

                        table.ReferencedBy.Add(foreignKeyConstraint);
                    }
                }
            }
        }

        private void FetchIndexes()
        {
            foreach (var table in Tables)
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT COLUMN_NAME, INDEX_NAME, [UNIQUE], [CLUSTERED] FROM INFORMATION_SCHEMA.INDEXES WHERE PRIMARY_KEY = 0 AND TABLE_NAME='" + table.DisplayName + "' ORDER BY TABLE_NAME, COLUMN_NAME, INDEX_NAME";

                    var dataTable = new DataTable();
                    using (var adapter = new SqlCeDataAdapter(cmd))
                        adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                        continue;

                    table.Indexes = new List<Index>(dataTable.Rows.Count);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //var indexName = row.Field<string>("INDEX_NAME");
                        //if (indexName.Contains(" "))
                        //    indexName = string.Format("[{0}]", indexName);

                        var index = new Index
                        {
                            Name = row.Field<string>("INDEX_NAME"),
                            Unique = row.Field<bool>("UNIQUE"),
                            Clustered = row.Field<bool>("CLUSTERED"),
                            Column = table.Columns.Values.FirstOrDefault(c => c.DisplayName == row.Field<string>("COLUMN_NAME"))
                        };
                        table.Indexes.Add(index);
                    }
                }
            }
        }

        private void FetchPrimaryKeys()
        {
            foreach (var table in Tables)
            {
                using (var conn = new SqlCeConnection(ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME=@Name AND PRIMARY_KEY=1";
                    cmd.Parameters.AddWithValue("@Name", table.DisplayName);
                    var primaryKeyColumnName = cmd.ExecuteScalar() as string;
                    if (primaryKeyColumnName != null)
                    {
                        table.PrimaryKeyColumnName = primaryKeyColumnName;
                        if (primaryKeyColumnName.Contains(" "))
                        {
                            table.PrimaryKeyColumnName = string.Format("[{0}]", primaryKeyColumnName);
                            table.PrimaryKeyColumnFieldName = primaryKeyColumnName.Replace(" ", string.Empty);
                            table.PrimaryKeyColumnDisplayName = primaryKeyColumnName;
                        }
                        if (table.Columns.ContainsKey(table.PrimaryKeyColumnName))
                            table.Columns[table.PrimaryKeyColumnName].IsPrimaryKey = true;
                    }

                    var constraints = new List<string>();
                    cmd.CommandText = @"SELECT CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME=@Name AND CONSTRAINT_TYPE='FOREIGN KEY'";
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            constraints.Add(reader[0].ToString());

                    foreach (var constraint in constraints)
                    {
                        cmd.CommandText = @"SELECT COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME=@Name";
                        cmd.Parameters["@Name"].Value = constraint;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var key = reader.GetString(0);
                                if (table.Columns.ContainsKey(key))
                                    table.Columns[key].IsForeignKey = true;
                            }
                        }

                    }
                }
            }
        }

        private static Dictionary<string, Table> GetTableInformation(string connectionString, ICollection<string> tables)
        {
            var tableList = new Dictionary<string, Table>(tables.Count);
            foreach (var tableName in tables)
            {
                string table = tableName;
                Trace.WriteLine("Analyazing " + table);

                table = string.Format("[{0}]", table);
                var schema = new DataTable(table);

                using (var connection = new SqlCeConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tableName + "'";
                    using (var adapter = new SqlCeDataAdapter(command))
                        adapter.Fill(schema);

                    var columnDescriptions = new DataTable();
                    command.CommandText = @"SELECT * FROM " + table;
                    using (var adapter = new SqlCeDataAdapter(command))
                        adapter.Fill(columnDescriptions);

                    var item = new Table
                    {
                        Name = table,
                        DisplayName = tableName,
                        ClassName = tableName.Replace(" ", string.Empty),
                        Columns = new Dictionary<string, Column>(schema.Rows.Count)
                    };

                    foreach (DataRow row in schema.Rows)
                    {
                        var displayName = row.Field<string>("COLUMN_NAME");
                        var name = displayName;
                        if (name.Contains(" "))
                            name = string.Format("[{0}]", name);
                        var column = new Column
                        {
                            Name = name,
                            DisplayName = displayName,
                            FieldName = displayName.Replace(" ", string.Empty),
                            DatabaseType = row.Field<string>("DATA_TYPE"),
                            MaxLength = row.Field<int?>("CHARACTER_MAXIMUM_LENGTH"),
                            ManagedType = columnDescriptions.Columns[displayName].DataType,
                            AllowsNull = (String.Compare(row.Field<string>("IS_NULLABLE"), "YES", StringComparison.OrdinalIgnoreCase) == 0),
                            AutoIncrement = row.Field<long?>("AUTOINC_INCREMENT"),
                            AutoIncrementSeed = row.Field<long?>("AUTOINC_SEED"),
                            Ordinal = row.Field<int>("ORDINAL_POSITION")
                        };
                        item.Columns.Add(name, column);
                    }

                    tableList.Add(table, item);
                }
            }
            return tableList;
        }

        public string GenerateCreateScript()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
