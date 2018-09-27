﻿using System;
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
    public class SessionLogic : ISessionLogic
    {
        ISessionRepository repository; 
        IUserLogic userLogic;
        public SessionLogic(IRepositoryUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Session;
            userLogic = new UserLogic(unitOfWork);
        }

        public User GetUserFromToken(Guid token)
        {
            Session session = repository.FindByCondition(s => s.Token == token).FirstOrDefault();
            ValidateNotNullSession(session);
            return session.User;
        }

        public Guid LogInUser(string username, string password)
        {
            User user = userLogic.GetUserByUserName(username);
            ValidateNotNullUser(user);
            user.ValidatePassword(password);
            LogoutByUser(user);
            Guid token = CreateSession(user);
            return token;
        }

        private Guid CreateSession(User user)
        {
            Session newSession = new Session
            {
                User = user,
                Token = Guid.NewGuid()
            };
            repository.Create(newSession);
            return newSession.Token;
        }

        public void LogoutByUser(User user)
        {
            ICollection<Session> sessions = repository.FindByCondition(s => s.User.Equals(user));
            foreach (Session existingSession in sessions)
            {
                repository.Delete(existingSession);
            }
        }

        private void ValidateNotNullUser(User user)
        {
            if (user == null)
            {
                throw new UserDoesNotExistException(UserNotFound.USER_NOT_FOUND_MESSAGE);
            }
        }


        private void ValidateNotNullSession(Session session)
        {
            if (session == null)
            {
                throw new SessionDoesNotExistException(SessionValidation.TOKEN_NOT_EXIST_MESSAGE);
            }
        }
    }
}
