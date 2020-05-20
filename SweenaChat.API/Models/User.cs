using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List <Message> Messages { get; set; }

    }
}
