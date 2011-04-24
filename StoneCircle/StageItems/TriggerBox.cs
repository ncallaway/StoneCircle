using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
  class TriggerBox
    {
        protected BoundingBox area;
        protected bool active;
        protected Stage stage;


        public void Update(GameTime t, Actor player)
        {
            if ( area.Intersects(player.GetBounds()) && active) execute();
        }

        public virtual void execute()
        {


        }


    }

    class DialogueTrigger : TriggerBox
    {
        private String callID;
        private bool oneTime;

        public DialogueTrigger(String CallID, Vector3 min, Vector3 max, Stage Stage)
        {
            callID = CallID;
            area = new BoundingBox(min, max);
            active = true;
            oneTime = true;
            stage = Stage;
        }

        public override void execute()
        {

            stage.RunLine(callID);
            if (oneTime) active = false;

        }

    }
   class StageTrigger : TriggerBox
    {
        private String callID;
        private Vector2 coords;


        public StageTrigger(String CallID, Vector3 min, Vector3 max, Stage Stage)
        {
            callID = CallID;
            area = new BoundingBox(min, max);
            active = true;
            stage = Stage;
            coords = new Vector2(150, 600);
        }

        public StageTrigger(String CallID, Vector3 min, Vector3 max, Stage Stage, Vector2 Coords)
        {
            callID = CallID;
            area = new BoundingBox(min, max);
            active = true;
            stage = Stage;
            coords = Coords;
        }

        public override void execute()
        {

            //stage.SM.SetStage(callID);
            stage.SM.SetStage(callID, coords);

        }

    }


     class EventTrigger : TriggerBox
    {
        String eventID;

        public EventTrigger(String EventID, Stage Stage, Vector3 BBmin, Vector3 BBmax)
        {
            eventID = EventID;
            stage = Stage;
            area = new BoundingBox(BBmin, BBmax);
            active = true;

        }


        public virtual void Execute()
        {
            stage.RunEvent(eventID);
            active = false;
        }




    }
}
