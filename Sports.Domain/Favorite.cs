using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Domain
{
    public class Favorite
    {
        //chequear que user y team no sean null
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
    }
}
