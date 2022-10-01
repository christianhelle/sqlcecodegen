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
namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class DataAccessLayerGeneratorOptions
    {
        public DataAccessLayerGeneratorOptions()
        {
            ThrowExceptions = false;
            DebuggerOutput = false;
            PerformanceMeasurementOutput = false;
            GenerateXmlDocumentation = false;

            GenerateSelectAll = true;
            GenerateSelectAllWithTop = true;
            GenerateSelectBy = true;
            GenerateSelectByWithTop = true;
            GenerateSelectByTwoColumns = false;
            GenerateSelectByThreeColumns = false;
            GenerateCreate = true;
            GenerateCreateIgnoringPrimaryKey = true;
            GenerateCreateUsingAllColumns = true;
            GeneratePopulate = true;
            GenerateDelete = true;
            GenerateDeleteBy = true;
            GenerateDeleteAll = true;
            GenerateSaveChanges = true;
            GenerateUpdate = true;
            GenerateCount = true;
        }

        public bool ThrowExceptions { get; set; }
        public bool DebuggerOutput { get; set; }
        public bool PerformanceMeasurementOutput { get; set; }
        public bool GenerateXmlDocumentation { get; set; }

        public bool GenerateSelectAll { get; set; }
        public bool GenerateSelectAllWithTop { get; set; }
        public bool GenerateSelectBy { get; set; }
        public bool GenerateSelectByWithTop { get; set; }
        public bool GenerateSelectByTwoColumns { get; set; }
        public bool GenerateSelectByThreeColumns { get; set; }
        public bool GenerateCreate { get; set; }
        public bool GenerateCreateIgnoringPrimaryKey { get; set; }
        public bool GenerateCreateUsingAllColumns { get; set; }
        public bool GeneratePopulate { get; set; }
        public bool GenerateDelete { get; set; }
        public bool GenerateDeleteBy { get; set; }
        public bool GenerateDeleteAll { get; set; }
        public bool GenerateSaveChanges { get; set; }

        public bool GenerateUpdate { get; set; }
        public bool GenerateCount { get; set; }
    }
}