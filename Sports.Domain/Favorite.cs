using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain.Exceptions;
using Sports.Domain.Constants;


namespace Sports.Domain
{
    public class Favorite
    {
        public int Id { get; private set; }
        public User User { get; set; }
        public Team Team { get; set; }

        public void Validate()
        {
            CheckUserNotNull();
            CheckTeamNotNull();
        }

        private void CheckUserNotNull()
        {
            if (User == null)
            {
                throw new InvalidEmptyUserException(EmptyUser.EMPTY_USER);
            }
        }

        private void CheckTeamNotNull()
        {
            if (Team == null)
            {
                throw new InvalidTeamIsEmptyException(EmptyTeam.EMPTY_LOCAL_TEAM_MESSAGE);
            }
        }
    }
}
