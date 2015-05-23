using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        BinaryFormatter bformatter = new BinaryFormatter();

        private Game() {
            me = new Player();
            Console.SetOut(new PKMNWriter());
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
            load();
            ingame = true;
            while (ingame)
            {
                save();
                Console.WriteLine();
                Console.WriteLine("1: Adventure!");
                Console.WriteLine("2: Visit Pkmn Center");
                Console.WriteLine("3: Status");
                int inputtxt = getIntOptionFromQuestion("What would you like to do:");
                Console.WriteLine();
                switch(inputtxt)
                {
                    case 1:
                        doAdventureRoll();
                        break;
                    case 2:
                        doPokemonCenter();
                        break;
                    case 3:
                        me.printStatus();
                        me.printParty();
                        me.printInventory();
                        Console.WriteLine("<Description Of location>");
                        break;
                    default:
                        break;
                }
            }
        }

        private void save()
        {
            Stream stream = File.Open("player.pkrg", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, me);
            stream.Close();
            Console.WriteLine("~2Your game has been saved!");
        }
        private void load()
        {
            Stream stream = null;
            try
            {
                Console.Write("~2Loading safe file...");
                stream = File.Open("player.pkrg", FileMode.Open);
                me = (Player)bformatter.Deserialize(stream);
                Console.WriteLine(" welcome back {0}!", me.name);
            }
            catch
            {
                Console.WriteLine("~4Load failed");
                newGame();//Welcome to the pokemon world roll
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        private void doAdventureRoll()
        {
            Console.WriteLine();
            int action = 2;// rand.Next(6);
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

        private string getRandomName()
        {
            return "nobsy";
        }

        //Events
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
            string trainername = getRandomName();
            int cash = rand.Next(200) + 100;
            if (trainerBattle(trainername,cash,2))
            {
                Console.WriteLine("You have defeated {0}!.", trainername);
                Console.WriteLine("You gain {0}$.",cash);
                Console.WriteLine("{0} hands you {1} rare candys.", trainername,5);
                giveOptionToGivePokemonLevels(5);
            }
            else
            {
                Console.WriteLine("You have been defeated by {0}!.", trainername);
                cash = Convert.ToInt32(me.cash * 0.1);
                Console.WriteLine("You loose {0}$ by loosing!.", cash);
                me.cash -= cash;
                do_loose();
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
            bool wildpokealive = true;
            Pokemon poke = getRandomPoke();
            Console.WriteLine("A wild {0} has appeared!", poke);
            Pokemon firstpoke = me.getFirstPokemonInParty();
            Console.WriteLine("GO {0}!", firstpoke);

            while (wildpokealive)
            {
                Console.WriteLine();
                Console.WriteLine(" 0: Catch");
                Console.WriteLine(" 1: Battle");
                int input = getIntOptionFromQuestion("What do you want to do?:");
                
                if (input == 0)
                {
                    //Catch
                    int catchchance = me.getPokeballFromBackpack();
                    if(catchchance == 0)
                    {
                        Console.WriteLine("~4You dont have any Pokeballs to use!");
                    }
                    else
                    {
                        int catchroll = rand.Next(100);
                        if (catchroll < catchchance)
                        {
                            Console.WriteLine("Ding...\n Ding...\n Ding...\n CLICK!");
                            Console.WriteLine("~2Wild {0} was caught!",poke.getName());
                            me.addPokemon(poke);
                            wildpokealive = false;
                        }
                        else
                        {
                            for(int i =0; i< rand.Next(3);i++)
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
                    //TODO: a more realistic battle
                    Console.WriteLine("{0} used tackle!", firstpoke.getName());
                    Console.WriteLine("{0} was defeated!", poke.getName());
                    Console.WriteLine("You find 2 rare candy on its body.");
                    giveOptionToGivePokemonLevels(2);
                    wildpokealive = false;
                }
            }
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
        private void doPokemonCenter()
        {
            Console.WriteLine("Hello! and welcome to the pokemon Center!");
            Console.WriteLine("Let me heal your pokemon for you.");

            List<Pokemon> party = me.getParty();
            foreach (Pokemon poke in party)
            {
                if (poke.fainted)
                {
                    poke.fainted = false;
                    Console.WriteLine("{0} has been healed!", poke);
                }
            }
            bool useing = true;
            while (useing)
            {
                Console.WriteLine("\n 1: PC(Move pokemon)");
                Console.WriteLine(" 2: Store");
                Console.WriteLine(" 3: Trade");
                Console.WriteLine(" 4: Leave");
                int input = getIntOptionFromQuestion("Im nurse joy, how can I help you?:");
                switch (input)
                {
                    case 1:
                        do_PokemonCenter_PC();
                        break;
                    case 2:
                        do_PokemonCenter_Mart();
                        break;
                    case 3:
                        do_PokemonCenter_Link();
                        break;
                    case 4:
                        useing = false;
                        Console.WriteLine("Have a nice day!");
                        break;
                }
            }
        }
        private void do_PokemonCenter_PC()
        {
            //Withdraw?
            Console.WriteLine("Booted up pc.");
            Console.WriteLine("Accessing pokemon storage system.");
            Console.WriteLine("TODO:");
        }
        private void do_PokemonCenter_Mart()
        {
            bool inuse = true;
            while (inuse)
            {
                List<ItemID> shopitems = new List<ItemID>();
                shopitems.Add(ItemID.pokeball);
                shopitems.Add(ItemID.greatball);
                shopitems.Add(ItemID.ultraball);
                shopitems.Add(ItemID.revive);
                shopitems.Add(ItemID.rare_candy);
                me.printCash();
                for (int i = 0; i < shopitems.Count; i++)
                {
                    Console.WriteLine(" {0}: {1}    -   {2}$", i, shopitems[i]._name, shopitems[i]._cost);
                }
                Console.WriteLine(" {0}:  Leave", shopitems.Count);
                int input = getIntOptionFromQuestion("What items do you need?:");
                int amount = 0;
                int cost = 0;
                if (input != shopitems.Count && input < shopitems.Count)
                {
                    amount = getIntOptionFromQuestion("How many do you need (0-9)?:");
                    if (amount > 0)
                    {
                        cost = shopitems[input]._cost * amount;
                        if (me.cash >= cost)
                        {
                            Item itemtoadd = new Item(shopitems[input], amount);
                            Console.WriteLine("You purchase {0} {1}s costing {2}$", amount, shopitems[input], cost);
                            if (itemtoadd.id == ItemID.rare_candy)
                            {
                                giveOptionToGivePokemonLevels(1);
                            }
                            else
                            {
                                me.addItems(itemtoadd);
                                me.cash -= cost;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You cant aford {0} {1}s that costs {2} and you only have {3}", amount, shopitems[input], cost, me.cash);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nurse joy wonders how you buy {0} {1}s.", amount, shopitems[input]);
                    }
                }
                else
                {
                    inuse = false;
                }
            }
        }
        private void do_PokemonCenter_Link()
        {
            //Trade
            //Select poke from PC
            //Generate tradecode
            //Waint for input tradecode
        }

        //Other
        public void newGame()
        {
            Console.Write("Welcome to the world of ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Pokemon!");
            Console.ResetColor();
            Console.WriteLine("I will be your guide on this adventure.");
            Console.Write("What is your name?:");
            me.name = Console.ReadLine();
            Console.WriteLine("Hello, {0}!", me.name);
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
                me.printParty();
                try
                {
                    int input = getIntOptionFromQuestion("Choose a pokemon to give levels to:");
                    Pokemon poke = me.getPartyPokemon(input);
                    input = getIntOptionFromQuestion("Give " + poke.getName()+ " how many levels (MAX:" + amount + "):");
                    if(input <= amount)
                    {
                        poke.gainLvs(input);
                        amount -= input;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch
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
                int input = getIntOptionFromQuestion("What is your choice:");
                char cinput = '0';
                if(input < list.Length && input >= 0)
                {
                    cinput = getCharOptionFromQuestion("You point at the " + Pokemon.getName(list[input]) + " are you sure? (y/n):");
                    if(Convert.ToChar(cinput) == 'Y')
                    {
                        haschosen = true;
                        Console.WriteLine("May you and {0} value and cherish each other forever!", Pokemon.getName(list[input]));
                        me.addPokemon(new Pokemon(list[input], 5, me.id));
                    }
                }
                else
                {
                    cinput = getCharOptionFromQuestion("You point at the table are you sure? (y/n):");
                    if (input == 'Y')
                    {
                        Console.WriteLine("Um... the table is not a pokemon, least not yet.");
                    }
                }
                
            }
        }

        public Pokemon getRandomPoke()
        {
            IDS pokeid = (IDS)rand.Next(720) + 1;
            Pokemon poke = new Pokemon(pokeid,cycles + 5,me.id);
            if(rand.Next(999) == 69)
            {
                poke.isshiny = true;
            }
            return poke;
        }

        public string getStringOptionFromQuestion(string question)
        {
            Console.Write("~3{0}", question);
            string input = Console.ReadLine();
            return input;
        }

        public int getIntOptionFromQuestion(string question)
        {
            ConsoleKeyInfo input;
            Console.Write("~3{0}",question);
            int outint = 0;
            string output = "";
            while(!Int32.TryParse(output,out outint))
            {
                input = Console.ReadKey();
                output = input.Key.ToString();
                if (output.Length > 1)
                {
                    output = input.Key.ToString().Substring(1);
                }
            }
            Console.WriteLine(outint);
            return outint;
        }

        public char getCharOptionFromQuestion(string question)
        {
            ConsoleKeyInfo input;
            Console.Write("~3{0}", question);
            input = Console.ReadKey();
            string output = input.Key.ToString();
            if (output.Length > 1)
            {
                output = input.Key.ToString().Substring(1);
            }
            Console.WriteLine(output);
            return output[0];
        }

        public bool trainerBattle(string trainername,int cash,int rarecandys)
        {
            Console.WriteLine("'Hey you! Stop right there in the name of senseless battle!'");
            int numpokes = rand.Next(rand.Next(rand.Next(5))) + 1;
            Pokemon pokeout = null;
            Pokemon pokebtl = null;
            while (numpokes > 0)
            {
                if (pokebtl == null)
                {
                    pokebtl = getRandomPoke();
                    Console.WriteLine("{1} sends out: {0}", pokebtl.getName(), trainername);
                }
                if (pokeout == null)
                {
                    try
                    {
                        pokeout = me.getFirstPokemonInParty();
                    }
                    catch
                    {
                        //yoy have no pokemon left.
                        return false;
                    }
                    Console.WriteLine("GO {0}!", pokeout.getName());
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
            return true;
        }
    }
}
