using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Domain.Exceptions;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Domain.Constants;
using Sports.Logic.Constants;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Sports.Logic
{
    public class FixtureLogic : IFixtureLogic
    {
        private static ICollection<IFixtureGeneratorStrategy> fixtureGeneratorStrategies;
        private ISportLogic sportLogic;
        private IMatchLogic matchLogic;
        private ISessionLogic sessionLogic;
        private User user;
        private string implementationsPath;

        public FixtureLogic(IRepositoryUnitOfWork unit)
        {
            if (fixtureGeneratorStrategies == null)
            {
                ResetFixtureStrategies();
            }
            sportLogic = new SportLogic(unit);
            matchLogic = new MatchLogic(unit);
            sessionLogic = new SessionLogic(unit);
        }

        public void ResetFixtureStrategies()
        {
            JObject jsonPaths = JObject.Parse(File.ReadAllText(@"fixturesPath.json"));
            implementationsPath = jsonPaths.SelectToken("FixtureDlls").ToString();
            fixtureGeneratorStrategies = new List<IFixtureGeneratorStrategy>();
        }
        
        public ICollection<string> RefreshFixtureImplementations()
        {
            ResetFixtureStrategies();
            sessionLogic.ValidateUser(user); 
            VerifyPath(implementationsPath);
            DirectoryInfo directory = new DirectoryInfo(implementationsPath);
            EvaluateAllDlls(directory);
            return GetFixtureImplementations();
        }
        
        private void VerifyPath(string dllFilesPath)
        {
            if (!Directory.Exists(dllFilesPath))
            {
                throw new FixtureImportingException(FixtureValidation.INVALID_FIXTURE_PATH);
            }
        }

        private void EvaluateAllDlls(DirectoryInfo directory)
        {
            List<FileInfo> fileList = directory.EnumerateFiles("*.dll").ToList();
            foreach (FileInfo file in fileList)
            {
                IncludeFileIfStrategy(file);
            }
        }

        private void IncludeFileIfStrategy(FileInfo file)
        {
            Assembly assembly = Assembly.LoadFile(file.FullName);
            List<Type> types = assembly.GetExportedTypes().ToList();
            for (int j = 0; j < types.Count; j++)
            {
                Type type = types[j];
                if (typeof(IFixtureGeneratorStrategy).IsAssignableFrom(type))
                {
                    fixtureGeneratorStrategies.Add((IFixtureGeneratorStrategy)Activator.CreateInstance(type));
                }
            }
        }

        public ICollection<string> GetFixtureImplementations()
        {
            ICollection<string> implementations = new List<string>();
            sessionLogic.ValidateUser(user);
            CheckStrategiesAreImported();
            try
            {
                foreach (IFixtureGeneratorStrategy strategy in fixtureGeneratorStrategies)
                {
                    implementations.Add(strategy.FixtureInfo());
                }
                return implementations;
            }
            catch (Exception)
            {
                throw new MalfunctioningImplementationException(FixtureValidation.FAILING_FIXTURE_STRATEGY);
            }
        }

        private void CheckStrategiesAreImported()
        {
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException(FixtureValidation.MISSING_FIXTURE_STRATEGIES);
            }
        }


        public void GenerateFixture(int pos, int sportId, DateTime startDate)
        {
            sessionLogic.ValidateUser(user);
            FixtureGenerationValidations(startDate);
            Sport realSport = sportLogic.GetSportById(sportId);
            ICollection<Match> fixtureMatches = new List<Match>();
            IFixtureGeneratorStrategy fixtureStrategy = fixtureGeneratorStrategies.ElementAt(pos);
            try
            {
                fixtureMatches = fixtureStrategy.GenerateFixture(realSport, startDate);
            }
            catch(Exception)
            {
                throw new MalfunctioningImplementationException(FixtureValidation.FAILING_FIXTURE_STRATEGY);
            }
            matchLogic.AddMatches(fixtureMatches);
        }

        private void ValidateDate(DateTime startDate)
        {
            if (startDate.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                throw new InvalidMatchDateFormatException(MatchDateFormat.INVALID_DATE_FORMAT_MESSAGE);
            }
        }

        private void FixtureGenerationValidations( DateTime startDate)
        {
            ValidateDate(startDate);
            CheckFixtureImported();
        }
        

        private void CheckFixtureImported()
        {
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException(FixtureValidation.MISSING_FIXTURE_STRATEGIES);
            }
        }

        public User SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            matchLogic.SetSession(token);
            return user;
        }
    }
}
