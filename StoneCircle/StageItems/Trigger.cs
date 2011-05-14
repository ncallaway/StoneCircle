
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    class Trigger
    {
        protected String id;
        public String ID { get { return id; } }
        protected String target;
        public String Target { get {return target;}}
        protected TriggerCondition tC;
        protected bool active;
        protected bool oneTime;

        

        internal Trigger(String targetID, TriggerCondition TC, bool Avail, bool OneTime)
        {
            target = targetID;
            tC = TC;
            active = Avail;
            oneTime = OneTime;
        }

        public bool CheckCondition()
        {
            return tC.CheckCondition() && active;
        }

        public void UpdateAvailability()
        {
            active = !oneTime;
        }


    }

    class TriggerCondition
    {

        public virtual bool CheckCondition()
        {
            return false;
        }

    }

    class TriggerActorHasProperty : TriggerCondition
    {
        Actor actor;
        String property;

        public TriggerActorHasProperty(Actor Actor, String Property)
        {
            actor = Actor;
            property = Property;
        }


        public override bool CheckCondition()
        {
            return actor.HasProperty(property);
        }


    }

    class TriggerActorHasNotProperty : TriggerCondition
    {
        Actor actor;
        String property;

        public TriggerActorHasNotProperty(Actor Actor, String Property)
        {
            actor = Actor; 
            property = Property;
        }
        

        public override bool CheckCondition()
        {
            return actor.DoesNotHaveProperty(property);
        }


    }

    class TriggerActorEquippedItemHasProperty : TriggerCondition
    {
        Actor actor;
        String property;


        public TriggerActorEquippedItemHasProperty(Actor Actor, String Property)
        {
            actor = Actor; property = Property;

        }

        public override bool CheckCondition()
        {
            return actor.CurrentItem.HasProperty(property);
        }

    }

    class TriggerPlayerBoxCondition : TriggerCondition
    {

        protected CollisionCylinder area;
        protected Player player;

        public TriggerPlayerBoxCondition(CollisionCylinder box, Player Player)
        {
            area = box;
            player = Player;
        }

        public override bool CheckCondition()
        {
            return area.Intersects(player.Bounds);
        }

    }
      
    class TriggerORCondition : TriggerCondition
    {
        TriggerCondition TC1;
        TriggerCondition TC2;

        public TriggerORCondition(TriggerCondition tc1, TriggerCondition tc2)
        {
            TC1 = tc1;
            TC2 = tc2;
        }

        public override bool CheckCondition()
        {
            return (TC1.CheckCondition() || TC2.CheckCondition());
        }

    }

    class TriggerANDCondition : TriggerCondition
    {
        TriggerCondition TC1;
        TriggerCondition TC2;

        public TriggerANDCondition(TriggerCondition tc1, TriggerCondition tc2)
        {
            TC1 = tc1;
            TC2 = tc2;
        }

        public override bool CheckCondition()
        {
            return (TC1.CheckCondition() && TC2.CheckCondition());
        }

    }

    class TriggerNOTCondition : TriggerCondition
    {
        TriggerCondition Condition;

        public TriggerNOTCondition(TriggerCondition TC)
        {
            Condition = TC;
        }

        public override bool CheckCondition()
        {
            return !Condition.CheckCondition();
        }


    }

    class TriggerPlayerInteracting : TriggerCondition
    {
        Actor actor;

        public TriggerPlayerInteracting(Actor Actor)
        {
            actor = Actor;

        }

        public override bool CheckCondition()
        {
            return actor.Interacting;
        }


    }
  class TriggerStateCondition : TriggerCondition
    {
        StageManager SM;
        String Condition;

        public TriggerStateCondition(StageManager SM, String Condition)
        {
            this.SM = SM;
            this.Condition = Condition;

        }

        public override bool CheckCondition()
        {
            return SM.StateConditions.Contains(Condition);
        }



    }


}

