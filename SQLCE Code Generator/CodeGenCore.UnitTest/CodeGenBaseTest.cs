using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    public class CodeGenBaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var fi = new FileInfo("TestDatabase.sdf");
            fi.Attributes = FileAttributes.Normal;
        }
    }
}