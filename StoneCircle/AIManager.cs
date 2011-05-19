using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoneCircle
{
    struct AIOption
    {
        public AICondition condition;
       public AIAction action;

      }

    class AICondition
    {
        protected Actor actor;
        public Actor Actor { set { actor = value; } }

        public bool Condition { get {return CheckCondition();}}

        protected virtual bool CheckCondition()
    {
        return true;
    }

    }

    class CloseToLocation : AICondition
    {
        Vector2 Location;

        public CloseToLocation(Vector2 location, Actor Actor)
        {
            actor = Actor; Location = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = Location - actor.Position;
            float dist = temp.LengthSquared();
            return (dist <= 400);

        }

    }

    class MidToLocation : AICondition
    {
        Vector2 Location;

          public MidToLocation(Vector2 location, Actor Actor)
        {
            actor = Actor; Location = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = Location - actor.Position;
            float dist = temp.LengthSquared();
            return (dist > 400 && dist < 25000);

        }

    }

    class FarToLocation : AICondition
    {
        Vector2 Location;

          public FarToLocation(Vector2 location, Actor Actor)
        {
            actor = Actor; Location = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = Location - actor.Position;
            float dist = temp.LengthSquared();
            return (dist >= 25000);

        }

    }

    class CloseToActor : AICondition
    {
        Actor target;

          public CloseToActor(Actor location, Actor Actor)
        {
            actor = Actor; target = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = target.Position - actor.Position;
            float dist = temp.LengthSquared();
            return (dist <= 400);

        }

    }

    class MidToActor : AICondition
    {
        Actor target;

        public MidToActor(Actor location, Actor Actor)
        {
            actor = Actor; target = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = target.Position - actor.Position;
            float dist = temp.LengthSquared();
            return (dist > 400 && dist < 250000);

        }

    }

    class FarToActor : AICondition
    {
        Actor target;

        public FarToActor(Actor location, Actor Actor)
        {
            actor = Actor; target = location;
        }

        protected override bool CheckCondition()
        {
            Vector2 temp = target.Position - actor.Position;
            float dist = temp.LengthSquared();
            return (dist >= 250000);

        }

    }
    
    class AIAction
    {
        protected Actor actor;
        public virtual String ActionReturn ()
        {
            return "Standing";
        }


    }

    class WalkToLocation:AIAction
    {
        Vector2 Location;

        public WalkToLocation(Actor Actor, Vector2 location)
        {
            actor = Actor; Location = location;

        }

        public override string ActionReturn()
        {
            Vector2 temp = (Location - actor.Position);
            temp.Normalize();
            actor.Facing = temp;

            return "Walking";

        }


    }

    class WalkToActor : AIAction
    {
        Actor target;


           public WalkToActor(Actor Actor, Actor location)
        {
            actor = Actor; target = location;

        }
        public override string ActionReturn()
        {   Vector2  temp =(target.Position - actor.Position);
        temp.Normalize();
        actor.Facing = temp;

            return "Walking";

        }


    }




















    class AIManager
    {
        public Dictionary<String, AIProfile> Profiles = new Dictionary<String, AIProfile>();

        public AIManager()
        { 
             
        
        }

    }



    class AIProfile
    {
        public Dictionary<String, int> ActionPriorities = new Dictionary<string, int>();

        public AIProfile() { }

        public virtual Vector2 UpdateFacing(Vector3 location)
        {
            return Vector2.Zero;
        }
        
    }


    class AIPMoveToActor: AIProfile
    {
        public Actor target;

        public AIPMoveToActor(Actor target)
        {
            this.target = target;
            ActionPriorities.Add("Walking", 15);
        }

        public override Vector2 UpdateFacing(Vector3 location)
        {
            return new Vector2 (location.X - target.Location.Y, location.Y - target.Location.Y);
        }



    }

    class AIPFullRandom : AIProfile
    {
        public AIPFullRandom()
        {
            ActionPriorities.Add("Walking", 1);
            ActionPriorities.Add("Standing", 1);
            ActionPriorities.Add("Running", 1);
            ActionPriorities.Add("Stand Up", 1);
            ActionPriorities.Add("Fall Backward", 1);
            ActionPriorities.Add("Fall Forward", 1);
            ActionPriorities.Add("Jumping", 1);
            ActionPriorities.Add("Limping", 1);
            ActionPriorities.Add("HighBlock", 1);
            ActionPriorities.Add("MidBlock", 1);
            ActionPriorities.Add("LowBlock", 1);
            ActionPriorities.Add("HighHorizontalSlash", 1);
            ActionPriorities.Add("Thrust", 1);
            ActionPriorities.Add("LowHorizontalSlash", 1);
            ActionPriorities.Add("HorizontalSlash", 1);
            ActionPriorities.Add("Dash", 1);

        }

        public override Vector2 UpdateFacing(Vector3 location)
        {
          return Vector2.UnitX;
        }

    }

}
