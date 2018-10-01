using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class NonAdminException : Exception
    {
        public NonAdminException(string message) : base(message)
        {
        }

    }
}
