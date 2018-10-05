using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FixtureImportingException : LogicException
    {
        public FixtureImportingException(string message) : base(message)
        {
        }

    }
}
