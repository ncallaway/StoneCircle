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
    class Actionstate
    {

        public CollisionCylinder EffectBox { get; set; }
        public string id;
        public String ID { get { return id; } }
        private List<String> properties;
        public List<String> Properties { get { return properties; } }
        protected float fatigue;
        public float Fatigue { get { return fatigue; } }

        public struct ActionList
        {

            public String XButton;
            public String YButton;
            public String AButton;
            public String BButton;
            public String LStickAction;
            public String NoButton;
           
        }

        public Actor Actor;

        protected int frame;
        public int Frame { get { return frame; } set { frame = value; } }
        protected int maxFrame;
        private float time;
        public ActionList AvailableLow;
        public ActionList AvailableHigh;
       
       
    public Actionstate()
{
    maxFrame = 3;
    AvailableLow = new ActionList();
    AvailableHigh = new ActionList();
    fatigue = 0f;
    AvailableLow.LStickAction = "Walking";
    AvailableHigh.LStickAction = "Running";
    AvailableLow.YButton = "Resting";
    AvailableHigh.YButton = "Bandage Self";
}

    public Actionstate(string newId)
    {
        id = newId;

        maxFrame = 3;
        AvailableLow.LStickAction = "Walking";
        AvailableHigh.LStickAction = "Running";
        AvailableLow.YButton = "Resting";
        AvailableHigh.YButton = "Bandage Self";
        
    }


    public virtual void Update(GameTime t)
    {
        
    }

    public virtual void Reset()
    {
        frame = 0;
        time = 0;
    }

    public virtual void Update(Actor actor){}

    public void UpdateFrame(GameTime t)
    {
        time += t.ElapsedGameTime.Milliseconds;
        if (time >= 17) { frame++; time = 0; }
        frame %= maxFrame;
    }

    public virtual void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
    {

    }


    }



    class Attack : Actionstate
    {   
        protected Item weapon;
        protected CollisionArc hitBox;
        public CollisionArc HitBox { get { return hitBox;}}
        protected float attackLength;
        protected Vector2 direction;
    }

    class HighHorizontal : Attack
    {
        private float knockback;

        public HighHorizontal(Actor Actor)
        {
            id = "High Horizontal Swing";
            //weapon = Weapon;
            maxFrame = 10;
            attackLength = 35;
            AvailableLow.LStickAction = null;
            AvailableHigh.LStickAction = null;
            AvailableLow.YButton = null;
            AvailableHigh.YButton = null;
            AvailableHigh.NoButton = null;
            AvailableLow.NoButton = null;
            knockback = 15;
           hitBox = new CollisionArc( Vector3.Zero, 0f, 40f,
                   0f, (float)Math.PI / 6);
            hitBox.Radius = 0;
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            switch (frame)
            {    case 0:   AvailableLow.LStickAction = null;
                    AvailableHigh.LStickAction = null;
                    AvailableLow.YButton = null;
                    AvailableHigh.YButton = null;
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    direction = Actor.Facing;
                    break;
            case 5: hitBox.Location = Actor.Location; hitBox.Radius = Actor.Radius + attackLength + 5; hitBox.CenterAngle = (float)Math.Atan2(Actor.Facing.Y, Actor.Facing.X); break;

            case 4: hitBox.Location = Actor.Location; hitBox.Radius = Actor.Radius + attackLength; hitBox.CenterAngle = (float)Math.Atan2(Actor.Facing.Y, Actor.Facing.X); break;
            case 6: hitBox.Radius = 0; break;
                case 9: 
                    AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;
            }

            Actor.UpdateFacing(direction);

            foreach (Actor y in targets)
            {
                if ((HitBox.IntersectsType(y.Bounds) && Actor != y)) //Collision Detection. Ideally reduces movement to outside collision bounds.
                {
                    // Combat TestGoes Here::
                    y.AttackResponse(this);
                    y.UpdateVector -= (Actor.Location - y.Location);
                    y.Move();

                    Actor.SetAction("MidBlock");
                }

            }



        }










    }

    class HighBlock : Actionstate
    {
        Vector2 direction;

        public HighBlock()
        {
            id = "HighBlock";
            maxFrame = 15;

        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            switch (frame)
            {
                case 0: Actor.ImageXindex = 1;
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    direction = Actor.Facing;

                    Actor.UpdateFacing(Vector2.One);
                    break;



                case 14: AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;



            }

            Actor.UpdateFacing(direction);
            
        }
    }



    class MidBlock : Actionstate
    {
        public MidBlock()
        {
            id = "MidBlock";
            maxFrame = 35;

            AvailableHigh.NoButton = "";
            AvailableLow.NoButton = "";

        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            AvailableHigh.NoButton = "";
            AvailableLow.NoButton = "";
            switch (frame)
            {
                case 0: Actor.ImageXindex = 1;

                    Actor.UpdateFacing(Vector2.One);
                    break;
                case 10: break;


                case 34: AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;
                default: break;


            }

        }
    }



    class LowBlock : Actionstate
    {
        public LowBlock()
        {
            id = "LowBlock";
            maxFrame = 15;

        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            UpdateFrame(t);

            switch (frame)
            {
                case 0: Actor.ImageXindex = 1;
                    AvailableHigh.NoButton = null;
                    AvailableLow.NoButton = null;
                    Actor.UpdateFacing(Vector2.One);
                    break;



                case 14: AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;



            }
            
        }




    }



}