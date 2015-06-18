using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class RareCandyEvent :GameEventBase
    {
        public override double GetChance()
        {
            return 5.0;
        }

        public override void trigger()
        {
            //Rare Candy
            Console.WriteLine("You reach a dead end, and for some unimaginable reason you decide to search the ground.");
            Console.WriteLine("~2You find 15 Rare candys!");
            Game.Instance.player.giveOptionToGivePokemonLevels(15);
        }
    }
}
