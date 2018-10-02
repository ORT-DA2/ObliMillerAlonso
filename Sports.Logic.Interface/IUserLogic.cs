using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IUserLogic
    {
        void AddUser(User user);
        User GetUserById(int id);
        void UpdateUser(int id, User updatedUser);
        User GetUserByUserName(string userName);
        void RemoveUser(int id);
        ICollection<User> GetAll();
        void SetSession(Guid token);
    }
}
