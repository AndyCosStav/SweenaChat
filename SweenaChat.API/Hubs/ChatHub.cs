﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SweenaChat.API.Data;
using SweenaChat.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(Message message)
        {
            var newMessage = new Message
            {
                MessageContent = message.MessageContent,
                DateCreated = DateTime.Now,
                Sender = message.Sender,
                Receiver = message.Receiver

            };

            _dbContext.Messages.Add(newMessage);

            var user = _dbContext.Users.Include(u => u.Messages).SingleOrDefault(x => x.Username == message.Sender);

            if (user != null)
            {
                user.Messages.Add(newMessage);


                await _dbContext.SaveChangesAsync();
            }

            await Clients.All.SendAsync("ReceiveMessage", message);
        }



        public async Task UpdateCurrentConvo(string user1, string user2)
        {
            var conversations = _dbContext.Messages.Where(x =>
                                     (x.Receiver == user1 || x.Receiver == user2) &&
                                     (x.Sender == user2 || x.Sender == user1)).ToList();

            await Clients.All.SendAsync("ReceiveConversation", conversations);
        }

        
    }
}
