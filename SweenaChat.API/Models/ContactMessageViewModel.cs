using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Models
{
    public class ContactMessageViewModel
    {
        public List<Message> Messages { get; set; } = new  List<Message>();
    }
}
