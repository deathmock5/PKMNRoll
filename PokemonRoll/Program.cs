using System;
using System.Collections.Generic;
using System.Linq;
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
            Game.Instance.start();
        }
    }
}
