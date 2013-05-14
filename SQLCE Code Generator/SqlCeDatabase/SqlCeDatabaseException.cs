using System;

namespace ChristianHelle.DatabaseTools.SqlCe
{
    [Serializable]
    public class SqlCeDatabaseException : Exception
    {
        public SqlCeDatabaseException(string message, Exception inner) : base(message, inner)
        {
        }

        public int NativeError { get; set; }    
    }
}
