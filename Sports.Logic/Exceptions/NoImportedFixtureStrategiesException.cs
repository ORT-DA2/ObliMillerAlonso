using System;
using System.Runtime.Serialization;
using Sports.Logic.Interface.Exceptions;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class NoImportedFixtureStrategiesException : LogicException
    {
        public NoImportedFixtureStrategiesException(string message) : base(message)
        {
        }

    }
}
