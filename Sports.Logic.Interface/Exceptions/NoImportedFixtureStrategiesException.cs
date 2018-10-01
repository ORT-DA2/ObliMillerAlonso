using System;
using System.Runtime.Serialization;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class NoImportedFixtureStrategiesException : Exception
    {
        public NoImportedFixtureStrategiesException(string message) : base(message)
        {
        }

    }
}
