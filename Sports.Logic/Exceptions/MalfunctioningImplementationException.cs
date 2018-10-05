using System;
using System.Collections.Generic;
using System.Text;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class MalfunctioningImplementationException : LogicException
    {
        public MalfunctioningImplementationException(string message) : base(message)
        {
        }

    }
}
