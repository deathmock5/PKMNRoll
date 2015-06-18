using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll
{
    public abstract class GameEventBase
    {
        protected static Random rand = new Random();
        public abstract double GetChance();

        public abstract void trigger();
    }
}
