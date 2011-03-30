using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest.Properties;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    public class SqlCeDatabaseTest
    {
        private readonly string connectionString = Settings.Default.TestDatabaseConnectionString;

        [TestMethod]
        public void TablesNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            Assert.IsNotNull(target.Tables);
            CollectionAssert.AllItemsAreNotNull(target.Tables);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            Assert.AreEqual("SqlCeCodeGen", target.Namespace);
        }

        [TestMethod]
        public void DatabaseConstructorWithConnectionStringAndNamespaceTest()
        {
            string defaultNamespace = "SqlCeCodeGenTest";
            SqlCeDatabase target = new SqlCeDatabase(defaultNamespace, connectionString);
            Assert.AreEqual(defaultNamespace, target.Namespace);
        }

        [TestMethod]
        public void TableNameNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);         
            foreach (var table in target.Tables)
                Assert.IsFalse(string.IsNullOrEmpty(table.Name));
        }

        [TestMethod]
        public void ColumnsNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            foreach (var table in target.Tables)
            {
                Assert.IsNotNull(table.Columns);
                CollectionAssert.AllItemsAreNotNull(table.Columns);
            }
        }

        [TestMethod]
        public void ColumnOrdinalTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.AreNotEqual(0, column.Value.Ordinal);
        }

        [TestMethod]
        public void ColumnNameNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.Name));
        }

        [TestMethod]
        public void ColumnManagedTypeNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString); 
            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsNotNull(column.Value.ManagedType);
        }

        [TestMethod]
        public void ColumnDatabaseTypeNotNullTest()
        {
            SqlCeDatabase target = new SqlCeDatabase(connectionString);
            foreach (var table in target.Tables)
                foreach (var column in table.Columns)
                    Assert.IsFalse(string.IsNullOrEmpty(column.Value.DatabaseType));
        }
    }
}
