namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class xUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public xUnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces()
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
            code.AppendLine(@"
    public static class CollectionAssert
    {
        public static void AllItemsAreNotNull<T>(System.Collections.Generic.IEnumerable<T> items)
        {
            foreach (var item in items) 
                Assert.NotNull(item);
        }
    }"
                );
        }
    }
}
