using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    public class SqlCeDatabaseTest
    {
        private readonly string connectionString = "Data Source=TestDatabase.sdf";

        [TestMethod]
        public void AnalyzeDatabaseTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();
        }

        [TestMethod]
        public void TablesNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            Assert.IsNotNull(target.Tables);
            CollectionAssert.AllItemsAreNotNull(target.Tables);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringTest()
        {
            var target = new SqlCeDatabase(connectionString);
            Assert.AreEqual("SqlCeCodeGen", target.Namespace);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringAndNamespaceTest()
        {
            string defaultNamespace = "SqlCeCodeGenTest";
            var target = new SqlCeDatabase(defaultNamespace, connectionString);
            Assert.AreEqual(defaultNamespace, target.Namespace);
        }

        [TestMethod]
        public void TableNameNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                Assert.IsFalse(string.IsNullOrEmpty(table.Name));
        }

        [TestMethod]
        public void ColumnsNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
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
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.AreNotEqual(0, column.Value.Ordinal);
        }

        [TestMethod]
        public void ColumnNameNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.Name));
        }

        [TestMethod]
        public void ColumnManagedTypeNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsNotNull(column.Value.ManagedType);
        }

        [TestMethod]
        public void ColumnDatabaseTypeNotNullTest()
        {
            var target = new SqlCeDatabase(connectionString);
            target.AnalyzeDatabase();

            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.DatabaseType));
        }
    }
}
