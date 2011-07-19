using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class MSTestUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public MSTestUnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces(StringBuilder code)
        {
            code.AppendLine("\tusing Microsoft.VisualStudio.TestTools.UnitTesting;");
        }

        protected override string GetTestClassAttribute()
        {
            return "[TestClass]";
        }

        protected override string GetTestMethodAttribute()
        {
            return "[TestMethod]";
        }

        protected override string GetTestInitializeAttribute()
        {
            return "[TestInitialize]";
        }
    }
}