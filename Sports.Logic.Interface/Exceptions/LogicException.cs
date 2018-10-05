using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Interface.Exceptions
{
    [Serializable]
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message) { }
    }
        
}
