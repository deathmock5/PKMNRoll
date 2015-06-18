
using DM_LangLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll.Code.gameEvents
{
    class EggEvent : GameEventBase
    {
        public override double GetChance()
        {
            return 10.0; //10%
        }

        public override void trigger()
        {
            Game game = Game.Instance;
            if (game.player.hasegg)
            {
                LangHelper.WriteLine("game.egg.shake"); //"The egg in your backpack shakes violently!"
                int val = rand.Next(10);
                Pokemon eggpoke = null;
                switch (val)
                {
                    case 0:
                        eggpoke = new Pokemon(IDS.TOGEPI, 5, game.player.id);
                        break;
                    case 1:
                        eggpoke = new Pokemon(IDS.TYROGUE, 5, game.player.id);
                        break;
                    case 2:
                        eggpoke = new Pokemon(IDS.CLEFFA, 5, game.player.id);
                        break;
                    case 3:
                        eggpoke = new Pokemon(IDS.ELEKID, 5, game.player.id);
                        break;
                    case 4:
                        eggpoke = new Pokemon(IDS.MAGBY, 5, game.player.id);
                        break;
                    case 5:
                        eggpoke = new Pokemon(IDS.DRATINI, 5, game.player.id);
                        break;
                    case 6:
                        eggpoke = new Pokemon(IDS.LARVITAR, 5, game.player.id);
                        break;
                    case 7:
                        eggpoke = new Pokemon(IDS.GIBLE, 5, game.player.id);
                        break;
                    case 8:
                        eggpoke = new Pokemon(IDS.BAGON, 5, game.player.id);
                        break;
                    case 9:
                        eggpoke = new Pokemon(IDS.EEVEE, 5, game.player.id);
                        break;
                }
                LangHelper.WriteLine("game.egg.reveal", eggpoke.getName()); //~2Theres a cracking noise, and a {0}~2 hatches from the egg
                game.player.addPokemon(eggpoke);
                game.player.hasegg = false;
            }
            else
            {
                LangHelper.WriteLine("game.egg.find"); //On the side of the road, you find a pokemon egg!
                LangHelper.WriteLine("game.egg.bag"); //You place the egg in your backpack.
                game.player.hasegg = true;
            }
        }
    }
}
