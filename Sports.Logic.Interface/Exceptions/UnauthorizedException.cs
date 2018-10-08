using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Interface.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
        
}
