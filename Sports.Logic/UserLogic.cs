using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
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
            _repository.Create(user);
        }

        public User GetUserById(int id)
        {
            //ICollection<User> users = _repository.FindByCondition(u => u.Id==id);
            ICollection<User> users = _repository.FindAll();
            return users.First();

        }
    }
}
