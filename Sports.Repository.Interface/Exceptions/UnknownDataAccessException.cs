using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Repository.Interface.Exceptions
{
    [Serializable]
    public class UnknownDataAccessException : Exception
    {
        public UnknownDataAccessException(string message) : base(message)
        {
        }
    }
}
