using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM_LangLib;

namespace PokemonRoll.Code.gameEvents
{
    class GymEvent : GameEventBase
    {
        public override double GetChance()
        {
            return 15.0;
        }

        public override void trigger()
        {
            Game game = Game.Instance;
            Console.WriteLine("You find a flier requesting trainers of great skill chalenge themselves by running through the pokemon leauge.");
            if (FormatQuestion.getBoolOptionFromQuestion("Interested?"))
            {
                if (game.player.badges % 8 == 0)
                {
                    //Posible Elite4
                    if (game.player.badges / 8 != game.player.champ)
                    {
                        fightLeauge(game,game.player);
                        return;
                    }
                }
                fightGym(game, game.player);
                return;
            }
            else
            {
                Console.WriteLine("You toss the flier. Proclaming: 'Nope.'");
                return;
            }
        }

        static private bool fightLeauge(Game game,Player player)
        {
            string name = "";
            Console.WriteLine("You make your way to the pokemon leauge!");
            //5x pokemon
            int pokes = 5;
            while (pokes-- > 0)
            {
                WildEvent.wildEncounter(game);
            }
            Console.WriteLine("After a long and arduess journey you make it to the leauge!");
            PokemonCenterEvent.do_PokemonCenter(game);
            int members = 0;
            while (members++ < 4)
            {
                Console.WriteLine("You step through the leauge doors, and they slam behind you");
                name = "Elite 4 member " + Enum.GetName(typeof(Elite4), (player.champ + 1) * members);
                Console.WriteLine("You are chalenged by {0}!", name);
                if (!TrainerEvent.trainerBattle(name, members, 1000 * (player.champ + 1),game))
                {
                    //Defeat
                    return false;
                }
            }
            //Beat the leauge!
            Console.WriteLine("TODO: <Champion>");
            Console.WriteLine("You have conqured the pokemon leauge!");
            player.champ++;
            return true;
        }
        private bool fightGym(Game game, Player player)
        {
            string name = "";
            int cash = 0;
            int candys = 0;
            Console.WriteLine("You make your way to the nearest gym!");
            Console.WriteLine("You then arrive and are imedeatly ambushed by trainers!");
            int trainers = 1;
            while (trainers++ != 4)
            {
                name = "Gym asistant " + RandomPerson.getName();
                cash = rand.Next(100 * trainers) + 100;
                candys = rand.Next(4) + trainers;
                if (TrainerEvent.trainerBattle(name, trainers, 100, game))
                {
                    //Won
                    Console.WriteLine("You gain {0}$.", cash);
                    player.cash += cash;
                    Console.WriteLine("{0} hands you {1} rare candys.", name, candys);
                    player.giveOptionToGivePokemonLevels(candys);
                }
                else
                {
                    //Lost
                    cash = Convert.ToInt32(cash * 0.5);
                    Console.WriteLine("You loose {0}$.", cash);
                    player.cash -= cash;
                    return false;
                }
            }
            //Beat the trainers, now for leader!
            name = Enum.GetName(typeof(GymLeader), player.badges);
            cash = rand.Next(1000) + 100 * player.badges;
            candys = rand.Next(10) + player.badges + 1;
            Console.WriteLine("You are chalenged by gym leader {0}!", name);
            if (TrainerEvent.trainerBattle(name, player.badges, 100 * (player.badges + 1),game))
            {
                //Won!
                Console.WriteLine("You gain {0}$.", cash);
                player.cash += cash;
                Console.WriteLine("{0} hands you {1} rare candys.", name, candys);
                player.giveOptionToGivePokemonLevels(candys);
                Console.WriteLine("{0} also hands you a badge, which you pin to your clothing.", name);
                player.badges++;
                return true;
            }
            Console.WriteLine("~4You loose consiousness due to your defeat as {0} cackles with laughter.", name);
            Console.WriteLine("You wake up later in the pokemon center, with the bitter taste of defeat in your mouth.");
            return false;
        }

    }
}
