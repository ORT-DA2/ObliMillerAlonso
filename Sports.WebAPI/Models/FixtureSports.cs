﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class FixtureSports
    {
        public ICollection<SportModelIn> Sports { get; set; }
        public string Date { get; set; }
    }
}
