using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string MessageContent { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
