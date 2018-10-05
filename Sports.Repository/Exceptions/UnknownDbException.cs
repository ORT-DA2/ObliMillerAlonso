using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repsository.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class UnknownDbException : UnknownDataAccessException
    {
        public UnknownDbException(string message) : base(message)
        {
        }
    }
}
