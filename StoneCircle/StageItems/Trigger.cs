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

    class ActorHasProperty : TriggerCondition
    {
        Actor actor;
        String property;

        public override bool CheckCondition()
        {
            return actor.HasProperty(property);
        }


    }

    class TriggerPlayerBoxCondition : TriggerCondition
    {

        protected BoundingBox area;
        protected Player player;

        public TriggerPlayerBoxCondition(BoundingBox box, Player Player)
        {
            area = box;
            player = Player;
        }

        public override bool CheckCondition()
        {
            return area.Intersects(player.GetBounds());
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

    class TriggerANDCondtion : TriggerCondition
    {
        TriggerCondition TC1;
        TriggerCondition TC2;

        public TriggerANDCondtion(TriggerCondition tc1, TriggerCondition tc2)
        {
            TC1 = tc1;
            TC2 = tc2;
        }

        public override bool CheckCondition()
        {
            return (TC1.CheckCondition() && TC2.CheckCondition());
        }

    }

}

