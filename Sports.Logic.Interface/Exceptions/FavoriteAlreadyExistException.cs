using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FavoriteAlreadyExistException : Exception
    {
        public FavoriteAlreadyExistException(string message) : base(message)
        {
        }

    }
}
