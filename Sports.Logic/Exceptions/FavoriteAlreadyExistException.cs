using System;
using System.Collections.Generic;
using System.Text;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FavoriteAlreadyExistException : LogicException
    {
        public FavoriteAlreadyExistException(string message) : base(message)
        {
        }

    }
}
