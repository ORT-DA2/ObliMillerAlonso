using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Logic.Interface;
using Sports.Logic.Exceptions;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Sports.Logic
{
    public class FixtureLogic : IFixtureLogic
    {
        private ICollection<IFixtureGeneratorStrategy> fixtureGeneratorStrategies;
        private int currentStrategy;

        public FixtureLogic()
        {   
            fixtureGeneratorStrategies = new List<IFixtureGeneratorStrategy>();
        }
        
        public void AddFixtureImplementations(string dllFilesPath)
        {
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

        public void ChangeFixtureImplementation()
        {
            if (fixtureGeneratorStrategies.Count == 0)
            {
                throw new NoImportedFixtureStrategiesException("No strategies are imported");
            }
            currentStrategy = (currentStrategy + 1) % fixtureGeneratorStrategies.Count;
        }

        public ICollection<Match> GenerateFixture(ICollection<Sport> sports)
        {
            CheckFixtureImported();
            CheckListIsNotNull(sports);
            IFixtureGeneratorStrategy fixtureStrategy = fixtureGeneratorStrategies.ElementAt(currentStrategy);
            return fixtureStrategy.GenerateFixture(sports);
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
                throw new NoFixturesImportedException("No fixtures have been imported");
            }
        }
    }
}
