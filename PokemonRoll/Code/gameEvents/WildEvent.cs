using DM_LangLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class WildEvent : GameEventBase
    {
        public override double GetChance()
        {
            return 50.0;
        }

        public override void trigger()
        {
            Game game = Game.Instance;
            wildEncounter(game);
        }

        public static void wildEncounter(Game game)
        {
            bool wildpokealive = true;
            Pokemon poke = Pokemon.getRandomPoke(rand,game.player);
            Console.WriteLine("A wild {0} has appeared!", poke);
            Console.WriteLine("GO {0}!", game.player.getFirstPokemonInParty());

            while (wildpokealive)
            {
                Console.WriteLine();
                Console.WriteLine(" 0: Catch");
                Console.WriteLine(" 1: Battle");
                int input = FormatQuestion.getIntOptionFromQuestion("What do you want to do?:");

                if (input == 0)
                {
                    //Catch
                    int catchchance = game.player.getPokeballFromBackpack();
                    if (catchchance == 0)
                    {
                        Console.WriteLine("~4You dont have any Pokeballs to use!");
                    }
                    else
                    {
                        int catchroll = rand.Next(100);
                        if (catchroll < catchchance)
                        {
                            Console.WriteLine("Ding...\n Ding...\n Ding...\n CLICK!");
                            Console.WriteLine("~2Wild {0} was caught!", poke.getName());
                            game.player.addPokemon(poke);
                            wildpokealive = false;
                        }
                        else
                        {
                            for (int i = 0; i < rand.Next(3); i++)
                            {
                                Console.WriteLine("Ding...");
                            }
                            Console.WriteLine("POOF!");
                            Console.WriteLine("~6Damn, it appeared it was caught!");
                        }
                    }

                }
                else if (input == 1)
                {
                    //TODO: a more realistic battle?
                    Console.WriteLine("{0} used tackle!", game.player.getFirstPokemonInParty());
                    Console.WriteLine("~2{0}~2 was defeated!", poke.getName());
                    Console.WriteLine("You find 2 rare candy on its body.");
                    game.player.giveOptionToGivePokemonLevels(2);
                    wildpokealive = false;
                }
            }
        }

    }
}
