using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Repository.Interface
{
    public interface IRepositoryUnitOfWork
    {
        IUserRepository User { get; }
        ITeamRepository Team { get; }
        ICommentRepository Comment { get; }
        ISportRepository Sport { get; }
        IMatchRepository Match { get; }
        ILoginRepository Login { get; }
    }
}
