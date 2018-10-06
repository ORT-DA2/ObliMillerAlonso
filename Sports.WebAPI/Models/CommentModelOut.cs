using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class CommentModelOut
    {
        public int MatchId { get; set; }
        public int UserId { get; set;  }
        public string Text { get; set; }
    }
}
