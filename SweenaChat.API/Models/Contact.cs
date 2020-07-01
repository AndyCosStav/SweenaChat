using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public ICollection<Message> Messages { get; set; } = new Collection<Message>();
    }
}
