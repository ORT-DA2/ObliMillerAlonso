using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Persistence.Interface;
using Sports.Persistence.Context;
using System.Linq;

namespace Sports.Persistence
{
    public class UserPersistence : IUserPersistence
    {
        public void Add(User user)
        {
            /*using (ContextDB context = new ContextDB())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }*/
        }

        public User GetById(int id)
        {
            User user = null ;
            /*using (ContextDB context = new ContextDB())
            {
                user = context.Users.FirstOrDefault(u => u.Id.Equals(id));
            }*/
            return user;
        }
    }
}
