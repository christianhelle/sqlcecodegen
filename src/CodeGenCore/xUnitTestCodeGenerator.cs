#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class XUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public XUnitTestCodeGenerator(ISqlCeDatabase tableDetails)
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

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
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
