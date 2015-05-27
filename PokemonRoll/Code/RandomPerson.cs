using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonRoll
{
    class RandomPerson
    {
        private static List<string> names = new List<string>();
        private static Mutex mut = new Mutex();
        private static Thread oThread;
        private static bool running = false;

        public static void startThread()
        {
            oThread = new Thread(threadedCall);
            running = true;
            oThread.Start();
        }

        public static void stopThread()
        {
            running = false;
            oThread.Join();
        }

        private static void threadedCall()
        {
            
            while(running)
            {
                mut.WaitOne(1000);
                if(names.Count() == 5)
                {
                    mut.ReleaseMutex();
                    Thread.Sleep(1000);
                }
                else
                {
                    string api = APIHelper.getApiCall("http://api.randomuser.me/?format=json");
                    if(api != null)
                    {
                        //Pull the information.
                        var obj = JsonConvert.DeserializeObject<dynamic>(api);
                        var result = obj.results;
                        var user = result[0].user;
                        var name = user.name;
                        string first = (string)name.first;
                        names.Add(UppercaseFirst(first));
                    }
                    else
                    {
                        mut.ReleaseMutex();
                        Game.log("The api call is not returning good data. exiting RandomUser thread.");
                        running = false;
                    }
                }
            }
        }

        public static string getName()
        {
            //mut.WaitOne(1000);
            if(names.Count == 0)
            {
                //mut.ReleaseMutex();
                return "Namless";
            }
            String name = names[names.Count - 1];
            names.Remove(name);
            //mut.ReleaseMutex();
            return name;
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
