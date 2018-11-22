using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Favorite
    {
        public int Id { get;  set; }
        public User User { get; set; }
        public Competitor Competitor { get; set; }

        public void Validate()
        {
            CheckUserNotNull();
            CheckCompetitorNotNull();
        }

        private void CheckUserNotNull()
        {
            if (User == null)
            {
                throw new InvalidEmptyUserException(EmptyUser.EMPTY_USER);
            }
        }

        private void CheckCompetitorNotNull()
        {
            if (Competitor == null)
            {
                throw new InvalidCompetitorEmptyException(CompetitorValidation.COMPETITOR_EMPTY);
            }
        }
    }
}
