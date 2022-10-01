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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class CodeGenFileSerializerTest
    {
        [TestMethod]
        public void SaveFileTest()
        {
            var actual = new CodeGenFile
                             {
                                 DataSource = Guid.NewGuid().ToString(),
                                 GeneratedCode = new GeneratedCode
                                                     {
                                                         Entities = Guid.NewGuid().ToString(),
                                                         DataAccessCode = Guid.NewGuid().ToString(),
                                                         EntityUnitTests = Guid.NewGuid().ToString(),
                                                         DataAccessUnitTests = Guid.NewGuid().ToString()
                                                     }
                             };
            var target = new CodeGenFileSerializer();
            target.SaveFile(actual, "test.xml");
        }

        [TestMethod]
        public void LoadFileTest()
        {
            var target = new CodeGenFileSerializer();
            target.LoadFile("test.xml");
        }
    }
}
