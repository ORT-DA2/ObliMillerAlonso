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
            CreateMap<User, UserFullModelOut>();
            CreateMap<User, UserModelOut>();
            CreateMap<User, UserSimpleModelOut>();
            CreateMap<CompetitorModelIn, Competitor>();
            CreateMap<Competitor, CompetitorModelOut>();
            CreateMap<Competitor, CompetitorSimpleModelOut>();
            CreateMap<CompetitorScoreModelIn, CompetitorScore>()
                .ForMember(m => m.Score, opt => opt.Condition(source => source.Score != 0))
                .ForMember(m => m.Competitor, s => s.MapFrom(src => new Competitor { Id = src.CompetitorId }));
            CreateMap<SportModelIn, Sport>();
            CreateMap<Sport, SportModelOut>();
            CreateMap<Sport, SportSimpleModelOut>();
            CreateMap<Match, MatchModelOut>();
            CreateMap<Match, MatchSimpleModelOut>();
            CreateMap<CompetitorScore, CompetitorScoreModelOut>();
            CreateMap<MatchModelIn, Match>()
                .ForMember(m => m.Sport, opt => opt.Condition(source => source.SportId != 0))
                .ForMember(m => m.Sport, s => s.MapFrom(src => new Sport { Id = src.SportId }));
            CreateMap<string, DateTime>().ConvertUsing(Convert.ToDateTime);
            CreateMap<DateTime, string>().ConvertUsing(Convert.ToString);
            CreateMap<Comment, CommentModelOut>();
            CreateMap<Comment, CommentSimpleModelOut>();
            CreateMap<CommentModelIn, Comment>();
        }
    }
}
