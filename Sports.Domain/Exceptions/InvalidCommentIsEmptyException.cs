using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain.Exceptions
{

    [Serializable]
    public class InvalidCommentIsEmptyException : DomainException
    {
        public InvalidCommentIsEmptyException(string message) : base(message)
        {
        }

    }
}
