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
        public int Frame { get { return frame; } }
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




    class UpperDefense : Actionstate { }

    class LowerDefense : Actionstate { }

    class Attack : Actionstate
    {
        protected Item weapon;
       // protected CollisionArc HitBox;
        protected CollisionCylinder HitBox;
        protected float attackLength;
    }

    class HighHorizontal : Attack
    {

        public HighHorizontal(Actor Actor)
        {
            id = "High Horizontal Swing";
            //weapon = Weapon;
            maxFrame = 10;
            attackLength = 15;
            AvailableLow.LStickAction = null;
            AvailableHigh.LStickAction = null;
            AvailableLow.YButton = null;
            AvailableHigh.YButton = null;
            AvailableHigh.NoButton = null;
            AvailableLow.NoButton = null;
            this.Actor = Actor;
            //HitBox = new CollisionArc(new Vector3(Actor.Location.X, Actor.Location.Y, 0), Actor.ImageWidth / 2 + attackLength, 200f,
                  // (float)Math.Atan2(Actor.Facing.Y, Actor.Facing.X), (float)Math.PI / 20);
           HitBox = new CollisionCylinder(new Vector3(Actor.Location.X, Actor.Location.Y, 0), Actor.ImageWidth / 2 + attackLength, 200f);
            HitBox.Radius = 0;
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
                    break;
            case 5: HitBox.Location = Actor.Location;  HitBox.Radius = Actor.ImageWidth / 2 + attackLength + 500; break;
                case 6: HitBox.Radius = 0; break;
                case 9: AvailableHigh.NoButton = "Standing";
                    AvailableLow.NoButton = "Standing";
                    break;
            }

            foreach (Actor y in targets)
            {
                if ((HitBox.Intersects(y.Bounds) && !Actor.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                {
                    y.SetAction("Fall Forward");
                    y.Move(new Vector3(100, 100, 25));
                }

            }



        }










    }

}