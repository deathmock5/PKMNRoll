using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRoll
{
    class Game
    {
        private static Game instance;
         
        Random rand = new Random();
        Player me;
        bool ingame;
        int cycles = 0;

        private Game() {
            me = new Player();
        }

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        public void start()
        {
            //if save file
            //Load save file
            //GOTO game
            //else
            newGame();//Welcome to the pokemon world roll
            ingame = true;
            while (ingame)
            {
                Console.WriteLine("\n <Description Of location>");
                me.ListParty();
                Console.WriteLine("1: Adventure!");
                Console.WriteLine("2: Visit Pkmn Center");
                Console.Write("What would you like to do:");

                string inputtxt = Console.ReadLine();
                switch(inputtxt)
                {
                    case "1":
                        doAdventureRoll();
                        break;
                    case "2":
                        doPokemonCenter();
                        break;
                    default:
                        break;
                }
                Console.ReadLine();
            }
        }

        //Events

        private void doPokemonCenter()
        {
            //PC
            //Heal All Pokemon
            //Withdraw?
            //Buy?
            //Trade
            //Select poke from PC
            //Generate tradecode
            //Waint for input tradecode
        }

        private void doAdventureRoll()
        {
            int action = 5;// rand.Next(6);
            switch(action)
            {
                case 0:
                    do_visitLocation();
                    break;
                case 1:
                    do_rareCandy();
                    break;
                case 2:
                    do_wildEncounter();
                    break;
                case 3:
                    do_gymbattle();
                    break;
                case 4:
                    do_teamRocket();
                    break;
                case 5:
                    do_trainer();
                    break;
                case 6:
                    do_eggEvent();
                    break;
                default:
                    break;
            }
            cycles++;
        }

        private void do_eggEvent()
        {
            if(me.hasegg)
            {
                Console.WriteLine("The egg in your backpack shakes violently!");
                int val = rand.Next(10);
                Pokemon eggpoke = null;
                switch(val)
                {
                    case 0:
                        eggpoke = new Pokemon(IDS.TOGEPI, 5, me.id);
                        break;
                    case 1:
                        eggpoke = new Pokemon(IDS.TYROGUE, 5, me.id);
                        break;
                    case 2:
                        eggpoke = new Pokemon(IDS.CLEFFA, 5, me.id);
                        break;
                    case 3:
                        eggpoke = new Pokemon(IDS.ELEKID, 5, me.id);
                        break;
                    case 4:
                        eggpoke = new Pokemon(IDS.MAGBY, 5, me.id);
                        break;
                    case 5:
                        eggpoke = new Pokemon(IDS.DRATINI, 5, me.id);
                        break;
                    case 6:
                        eggpoke = new Pokemon(IDS.LARVITAR, 5, me.id);
                        break;
                    case 7:
                        eggpoke = new Pokemon(IDS.GIBLE, 5, me.id);
                        break;
                    case 8:
                        eggpoke = new Pokemon(IDS.BAGON, 5, me.id);
                        break;
                    case 9:
                        eggpoke = new Pokemon(IDS.EEVEE, 5, me.id);
                        break;
                }
                Console.Write("Theres a cracking noise, and a {0} hatches from the egg!",eggpoke.getName());
                me.addPokemon(eggpoke);
                me.hasegg = false;
            }
            else
            {
                Console.WriteLine("On the side of the road, you find a pokemon egg!");
                Console.WriteLine("You place the egg in your backpack.");
                me.hasegg = true;
            }
        }

        private void do_trainer()
        {
            Console.WriteLine("'Hey you! Stop right there trainer scum!'");
            int numpokes = rand.Next(rand.Next(rand.Next(5))) + 1;
            Pokemon pokeout = null;
            Pokemon pokebtl = null;
            bool won = true;
            while(numpokes >0)
            {
                if(pokebtl == null)
                {
                    pokebtl = getRandomPoke();
                    Console.WriteLine("Trainer sends out: {0}", pokebtl.getName());
                }
                if(pokeout == null)
                {
                    try
                    {
                        pokeout = me.getFirstPokemonInParty();
                    }
                    catch(Exception e)
                    {
                        //They have no pokemon left.
                        do_loose();
                        won = false;
                        break;
                    }
                   Console.WriteLine("GO {0}!",pokeout.getName());
                }
                Console.WriteLine("Your pokemon clash in furious battle!");
                int partylevel = me.getPartyLevel();
                if (partylevel > rand.Next(partylevel) + cycles)
                {
                    pokebtl.faint();
                    numpokes--;
                    pokebtl = null;
                }
                else
                {
                    pokeout.faint();
                    pokeout = null;
                }
            }
            //Battle over.
            if(won)
            {
                Console.WriteLine("You have defeated trainer!.");
                Console.WriteLine("You gain 200$.");
                Console.WriteLine("Trainer hands you 5 rare candys.");
                giveOptionToGivePokemonLevels(5);
            }
            else
            {
                //Loose 100$
            }
        }

        private void do_loose()
        {
            Console.WriteLine("You have no more healty pokemon avaliable to you.");
            Console.WriteLine("Your failure causes you to somehow loose consiousness.");
            Console.WriteLine("Somehow you wake up in a pokemon center.");
        }

        private void do_teamRocket()
        {
            //Team Rocket
            //steal?
            //Battle
            //If win
            //Return back pokemon
            //Distribute levels(15)
            //else
            //Faint
        }

        private void do_gymbattle()
        {
            //GymBattle
            //fightGym(currentgymindex++)
        }

        private void do_wildEncounter()
        {
            Pokemon poke = getRandomPoke();
            Console.WriteLine("A wild {0} has appeared!",poke);
            //Wild poke
            //Roll pkmn
            //Take input
            //if Catch
            //Roll catch chance
            //else
            //Roll kill chance
            //distributeLevels(2).
        }


        private void do_rareCandy()
        {
            //Rare Candy
            Console.WriteLine("You reach a dead end, and for some unimaginable reason you decide to search the ground.");
            Console.WriteLine("You find 15 Rare candys!");
            giveOptionToGivePokemonLevels(15);
        }

        private void do_visitLocation()
        {
            //Visit location
            //OCEAN
            //Forest
            //Ruins
            //Cave
        }

        public void newGame()
        {
            Console.WriteLine("Hello there!");
            Console.WriteLine("Welcome to the world of Pokemon!");
            Console.WriteLine("I will be your guide on this adventure.");
            Console.Write("My name is professer ");
            switch(rand.Next(7))
            {
                case 0:
                    Console.WriteLine("Oak");
                    giveOptionToChooseRandomPokemon(IDS.BULBASAUR, IDS.CHARMANDER, IDS.SQUIRTLE);
                    break;
                case 1:
                    Console.WriteLine("Elm");
                    giveOptionToChooseRandomPokemon(IDS.CHIKORITA, IDS.CYNDAQUIL, IDS.TOTODILE);
                    break;
                case 2:
                    Console.WriteLine("Birch");
                    giveOptionToChooseRandomPokemon(IDS.TREECKO, IDS.TORCHIC, IDS.MUDKIP);
                    break;
                case 3:
                    Console.WriteLine("Rowan");
                    giveOptionToChooseRandomPokemon(IDS.TURTWIG, IDS.CHIMCHAR, IDS.PIPLUP);
                    break;
                case 4:
                    Console.WriteLine("Juniper");
                    giveOptionToChooseRandomPokemon(IDS.SNIVY, IDS.TEPIG, IDS.OSHAWOTT);
                    break;
                case 5:
                    Console.WriteLine("Sycamore");
                    giveOptionToChooseRandomPokemon(IDS.CHESPIN, IDS.FENNEKIN, IDS.FROAKIE);
                    break;
                default:
                    Console.WriteLine("Oak");
                    Console.WriteLine("I Appologise, but it appears that all the other trainers took the pokemon, and all we have left is a pikachu...");
                    giveOptionToChooseRandomPokemon(IDS.PIKACHU);
                    break;
            }

        }

        public void giveOptionToGivePokemonLevels(int amount)
        {
            while(amount > 1)
            {
                me.ListParty();
                Console.Write("Choose a pokemon to give levels to:");
                int pchoice = 0;
                try
                {
                    pchoice = Int32.Parse(Console.ReadLine());
                    Pokemon poke = me.getPartyPokemon(pchoice);
                    Console.Write("Give {0} how many levels (MAX:{1}):", poke.getName(), amount);
                    int lvchoice = Int32.Parse(Console.ReadLine());
                    if(lvchoice <= amount)
                    {
                        poke.gainLvs(lvchoice);
                        amount -= lvchoice;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("You get a text from the professer: 'You can't do that.'");
                    Console.ReadLine();
                }
            }
        }

        private void giveOptionToChooseRandomPokemon(params IDS[] list)
        {
            bool haschosen = false;
            while (haschosen == false)
            {
                Console.WriteLine("You are able to choose from these starting pokemon for your adventure:");
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine(" {0:G}: {1:G}", i, Pokemon.getName(list[i]));
                }

                Console.Write("What is your choice:");
                int input = Console.Read();
                int inputint = 0;
                while (!int.TryParse(Convert.ToChar(input).ToString(), out inputint))
                {
                    Console.Write("What is your choice:");
                }

                if(inputint <= list.Length && inputint >= 0)
                {
                    Console.WriteLine("You point at the {0} are you sure? (y/n)", Pokemon.getName(list[inputint]));
                    input = Console.Read();
                    if(Convert.ToChar(input) == 'y')
                    {
                        haschosen = true;
                    }
                }
                else
                {
                    Console.WriteLine("You point at the table are you sure? (y/n)");
                    input = Console.Read();
                    if (Convert.ToChar(input) == 'y')
                    {
                        Console.WriteLine("Um... the table is not a pokemon, least not yet.");
                    }
                }
                Console.WriteLine("May you and {0} value and cherish eachother forever!",Pokemon.getName(list[inputint]));
            }
        }

        public Pokemon getRandomPoke()
        {
            IDS pokeid = (IDS)rand.Next(720) + 1;
            Pokemon poke = new Pokemon(pokeid,5,me.id);
            if(rand.Next(999) == 69)
            {
                poke.isshiny = true;
            }
            return poke;
        }
    }
}
