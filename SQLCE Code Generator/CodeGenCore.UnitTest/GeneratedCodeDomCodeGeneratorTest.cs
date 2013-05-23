using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class GeneratedCSharpCodeDomCodeGeneratorTest : CodeGenBaseTest
    {
        private static CodeDomCodeGenerator GetCSharpCodeGenerator()
        {
            var database = GetDatabase();
            return new CSharpCodeDomCodeGenerator(database);
        }

        [TestMethod]
        public void CodeDomCodeGeneratorConstructorTest()
        {
            var generator = GetCSharpCodeGenerator();
            Assert.IsFalse(string.IsNullOrEmpty(generator.Database.Namespace));
        }

        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.WriteHeaderInformation();

            string actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void WriteGenerateEntitiesTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.GenerateEntities();

            var actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.GenerateEntities();

            var actual = generator.GetCode();
            AssertCSharpCompile(actual);
        }

        [TestMethod]
        public void WriteGenerateDataAccessLayerTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.GenerateDataAccessLayer();

            var actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void GenerateDataAccessLayerCodeCanCompileTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.GenerateEntities();
            generator.GenerateDataAccessLayer();

            var actual = generator.GetCode();
            AssertCSharpCompile(actual);
        }

        [TestMethod]
        public void DisposeTest()
        {
            var generator = GetCSharpCodeGenerator();
            generator.Dispose();
        }
    }

    [TestClass]
    public class GeneratedVisualBasicCodeDomCodeGeneratorTest : CodeGenBaseTest
    {
        private static CodeDomCodeGenerator GetVisualBasicCodeGenerator()
        {
            var database = GetDatabase();
            return new VisualBasicCodeDomCodeGenerator(database);
        }

        [TestMethod]
        public void CodeDomCodeGeneratorConstructorTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            Assert.IsFalse(string.IsNullOrEmpty(generator.Database.Namespace));
        }

        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.WriteHeaderInformation();

            var actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void WriteGenerateEntitiesTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.GenerateEntities();

            var actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.GenerateEntities();

            var actual = generator.GetCode();
            AssertVisualBasicCompile(actual);
        }

        [TestMethod]
        public void WriteGenerateDataAccessLayerTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.GenerateDataAccessLayer();

            var actual = generator.GetCode();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void GenerateDataAccessLayerCodeCanCompileTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.GenerateEntities();
            generator.GenerateDataAccessLayer();

            var actual = generator.GetCode();
            AssertVisualBasicCompile(actual);
        }

        [TestMethod]
        public void DisposeTest()
        {
            var generator = GetVisualBasicCodeGenerator();
            generator.Dispose();
        }
    }
}