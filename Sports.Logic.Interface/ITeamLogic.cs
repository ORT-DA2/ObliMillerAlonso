﻿using System;
using System.Collections.Generic;
using System.Text;
using Sports.Domain;
using Sports.Repository.Interface;

namespace Sports.Logic.Interface
{
    public interface ITeamLogic
    {
        void AddTeam(Team team);
        Team GetTeamById(int id);
    }
}