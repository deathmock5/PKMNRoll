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
         List<Pokemon> listofPokes; //0-5 are party. else are pc.
        Random rand = new Random();
        OriginalTrainer me;
        bool ingame;

        private Game() {
            listofPokes = new List<Pokemon>();
            me = new OriginalTrainer();
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
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1: Adventure!");
                Console.WriteLine("2: Visit Pkmn Center");

                string inputtxt = Console.ReadLine();
                switch(inputtxt)
                {
                    case "1":
                        doAdventureRoll();
                        break;
                    case "2":
                        //PC
                        //Heal All Pokemon
                        //Withdraw?
                        //Buy?
                        //Trade
                        //Select poke from PC
                        //Generate tradecode
                        //Waint for input tradecode
                        break;
                    default:
                        break;
                }
                
                
                
                Console.ReadLine();
            }
        }

        private void doAdventureRoll()
        {
            int action = rand.Next(6);
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
        }

        private void do_eggEvent()
        {
            //Egg
            //Have egg?
            //Hatch
            //else
            //Give egg
        }

        private void do_trainer()
        {
            //Trainer
            //Battle bla bla bla
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
            Console.WriteLine("Welcome to the world of Pokemon!");
            Console.WriteLine("I will be your guide on this adventure.");
            Console.Write("My name is professer ");
            switch(rand.Next(6))
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
                    Console.WriteLine("I Appologise, but it appears that all the other trainers, took the pokemon, and all we have left is a pikachu.");
                    giveOptionToChooseRandomPokemon(IDS.PIKACHU);
                    break;

            }

        }

        private void giveOptionToChooseRandomPokemon(params IDS[] list)
        {
            bool optionValid = false;
            while (optionValid == false)
            {
                Console.WriteLine("You are able to choose from these starting pokemon for your adventure:");
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine(" {0:G}: {1:G}", i, getPokemonName(list[i]));
                }

                Console.Write("What is your choice:");
                string input = Console.ReadLine();
                int inputint = Int32.Parse(input);
                if(inputint <= list.Length && inputint >= 0)
                {
                    listofPokes.Add(new Pokemon(list[inputint],5,me));
                    optionValid = true;
                }
                Console.WriteLine("May you and {0} value and cherish eachother forever!",getPokemonName(list[inputint]));
            }
        }

        private string getPokemonName(IDS id)
        {
            return Enum.GetName(typeof(IDS), id);;
        }
    }
}
