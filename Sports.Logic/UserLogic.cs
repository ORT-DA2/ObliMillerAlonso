using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;

namespace Sports.Logic
{
    public class UserLogic
    {
        IUserRepository _repository;

        public UserLogic(IRepositoryWrapper wrapper)
        {
            _repository = wrapper.User;
        }
        public void AddUser(User user)
        {
            ValidateUser(user);
            _repository.Create(user);
        }

        private static void ValidateUser(User user)
        {
            CheckNotNull(user);
            user.IsValid();
        }

        private static void CheckNotNull(User user)
        {
            if (user == null)
            {
                throw new InvalidUserDataException("Cannot add null user");
            }
        }

        public User GetUserById(int id)
        { 
            ICollection<User> users = _repository.FindByCondition(u => u.Id==id);
            return users.First();

        }

        public void UpdateUser(int id, User user)
        {
            User originalUser = GetUserById(id);
            user.Id = originalUser.Id;
            _repository.Update(user);
        }
    }
}
