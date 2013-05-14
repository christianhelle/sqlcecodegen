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
