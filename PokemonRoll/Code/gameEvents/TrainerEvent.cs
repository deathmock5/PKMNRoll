using DM_LangLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class TrainerEvent : GameEventBase
    {
        public override double GetChance()
        {
            return 25.0;
        }

        public override void trigger()
        {
            Game game = Game.Instance;
            string trainername = RandomPerson.getName();
            int cash = rand.Next(200) + 100;
            LangHelper.WriteLine("game.trainer.attack"); //"'Hey you! Stop right there in the name of senseless battle!'"
            if (trainerBattle(trainername, 1, rand.Next(game.player.getPartyLevel()) + game.player.cycles,game))
            {
                LangHelper.WriteLine("game.trainer.cashgain", cash); //"You gain {0}$."
                game.player.cash += cash;
                LangHelper.WriteLine("game.trainer.raregain", trainername, 5); //"{0} hands you {1} rare candys."
                game.player.giveOptionToGivePokemonLevels(5);
            }
            else
            {
                cash = Convert.ToInt32(game.player.cash * 0.1);
                LangHelper.WriteLine("game.trainer.loose", cash); //"You loose {0}$ by loosing!."
                game.player.cash -= cash;
                Console.WriteLine("game.loose.1");
                Console.WriteLine("game.loose.2");
                Console.WriteLine("game.loose.center");
            }
        }

        public static bool trainerBattle(string trainername, int dificultyMod, int offset,Game game)
        {
            int numpokes = rand.Next(rand.Next(rand.Next(5))) + 1;
            Pokemon pokeout = null;
            Pokemon pokebtl = null;
            bool first = true;
            while (numpokes > 0)
            {
                if (pokebtl == null)
                {
                    pokebtl = Pokemon.getRandomPoke(rand,game.player);
                    Console.WriteLine("{1} sends out: {0}", pokebtl.getName(), trainername);
                }
                if (pokeout == null)
                {
                    try
                    {
                        pokeout = game.player.getFirstPokemonInParty();
                        if (first)
                        {
                            Console.WriteLine("GO {0}~7!", pokeout.getName());
                            first = false;
                        }
                        else
                        {
                            Console.WriteLine("Your enemy is weak, GO {0}!", pokeout.getName());
                        }
                    }
                    catch
                    {
                        Console.WriteLine("~4You were defeated by {0}!", trainername);
                        return false;
                    }
                }
                Console.WriteLine("~3{0}~3 and ~3{1}~3 clash in furious battle!", pokeout.getName(), pokebtl.getName());
                int partylevel = game.player.getPartyLevel();
                int difficulty = rand.Next(Convert.ToInt32(game.player.getPartyLevel() * (0.22 * dificultyMod)) + (50 * (game.player.badges * game.player.badges))) + offset;
                Game.log("PartyLv: " + partylevel + " difficulty:" + difficulty);
                if (partylevel > difficulty)
                {
                    pokebtl.faint(false);
                    numpokes--;
                    pokebtl = null;
                }
                else
                {
                    pokeout.faint(true);
                    offset -= pokeout._level;
                    if (game.player.getRevive())
                    {
                        Console.WriteLine("~2You use a revive from your pack!");
                        pokeout.fainted = false;
                    }
                    else
                    {
                        pokeout = null;
                    }
                }
            }
            Console.WriteLine("~2You have defeated {0}!", trainername);
            return true;
        }
    }
}
