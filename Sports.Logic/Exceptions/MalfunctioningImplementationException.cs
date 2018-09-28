using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class MalfunctioningImplementationException : Exception
    {
        public MalfunctioningImplementationException(string message) : base(message)
        {
        }

    }
}
