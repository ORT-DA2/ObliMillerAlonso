using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Exceptions;
using Sports.Repository.Interface;
using Sports.Logic.Interface;


namespace Sports.Logic
{
    public class UserLogic : IUserLogic
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

        private void ValidateUser(User user)
        {
            CheckNotNull(user);
            user.IsValid();
            CheckNotExists(user.UserName, user.Id);
        }

        private void CheckNotExists(string username, int id = 0)
        {
            if (_repository.FindByCondition(u => u.UserName == username && u.Id != id).Count!=0)
            {
                throw new InvalidUserDataException("Cannot repeat username");
            }
        }
        
        private void CheckNotNull(User user)
        {
            if (user == null)
            {
                throw new InvalidUserDataException("Cannot add null user");
            }
        }

        public User GetUserById(int id)
        { 
            ICollection<User> users = _repository.FindByCondition(u => u.Id==id);
            if(users.Count == 0)
            {
                throw new InvalidUserDataException("Id does not match any existing users");
            }
            return users.First();
        }

        public void UpdateUser(int id, User updatedUser)
        {
            User originalUser = GetUserById(id);
            originalUser.UpdateData(updatedUser);
            ValidateUser(originalUser);
            _repository.Update(originalUser);
        }

        public User GetUserByUserName(string userName)
        {
            ICollection<User> users = _repository.FindByCondition(u => u.UserName == userName);
            return users.First();
        }

        public void RemoveUser(int id)
        {
            User user = GetUserById(id);
            _repository.Delete(user);
        }

        public ICollection<User> GetAll()
        {
            return _repository.FindAll();
        }
    }
}
