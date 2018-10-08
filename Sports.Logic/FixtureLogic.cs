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
using System.IO;
using System.Reflection;

namespace Sports.Logic
{
    public class FixtureLogic : IFixtureLogic
    {
        private static ICollection<IFixtureGeneratorStrategy> fixtureGeneratorStrategies;
        private int currentStrategy;
        private ISportLogic sportLogic;
        private IMatchLogic matchLogic;
        private ISessionLogic sessionLogic;
        User user;

        public FixtureLogic(IRepositoryUnitOfWork unit)
        {   
            fixtureGeneratorStrategies = new List<IFixtureGeneratorStrategy>();
            sportLogic = new SportLogic(unit);
            matchLogic = new MatchLogic(unit);
            sessionLogic = new SessionLogic(unit);
        }
        
        public void AddFixtureImplementations(string dllFilesPath)
        {
            sessionLogic.ValidateUser(user); 
            VerifyPath(dllFilesPath);
            DirectoryInfo directory = new DirectoryInfo(dllFilesPath);
            EvaluateAllDlls(directory);
        }
        
        private void VerifyPath(string dllFilesPath)
        {
            if (!Directory.Exists(dllFilesPath))
            {
                throw new FixtureImportingException("Invalid fixture implementation path");
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

        public string ChangeFixtureImplementation()
        {
            sessionLogic.ValidateUser(user);
            CheckStrategiesAreImported();
            currentStrategy = (currentStrategy + 1) % (fixtureGeneratorStrategies.Count);
            IFixtureGeneratorStrategy fixtureStrategy = fixtureGeneratorStrategies.ElementAt(currentStrategy);
            try
            {
                return fixtureStrategy.FixtureInfo();
            }
            catch (Exception)
            {
                throw new MalfunctioningImplementationException("Fixture generation strategy is failing");
            }
        }

        private void CheckStrategiesAreImported()
        {
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException("No strategies are imported");
            }
        }


        public void GenerateFixture(ICollection<Sport> sports, DateTime startDate)
        {
            sessionLogic.ValidateUser(user);
            FixtureGenerationValidations(sports,startDate);
            ICollection<Sport> realSports = GetRealSports(sports);
            ICollection<Match> fixtureMatches = new List<Match>();
            IFixtureGeneratorStrategy fixtureStrategy = fixtureGeneratorStrategies.ElementAt(currentStrategy);
            try
            {
                fixtureMatches = fixtureStrategy.GenerateFixture(realSports, startDate);
            }
            catch(Exception)
            {
                throw new MalfunctioningImplementationException("Fixture generation strategy is failing");
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

        private void FixtureGenerationValidations(ICollection<Sport> sports, DateTime startDate)
        {
            ValidateDate(startDate);
            CheckFixtureImported();
            CheckListIsNotNull(sports);
        }

        private ICollection<Sport> GetRealSports(ICollection<Sport> sports)
        {
            ICollection<Sport> realSports = new List<Sport>();
            foreach (Sport sport in sports)
            {
                realSports.Add(sportLogic.GetSportByName(sport.Name));
            }
            return realSports;
        }

        private void CheckListIsNotNull(ICollection<Sport> sports)
        {
            if (sports == null)
            {
                throw new InvalidNullValueException("List of sports is empty");
            }
        }

        private void CheckFixtureImported()
        {
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException("No fixtures have been imported");
            }
        }

        public void SetSession(Guid token)
        {
            user = sessionLogic.GetUserFromToken(token);
            sportLogic.SetSession(token);
            matchLogic.SetSession(token);
        }
    }
}
