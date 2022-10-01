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
    public class UnitTestCodeGeneratorFactory
    {
        private readonly ISqlCeDatabase database;

        public UnitTestCodeGeneratorFactory(ISqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string testfx, string target = null)
        {
            return Create(database, testfx, target);
        }

        public static CodeGenerator Create(ISqlCeDatabase database, string testfx, string target = null)
        {
            if (!string.IsNullOrEmpty(target) && target == "Mango")
                throw new NotSupportedException("Unit Test Framework not supported"); //return new CSharpMangoFakeDataAccessLayerCodeGenerator(database);

            switch (testfx.ToLower())
            {
                case "mstest":
                    return new MSTestUnitTestCodeGenerator(database);
                case "nunit":
                    return new NUnitTestCodeGenerator(database);
                case "xunit":
                    return new XUnitTestCodeGenerator(database);
            }

            throw new NotSupportedException("Unit Test Framework not supported");
        }

        public CodeGenerator Create()
        {
            return Create(database);
        }

        public static CodeGenerator Create(ISqlCeDatabase database)
        {
            return new MSTestUnitTestCodeGenerator(database);
        }
    }
}
