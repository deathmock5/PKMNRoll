using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DM_LangLib;

namespace PokemonRoll
{
    [Serializable()]
    class Player : ISerializable
    {
        public string name = "null";
        public TrainerID id = new TrainerID();
        public List<Pokemon> listofPokes = new List<Pokemon>(); //Pokemon in pc.
        public List<Pokemon> party = new List<Pokemon>();
        public List<Pokemon> stolenPokes = new List<Pokemon>();
        public List<Item> listofitems = new List<Item>();
        public bool hasegg = false;
        public int badges = 0;
        public int champ = 0;
        public int cash = 0;
        public int cycles = 0;
        
        public Player()
        {
            //Give starting loadout
            cash = 200;
            listofitems.Add(new Item(ItemID.pokeball, 10));
            listofitems.Add(new Item(ItemID.revive, 2));
        }

        //Output prints:
        public void printParty()
        {
            Console.WriteLine("~5Current Party:");
            int i = 0;
            foreach(Pokemon poke in getParty())
            {
                if(poke != null)
                {
                    if (poke.fainted)
                    {
                        Console.Write("~4FNT~8");
                    }
                    Console.WriteLine(" ~F{0}: {1}", i, poke);
                }
                i++;
            }
        }
        public void printCash()
        {
            Console.WriteLine("You curently have {0}$.", cash);
        }
        public void printInventory()
        {
            Console.WriteLine("~1Item In Bag:");
            foreach(Item item in listofitems)
            {
                if(item.amount == 0)
                {
                    //prune it.
                }
                else
                {
                    Console.WriteLine(" ~F{0}x{1}", item.amount, item.id);
                }
            }
        }
        public void printStatus()
        {
            Console.WriteLine("Name: {0} ,badges: {1}, Cash: {2}, Dex: {3}", name, badges, cash, getDex());
        }
        public void printContentsOfBox(int box)
        {
            int col = 0;
            Console.WriteLine("Box {0}", box);
            for (int i = 1; i <= 30; i++)
            {
                if (col++ == 5)
                {
                    col = 1;
                    Console.WriteLine();
                }
                Pokemon poke = getPCPokemon(i * box);
                if (poke != null)
                {
                    Console.Write(" {0}:{1} ", i, poke.getName());
                }
            }
            Console.WriteLine();
        }

        public List<Pokemon> getParty()
        {
            return party;
        }

        public int getPartyCount()
        {
            int returnint = 0;

            foreach(Pokemon poke in party)
            {
                if (poke != null)
                {
                    returnint++;
                }
            }
            return returnint;
        }

        public Pokemon getPartyPokemon(int pchoice)
        {
            if(pchoice >= 6 || pchoice >= party.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return party[pchoice];
        }
        public Pokemon getPCPokemon(int pchoice)
        {
            if (pchoice >= listofPokes.Count)
            {
                return null;
            }
            return listofPokes[pchoice];
        }
        internal void addPokemon(Pokemon eggpoke)
        {
            if(getPartyCount() < 6)
            {
                //Add some pokemon to the party!
                party.Add(eggpoke);
                Console.WriteLine("{0} was added to your party.",eggpoke);
            }
            else
            {
                listofPokes.Add(eggpoke);
                Console.WriteLine("{0} was transported to the pc.", eggpoke);
            }
        }

        public Pokemon getFirstPokemonInParty()
        {
            foreach(Pokemon poke in getParty())
            {
                if(poke != null && !poke.fainted)
                {
                    return poke;
                }
            }
            throw new NotSupportedException();
        }

        internal int getPartyLevel()
        {
            int level = 0;
            foreach (Pokemon poke in getParty())
            {
                if (poke != null && !poke.fainted)
                {
                    level += poke._level;
                }
            }
            return level;
        }

        public int getPokeballFromBackpack()
        {
            Item ball = getItemIndexFromInventory(ItemID.masterball);
            if(ball != null)
            {
                if(ball.amount > 0)
                {
                    ball.amount -= 1;
                    return 100;
                }
            }
            ball = getItemIndexFromInventory(ItemID.ultraball);
            if (ball != null)
            {
                if (ball.amount > 0)
                {
                    ball.amount -= 1;
                    return 75;
                }
            }
            ball = getItemIndexFromInventory(ItemID.greatball);
            if (ball != null)
            {
                if (ball.amount > 0)
                {
                    ball.amount -= 1;
                    return 50;
                }
            }
            ball = getItemIndexFromInventory(ItemID.pokeball);
            if (ball != null)
            {
                if (ball.amount > 0)
                {
                    ball.amount -= 1;
                    return 25;
                }
            }
           return 0;
        }

        public Item getItemIndexFromInventory(ItemID id)
        {
            foreach(Item item in listofitems)
            {
                if(item.id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public int getDex()
        {
            //TODO: Get dex.
            return -1;
        }

        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            name = (string)info.GetValue("player_name", typeof(string));
            id = (TrainerID)info.GetValue("player_id", typeof(TrainerID));
            listofPokes = (List<Pokemon>)info.GetValue("player_pokes", typeof(List<Pokemon>));
            hasegg = (bool)info.GetValue("player_hasegg", typeof(bool));
            badges = (int)info.GetValue("player_badges", typeof(int));
            cash = (int)info.GetValue("player_cash", typeof(int));
            listofitems = (List<Item>)info.GetValue("player_inventory", typeof(List<Item>));
            champ = (int)info.GetValue("player_champ", typeof(int));
            party = (List<Pokemon>)info.GetValue("player_party",typeof(List<Pokemon>));
            stolenPokes = (List<Pokemon>)info.GetValue("player_stolen", typeof(List<Pokemon>));
        }
        
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("player_name",name);
            info.AddValue("player_id", id);
            info.AddValue("player_pokes", listofPokes);
            info.AddValue("player_inventory", listofitems);
            info.AddValue("player_hasegg",hasegg);
            info.AddValue("player_badges", badges);
            info.AddValue("player_cash", cash);
            info.AddValue("player_champ", champ);
            info.AddValue("player_party", party);
            info.AddValue("player_stolen", stolenPokes);
        }

        internal void addItems(Item itemtoadd)
        {
            foreach(Item invintory in listofitems)
            {
                if(invintory.id == itemtoadd.id)
                {
                    invintory.amount += itemtoadd.amount;
                    return;
                }
            }
            listofitems.Add(itemtoadd);
        }

        internal bool getRevive()
        {
            Item revive = getItemIndexFromInventory(ItemID.revive);
            if (revive != null)
            {
                if (revive.amount > 0)
                {
                    revive.amount -= 1;
                    return true;
                }
            }
            return false;
        }

        public void giveOptionToGivePokemonLevels(int amount)
        {
            while (amount > 0)
            {
                printParty();
                try
                {
                    int input = FormatQuestion.getIntOptionFromQuestion("Choose a pokemon to give levels to:");
                    Pokemon poke = getPartyPokemon(input);
                    input = FormatQuestion.getIntOptionFromQuestion("Give " + poke.getName() + "~3 how many levels (MAX:" + amount + "):");
                    if (input == 0 || input < 0)
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
                catch (Exception e)
                {
                    Game.log(e.Message);
                    Console.WriteLine("You get a text from the professer: 'You can't do that.'");
                }
            }
        }

    }
}
