using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FavoriteDoesNotExistException : Exception
    {
        public FavoriteDoesNotExistException(string message) : base(message)
        {
        }

    }
}
