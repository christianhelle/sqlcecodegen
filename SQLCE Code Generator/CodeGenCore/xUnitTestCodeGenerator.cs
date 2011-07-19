using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class XUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public XUnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces(StringBuilder code)
        {
            code.AppendLine("\tusing Xunit;");
        }

        protected override string GetTestClassAttribute()
        {
            return string.Empty;
        }

        protected override string GetTestMethodAttribute()
        {
            return "[Fact]";
        }

        protected override string GetTestInitializeAttribute()
        {
            return string.Empty;
        }

        protected override string GetAssertAreEqualMethod()
        {
            return "Assert.Equal";
        }

        protected override string GetAssertAreNotEqualMethod()
        {
            return "Assert.NotEqual";
        }

        protected override string GetAssertIsNotNullMethod()
        {
            return "Assert.NotNull";
        }

        protected override string GetAssertIsTrueMethod()
        {
            return "Assert.True";
        }

        protected override string GetAssertIsNullMethod()
        {
            return "Assert.Null";
        }

        protected override void GenerateHelperClasses()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            IncludeUnitTestNamespaces(code);
            code.AppendLine();

            code.AppendLine(@"
    internal static class CollectionAssert
    {
        internal static void AllItemsAreNotNull<T>(System.Collections.Generic.IEnumerable<T> items)
        {
            foreach (var item in items) 
                Assert.NotNull(item);
        }
    }");
            code.AppendLine("}");

            AppendCode("CollectionAssert", code);
        }
    }
}
