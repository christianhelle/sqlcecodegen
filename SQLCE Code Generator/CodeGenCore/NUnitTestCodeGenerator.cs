using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class NUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public NUnitTestCodeGenerator(ISqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces(StringBuilder code)
        {
            code.AppendLine("\tusing NUnit.Framework;");
        }

        protected override string GetTestClassAttribute()
        {
            return "[TestFixture]";
        }

        protected override string GetTestMethodAttribute()
        {
            return "[Test]";
        }

        protected override string GetTestInitializeAttribute()
        {
            return "[SetUp]";
        }
    }
}