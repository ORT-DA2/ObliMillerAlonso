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
    public class LoginLogic : ILoginLogic
    {
        ILoginRepository _repository;
        public LoginLogic(IRepositoryUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Login;
      
        }
    }
}
