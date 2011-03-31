using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class VisualBasicCodeGenerator : VisualBasicCodeDomCodeGenerator
    {
        public VisualBasicCodeGenerator(SqlCeDatabase database)
            : base(database)
        {
        }
    }
}