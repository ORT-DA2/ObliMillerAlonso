﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class CommentModelOut
    {
        public MatchModelOut Match { get; set; }
        public UserSimpleModelOut User { get; set;  }
        public string Text { get; set; }
    }
}