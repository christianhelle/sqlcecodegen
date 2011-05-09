using System;
namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CodeGeneratorFactory
    {
        private readonly SqlCeDatabase database;

        public CodeGeneratorFactory(SqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string language)
        {
            if (string.Compare(language, "C#", true) == 0 || 
                string.Compare(language, "CSharp", true) == 0 || 
                string.Compare(language, "CS", true) == 0)
                return new CSharpCodeGenerator(database);

            if (string.Compare(language, "VB.NET", true) == 0 ||
                string.Compare(language, "Visual Basic", true) == 0 ||
                string.Compare(language, "VisualBasic", true) == 0 ||
                string.Compare(language, "VB", true) == 0)
                return new VisualBasicCodeGenerator(database);
            
            return Create();
        }

        public static CodeGenerator Create(SqlCeDatabase database, string language)
        {
            if (string.Compare(language, "C#", true) == 0 ||
                string.Compare(language, "CSharp", true) == 0 ||
                string.Compare(language, "CS", true) == 0)
                return new CSharpCodeGenerator(database);

            if (string.Compare(language, "VB.NET", true) == 0 ||
                string.Compare(language, "Visual Basic", true) == 0 ||
                string.Compare(language, "VisualBasic", true) == 0 ||
                string.Compare(language, "VB", true) == 0)
                return new VisualBasicCodeGenerator(database);

            return Create(database);
        }

        public CodeGenerator Create()
        {
            return new CSharpCodeGenerator(database);
        }

        public static CodeGenerator Create(SqlCeDatabase database)
        {
            return new CSharpCodeGenerator(database);
        }
    }
}