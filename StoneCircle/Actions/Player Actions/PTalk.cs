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
    class PTalk: PAction
    {
        public PTalk(InputController Input)
        {
            id = "Talking";
            input = Input;
            EffectBox = new BoundingBox(new Vector3(0, 0, 0), new Vector3(60, 60, 80));
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            
        }
    }

    class PInteract : PAction
    {
        public PInteract(InputController Input)
        {
            id = "Interact";
            input = Input;
            EffectBox = new BoundingBox(new Vector3(0, 0, 0), new Vector3(60, 60, 80));
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {

            Vector3 update = new Vector3(20 * Actor.Facing, 0);
            BoundingBox CheckBox = Actor.GetBounds(update);

                foreach (Actor y in targets)
                {
                    if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                    {
                        y.ApplyAction(this, Actor);
                       
                    }

                }
        }



        }
    }

