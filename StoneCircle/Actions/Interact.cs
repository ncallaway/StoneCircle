﻿using System;
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
            
            
            maxFrame = 2;
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

                case 1:


                    AvailableLow.LStickAction = "Walking";
                    AvailableLow.NoButton = "Standing";
                    AvailableHigh.LStickAction = "Running";
                    AvailableHigh.NoButton = "Standing";
                    break;
            }


            Vector3 update = new Vector3(30 * Actor.Facing, 0);
            CollisionCylinder CheckBox = new CollisionCylinder(Actor.Location + update, 50f, 50f); 

                foreach (Actor y in targets)
                {
                    if ((CheckBox.IntersectsType(y.Bounds) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                    {
                        y.Interacting = true;
                       
                    }

                }
        }



        }
    }

