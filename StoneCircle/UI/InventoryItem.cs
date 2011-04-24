using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace UserMenus
{
    public class InventoryItem : MenuItem
    {
        protected List<String> properties = new List<String>();
        protected int quantity;
        public int Quantity { get { return quantity; } }
        protected bool single;
        protected BoundingBox effectBox;
        public BoundingBox EffectBox { get { return effectBox; } }

        public InventoryItem(String ID, String Property)
        {
            id = ID;
            properties.Add(Property);
            single = true;
            quantity = 1;
            effectBox = new BoundingBox(new Vector3(-10, -10, -2), new Vector3(10, 10, 2));
        }

        public InventoryItem(String ID, String Property, int quant)
        {
            id = ID;
            properties.Add(Property);
            quantity = quant;
            single = false;

            effectBox = new BoundingBox(new Vector3(-10, -10, -2), new Vector3(10, 10, 2));
        }

        public void AddQuantity()        {            if(!single)quantity++;        }
        public void AddQuantity(int quant) { if (!single) quantity += quantity; }
        public void RemoveQuantity()         {            if(!single)quantity--;        }
        public bool HasProperty(String Property)        {            return properties.Contains(Property);        }
        public bool DoesNotHaveProperty(String Property)        {            return !HasProperty(Property);        }

       
        public virtual void Update(GameTime t)
        {
            

        }

        public virtual void Start() { }

    }



}
