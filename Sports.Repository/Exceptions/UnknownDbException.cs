using System;
using System.Collections.Generic;
using System.Text;
using Sports.Repository.Interface.Exceptions;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class UnknownDbException : UnknownDataAccessException
    {
        public UnknownDbException(string message) : base(message)
        {
        }
    }
}
