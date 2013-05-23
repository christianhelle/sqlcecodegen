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