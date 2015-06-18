using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokemonRoll.Code.helper;
using DM_LangLib;
using PokemonRoll.Code.gameEvents;

namespace PokemonRoll
{
    class Game
    {
        private static Game instance;
        List<GameEventBase> gameevents = new List<GameEventBase>();
        Random rand = new Random(); 
        public Player player;
        bool ingame;

        private Game()
        {
            player = new Player();
            Console.SetOut(new ColorWriter());
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

        internal void shutdown()
        {
            RandomPerson.stopThread();
            PokeApi.stopThread();
        }

        internal void init()
        {
            LangHelper.printLang();
            RandomPerson.startThread();
            PokeApi.startThread();
            addEvent(new EggEvent());
            addEvent(new GymEvent());
            addEvent(new RareCandyEvent());
            addEvent(new RocketEvent());
            addEvent(new TrainerEvent());
            addEvent(new TravelEvent());
            addEvent(new WildEvent());
        }

        public void newGame()
        {
            player = new Player();
            Console.Write("Welcome to the world of ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Pokemon!");
            Console.ResetColor();
            Console.WriteLine("I will be your guide on this adventure.");
            Console.Write("What is your name?:");
            player.name = Console.ReadLine();
            Console.WriteLine("Hello, {0}!", player.name);
            Console.Write("My name is professer ");
            switch (rand.Next(7))
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

        public void start()
        {
            load();
            ingame = true;
            while (ingame)
            {
                save();
                Console.WriteLine();
                if (player.getPartyLevel() == 0)
                {
                    LangHelper.WriteLine("game.center.hurt"); //~4Your pokemon are injured badly.You heal them before you go out!
                    PokemonCenterEvent.do_PokemonCenter(instance);
                }
                LangHelper.WriteLine("game.menu.adventure");    //	1.	Adventure!
                LangHelper.WriteLine("game.menu.visit");    //	2.	Visit PKMN center
                LangHelper.WriteLine("game.menu.status");    //	3.	Status
                LangHelper.WriteLine("game.menu.exit");    //	4.	Exit
                char inputtxt = FormatQuestion.getCharOptionFromQuestion("game.menu.option");
                Console.WriteLine();
                switch (inputtxt)
                {
                    case '1':
                        doAdventureRoll();
                        break;
                    case '2':
                        PokemonCenterEvent.do_PokemonCenter(instance);
                        break;
                    case '3':
                        player.printStatus();
                        player.printParty();
                        player.printInventory();
                        Console.WriteLine("<Description Of location>");
                        break;
                    case '4':
                        LangHelper.WriteLine("game.info.shutdown");   //Shutting down
                        ingame = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public void addEvent(GameEventBase newevent)
        {
            gameevents.Add(newevent);
        }
        
        private void save()
        {
            CryptographyHelper.SerialiseObjectAndEncript(player, "player.pkrg");
            LangHelper.WriteLine("game.data.saved"); //"~2Your game has been saved!"
        }

        private void load()
        {
            LangHelper.Write("game.data.loading");    //"~2Loading save file..."
            player = (Player)CryptographyHelper.DeserialiseAndDecript("player.pkrg");
            if (player == null)
            {
                LangHelper.WriteLine("game.data.fail");  //~4FAILED!
                if(System.IO.File.Exists("player.pkrg"))
                {
                    LangHelper.WriteLine("game.data.rename");  //~2Renamed Old save to player.pkrg.old
                    System.IO.File.Move(@"player.pkrg", @"player.pkrg.old");
                }
                newGame();
            }
            else
            {
                LangHelper.WriteLine("game.info.welcome", player.name); // welcome back {0}!
            }
        }

        private void doAdventureRoll()
        {
            int chancelevel = 0;
            foreach(GameEventBase ev in gameevents)
            {
                chancelevel = Convert.ToInt32(100 * ev.GetChance());
            }
            int roll = rand.Next(chancelevel);
            foreach (GameEventBase ev in gameevents)
            {
                int cval = Convert.ToInt32(ev.GetChance() * 100);
                if(roll < cval)
                {
                    //Trigger.
                    ev.trigger();
                    break;
                }
                else
                {
                    roll -= cval;
                }
            }
            player.cycles++;
        }   

        //Other
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
                int input = FormatQuestion.getIntOptionFromQuestion("What is your choice:");
                if (input < list.Length && input >= 0)
                {
                    if (FormatQuestion.getBoolOptionFromQuestion("You point at the " + Pokemon.getName(list[input]) + " are you sure?"))
                    {
                        haschosen = true;
                        Console.WriteLine("May you and {0} value and cherish each other forever!", Pokemon.getName(list[input]));
                        player.addPokemon(new Pokemon(list[input], 5, player.id));
                    }
                }
                else
                {
                    if (FormatQuestion.getBoolOptionFromQuestion("You point at the table are you sure?"))
                    {
                        Console.WriteLine("Um... the table is not a pokemon, least not yet.");
                    }
                }

            }
        }

        public static void log(string message)
        {
            //logfile.log
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"logfile.log",true))
            {
                file.WriteLine(message);
            }
        }

    }
}
