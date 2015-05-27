using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "PokeRoll by deathmock@gmail.com";
            File.WriteAllText("logfile.log","");
            //APIHelper.polCache();
            try
            {
                Game.Instance.start();
            }
            catch(Exception e)
            {
                Game.log(e.Message);
                Game.log(e.StackTrace);
                Console.WriteLine("The game has crashed in an irecoverable way.");
                Console.Write("Sending logfile.log to deathmock@gmail.com...");
                string message = "";
                foreach(string line in File.ReadAllLines("logfile.log"))
                {
                    message += line + "\r\n";
                }
                if (sendMail(message, Game.Instance.me.name))
                {
                    Console.WriteLine("Please send the contents of logfile.log to deathmock@gmail.com");
                }
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("Stacktrace: {0}",e.StackTrace);
            }
            Console.ReadLine();
        }

        static bool sendMail(string message,string subject)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("zomdonotreply@gmail.com", "bntkfzerbijlbwwu");;

                MailMessage mm = new MailMessage("zomdonotreply@gmail.com", "deathmock@gmail.com", subject, message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                Console.WriteLine("sent!");
                return true;
            }
            catch
            {
                Console.WriteLine("failed! =/");
            }
            return false;
        }
       
    }
}
