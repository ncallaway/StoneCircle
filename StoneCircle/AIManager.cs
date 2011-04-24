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



}
