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

    class PWalk : PAction
    {

        public PWalk(BoundingBox effect)
        {
            EffectBox = effect;
            id = "Walking";
        } 

        public PWalk(InputController Input)
        {
            input = Input;
            EffectBox = new BoundingBox(new Vector3(0, 0, 0), new Vector3(40, 40, 80));
            id = "Walking";
            maxFrame = 3;
        }

        public override void Update(GameTime t)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    break;
                case 1: Actor.ImageXindex = 0; Actor.ImageYindex = 1;
                    break;
                case 2: Actor.ImageXindex = 0; Actor.ImageYindex = 1;
                    break;
                default: Actor.ImageYindex = 0;
                    break;
            }
            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed * 2 * new Vector3(Actor.Facing, 0);
            Ray updateRay = new Ray(Actor.Location, update);
            BoundingBox CheckBox = Actor.GetBounds(update);
            Actor.Move(update);            
        }

        /*
        public override Actionstate Update(Player player, Dictionary<String,Actor>.ValueCollection targets, GameTime t)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    player.ImageXindex = 0; player.ImageYindex = 0;
                    break;
                case 1: player.ImageXindex = 0; player.ImageYindex = 1;
                    break;
                case 2: player.ImageXindex = 0; player.ImageYindex = 1;
                    break;
                default: player.ImageYindex = 0;
                    break;
            }
            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * player.Speed * 2 * new Vector3(player.Facing, 0);
            Ray updateRay = new Ray(player.Location, update);
            BoundingBox CheckBox = player.GetBounds(update);
            Boolean illegal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !player.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { illegal = false; }
            }


            if (illegal) player.Move(update);
           // if (input.IsAButtonNewlyPressed()) return player.interact;
           // if (input.IsXButtonNewlyPressed()) return player.useItem;
           // if(input.IsMoveUpPressed() || input.IsMoveDownPressed() || input.IsMoveLeftPressed() || input.IsMoveRightPressed()) return this;
            
            

           // else  return player.standing;
            return this;
        }
        */



    }

}
