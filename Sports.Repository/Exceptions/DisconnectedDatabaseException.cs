using System;
using System.Runtime.Serialization;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class DisconnectedDatabaseException : Exception
    {
        public DisconnectedDatabaseException(string message) : base(message) { }
    }
        
}
