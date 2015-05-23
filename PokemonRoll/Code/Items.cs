using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PokemonRoll
{
    //public enum ItemID : int
    //{
    //    POKEBALL = 1,
    //    GREATBALL = 2,
    //    ULTRABALL = 3,
    //    MASTERBALL = 4,
    //    REVIVE = 5,
    //    RARECANDY = 6
    //}

    [Serializable()]
    public sealed class ItemID : ISerializable
    {
        public readonly string _name;
        public readonly int _cost;

        //Members
        public static ItemID pokeball = new ItemID("Pokeball", 100);
        public static ItemID greatball = new ItemID("Greatball", 200);
        public static ItemID ultraball = new ItemID("Ultraball", 300);
        public static ItemID masterball = new ItemID("Masterball", -1);
        public static ItemID revive = new ItemID("Revive", 800);
        public static ItemID rare_candy = new ItemID("Rare candy",1000);

        //operators
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            ItemID p = obj as ItemID;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (_name == p._name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ItemID c1, ItemID c2)
        {
            if (System.Object.ReferenceEquals(c1, c2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return c1._name == c2._name;
        }

        public static bool operator !=(ItemID c1, ItemID c2)
        {
            return !(c1 == c2);
        }

        public bool Equals(ItemID p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (_name == p._name);
        }

        //methods
        public ItemID(string p1, int p2)
        {
            this._name = p1;
            this._cost = p2;
        }
        public override string ToString()
        {
            return _name;
        }

        //Serialisation
        public ItemID(SerializationInfo info, StreamingContext ctxt)
        {
            _name = (string)info.GetValue("itemID_name", typeof(string));
            _cost = (int)info.GetValue("itemID_cost", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("itemID_name",_name);
            info.AddValue("itemID_cost",_cost);
        }
    }

    [Serializable()]
    class Item : ISerializable
    {
        public ItemID id = null;
        public int amount = 0;

        public Item(ItemID _id,int _amount)
        {
            id = _id;
            amount = _amount;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Item p = obj as Item;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (id == p.id);
        }

        public bool Equals(Item p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (id == p.id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        } 

        public static bool operator ==(Item c1, Item c2)
        {
            if (System.Object.ReferenceEquals(c1, c2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return c1.id == c2.id;
        }

        public static bool operator !=(Item c1, Item c2)
        {
            return !(c1 == c2);
        }



        public Item(SerializationInfo info, StreamingContext ctxt)
        {
            id = (ItemID)info.GetValue("item_id", typeof(ItemID));
            amount = (int)info.GetValue("item_amount", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("item_id", id);
            info.AddValue("item_amount", amount);
        }
    }
}
