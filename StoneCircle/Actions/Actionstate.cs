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




    class CombatAction : Actionstate
    {
       // CollisionCylinder DefenseBox;



        public virtual void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            

        }





    





    }


}