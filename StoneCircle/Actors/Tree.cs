using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StoneCircle
{
    class Tree:SetProp
    {
        Actionstate burning = new Burning();
        ActorLightSource onFire;

        public Tree(Vector2 starting, Stage Parent, GameManager gameManager) : base(gameManager)
        {
             name = "Tree";
             Location = new Vector3(starting, 0);
             asset_Name = "tree3";
             parent = Parent;
            onFire = new ActorLightSource(this, 700f);
            ImageHeight = 194;
            ImageWidth = 162;
            radius = 40;
            height = 300f;
            
        }

        public Tree(GameManager gameManager, Vector2 starting)
            : base(gameManager)
        {
            name = "Tree";
            asset_Name = "tree3";
            Location = new Vector3(starting, 0);
            
            onFire = new ActorLightSource(this, 700f);
            ImageHeight = 194;
            ImageWidth = 162;
            radius = 40;
            height = 300f;
            
        }

        public override void loadImage(ContentManager theContentManager)
        {
            base.loadImage(theContentManager);
            origin = new Vector2(81, 164);
        }

        public Tree(uint objectId) : base(objectId) { }


        public override void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Talking":
                   break;

                case "UseItem":

                   if (parent.player.CurrentItem.HasProperty("Fire")) { 
                       current_Action = burning; 
                       parent.addLight(onFire);
                       parent.AM.InstantiateEffect("fire", this, true);
                      }
                   if (parent.player.CurrentItem.HasProperty("Water") && current_Action.id == "Burning") { SetAction("Standing"); parent.removeLight(onFire); }

                   break;


                case "Nothing":
                   break;

                default:
                    break;

            }
        }

    }

    class Shack : SetProp
    {

         public Shack(GameManager gameManager, Vector2 starting)
            : base(gameManager)
        {
            name = "Shack";
            asset_Name = "Shack";
            Location = new Vector3(starting, 0);
            
            ImageHeight = 194;
            ImageWidth = 162;
            radius = 125;
            height = 100f;
            
        }

     public override void loadImage(ContentManager theContentManager)
        {
            base.loadImage(theContentManager);
            origin = new Vector2(150, 200);
        }


    }
}
