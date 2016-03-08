using DiscordSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordSharp_Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordClient client = new DiscordClient();
            client.ClientPrivateInformation.email = "Your Email";
            client.ClientPrivateInformation.password = "Your password";
            client.SendLoginRequest();
            client.Connect(); // Login request, and then connect using the discordclient i just made.
            client.UpdateCurrentGame("Bot online!"); // This will display at "Playing: "


            client.Connected += (sender, e) =>
            {
                Console.WriteLine("Connected! User: " + e.user.Username);
                // If the bot is connected, this message will show.
            };


            client.PrivateMessageReceived += (sender, e) => // Private message is received
            {
                if (e.message == "help")
                {
                    e.author.SendMessage("This is a private message!"); 
                    // Because this is a private message, the bot should send a private message back
                    // A private message does NOT have a channel
                }
                if (e.message.StartsWith("join "))
                {
                    string joinID = e.message.Substring(24, 16);
                    client.AcceptInvite(joinID);
                    e.author.SendMessage("Joined your discord server!");
                    Console.WriteLine("Got join request from " + joinID);
                }
            };


            client.MessageReceived += (sender, e) => // Channel message is received
            {
                if(e.message_text == "help"){
                    e.Channel.SendMessage("This is a public message!");
                    // Because this is a public message, 
                    // the bot should send a message to the channel the message was received.
                }
            };


            Console.ReadKey(); // If the user presses a key, the bot will shut down.
        }

        public int number(string name, int min, int max)
        {
            // Bonus code: returns number based of bytes of string.
            // If something goes wrong, (eg: too long int) it return the min value.
            try
            {
                byte[] namebt = Encoding.UTF8.GetBytes(name);
                string namebtstring = namebt.ToString();
                int namebtint = Int32.Parse(namebtstring);
                Random rnd = new Random(namebtint);
                int returnint = rnd.Next(min, max);
                return returnint;
            }
            catch (Exception)
            {
                return min;
            }
        }
    }
}
