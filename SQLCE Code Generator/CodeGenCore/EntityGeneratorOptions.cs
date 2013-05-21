namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class EntityGeneratorOptions
    {
        public EntityGeneratorOptions()
        {
            ThrowExceptions = true;
            AutoPropertiesOnly = false;
            DebuggerOutput = false;
        }

        public bool ThrowExceptions { get; set; }
        public bool AutoPropertiesOnly { get; set; }
        public bool DebuggerOutput { get; set; }
    }
}