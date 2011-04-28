using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle


{

    class EventGroup : Event
    {
        protected List<Event> events = new List<Event>();
        

        public void AddEvent(Event add) { events.Add(add); }

    }

    class ParallelEventGroup: EventGroup
    {
        public ParallelEventGroup(String callID)
        {
            id = callID;
        }

        public ParallelEventGroup() { }



        public override void Start()
        {
            foreach (Event E in events) E.Start();
        }


        public override bool Update(GameTime t)
        {
            bool Ready = true;
            foreach (Event E in events) if (!E.Ready) { E.Update(t); Ready = (Ready && E.Ready); }
            return Ready;

        }

    }

    class SerialEventGroup : EventGroup
    {
        Event currentEvent;
        int index;

        public SerialEventGroup(String ID)
        {
            id = ID;
        }

        public override void Start()
        {
            index = 0;
            currentEvent = events[index];
            currentEvent.Start();
        }

        public override bool Update(GameTime t)
        {
            if (currentEvent.Update(t))
            {
                index++;
                if (index >= events.Count) { ready = true; }
                else { currentEvent = events[index];
                currentEvent.Start();
                }
            }
            return ready;

        }

    }

    public class Event
    {
        protected String id;
        public String ID { get { return id; } }
        protected bool ready;
        public bool Ready { get { return ready; } }



        public virtual void Start(){  }

        public virtual bool Update(GameTime t) { return ready; }

        public virtual void End()
        {
            ready = true;
            
        }
    }

     class StateConditionONEvent : Event
    {
        StageManager SM;
        String StateCondition;

        public StateConditionONEvent(String sc, StageManager sm)
        {
            SM = sm;
            StateCondition = sc;

        }

        public override void Start()
        {
            
        }
    }

     class PlayerDeactivateEvent : Event
    {
        Player player;
        public PlayerDeactivateEvent(Player player)
        { this.player = player; }


        public override void Start()
        {
            player.Active = false;
            ready = true;
        }

    }

    class PlayerReactivateEvent : Event
    {
        
        Player player;
        public PlayerReactivateEvent(Player player)
        { this.player = player; }


        public override void Start()
        {
            player.Active = true;
            ready = true;
        }



    }

        class StageDeactivateEvent : Event
    {
        Stage stage;
        
        public StageDeactivateEvent(Stage stage)
        { this.stage = stage; }


        public override void Start()
        {
            foreach ( Actor A in stage.Actors) A.Active = false;
            ready = true;
        }

    }

     class StageReactivateEvent : Event
    {
        
        Stage stage;
        
        public StageReactivateEvent(Stage stage)
        { this.stage = stage; }


        public override void Start()
        {
            foreach ( Actor A in stage.Actors) A.Active = false;
            ready = true;
        }

    }


     class CameraDeactivateEvent : Event
     {

         Camera camera;

         public CameraDeactivateEvent(Camera camera)
         { this.camera = camera; }


         public override void Start()
         {
             camera.Active = false;
             ready = true;
         }

     }

     class CameraReactivateEvent : Event
     {

         Camera camera;

         public CameraReactivateEvent(Camera camera)
         { this.camera = camera; }


         public override void Start()
         {
             camera.Active = true;
             ready = true;
         }

     }

    public class DialogueEvent : Event
    {

        private String dialogueID;
        private Stage Stage;

        internal DialogueEvent(String callID, Stage stage)
        {
            dialogueID = callID;
            Stage = stage;
        }

        public override void Start()
        {
            Stage.RunLine(dialogueID);
            
        }

        public override bool Update(GameTime t)
        {
            if (Stage.input.IsAButtonNewlyPressed()) { ready = true; Stage.RemoveDialogue(dialogueID); }
            return ready;
        }


    }

    public class DramaticPauseEvent : PauseEvent
    {
        float time;
        float etime;

        public DramaticPauseEvent(float Time) { time = Time; }

        public override void Start()
        {
            etime = 0;
        }

        public override bool Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            ready = etime >= time;
            return ready;
        }

    }

    public abstract class PauseEvent : Event
    {
    }

    class AcknowledgePauseEvent : PauseEvent
    {

        private Stage Stage;

        internal AcknowledgePauseEvent(Stage stage) {Stage = stage ; }


        public override bool Update(GameTime t)
        {
            if (Stage.input.IsAButtonNewlyPressed()) ready = true;
            return Ready;
        }

    }

    class MoveActorEvent : Event
    {
        Actor actor;
        Vector2 destination;
        Stage Stage;

        public MoveActorEvent(Actor Actor, Vector2 Destination, Stage stage)
        {
            destination = Destination;
            actor = Actor;          
            ready = false;
            Stage = stage;
        }

        public override void Start()
        {
           actor.Facing = destination - actor.Position;
           actor.Facing /= actor.Facing.Length();

           actor.SetAction("Walking");
          
        }

        public override bool Update(GameTime t)
        {  
            actor.ActionUpdate(t, Stage.Actors);
            if((destination-actor.Position).LengthSquared() < 50f) ready = true;
            return ready;
        }



    }

    class StageChangeEvent : Event
    {
        StageManager SM;
        String stageName;

        public StageChangeEvent(StageManager sM, String StageName)
        {
            SM = sM;
            stageName = StageName;

        }

        public override void Start()
        {
            SM.SetStage(stageName);
        }



    }

    class SetCameraEvent : Event
    {
        Camera camera;
        Vector2 location;

        public SetCameraEvent(Camera Camera, Vector2 spot)
    {
        camera = Camera;
        location = spot;

    }

        public override void Start()
        {
            camera.Location = location;
            ready = true;
        }

        

    }

    class ChangeAmbient : Event
    {
        Stage stage;
        Vector3 color;

        float strength;
        float time;
        float etime;

        public ChangeAmbient(Stage Stage, Vector3 Color, float Strength, float Time){

            stage = Stage;
            color = Color;
            strength = Strength;
            time = Time;
            etime = 0;
        }

        public ChangeAmbient(Stage Stage, Vector3 Color, float Strength)
        {
            stage = Stage;
            color = Color;
            strength = Strength;
            time = 0;
            etime = 0;
        }



        public override void Start()
        {
            color -= stage.AMBColor;
            strength -= stage.AMBStrength;
        }

        public override bool Update(GameTime t)
        {
            stage.AMBColor += color * t.ElapsedGameTime.Milliseconds/time;
            stage.AMBStrength += strength * t.ElapsedGameTime.Milliseconds / time;
            etime += t.ElapsedGameTime.Milliseconds;
            ready = (etime >=time);
            return ready;
        }



    }

    class MoveCameraEvent : Event
    {
        Camera camera;
        Vector2 destination;
        Vector2 direction;
        float time;
        float etime;

        public MoveCameraEvent(Camera Camera, Vector2 Destination, float Time)
        {
            camera = Camera;
            destination = Destination;
            time = Time;
            etime = 0;
        }

        public override void Start() { camera.Active = false; direction = destination - camera.Location; }

        public override bool Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            camera.Pan( direction * (t.ElapsedGameTime.Milliseconds / time));
            ready = (etime>= time);
            return ready;
        }
    }

    class ScaleCameraEvent: Event
    {
        Camera camera;
        float time;
        float etime;
        float EndScale;
        float ScaleInterval;


        public ScaleCameraEvent(Camera Camera, float Scale, float Time)
        {
            camera = Camera;
            EndScale = Scale;
            time = Time;
        }

        public override void Start()
        {
            ScaleInterval = EndScale - camera.Scale;
            etime = 0;
            camera.Active = false;
        }


        public override bool Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            camera.Zoom( ScaleInterval * t.ElapsedGameTime.Milliseconds / time); 
            ready = (etime>=time);
            return ready;
        }




    }

    class PerformActionEvent : Event
    {
        String action;
        Actor actor;
        float etime;
        float time;

        public PerformActionEvent(Actor Actor, String Action)
        {
            action = Action;
            actor = Actor;
            time = 500f;
            etime = 0f;
        }

        public override void Start()
        {
            actor.SetAction(action);
        }

        public override bool Update(GameTime t)
        {
            actor.ActionUpdate(t, null);
            etime += t.ElapsedGameTime.Milliseconds;
            ready = true;
            return ready;
        }

    }






}
