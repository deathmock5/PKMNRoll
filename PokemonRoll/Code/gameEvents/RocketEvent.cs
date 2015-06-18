using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class RocketEvent :GameEventBase
    {
        public override double GetChance()
        {
            return 1.0;
        }

        public override void trigger()
        {
            Game game = Game.Instance;
            if(game.player.stolenPokes.Count > 10)
            {
                //You come acrost a rocket hideout!
                //DO you want to enter?
                // Yes: You make your way inside the rockets base
                // no: you decide that its probly not the best idea at them moment.
                // 5x rocket grunts.
                // 1x rocket boss
                // get all stolen pokemon back!
            }
            else
            {
                string name = "Rocket grunt " + RandomPerson.getName();
                //TODO: chance of pokemon back.
                Console.WriteLine("game.rocket.ambush", name);
                if (game.player.getPartyCount() > 1)
                {
                    int stealchance = rand.Next(100);
                    if (stealchance < 75)
                    {
                        //roll pokemon stolen
                        Pokemon stolenpoke = null;
                        int stolen = game.player.getPartyCount();
                        stolen = rand.Next(stolen);
                        stolenpoke = game.player.getParty()[stolen];
                        game.player.party.Remove(stolenpoke);
                        Console.WriteLine("game.rocket.snatch", name, stolenpoke);
                        Console.WriteLine("game.rocket.chase", name);
                        if (TrainerEvent.trainerBattle(name, 2, rand.Next(game.player.getPartyLevel()) + game.player.cycles,game))
                        {
                            //return poke
                            Console.WriteLine("game.rocket.return", stolenpoke, name);
                            game.player.party.Add(stolenpoke);
                        }
                        else
                        {
                            Console.WriteLine("game.rocket.escape", name);
                            Console.WriteLine("game.rocket.jenny");
                            Console.WriteLine("game.rocket.jenny2", stolenpoke);
                            Console.WriteLine("game.rocket.poster", stolenpoke);
                            Console.WriteLine("game.rocket.defeat");
                            game.player.stolenPokes.Add(stolenpoke);
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("game.rocket.grabfail", name);
                    }
                }
                Console.WriteLine("game.rocket.punch", name);
                Console.WriteLine("game.rocket.drag");
                int candys = rand.Next(5) + 10;
                int cash = rand.Next(400) + 400;
                Console.WriteLine("game.rocket.jenny3", candys, cash);
                game.player.cash += cash;
                game.player.giveOptionToGivePokemonLevels(candys);
            }
            
        }
    }
}
