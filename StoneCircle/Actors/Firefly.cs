using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    class Firefly : Actor
    {
        ActorLightSource lit;
        Actionstate burning = new Burning();

        public Firefly(Vector2 starting, Stage Parent, GameManager gameManager) : base(gameManager)
        {
             name = "fire";
             Location = new Vector3(starting, 0);
             asset_Name = "Characters/ElysiaSheet";
             parent = Parent;
             lit = new ActorLightSource(this, 300f);
             lit.ChangeColor(Color.Green);
             ImageHeight = 80;
             ImageWidth = 80;
             radius = 1;
             height = 1;
             learnAction(new Walk());
             speed = 300;
        }

        public Firefly(uint objectId) : base(objectId) { }

        public override void Initialize()
        {

            current_Action = burning;
            parent.addLight(lit);
            parent.AM.InstantiateEffect("fire", this, true);

        }

        public override void UnInitialize()
        {
            parent.removeLight(lit);
           
        }
 

        public override void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Talking":
                   break;

                case "UseItem":

                   if (parent.player.CurrentItem.HasProperty("Fire") && current_Action.ID !="Burning") { 
                       current_Action = burning; 
                       parent.addLight(lit);
                       gameManager.AudioManager.InstantiateEffect("fire", this, true);
                      }
                   if (parent.player.CurrentItem.HasProperty("Water") && current_Action.id == "Burning") { SetAction("Standing"); parent.removeLight(lit); }

                   break;


                case "Nothing":
                   break;

                default:
                    break;

            }
        }



    }
}
