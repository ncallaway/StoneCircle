using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserMenus;

namespace StoneCircle
{
    class Item: Actor
    {
        InventoryItem item;


        public override void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Interact":
                    affector.Inventory.AddItem(item);
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
}
