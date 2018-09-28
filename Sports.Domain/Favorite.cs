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

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }
            else
            {
                return this.User.Equals(((Favorite)obj).User)&& this.Team.Equals(((Favorite)obj).Team);
            }
        }

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
