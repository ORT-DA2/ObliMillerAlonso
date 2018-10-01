using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sports.Domain;
using Sports.Repository.Interface;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using Sports.Logic.Constants;
using System.IO;
using System.Reflection;

namespace Sports.Logic
{
    public class FixtureLogic : IFixtureLogic
    {
        private ICollection<IFixtureGeneratorStrategy> fixtureGeneratorStrategies;
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
            ValidateUser(); 
            VerifyPath(dllFilesPath);
            DirectoryInfo directory = new DirectoryInfo(dllFilesPath);
            EvaluateAllDlls(directory);
        }

        private void ValidateUser()
        {
            ValidateUserNotNull();
            ValidateUserNotAdmin();
        }

        private void ValidateUserNotNull()
        {
            if (user == null)
            {
                throw new InvalidNullValueException(NullValue.INVALID_USER_NULL_VALUE_MESSAGE);
            }
        }

        private void ValidateUserNotAdmin()
        {
            if (!user.IsAdmin)
            {
                throw new NonAdminException(AdminException.NON_ADMIN_EXCEPTION_MESSAGE);
            }
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

        public void ChangeFixtureImplementation()
        {
            ValidateUser();
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException("No strategies are imported");
            }
            currentStrategy = (currentStrategy + 1) % (fixtureGeneratorStrategies.Count);
        }

        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            ValidateUser();
            FixtureGenerationValidations(sports);
            ICollection<Sport> realSports = GetRealSports(sports);
            ICollection<Match> fixtureMatches = new List<Match>();
            IFixtureGeneratorStrategy fixtureStrategy = fixtureGeneratorStrategies.ElementAt(currentStrategy);
            try
            {
                fixtureMatches = fixtureStrategy.GenerateFixture(realSports);
            }
            catch(Exception)
            {
                throw new MalfunctioningImplementationException("Fixture generation strategy is failing");
            }
            return fixtureMatches;
        }

        private void FixtureGenerationValidations(ICollection<Sport> sports)
        {
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
