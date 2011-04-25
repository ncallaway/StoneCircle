using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    class EventGroup
    {
        List<Event> events = new List<Event>();
        public String NextEvent;
        private String id;
        public String ID { get { return id; } }

        public EventGroup(String callID, String nextEvent)
        {
            id = callID;
            NextEvent = nextEvent;
        }

        public EventGroup() { }



        public void Start()
        {
            foreach (Event E in events) E.Start();
        }

        public void AddEvent(Event add)
        {
            events.Add(add);

        }

        public bool Update(GameTime t)
        {

            bool Ready = true;
            foreach (Event E in events) if (!E.Ready) { E.Update(t); Ready = (Ready && E.Ready); }
            return Ready;

        }

    }



    public class Event
    {
        protected String id;
        protected bool ready;
        public bool Ready { get { return ready; } }

        public virtual void Start(){  }

        public virtual void Update(GameTime t){ }


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

        public override void Update(GameTime t)
        {
            if (Stage.input.IsAButtonNewlyPressed()) { ready = true; Stage.RemoveDialogue(dialogueID); }
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

        public override void Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            ready = etime >= time;
        }

    }

    public abstract class PauseEvent : Event
    {
        /* Test event */
    }

    public class AcknowledgePauseEvent : PauseEvent
    {

        private Stage Stage;

        internal AcknowledgePauseEvent(Stage stage) {Stage = stage ; }


        public override void Update(GameTime t)
        {
            if (Stage.input.IsAButtonNewlyPressed()) ready = true;
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

        public override void Update(GameTime t)
        {  
            actor.ActionUpdate(t, Stage.Actors);
            if((destination-actor.Position).LengthSquared() < 50f) ready = true;
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

        public override void Update(GameTime t)
        {
            stage.AMBColor += color * t.ElapsedGameTime.Milliseconds/time;
            stage.AMBStrength += strength * t.ElapsedGameTime.Milliseconds / time;
            etime += t.ElapsedGameTime.Milliseconds;
            ready = (etime >=time);   
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

        public override void Start()       { direction = destination - camera.Location; }

        public override void Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            camera.Pan( direction * (t.ElapsedGameTime.Milliseconds / time));
            if(etime>= time) ready = true;
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
        }


        public override void Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            camera.Zoom( ScaleInterval * t.ElapsedGameTime.Milliseconds / time); 
            ready = (etime>=time);
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

        public override void Update(GameTime t)
        {
            actor.ActionUpdate(t, null);
            etime += t.ElapsedGameTime.Milliseconds;
            ready = true;
        }

    }






}
