//  If you have any questions or just want to talk, join my server!
//  https://discord.gg/0oZpaYcAjfvkDuE4
using DiscordSharp;
using DiscordSharp.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordSharp_Starter
{
    class Program
    {
        public static DiscordSharp.Objects.DiscordChannel lastchannel;
        static void Main(string[] args)
        {
            // First of all, a DiscordClient will be created, and the email and password will be defined.
            Console.WriteLine("Defining variables");
            DiscordClient client = new DiscordClient();
            client.ClientPrivateInformation.Email = "email";
            client.ClientPrivateInformation.Password = "pass";

            // Then, we are going to set up our events before connecting to discord, to make sure nothing goes wrong.

            Console.WriteLine("Defining Events");
            // find that one you interested in 

            client.Connected += (sender, e) => // Client is connected to Discord
            {
                Console.WriteLine("Connected! User: " + e.User.Username);
                // If the bot is connected, this message will show.
                // Changes to client, like playing game should be called when the client is connected,
                // just to make sure nothing goes wrong.
                client.UpdateCurrentGame("Bot online!"); // This will display at "Playing: "
                //Whoops! i messed up here. (original: Bot online!\nPress any key to close this window.)
            };


            client.PrivateMessageReceived += (sender, e) => // Private message has been received
            {
                if (e.Message == "!help")
                {
                    e.Author.SendMessage("This is a private message!");
                    // Because this is a private message, the bot should send a private message back
                    // A private message does NOT have a channel
                }
                if (e.Message.StartsWith("join "))
                {
                    string inviteID = e.Message.Substring(e.Message.LastIndexOf('/') + 1);
                    // Thanks to LuigiFan (Developer of DiscordSharp) for this line of code!
                    client.AcceptInvite(inviteID);
                    e.Author.SendMessage("Joined your discord server!");
                    Console.WriteLine("Got join request from " + inviteID);
                }
            };


            client.MessageReceived += (sender, e) => // Channel message has been received
            {
                if (e.MessageText == "!admin")
                {
                    bool isadmin = false;
                    List<DiscordRole> roles = e.Author.Roles;
                    foreach(DiscordRole role in roles){
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
                if (e.MessageText == "!help")
                {
                    e.Channel.SendMessage("This is a public message!");
                    // Because this is a public message, 
                    // the bot should send a message to the channel the message was received.
                }
                if (e.MessageText == "!cat")
                {
                    Thread t = new Thread(new ParameterizedThreadStart(randomcat));
                    t.Start(e.Channel);
                    string s;
                    using (WebClient webclient = new WebClient())
                    {
                        s = webclient.DownloadString("http://random.cat/meow");
                        int pFrom = s.IndexOf("\\/i\\/") + "\\/i\\/".Length;
                        int pTo = s.LastIndexOf("\"}");
                        string cat = s.Substring(pFrom, pTo - pFrom);
                        webclient.DownloadFile("http://random.cat/i/" + cat, "cat.png");
                        client.AttachFile(e.Channel, "meow!", "cat.png");
                    }
                }
            };

            //  Below: some things that might be nice?

            //  This sends a message to every new channel on the server
            client.ChannelCreated += (sender, e) =>
                {
                    e.ChannelCreated.SendMessage("Nice! a new channel has been created!");
                };

            //  When a user joins the server, send a message to them.
            client.UserAddedToServer += (sender, e) =>
                {
                    e.AddedMember.SendMessage("Welcome to my server! rules:");
                    e.AddedMember.SendMessage("1. be nice!");
                    e.AddedMember.SendMessage("- Your name!");
                };

            //  Don't want messages to be removed? this piece of code will
            //  Keep messages for you. Remove if unused :)
            client.MessageDeleted += (sender, e) =>
                {
                    e.Channel.SendMessage("Removing messages has been disabled on this server!");
                    e.Channel.SendMessage("<@" + e.DeletedMessage.Author.ID + "> sent: " +e.DeletedMessage.Content.ToString());
                };

            

            // Now, try to connect to Discord.
            try{ 
                // Make sure that IF something goes wrong, the user will be notified.
                // The SendLoginRequest should be called after the events are defined, to prevent issues.
                Console.WriteLine("Sending login request");
                client.SendLoginRequest();
                Console.WriteLine("Connecting client in separate thread");
                Thread connect = new Thread(client.Connect);
                connect.Start();
                 // Login request, and then connect using the discordclient i just made.
                Console.WriteLine("Client connected!");
            }catch(Exception e){
                Console.WriteLine("Something went wrong!\n" + e.Message + "\nPress any key to close this window.");
            }

            // Done! your very own Discord bot is online!


            // Now to make sure the console doesnt close:
            Console.ReadKey(); // If the user presses a key, the bot will shut down.
            Environment.Exit(0); // Make sure all threads are closed.
        }

        public static void randomcat(object channel)
        {

        }
    }
}
