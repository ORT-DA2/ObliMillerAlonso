using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Domain.Exceptions
{
    [Serializable]
    public class InvalidEmptyTextFieldException : Exception
    {

        public InvalidEmptyTextFieldException(string message) : base(message)
        {
        }
        
    }
}
