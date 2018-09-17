﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sports.Exceptions
{
    [Serializable]
    public class InvalidMatchDataException : Exception
    {
        public InvalidMatchDataException(string message) : base(message)
        {
        }
        
    }
}
