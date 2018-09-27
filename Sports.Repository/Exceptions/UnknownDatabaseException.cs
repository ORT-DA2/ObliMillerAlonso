using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Repository.Exceptions
{
    [Serializable]
    public class UnknownDatabaseException : Exception
    {
        public UnknownDatabaseException(string message) : base(message)
        {
        }
    }
}
