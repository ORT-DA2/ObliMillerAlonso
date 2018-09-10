using System;
using System.Collections.Generic;
using System.Text;
using Sports.Exceptions;

namespace Sports.Domain
{
    public class Sport
    {
        public string Name { get; set; }

        public void IsValid()
        {
            IsValidSportName();
        }

        private void IsValidSportName()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidSportDataException("Invalid Name");
        }
    }
}
