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
    public class SessionLogic : ISessionLogic
    {
        ISessionRepository _repository; 
        IUserLogic _userLogic;
        public SessionLogic(IRepositoryUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Session;
            _userLogic = new UserLogic(unitOfWork);
        }

        public User GetUserFromToken(Guid token)
        {
            Session session = _repository.FindByCondition(s => s.Token == token).FirstOrDefault();
            ValidateNotNullSession(session);
            return session.User;
        }

        public Guid LogInUser(string username, string password)
        {
            User user = _userLogic.GetUserByUserName(username);
            ValidateNotNullUser(user);
            user.ValidatePassword(password);
            LogoutByUser(user);
            Guid token = CreateSession(user);
            return token;
        }

        public Guid CreateSession(User user)
        {
            Session newSession = new Session
            {
                User = user,
                Token = Guid.NewGuid()
            };
            _repository.Create(newSession);
            return newSession.Token;
        }

        public void LogoutByUser(User user)
        {
            ICollection<Session> sessions = _repository.FindByCondition(s => s.User.Equals(user));
            foreach (Session existingSession in sessions)
            {
                _repository.Delete(existingSession);
            }
        }

        private void ValidateNotNullUser(User user)
        {
            if (user == null)
            {
                throw new InvalidUserDataException("User deleted or not exist");
            }
        }


        private void ValidateNotNullSession(Session session)
        {
            if (session == null)
            {
                throw new InvalidUserDataException("Session token does not exist");
            }
        }
    }
}
