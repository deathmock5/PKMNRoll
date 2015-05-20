using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PokemonRoll
{
    [Serializable()]
    class TrainerID : ISerializable
    {
        public TrainerID()
        {

        }

        public TrainerID(SerializationInfo info, StreamingContext ctxt)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }
    }
}
