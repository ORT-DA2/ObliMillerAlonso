using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.WebAPI.Models
{
    public class UserFullModelOut
    {
        public int Id { get;  set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }
    }
}
