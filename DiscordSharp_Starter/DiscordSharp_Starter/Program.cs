﻿//  If you have any questions or just want to talk, join my server!
//  https://discord.gg/0oZpaYcAjfvkDuE4
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;


namespace DiscordSharp_Starter
{
    class Program
    {
        public static Channel lastchannel;
        public static bool isbot = true;
        static void Main(string[] args)
        {
            // First of all, a DiscordClient will be created, and the email and password will be defined.
            Console.WriteLine("Defining variables");
            // Fill in token and change isbot to true if you use the API
            // Else, leave token alone and change isbot to false
            // But believe me, the API bots are nicer because of a sexy bot tag!
            DiscordClient _client = new DiscordClient();

            // Then, we are going to set up our events before connecting to discord, to make sure nothing goes wrong.

            Console.WriteLine("Defining Events");
            // find that one you interested in 

            _client.MessageReceived += (sender, e) => // Channel message has been received
            {
                if (e.Message.Text == "!admin")
                {
                    bool isadmin = false;
                    IEnumerable<Role> roles = e.User.Roles;
                    foreach(Role role in roles){
                        if (role.Name.Contains("Administrator"))
                        {
                            isadmin = true;
                        }
                    }
                    if (isadmin)
                    {
                        e.Channel.SendMessage("Yes, you are! :D");
                    }
                    else
                    {
                        e.Channel.SendMessage("No, you aren't :c");
                    }
                }
                if (e.Message.Text == "!help")
                {
                    e.Channel.SendMessage("This is a test help message!");
                    // Because this is a public message, 
                    // the bot should send a message to the channel the message was received.
                }
                if (e.Message.Text == "!cat")
                {
                    Thread t = new Thread(new ParameterizedThreadStart(randomcat));
                    t.Start(e.Channel);
                    string s;
                    using (WebClient web_client = new WebClient())
                    {
                        s = web_client.DownloadString("http://random.cat/meow");
                        int pFrom = s.IndexOf("\\/i\\/") + "\\/i\\/".Length;
                        int pTo = s.LastIndexOf("\"}");
                        string cat = s.Substring(pFrom, pTo - pFrom);
                        web_client.DownloadFile("http://random.cat/i/" + cat, "cat.png");
                        e.Channel.SendMessage("Neko-chan!");
                        e.Channel.SendFile("cat.png");
                    }
                }
                if (e.Message.Text.StartsWith("!repeat"))
                {
                    string recievedMessage = e.Message.Text;
                    string newMessage = recievedMessage.TrimStart('!','r','e','p','e','a','t');
                    e.Channel.SendMessage(newMessage);
                }
                if (e.Message.Text.StartsWith("!stats"))
                {

                }
                if (e.Message.Text == "!die")
                {
                    bool isadmin = false;
                    IEnumerable<Role> roles = e.User.Roles;
                    foreach (Role role in roles)
                    {
                        if (role.Name.Contains("Administrator"))
                        {
                            isadmin = true;
                        }

                    }
                    if (isadmin)
                    {
                        _client.Disconnect();
                    }
                }
            };

            //  Below: some things that might be nice?

            //  This sends a message to every new channel on the server
            _client.ChannelCreated += (sender, e) =>
                {
                    if(e.Channel.Type == ChannelType.Text)
                    {
                        e.Channel.SendMessage("Nice! a new channel has been created!");
                    }
                };

            //  When a user joins the server, send a message to them.
            _client.UserJoined += (sender, e) =>
                {
                    e.User.SendMessage("Welcome to my server! rules:");
                    e.User.SendMessage("1. be nice!");
                    e.User.SendMessage("- D-Rock!");
                };

            //  Don't want messages to be removed? this piece of code will
            //  Keep messages for you. Remove if unused :)
            _client.MessageDeleted += (sender, e) =>
                {
                    e.Channel.SendMessage("Removing messages has been disabled on this server!");
                    e.Channel.SendMessage("<@" + e.Message.User.Id + "> sent: " +e.Message.Text);
                };

            _client.ExecuteAndWait(async () => {
                // yes i left my token here but i refreshed it lmao
                await _client.Connect("MjM3NjIxODc0NzQ4NTU1MjY0.Cuezcg.oI5i9zAirrtYeTYvu1r33-GYTKk", TokenType.Bot);
                _client.SetGame("D.NET_starter!", GameType.Twitch, "https://github.com/NaamloosDT/DiscordSharp_Starter");
            });


                Console.WriteLine("Client connected!");

            // Done! your very own Discord bot is online!
        }

        public static void randomcat(object channel)
        {

        }
    }
}
