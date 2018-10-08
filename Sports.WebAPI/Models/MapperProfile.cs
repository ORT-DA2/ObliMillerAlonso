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
            CreateMap<UserModelIn, User>().ConstructUsing(u => new User(u.IsAdmin));
            CreateMap<User, UserModelOut>();
            CreateMap<User, UserSimpleModelOut>();
            CreateMap<TeamModelIn, Team>();
            CreateMap<Team, TeamModelOut>();
            CreateMap<Team, TeamSimpleModelOut>();
            CreateMap<SportModelIn, Sport>();
            CreateMap<Sport, SportModelOut>();
            CreateMap<Sport, SportSimpleModelOut>();
            CreateMap<Match, MatchModelOut>();
            CreateMap<Match, MatchSimpleModelOut>();
            CreateMap<MatchModelIn, Match>();
            CreateMap<string, DateTime>().ConvertUsing(Convert.ToDateTime);
            CreateMap<DateTime, string>().ConvertUsing(Convert.ToString);
            CreateMap<Comment, CommentModelOut>();
            CreateMap<Comment, CommentSimpleModelOut>();
            CreateMap<CommentModelIn, Comment>();
        }
    }
}
