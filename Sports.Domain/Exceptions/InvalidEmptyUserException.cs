using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidEmptyUserException : DomainException
    {

        public InvalidEmptyUserException(string message) : base(message)
        {
        }
        
    }
}
