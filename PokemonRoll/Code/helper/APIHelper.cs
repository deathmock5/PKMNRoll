using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokemonRoll
{
    class APIHelper
    {
        public static string getApiCall(string api)
        {
            string urlAddress = api;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(Exception E)
            {
                Game.log(E.Message);
                return null;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    if(response.CharacterSet == "")
                    {
                        readStream = new StreamReader(receiveStream, Encoding.ASCII);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }

            return null;
        }

        public static T getObjectFromApiAndCache<T>(string api)
        {
            string something = "";
            bool needsaved = false;

            //http://pokeapi.co/api/v1/pokemon/19/
            if(api[api.Length-1] == '/')
            {
                api = api.Substring(0, api.Length - 1);
            }
            //http://pokeapi.co/api/v1/pokemon/19
            api = api.Replace("http://", "");
            //pokeapi.co/api/v1/pokemon/19
            makeFolderIfNotExist("cache/" + api.Substring(0,api.LastIndexOf("/")));
            if (System.IO.File.Exists("cache/" + api + ".json"))
            {
                //Load from file.
                StreamReader reader = new StreamReader("cache/" + api + ".json");
                something = reader.ReadToEnd();
                reader.Close();
            }
            else
            {
                something = getApiCall("http://" + api + "/");
                needsaved = true;
            }
            if(something == null || something == "")
            {
                return default(T);
            }
            T temp = JsonConvert.DeserializeObject<T>(something);
            if (needsaved)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("cache/" + api + ".json", true))
                {
                    file.WriteLine(something);
                }
            }
            return temp;
        }

        

        public static void makeFolderIfNotExist(string folder)
        {
            bool exists = System.IO.Directory.Exists(folder);

            if (!exists)
                System.IO.Directory.CreateDirectory(folder);
        }
    }
}
