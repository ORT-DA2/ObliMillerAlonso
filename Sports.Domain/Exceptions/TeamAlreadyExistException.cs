using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class TeamAlreadyExistException : DomainException
    {
        public TeamAlreadyExistException(string message) : base(message)
        {
        }

    }
}
