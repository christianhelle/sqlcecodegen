using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class VisualBasicCodeGenerator : VisualBasicCodeDomCodeGenerator
    {
        public VisualBasicCodeGenerator(ISqlCeDatabase database)
            : base(database)
        {
        }
    }
}