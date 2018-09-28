using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Logic.Exceptions
{
    [Serializable]
    public class NoFixturesImportedException : Exception
    {
        public NoFixturesImportedException(string message) : base(message)
        {
        }

    }
}
