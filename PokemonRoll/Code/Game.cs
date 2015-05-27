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

namespace PokemonRoll
{
    class Game
    {
        private static Game instance;

        Random rand = new Random();
        public Player me;
        bool ingame;
        int cycles = 0;

        BinaryFormatter bformatter = new BinaryFormatter();

        private Game()
        {
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
            RandomPerson.startThread();
            load();
            ingame = true;
            while (ingame)
            {
                save();
                Console.WriteLine();
                if (me.getPartyLevel() == 0)
                {
                    Console.WriteLine("~4Your pokemon are injured badly.You heal them before you go out!");
                    do_PokemonCenter();
                }
                Console.WriteLine("1: Adventure!");
                Console.WriteLine("2: Visit Pkmn Center");
                Console.WriteLine("3: Status");
                int inputtxt = getIntOptionFromQuestion("What would you like to do:");
                Console.WriteLine();
                switch (inputtxt)
                {
                    case 1:
                        doAdventureRoll();
                        break;
                    case 2:
                        do_PokemonCenter();
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
            RandomPerson.stopThread();
        }

        private void save()
        {
            CryptographyHelper.SerialiseObjectAndEncript(me, "player.pkrg");
            Console.WriteLine("~2Your game has been saved!");
        }
        private void load()
        {
            Console.Write("~2Loading save file...");
            me = (Player)CryptographyHelper.DeserialiseAndDecript("player.pkrg");
            if (me == null)
            {
                Console.WriteLine("~4FAILED!");
                newGame();
            }
            else
            {
                Console.WriteLine(" welcome back {0}!", me.name);
            }
        }
        private void doAdventureRoll()
        {
            //500 WildPokemon();
            //650	Trainer();
            //750	gymBattle()
            //800	visitLocation();
            //900	Egg()
            //950	Rare Candy
            //999	Rocket
            Console.WriteLine();
            int action = rand.Next(999);
            log("Adventure:" + action);
            if(action < 500)
            {
                do_wildEncounter();
            }
            else if(action < 650)
            {
                do_trainer();
            }
            else if(action < 750)
            {
                do_gymbattle();
            }
            else if(action < 800)
            {
                do_visitLocation();
            }
            else if(action < 900)
            {
                do_eggEvent();
            }
            else if(action < 950)
            {
                do_rareCandy();
            }
            else
            {
                do_teamRocket();
            }
            cycles++;
        }

        private string getRandomName()
        {
            return RandomPerson.getName() ;
        }

        //Events
        private void do_eggEvent()
        {
            if (me.hasegg)
            {
                Console.WriteLine("The egg in your backpack shakes violently!");
                int val = rand.Next(10);
                Pokemon eggpoke = null;
                switch (val)
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
                Console.WriteLine("~2Theres a cracking noise, and a {0}~2 hatches from the egg!", eggpoke.getName());
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
            Console.WriteLine("'Hey you! Stop right there in the name of senseless battle!'");
            if (trainerBattle(trainername, 1, rand.Next(me.getPartyLevel()) + cycles))
            {
                Console.WriteLine("You gain {0}$.", cash);
                me.cash += cash;
                Console.WriteLine("{0} hands you {1} rare candys.", trainername, 5);
                giveOptionToGivePokemonLevels(5);
            }
            else
            {
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
            string name = "Rocket grunt " + getRandomName();
            Console.WriteLine("You are ambushed by {0}!", name);
            if (me.listofPokes.Count > 1)
            {
                int stealchance = rand.Next(100);
                if (stealchance < 200)
                {
                    //roll pokemon stolen
                    Pokemon stolenpoke = null;
                    int stolen = 5;
                    if(me.listofPokes.Count < 6)
                    {
                        stolen = me.listofPokes.Count;
                    }
                    stolen = rand.Next(stolen);
                    stolenpoke = me.listofPokes[stolen];
                    me.listofPokes[stolen] = null;
                    Console.WriteLine("~4{0} has snached {1}~4!", name, stolenpoke);
                    Console.WriteLine("You chase down {0} and force him to battle.", name);
                    if (trainerBattle(name, 2,rand.Next(me.getPartyLevel()) + cycles))
                    {
                        //return poke
                        Console.WriteLine("~2You take back {0}~2 from {1}!", stolenpoke, name);
                        me.listofPokes[stolen] = stolenpoke;
                    }
                    else
                    {
                        Console.WriteLine("~4{0} gets away!", name);
                        Console.WriteLine("You quickly rush to the police station and inform officer Jenny.");
                        Console.WriteLine("She informs you that you might never see {0} again unfortunently.", stolenpoke);
                        Console.WriteLine("You add {0} missing poster to the wall.", stolenpoke);
                        Console.WriteLine("Sadly, you walk back to town.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("~6{0} tried to snach one of your pokemon but fails.", name);
                }
            }
            Console.WriteLine("You punch {0} in the nose breaking it, they pass out.", name);
            Console.WriteLine("You drag them back to the police station.");
            int candys = rand.Next(5) + 10;
            int cash = rand.Next(400) + 400;
            Console.WriteLine("Officer jenny hads you a reward of {0} rare candys and {1}$", candys, cash);
            me.cash += cash;
            giveOptionToGivePokemonLevels(candys);
        }
        private void do_gymbattle()
        {
            Console.WriteLine("You find a flier requesting trainers of great skill chalenge themselves by running through the pokemon leauge.");
            if (getBoolOptionFromQuestion("Interested?"))
            {
                if (me.badges % 8 == 0)
                {
                    //Posible Elite4
                    if (me.badges / 8 != me.champ)
                    {
                        fightLeauge();
                        return;
                    }
                }
                fightGym();
                return;
            }
            else
            {
                Console.WriteLine("You toss the flier. Proclaming: 'Nope.'");
                return;
            }
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
                            me.addPokemon(poke);
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
                    //TODO: a more realistic battle
                    Console.WriteLine("{0} used tackle!", firstpoke.getName());
                    Console.WriteLine("~2{0}~2 was defeated!", poke.getName());
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
            Console.WriteLine("~2You find 15 Rare candys!");
            giveOptionToGivePokemonLevels(15);
        }
        private void do_visitLocation()
        {
            //Visit location
            //OCEAN
            //Forest
            //Ruins
            //Cave
            Console.WriteLine("TODO: travel.");
        }
        private void do_PokemonCenter()
        {
            Console.WriteLine("Hello! and welcome to the pokemon Center!");
            Console.WriteLine("Let me heal your pokemon for you.");

            List<Pokemon> party = me.getParty();
            foreach (Pokemon poke in party)
            {
                if (poke.fainted)
                {
                    poke.fainted = false;
                    Console.WriteLine(" {0}~2 has been healed!", poke);
                }
            }
            bool useing = true;
            while (useing)
            {
                Console.WriteLine("\n 1: PC(Move pokemon)");
                Console.WriteLine(" 2: Mart");
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
                            Console.WriteLine("~2You purchase {0} {1}s costing {2}$", amount, shopitems[input], cost);
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

            Pokemon poketotrade = me.getFirstPokemonInParty();
            string serial = JsonConvert.SerializeObject(poketotrade);
            Clipboard.SetText(poketotrade.ToString() + '\n' +  serial +'\n' + CryptographyHelper.ComputeHash(serial));
        }

        //Other
        public void newGame()
        {
            me = new Player();
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

        public void giveOptionToGivePokemonLevels(int amount)
        {
            while (amount > 0)
            {
                me.printParty();
                try
                {
                    int input = getIntOptionFromQuestion("Choose a pokemon to give levels to:");
                    Pokemon poke = me.getPartyPokemon(input);
                    input = getIntOptionFromQuestion("Give " + poke.getName() + "~3 how many levels (MAX:" + amount + "):");
                    if(input == 0 || input < 0)
                    {
                        throw new InvalidOperationException("Cant give pokemon 0>= levels.");
                    }
                    else if (input <= amount)
                    {
                        poke.gainLvs(input);
                        amount -= input;
                    }
                    else
                    {
                        poke.gainLvs(amount);
                        amount -= input;
                    }
                }
                catch(Exception e)
                {
                    log(e.Message);
                    Console.WriteLine("You get a text from the professer: 'You can't do that.'");
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
                if (input < list.Length && input >= 0)
                {
                    if (getBoolOptionFromQuestion("You point at the " + Pokemon.getName(list[input]) + " are you sure?"))
                    {
                        haschosen = true;
                        Console.WriteLine("May you and {0} value and cherish each other forever!", Pokemon.getName(list[input]));
                        me.addPokemon(new Pokemon(list[input], 5, me.id));
                    }
                }
                else
                {
                    if (getBoolOptionFromQuestion("You point at the table are you sure?"))
                    {
                        Console.WriteLine("Um... the table is not a pokemon, least not yet.");
                    }
                }

            }
        }

        public Pokemon getRandomPoke()
        {
            IDS pokeid = (IDS)rand.Next(720) + 1;
            Pokemon poke = new Pokemon(pokeid, cycles + 5, me.id);
            if (rand.Next(999) == 69)
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
            Console.Write("~3{0}", question);
            int outint = 0;
            string output = "";
            while (!Int32.TryParse(output, out outint))
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
            question = question.Replace("~6", "");
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

        public bool getBoolOptionFromQuestion(string question)
        {
            bool valid = false;
            bool output = false;
            while(!valid)
            {
                char option = getCharOptionFromQuestion(question + "(y/n):");
                if(option == 'Y')
                {
                    output = true;
                    valid = true;
                }
                else if( option == 'N')
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid.");
                }
            }
            return output;
        }

        public bool trainerBattle(string trainername, int dificultyMod,int offset)
        {
            int numpokes = rand.Next(rand.Next(rand.Next(5))) + 1;
            Pokemon pokeout = null;
            Pokemon pokebtl = null;
            bool first = true;
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
                        if(first)
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
                Console.WriteLine("~3{0}~3 and ~3{1}~3 clash in furious battle!",pokeout.getName(),pokebtl.getName());
                int partylevel = me.getPartyLevel();
                int difficulty = rand.Next(Convert.ToInt32(me.getPartyLevel() * (0.22 * dificultyMod)) + (50 * (me.badges * me.badges))) + offset;
                log("PartyLv: " + partylevel + " difficulty:" +difficulty);
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
                    if (me.getRevive())
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


        private bool fightLeauge()
        {
            string name = "";
            Console.WriteLine("You make your way to the pokemon leauge!");
            //5x pokemon
            int pokes = 5;
            while (pokes-- > 0)
            {
                do_wildEncounter();
            }
            Console.WriteLine("After a long and arduess journey you make it to the leauge!");
            do_PokemonCenter();
            int members = 0;
            while (members++ < 4)
            {
                Console.WriteLine("You step through the leauge doors, and they slam behind you");
                name = "Elite 4 member " + Enum.GetName(typeof(Elite4), (me.champ + 1) * members);
                Console.WriteLine("You are chalenged by {0}!", name);
                if (!trainerBattle(name, members, 1000 * (me.champ + 1)))
                {
                    //Defeat
                    return false;
                }
            }
            //Beat the leauge!
            Console.WriteLine("TODO: <Champion>");
            Console.WriteLine("You have conqured the pokemon leauge!");
            me.champ++;
            return true;
        }
        private bool fightGym()
        {
            string name = "";
            int cash = 0;
            int candys = 0;
            Console.WriteLine("You make your way to the nearest gym!");
            Console.WriteLine("You then arrive and are imedeatly ambushed by trainers!");
            int trainers = 1;
            while (trainers++ != 4)
            {
                name = "Gym asistant " + getRandomName();
                cash = rand.Next(100 * trainers) + 100;
                candys = rand.Next(4) + trainers;
                if (trainerBattle(name, trainers,100))
                {
                    //Won
                    Console.WriteLine("You gain {0}$.", cash);
                    me.cash += cash;
                    Console.WriteLine("{0} hands you {1} rare candys.", name, candys);
                    giveOptionToGivePokemonLevels(candys);
                }
                else
                {
                    //Lost
                    cash = Convert.ToInt32(cash * 0.5);
                    Console.WriteLine("You loose {0}$.", cash);
                    me.cash -= cash;
                    return false;
                }
            }
            //Beat the trainers, now for leader!
            name = Enum.GetName(typeof(GymLeader), me.badges);
            cash = rand.Next(1000) + 100 * me.badges;
            candys = rand.Next(10) + me.badges + 1;
            Console.WriteLine("You are chalenged by gym leader {0}!", name);
            if (trainerBattle(name, me.badges,100 * (me.badges + 1)))
            {
                //Won!
                Console.WriteLine("You gain {0}$.", cash);
                me.cash += cash;
                Console.WriteLine("{0} hands you {1} rare candys.", name, candys);
                giveOptionToGivePokemonLevels(candys);
                Console.WriteLine("{0} also hands you a badge, which you pin to your clothing.", name);
                me.badges++;
                return true;
            }
            Console.WriteLine("~4You loose consiousness due to your defeat as {0} cackles with laughter.", name);
            Console.WriteLine("You wake up later in the pokemon center, with the bitter taste of defeat in your mouth.");
            return false;
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
