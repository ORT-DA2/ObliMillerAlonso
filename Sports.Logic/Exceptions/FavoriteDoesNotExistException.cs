using System;
using System.Collections.Generic;
using System.Text;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FavoriteDoesNotExistException : LogicException
    {
        public FavoriteDoesNotExistException(string message) : base(message)
        {
        }

    }
}
