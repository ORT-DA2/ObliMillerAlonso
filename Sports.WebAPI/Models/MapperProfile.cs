using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sports.Domain;

namespace Sports.WebAPI.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserModelIn, User>().ConstructUsing(u => new User(u.IsAdmin)); ;
            CreateMap<User, UserModelOut>();
            CreateMap<TeamModelIn, Team>();
            CreateMap<Team, TeamModelOut>();
        }
    }
}
