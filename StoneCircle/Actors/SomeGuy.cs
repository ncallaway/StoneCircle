using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UserMenus;

namespace StoneCircle
{
    class SomeGuy:Actor
    {
        public SomeGuy(Vector2 starting, Stage Parent, GameManager gameManager) : base(gameManager)
        {
             name = "SomeGuy";
             Location = new Vector3(starting, 0);
             asset_Name = "Actor2";
             parent = Parent;
             
            
        }



        public override void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Interact":
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
