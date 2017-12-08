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
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("Northwind.sdf")]
    [DeploymentItem("SqlCe40\\SqlCeDatabase.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\SqlCeDatabase40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\sqlceca40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\sqlcecompact40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\sqlceme40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\sqlceqp40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\sqlcese40.dll", "SqlCe40")]
    [DeploymentItem("SqlCe40\\System.Data.SqlServerCe.dll", "SqlCe40")]
    public class SqlCeDatabaseTest
    {
        private readonly string connectionString = "Data Source=Northwind.sdf";

        [TestMethod]
        public void AnalyzeDatabaseTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();
        }

        [TestMethod]
        public void TablesNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            Assert.IsNotNull(target.Tables);
            CollectionAssert.AllItemsAreNotNull(target.Tables);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            Assert.AreEqual("SqlCeCodeGen", target.DefaultNamespace);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringAndNamespaceTest()
        {
            string defaultNamespace = "SqlCeCodeGenTest";
            var target = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            Assert.AreEqual(defaultNamespace, target.DefaultNamespace);
        }

        [TestMethod]
        public void TableNameNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                Assert.IsFalse(string.IsNullOrEmpty(table.Name));
        }

        [TestMethod]
        public void ColumnsNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
            {
                Assert.IsNotNull(table.Columns);
                CollectionAssert.AllItemsAreNotNull(table.Columns);
            }
        }

        [TestMethod]
        public void ColumnOrdinalTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.AreNotEqual(0, column.Value.Ordinal);
        }

        [TestMethod]
        public void ColumnNameNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.Name));
        }

        [TestMethod]
        public void ColumnManagedTypeNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsNotNull(column.Value.ManagedType);
        }

        [TestMethod]
        public void ColumnDatabaseTypeNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.DatabaseType));
        }

        [TestMethod]
        public void IndexesNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
            {
                Assert.IsNotNull(table.Indexes);
                CollectionAssert.AllItemsAreNotNull(table.Indexes);
            }                
        }

        [TestMethod]
        public void IndexColumnNotNullTest()
        {
            var target = SqlCeDatabaseFactory.Create(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var index in table.Indexes)
                    Assert.IsNotNull(index.Column);
        }
    }
}
