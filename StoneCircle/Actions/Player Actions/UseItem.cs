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
    class UseItem: Actionstate
    {
        Item currentItem;
       



        public UseItem()
        {
            id = "UseItem";
            maxFrame = 3;
        }


        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);
            currentItem = Actor.CurrentItem;

           
            Vector3 update = new Vector3(20* Actor.Facing, 0);

            switch (frame)
            {   case 0:

                    break;
                case 1:
                    if (currentItem != null)
                    {

                        EffectBox = new BoundingBox(currentItem.EffectBox.Min + update + Actor.Location, currentItem.EffectBox.Max + update + Actor.Location);
                        foreach (Actor y in targets)
                        {
                            if ((EffectBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                            {
                                y.ApplyAction(this, Actor);
                            }
                        }

                    }
                    break;
                case 2:

                    AvailableLow.NoButton = "Standing";
                    AvailableLow.LStickAction = "Walking";
                    break;
                case 3:
                    break;
                default: break;
            }
                   
            }
            
        } // End of Class


    class BandageSelf : Actionstate
    {
        Actor actor;



        public BandageSelf(Actor Actor)
        {
            actor = Actor;
            maxFrame = 120;
            id = "Bandage Self";
                fatigue = 0;

                AvailableLow.LStickAction = "Walking";
                AvailableHigh.LStickAction = "Running";
                AvailableHigh.YButton = "Standing";
                AvailableLow.YButton = "Resting";
            
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            
            UpdateFrame(t);
            switch (frame)
            {   case 0:
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    break;




                case 119:
                    actor.StopBleeding();
                    AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;        
            }


        }


    }


    class Rest : Actionstate
    {
        public Rest()
        {
            id = "Resting";
            maxFrame = 10;
            fatigue = .75f;
            AvailableHigh.YButton = "Bandage Self";
            AvailableLow.YButton = "Standing";
            AvailableLow.LStickAction = "Walking";
            AvailableHigh.LStickAction = "Running";
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

        }




    }

    }

