#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    public static class SqlCeEngineExtensions
    {
        public static bool DoesTableExist(this SqlCeEngine source, string tablename)
        {
            using (var conn = new SqlCeConnection(source.LocalConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@Name";
                    cmd.Parameters.AddWithValue("@Name", tablename);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
        }

        public static string[] GetTables(this SqlCeEngine source)
        {
            using (var conn = new SqlCeConnection(source.LocalConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
                    return PopulateStringList(cmd);
                }
            }
        }

        public static string[] GetTableConstraints(this SqlCeEngine source)
        {
            using (var conn = new SqlCeConnection(source.LocalConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS";
                    return PopulateStringList(cmd);
                }
            }
        }

        public static string[] GetTableConstraints(this SqlCeEngine source, string tablename)
        {
            using (var conn = new SqlCeConnection(source.LocalConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME=@Name";
                    cmd.Parameters.AddWithValue("@Name", tablename);
                    return PopulateStringList(cmd);
                }
            }
        }

        private static string[] PopulateStringList(SqlCeCommand cmd)
        {
            var list = new List<string>();
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    list.Add(reader.GetString(0));

            return list.ToArray();
        }

        public static DataSet GetSchemaInformationViews(this SqlCeEngine source)
        {
            var dataSet = new DataSet();
            using (var conn = new SqlCeConnection(source.LocalConnectionString))
            {
                var views = new[] { "TABLES", "TABLE_CONSTRAINTS", "INDEXES", "KEY_COLUMN_USAGE", "COLUMNS" };
                foreach (var view in views)
                {
                    var table = new DataTable("INFORMATION_SCHEMA." + view);
                    using (var adapter = new SqlCeDataAdapter("SELECT * FROM INFORMATION_SCHEMA." + view, conn))
                        adapter.Fill(table);

                    dataSet.Tables.Add(table);
                }
            }
            return dataSet;
        }
    }
}