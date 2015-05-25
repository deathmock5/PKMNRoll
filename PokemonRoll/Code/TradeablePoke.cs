using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code
{
    [Serializable()]
    class TradeablePoke : ISerializable
    {
        Pokemon poke;
        string hash;

        public override string ToString()
        {
            return poke.getName() + "|" + poke._level + "|" + poke.isshiny;
        }

        public TradeablePoke(SerializationInfo info, StreamingContext ctxt)
        {
            poke = (Pokemon)info.GetValue("t_poke", typeof(Pokemon));
            hash = (string)info.GetValue("t_hash", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("t_poke", poke);
            info.AddValue("t_hash", hash);
        }

        public string getEncriptedSerialisedObjectString()
        {
            SoapFormatter soap = new SoapFormatter();

            return null;
        }

        public bool validate()
        {
            //Convert the deserialised poke into a serial stream again, Hash, and check.
            return false;
        }
    }
}
