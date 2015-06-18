using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class TravelEvent :GameEventBase
    {
        public override double GetChance()
        {
            return 25.0;
        }

        public override void trigger()
        {
            //Visit location
            //OCEAN
            //Forest
            //Ruins
            //Cave
            Console.WriteLine("TODO: travel.");
        }
    }
}
