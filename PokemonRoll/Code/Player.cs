using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PokemonRoll
{
    [Serializable()]
    class Player : ISerializable
    {
        public string name = "null";
        public TrainerID id = new TrainerID();
        public List<Pokemon> listofPokes = new List<Pokemon>(); //0-5 are party. else are pc.
        public List<Item> listofitems = new List<Item>();
        public bool hasegg = false;
        public int badges = 0;
        public int champ = 0;
        public int cash = 0;
        
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
            Console.WriteLine("Current Party:");
            int i = 0;
            foreach(Pokemon poke in getParty())
            {
                if(poke.fainted)
                {
                    Console.Write("~4FNT~8");
                }
                Console.WriteLine(" {0}: {1}", i++, poke);
            }
        }
        public void printCash()
        {
            Console.WriteLine("You curently have {0}.", cash);
        }
        public void printInventory()
        {
            Console.WriteLine("Item In Bag:");
            foreach(Item item in listofitems)
            {
                if(item.amount == 0)
                {
                    //prune it.
                }
                else
                {
                    Console.WriteLine(" {0}x{1}", item.amount, item.id);
                }
            }
        }
        public void printStatus()
        {
            Console.WriteLine("Name: {0} ,badges: {1}, Cash: {2}, Dex: {3}", name, badges, cash, getDex());
        }

        public List<Pokemon> getParty()
        {
            List<Pokemon> pokes = new List<Pokemon>();
            int maxsize = 6;
            if (listofPokes.Count < 6)
            {
                maxsize = listofPokes.Count;
            }
            for (int i = 0; i < maxsize; i++)
            {
                pokes.Add(listofPokes[i]);
            }
            return pokes;
        }

        public Pokemon getPartyPokemon(int pchoice)
        {
            if(pchoice >= 6 || pchoice >= listofPokes.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return listofPokes[pchoice];
        }

        internal void addPokemon(Pokemon eggpoke)
        {
            if(listofPokes.Count >=6)
            {
                Console.WriteLine("{0} was transported to the pc.",eggpoke);
            }
            listofPokes.Add(eggpoke);
        }

        public Pokemon getFirstPokemonInParty()
        {
            foreach(Pokemon poke in getParty())
            {
                if(!poke.fainted)
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
                if (!poke.fainted)
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
            //TODO: this.
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

    }
}
