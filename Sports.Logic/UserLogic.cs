using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Logic.Constants;


namespace Sports.Logic
{
    public class UserLogic : IUserLogic
    {
        IUserRepository repository;
        ISessionLogic sessionLogic;
        User sessionUser;
        public UserLogic(IRepositoryUnitOfWork unitOfwork)
        {
            repository = unitOfwork.User;
            sessionLogic = new SessionLogic(unitOfwork);

        }
        public void AddUser(User user)
        {
            sessionLogic.ValidateUser(sessionUser);
            ValidateUser(user);
            repository.Create(user);
            repository.Save();
        }

        private void ValidateUser(User user)
        {
            CheckNotNull(user);
            user.IsValid();
            CheckNotExists(user.UserName, user.Id);
        }

        private void CheckNotExists(string username, int id = 0)
        {
            if (repository.FindByCondition(u => u.UserName == username && u.Id != id).Count!=0)
            {
                throw new UserAlreadyExistException(UniqueUsername.DUPLICATE_USERNAME_MESSAGE);
            }
        }
        
        private void CheckNotNull(User user)
        {
            if (user == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        public User GetUserById(int id)
        {
            sessionLogic.ValidateUser(sessionUser);
            ICollection<User> users = repository.FindByCondition(u => u.Id==id);
            if(users.Count == 0)
            {
                throw new UserDoesNotExistException(UserNotFound.USER_ID_NOT_FOUND_MESSAGE);
            }
            return users.First();
        }

        public void UpdateUser(int id, User updatedUser)
        {
            sessionLogic.ValidateUser(sessionUser);
            User originalUser = GetUserById(id);
            originalUser.UpdateData(updatedUser);
            ValidateUser(originalUser);
            repository.Update(originalUser);
            repository.Save();
        }

        public User GetUserByUserName(string userName)
        {
            sessionLogic.ValidateUser(sessionUser);
            ICollection<User> users = repository.FindByCondition(u => u.UserName == userName);
            return users.FirstOrDefault();
        }

        public void RemoveUser(int id)
        {
            sessionLogic.ValidateUser(sessionUser);
            User user = GetUserById(id);
            repository.Delete(user);
            repository.Save();
        }

        public ICollection<User> GetAll()
        {
            sessionLogic.ValidateUser(sessionUser);
            return repository.FindAll();
        }
        public void SetSession(Guid token)
        {
            sessionUser = sessionLogic.GetUserFromToken(token);
        }
    }
}
