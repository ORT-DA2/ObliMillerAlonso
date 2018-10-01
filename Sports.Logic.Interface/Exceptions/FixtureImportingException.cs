using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class FixtureImportingException : Exception
    {
        public FixtureImportingException(string message) : base(message)
        {
        }

    }
}
