using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;

namespace Sports.Logic.Interface
{
    public interface IFixtureLogic
    {
        void AddFixtureImplementation(String path);
        void GenerateFixture(ICollection<Sport> sports);
        void ChangeFixtureImplementation();
    }
}
