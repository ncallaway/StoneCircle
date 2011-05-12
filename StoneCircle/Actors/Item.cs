using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserMenus;

using Microsoft.Xna.Framework;



namespace StoneCircle
{
    class Item
    {
        public BoundingBox EffectBox;
        protected Actor owner;
        protected List<String> properties = new List<String>();
        protected String name;
        public String Name { get { return name; } }
        protected String iconName;
        public String IconName { get { return iconName; } }
        public InventoryItem II;

        public Item()
        {
            name = "Empty";
            iconName = "BlankIcon";
            II = new InventoryItem(this);
        }

       

        public Item(String Name, String iconName)
        {
            name = Name;
            this.iconName = iconName;
            II = new InventoryItem(this);
        }

        public void SetOwner(Actor Owner) { owner = Owner; }
        public bool HasProperty(String Property) { return properties.Contains(Property); }
        public bool DoesNotHaveProperty(String Property) { return !HasProperty(Property); }

        public void ActionUpdate()
        {

        }

        public virtual void OnEquipItem()
        {

        }

        public virtual void OnUnequipItem()
        {

        }

        public virtual void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            
        }

        public virtual void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Interact":
                   // affector.Inventory.AddItem(this);
                    break;

                case "UseItem":
                    break;
                
                case "Nothing":
                    break;

                default:
                    break;

            }
        }




    }



    class Lantern : Item
    {
        ActorLightSource light;

        public Lantern(Actor Owner)
        {
            owner = Owner;
            light = new ActorLightSource(Owner, 1000f);
            properties.Add("Fire");
            properties.Add("Light");
            name = "Lantern";
            II = new InventoryItem(this);
            
        }



        public override void OnEquipItem()
        {
            owner.parent.addLight(light);
        }

        public override void OnUnequipItem()
        {

            owner.parent.removeLight(light);
        }

       

    }


    class Sword : Item
    {
          public Sword(Actor Owner)
        {
            owner = Owner;
            name = "Sword";
            II = new InventoryItem(this);
            
        }


          public override void OnEquipItem()
          {
              owner.SetAction("FightStance");
          }

          public override void OnUnequipItem()
          {
              owner.SetAction("Standing");
          }






    }
}
