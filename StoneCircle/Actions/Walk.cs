using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace StoneCircle
{
    class Walk : Actionstate
    {

        

        public Walk()
        {   
            id = "Walking";
            AvailableLow.LStickAction = "Walking";
            AvailableLow.NoButton = "Standing";
            AvailableLow.XButton = "UseItem";
            AvailableLow.AButton = "Interact";
            AvailableHigh.AButton = "Jump";
            AvailableHigh.LStickAction = "Running";
            AvailableLow.BButton = "Fall Down";
            fatigue = +.1f;
        }


        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                case 1: Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                case 2: Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                default: Actor.ImageYindex = 0;



                    break;
            }


            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed *  new Vector3(Actor.Facing, 0);
            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);
            

        }



    }

    class Limp : Actionstate
    {

        public Limp()
        {
            id = "Limping";
            AvailableLow.NoButton = "Standing";
            AvailableLow.XButton = "UseItem";
            AvailableLow.AButton = "Interact";
            maxFrame = 10;
        }




        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed *  new Vector3(Actor.Facing, 0);
           

            switch (frame)
            {
                case 0:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4;

                    break;
                case 1: Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4;

                    break;
                case 2: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 4;

                    break;

                case 3: Actor.ImageXindex = 1; update /= 4;
                    break;

                case 4:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 5:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 6:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 7:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 8:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 9:
                    Actor.ImageXindex = 1; Actor.ImageYindex = 0;
                    update /= 4; break;
                case 10: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 2; break;
                case 11: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 2; break;
                case 12: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 2; break;
                case 13: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 2; break;
                case 14: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    update /= 2; break;



                default: Actor.ImageYindex = 0;



                    break;
            }


            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);


        }



    }

    class Jump : Actionstate
    {
        Vector2 direction;

        public Jump()
        {
            id = "Jump";
            maxFrame = 16;


        }



        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            float updateZ =0;
            UpdateFrame(t);
            
            switch (frame)
            {
                case 0:
                    fatigue = -2f;
                    AvailableLow.NoButton = null;
                    AvailableLow.LStickAction = null;
                    AvailableHigh.NoButton = null;
                    AvailableHigh.LStickAction = null;
                    Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    updateZ = 7;
                    direction = Actor.Facing;
                    break;
                case 1:
                    fatigue = 0f;
                    Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    updateZ = 4;

                    break;
                case 2: Actor.ImageXindex = 0; Actor.ImageYindex = 0;
                    updateZ = 4;
                    break;
                case 3:
                    updateZ = 2;
                    break;
                case 4:
                    updateZ = 2;
                    break;
                case 5:
                    updateZ = 2;
                    break;
                case 6:
                    updateZ = 1;
                    break;
                case 7: updateZ = 1;
                    break;
                case 8: updateZ = 1; break;
                case 9: updateZ = 1; break;
                case 10: updateZ = 0; break;
                case 11: updateZ = 0; break;
                case 12: updateZ = 0; break;
                case 13: break;
                case 14:
                    Random rand = new Random();
                    if (rand.Next(2) < 1) AvailableHigh.NoButton = "Fall Down";
                    else
                    {
                        AvailableLow.NoButton = "Standing";
                        AvailableLow.LStickAction = "Walking";
                        AvailableHigh.LStickAction = "Running";
                        AvailableHigh.NoButton = "Standing";
                    }
                    break;
                default:
                                                            break;
            }


            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed *2.5f * new Vector3(direction, updateZ);
            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);

        }









    }

    class Run : Actionstate
    {

        public Run()
        {
            id = "Running";
            AvailableLow.LStickAction = "Walking";
            AvailableLow.NoButton = "Standing";
            AvailableLow.XButton = "UseItem";
            AvailableLow.AButton = "Interact";
            AvailableHigh.AButton = "Jump";
            AvailableHigh.LStickAction = "Running";
            AvailableHigh.NoButton = "Standing";
            fatigue = -.25f;
        }




        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {

            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                case 1: Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                case 2: Actor.ImageXindex = 0; Actor.ImageYindex = 0;


                    break;
                default: Actor.ImageYindex = 0;



                    break;
            }


            Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed * 2.5f * new Vector3(Actor.Facing, 0);
            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);


        }

       





    }


    class Prone : Actionstate
    {
        public Prone()
        {


            id = "Prone";
            AvailableHigh.AButton = "Stand Up";
            AvailableLow.AButton = "Stand Up";
            AvailableLow.NoButton = "Prone";
            AvailableHigh.NoButton = "Prone";
            fatigue = +.5f;
        }

    }


    class StandUp : Actionstate
    {
        public StandUp()
        {
            id = "Stand Up";
            maxFrame = 30;
            fatigue = -.1f;
        }

        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0: AvailableLow.NoButton = null;
                    AvailableHigh.NoButton = null;break ;
                case 1: break;

                case 29:
                    AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;
            }
        }

    }

    class FallForward : Actionstate
    {
        public FallForward()
        {
            maxFrame = 14;
            id = "Fall Down";
        }

        


        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    fatigue = 0;
                    AvailableLow.NoButton = null;
                    AvailableHigh.NoButton = null;
                    AvailableHigh.AButton = null;
                    break;
                case 13:
                    fatigue = -5;
                    AvailableHigh.AButton = "Roll";
                    AvailableHigh.NoButton = "Prone";
                    AvailableLow.NoButton = "Prone";
                    break;
            }


        }


    }

    class FallBackward : Actionstate
    {
        public FallBackward()
        {
            maxFrame = 14;
            id = "Fall Down";
        }




        public override void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);
            switch (frame)
            {
                case 0:
                    fatigue = 0;
                    AvailableLow.NoButton = null;
                    AvailableHigh.NoButton = null;
                    AvailableHigh.AButton = null;
                    break;
                case 13:
                    fatigue = -5;
                    AvailableHigh.AButton = "Roll";
                    AvailableHigh.NoButton = "Prone";
                    AvailableLow.NoButton = "Prone";
                    break;
            }


        }


    }

    class Dash : Actionstate
    {
        float updateAmount;
        Vector2 direction;


        public Dash()
        {
            id = "Dash";
            maxFrame = 15;
            updateAmount = 4;
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            switch (frame)
            {
                case 0:
                    updateAmount = 6f;
                    fatigue = -.5f;
                    direction = Actor.Facing;


                    AvailableHigh.LStickAction = null;
                    AvailableLow.LStickAction = null;
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    break;

                case 1: fatigue = 0f; break;
                case 2: fatigue = 0f; break;
                case 4:
                    updateAmount = 0f;
                    break;
                

                case 14:
                    
                    AvailableHigh.LStickAction = "DashJump";
                    AvailableLow.LStickAction = "Dash";
                    AvailableHigh.NoButton = "FightStance";
                    AvailableLow.NoButton = "FightStance";
                    break;

            }

         Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed *updateAmount * new Vector3(direction, 0);
            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);

        }

    }


    class FightStance : Actionstate
    {

        public FightStance()
        {
            id = "FightStance";
            maxFrame = 13;
            AvailableHigh.LStickAction = "DashJump";
            AvailableLow.LStickAction = "Dash";
            AvailableHigh.NoButton = "FightStance";
            AvailableLow.NoButton = "FightStance";
            fatigue = .05f;

        }


  
    }


    class DashJump : Actionstate
    {  float updateAmount;
        Vector2 direction;
        float updateZ;

        public DashJump()
        {
            id = "DashJump";
            maxFrame = 13;

        }


            

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            switch (frame)
            {
                case 0:
                    updateAmount = 0f;
                    fatigue = -1f;
                    direction = Actor.Facing;

                    AvailableHigh.LStickAction = null;
                    AvailableLow.LStickAction = null;
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    updateZ = 3f;

                    break;

                case 1: fatigue = -3f; updateAmount = 9f; break;
                case 2: fatigue = 0f; break;
                case 5:
                    updateAmount = 2f; updateZ = 0;
                    break;
                case 8:
                    updateAmount = 0f;
                    break;

                case 12:
                    
                    AvailableHigh.LStickAction = "DashJump";
                    AvailableLow.LStickAction = "Dash";
                    AvailableHigh.NoButton = "FightStance";
                    AvailableLow.NoButton = "FightStance";
                    break;

            }

         Vector3 update = (float)t.ElapsedGameTime.TotalSeconds * Actor.Speed * new Vector3(direction* updateAmount, updateZ);
            BoundingBox CheckBox = Actor.GetBounds(update);
            bool legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                { legal = false; }
            }


            if (legal) Actor.Move(update);

        }

    }


    






    }
        

