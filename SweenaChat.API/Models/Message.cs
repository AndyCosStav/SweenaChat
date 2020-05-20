using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string MessageContent { get; set; }

        public DateTime DateCreated { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }
    }
}
