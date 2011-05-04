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
   
    class Interact : Actionstate
    {
        public Interact()
        {
            id = "Interact";
            
            EffectBox = new BoundingBox(new Vector3(0, 0, 0), new Vector3(60, 60, 80));
            maxFrame = 10;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {

            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    AvailableLow.LStickAction = null;
                    AvailableLow.NoButton = null;
                    AvailableHigh.LStickAction = null;
                    break;

                case 9:


                    AvailableLow.LStickAction = "Walking";
                    AvailableLow.NoButton = "Standing";
                    AvailableHigh.LStickAction = "Running";
                    AvailableHigh.NoButton = "Standing";
                    break;
            }


            Vector3 update = new Vector3(30 * Actor.Facing, 0);
            BoundingBox CheckBox = Actor.GetBounds(update);

                foreach (Actor y in targets)
                {
                    if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                    {
                        y.CheckTriggers();
                       
                    }

                }
        }



        }
    }

