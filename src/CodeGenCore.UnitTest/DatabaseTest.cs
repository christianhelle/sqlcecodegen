using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest.Properties;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    public class DatabaseTest
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
    }
}
