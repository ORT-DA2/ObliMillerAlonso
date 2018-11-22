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
    public class MatchLogic : IMatchLogic
    {
        IMatchRepository repository;
        ISportLogic sportLogic;
        ICommentLogic commentLogic;
        ICompetitorLogic competitorLogic;
        ICompetitorScoreRepository competitorScoreRepository;
        ISessionLogic sessionLogic;
        IUserLogic userLogic;
        User user;

        public MatchLogic(IRepositoryUnitOfWork unit)
        {
            repository = unit.Match;
            competitorScoreRepository = unit.CompetitorScore;
            sportLogic = new SportLogic(unit);
            competitorLogic = new CompetitorLogic(unit);
            commentLogic = new CommentLogic(unit);
            sessionLogic = new SessionLogic(unit);
            userLogic = new UserLogic(unit);
        }
        public void AddMatch(Match match)
        {
            sessionLogic.ValidateUser(user);
            ValidateMatch(match);
            CheckMatchDoesntExist(match);
            repository.Create(match);
            repository.Save();
        }

        private void ValidateMatch(Match match)
        {
            CheckNotNull(match);
            match.IsValid();
            ValidateSport(match);
            match.IsValidMatch();
        }
        
        private void CheckMatchDoesntExist(Match match)
        {
            foreach (CompetitorScore competitor in match.Competitors)
            {
                ICollection<Match> matches = repository.FindByCondition(m => match.Competitors.Contains(competitor) && m.Date.Date.Equals(match.Date.Date));
                if (matches.Count != 0)
                {
                    throw new MatchAlreadyExistsException(MatchValidation.COMPETITOR_ALREADY_PLAYING);
                }
            }
            
        }

        private void ValidateSport(Match match)
        {
            match.Sport = sportLogic.GetSportById(match.Sport.Id);
            Sport sport = match.Sport;
            ICollection<CompetitorScore> competitorsInMatch = match.Competitors.ToList();
            ICollection<Competitor> competitors = new List<Competitor>();
            foreach (CompetitorScore comp in competitorsInMatch)
            {
                comp.Competitor = sportLogic.GetCompetitorFromSport(sport.Id, comp.Competitor.Id);
            }
        }

        private void CheckNotNull(Match match)
        {
            if (match == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_MATCH_NULL_VALUE_MESSAGE);
            }
        }

        public Match GetMatchById(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            ICollection<Match> matches = repository.FindByCondition(m => m.Id == id);
            if (matches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchId.MATCH_ID_NOT_EXIST_MESSAGE);
            }
            return matches.First();
        }


        public ICollection<Match> GetAllMatchesForCompetitor(Competitor competitor)
        {
            sessionLogic.ValidateUserNotNull(user);
            List<Match> relatedMatches = repository.FindByCondition(m => m.Competitors.Where(c => c.Competitor.Equals(competitor)).Count() > 0).ToList();
            if (relatedMatches.Count == 0)
            {
                throw new MatchDoesNotExistException(MatchValidation.COMPETITOR_DOESNT_PLAY);
            }
            return relatedMatches;
        }


        public ICollection<Match> GetAllPastMatchesForSport(Sport sport)
        {
            sessionLogic.ValidateUserNotNull(user);
            List<Match> relatedMatches = repository.FindByCondition(m =>m.Sport.Equals(sport)&&m.Date.Date.CompareTo(DateTime.Now.Date) <= 0).ToList();
            return relatedMatches;
        }

        public void ModifyMatch(int id, Match match)
        {
            sessionLogic.ValidateUser(user);
            Match realMatch = GetMatchById(id);
            realMatch.UpdateMatch(match);
            ValidateMatch(realMatch);
            repository.Update(realMatch);
            repository.Save();
        }

        public void DeleteMatch(int matchId)
        {
            sessionLogic.ValidateUser(user);
            Match realMatch = GetMatchById(matchId);
            foreach (CompetitorScore comp in realMatch.Competitors)
            {
                competitorScoreRepository.Delete(comp);
            }
            repository.Delete(realMatch);
            repository.Save();
        }

        public ICollection<Match> GetAllMatches()
        {
            sessionLogic.ValidateUserNotNull(user);
            return repository.FindAll();
        }

        public void AddCommentToMatch(int id, Comment comment)
        {
            sessionLogic.ValidateUserNotNull(user);
            comment.User = user;
            commentLogic.AddComment(comment);
            Match commentedMatch = GetMatchById(id);
            commentedMatch.AddComment(comment);
            repository.Update(commentedMatch);
            repository.Save();
        }

        public ICollection<Comment> GetAllComments(int id)
        {
            sessionLogic.ValidateUserNotNull(user);
            Match commentedMatch = GetMatchById(id);
            return commentedMatch.GetAllComments();
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            commentLogic.SetSession(token);
            competitorLogic.SetSession(token);
        }

        public void AddMatches(ICollection<Match> matches)
        {
            foreach (Match match in matches)
            {
                CheckMatchDoesntExist(match);
            }
            foreach (Match match in matches)
            {
                AddMatch(match);
            }
        }

        public ICollection<CompetitorScore> GenerateRanking(int sportId)
        {
            sessionLogic.ValidateUserNotNull(user);
            Sport sport = sportLogic.GetSportById(sportId);
            ICollection<CompetitorScore> ranking = CreateRankingList(sport);
            AddRankingScores(sport, ranking);
            return ranking;
        }

        private void AddRankingScores(Sport sport, ICollection<CompetitorScore> ranking)
        {
            IRankingGenerator rankingGenerator = sport.GetRankingGenerator();
            ICollection<Match> matches = GetAllPastMatchesForSport(sport);
            foreach (Match match in matches)
            {
                ICollection<CompetitorScore> matchRanking = rankingGenerator.GenerateScores(match.Competitors);
                foreach (CompetitorScore competitorResult in matchRanking)
                {
                    ranking.Where(c => c.Competitor.Equals(competitorResult.Competitor)).FirstOrDefault().Score += competitorResult.Score;
                }
            }
        }

        private static ICollection<CompetitorScore> CreateRankingList(Sport sport)
        {
            ICollection<CompetitorScore> ranking = new List<CompetitorScore>();
            foreach (Competitor competitor in sport.Competitors)
            {
                CompetitorScore rank = new CompetitorScore(competitor);
                ranking.Add(rank);
            }

            return ranking;
        }
    }
}
