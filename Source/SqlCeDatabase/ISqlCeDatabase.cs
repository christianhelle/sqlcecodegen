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
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    public interface ISqlCeDatabase
    {
        string ConnectionString { get; set; }
        string DefaultNamespace { get; set; }
        List<Table> Tables { get; set; }
        string DatabaseFilename { get; set; }
        bool VerifyConnectionStringPassword();
        void AnalyzeDatabase();
        void Verify();
        void Shrink();
        void Compact();
        void Upgrade();
        void Rename(Table table, string newName);
        void Rename(Column column, string newName);
        object GetTableData(Table table);
        object GetTableData(string tableName, string columnName);
        object GetTableProperties(Table table);
        void SaveTableDataChanges(DataTable tableData);
        object ExecuteQuery(string query, StringBuilder errors, StringBuilder messages);
        void CreateDatabase(string filename, string password);
    }
}
