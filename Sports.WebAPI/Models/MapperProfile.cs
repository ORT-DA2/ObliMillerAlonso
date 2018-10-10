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
            CreateMap<MatchModelIn, Match>()
                .ForMember(m => m.Sport, opt => opt.Condition(source => source.SportId != 0))
                .ForMember(m=>m.Sport, s=>s.MapFrom(src=> new Sport { Id = src.SportId}))
                .ForMember(m => m.Local, opt => opt.Condition(source => source.LocalId != 0))
                .ForMember(m => m.Local, s => s.MapFrom(src => new Team { Id = src.LocalId }))
                .ForMember(m => m.Visitor, opt => opt.Condition(source => source.VisitorId != 0))
                .ForMember(m => m.Visitor, s => s.MapFrom(src => new Team { Id = src.VisitorId }));
            CreateMap<string, DateTime>().ConvertUsing(Convert.ToDateTime);
            CreateMap<DateTime, string>().ConvertUsing(Convert.ToString);
            CreateMap<Comment, CommentModelOut>();
            CreateMap<Comment, CommentSimpleModelOut>();
            CreateMap<CommentModelIn, Comment>();
        }
    }
}
