using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidSportIsEmptyException : Exception
    {
        public InvalidSportIsEmptyException(string message) : base(message)
        {
        }

    }
}
