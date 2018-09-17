using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Repository.Interface
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ITeamRepository Team { get; }
        ICommentRepository Comment { get; }
    }
}
