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
using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CodeGeneratorFactory
    {
        private readonly ISqlCeDatabase database;

        public CodeGeneratorFactory(ISqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string language, string target = null)
        {
            return Create(database, language, target);
        }

        public static CodeGenerator Create(ISqlCeDatabase database, string language, string target = null)
        {
            if (String.Compare(language, "C#", StringComparison.OrdinalIgnoreCase) == 0 ||
                String.Compare(language, "CSharp", StringComparison.OrdinalIgnoreCase) == 0 ||
                String.Compare(language, "CS", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(target) && target == "Mango")
                    return new CSharpMangoLinqToSqlCodeGenerator(database);
                if (!string.IsNullOrEmpty(target) && target == "LinqToSql")
                    return new CSharpMangoSqlMetalCodeGenerator(database);

                return new CSharpCodeGenerator(database);
            }

            if (String.Compare(language, "VB.NET", StringComparison.OrdinalIgnoreCase) == 0 ||
                String.Compare(language, "Visual Basic", StringComparison.OrdinalIgnoreCase) == 0 ||
                String.Compare(language, "VisualBasic", StringComparison.OrdinalIgnoreCase) == 0 ||
                String.Compare(language, "VB", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(target) && target == "Mango")
                    throw new NotSupportedException("Visual Basic Code Generation for Windows Phone \"Mango\" is not supported");

                return new VisualBasicCodeGenerator(database);
            }

            return Create(database);
        }

        public static CodeGenerator Create<T>(ISqlCeDatabase database) where T : CodeGenerator
        {
            return Activator.CreateInstance(typeof (T), database) as T;
        }

        public CodeGenerator Create()
        {
            return new CSharpCodeGenerator(database);
        }

        public static CodeGenerator Create(ISqlCeDatabase database)
        {
            return new CSharpCodeGenerator(database);
        }
    }
}