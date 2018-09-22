using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class TeamDoesNotExistInSportException : Exception
    {
        public TeamDoesNotExistInSportException(string message) : base(message)
        {
        }

    }
}
