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

        public CodeGenerator Create(string language, string target = null)
        {
            return Create(database, language, target);
        }

        public static CodeGenerator Create(SqlCeDatabase database, string language, string target = null)
        {
            if (string.Compare(language, "C#", true) == 0 ||
                string.Compare(language, "CSharp", true) == 0 ||
                string.Compare(language, "CS", true) == 0)
            {
                if (!string.IsNullOrEmpty(target) && target == "Mango")
                    return new CSharpMangoLinqToSqlCodeGenerator(database);

                return new CSharpCodeGenerator(database);
            }

            if (string.Compare(language, "VB.NET", true) == 0 ||
                string.Compare(language, "Visual Basic", true) == 0 ||
                string.Compare(language, "VisualBasic", true) == 0 ||
                string.Compare(language, "VB", true) == 0)
            {
                if (!string.IsNullOrEmpty(target) && target == "Mango")
                    throw new NotSupportedException("Visual Basic Code Generation for Windows Phone \"Mango\" is not supported");

                return new VisualBasicCodeGenerator(database);
            }

            return Create(database);
        }

        public static CodeGenerator Create<T>(SqlCeDatabase database) where T : CodeGenerator
        {
            return Activator.CreateInstance(typeof(T), database) as T;
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