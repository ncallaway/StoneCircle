using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{

    class EVENTGroup : EVENT
    {
        protected List<EVENT> EVENTs = new List<EVENT>();


        public void AddEVENT(EVENT add) { EVENTs.Add(add); }

        public override void Reset()
        {
            ready = false;
            foreach (EVENT E in EVENTs) E.Reset();
        }
        
    }

    class ParallelEVENTGroup : EVENTGroup
    {
        public ParallelEVENTGroup(String callID)
        {
            id = callID;
        }

        public ParallelEVENTGroup() { }



        public override void Start()
        {
            foreach (EVENT E in EVENTs) E.Start();
        }


        public override bool Update(GameTime t)
        {
            bool Ready = true;
            foreach (EVENT E in EVENTs) if (!E.Ready) { E.Update(t); Ready = (Ready && E.Ready); }
            return Ready;

        }

    }

    class SerialEVENTGroup : EVENTGroup
    {
        EVENT currentEVENT;
        int index;

        public SerialEVENTGroup(String ID)
        {
            id = ID;
        }

        public override void Start()
        {
            index = 0;
            currentEVENT = EVENTs[index];
            currentEVENT.Start();
            Reset();
        }

        public override bool Update(GameTime t)
        {
            if (currentEVENT.Update(t))
            {
                index++;
                if (index >= EVENTs.Count) { ready = true; }
                else
                {
                    currentEVENT = EVENTs[index];
                    currentEVENT.Start();
                }
            }
            return ready;

        }

        public override void End()
        {
            foreach (EVENT E in EVENTs) E.End();
        }

    }

    public class EVENT
    {
        protected String id;
        public String ID { get { return id; } }
        protected bool ready;
        public bool Ready { get { return ready; } }



        public virtual void Start() { }

        public virtual bool Update(GameTime t) { return ready; }

        public virtual void End()
        {
            ready = true;

        }

        public virtual void Reset()
        {
            ready = false;
        }

    }

    class EVENTStateConditionON : EVENT
    {
        StageManager SM;
        String StateCondition;

        public EVENTStateConditionON(String sc, StageManager sm)
        {
            SM = sm;
            StateCondition = sc;

        }

        public override void Start()
        {
            SM.SetCondition(StateCondition);
            ready = true;
        }
    }

    class EVENTPlayerDeactivate : EVENT
    {
        Player player;
        public EVENTPlayerDeactivate(Player player)
        { this.player = player; }


        public override void Start()
        {
            player.Active = false;
            ready = true;
        }

    }

    class EVENTPlayerReactivate : EVENT
    {

        Player player;
        public EVENTPlayerReactivate(Player player)
        { this.player = player; }


        public override void Start()
        {
            player.Active = true;
            ready = true;
        }



    }

    class EVENTStageDeactivate : EVENT
    {
        Stage stage;

        public EVENTStageDeactivate(Stage stage)
        { this.stage = stage; }


        public override void Start()
        {
            foreach (Actor A in stage.Actors) A.Active = false;
            ready = true;
        }

    }

    public class EVENTStageReactivate : EVENT
    {

        Stage stage;

        internal EVENTStageReactivate(Stage stage)
        { this.stage = stage; }


        public override void Start()
        {
            foreach (Actor A in stage.Actors) A.Active = false;
            ready = true;
        }

    }

    public class EVENTCameraDeactivate : EVENT
    {

        Camera camera;

        internal EVENTCameraDeactivate(Camera camera)
        { this.camera = camera; }


        public override void Start()
        {
            camera.Active = false;
            ready = true;
        }

    }

    public class EVENTCameraReactivate : EVENT
    {

        Camera camera;

        internal EVENTCameraReactivate(Camera camera)
        { this.camera = camera; }


        public override void Start()
        {
            camera.Active = true;
            ready = true;
        }

    }

    public class EVENTDialogueTimed : EVENT
    {
        private Lines line;
        private Actor actor;
        private float etime;
        const float TEXTTIME = 5000f;

        internal EVENTDialogueTimed(String text, Actor actor, Stage stage)
        {
            line = new Lines(text, actor);
            this.actor = actor;
            stage.StartLine(line);
        }

        internal EVENTDialogueTimed(String callID, String text, Actor actor)
        {
            id = callID;
            line = new Lines(text, actor);
            this.actor = actor;
            actor.parent.StartLine(line);
        }

        public override void Start()
        {
            etime = 0;
            actor.parent.StartLine(line);

            ready = false;
        }

        public override bool Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            if (etime > TEXTTIME) { ready = true; actor.parent.StopLine(line); }
            return ready;
        }
    }

    public class EVENTDialogueConfirmed : EVENT
    {
        private Lines line;
        private Actor actor;
        private Stage stage;

        internal EVENTDialogueConfirmed(String text, Actor actor, Stage stage)
        {
            line = new Lines(text, actor);
            this.actor = actor;
            this.stage = stage;
            stage.StartLine(line);
        }

        internal EVENTDialogueConfirmed(String callID, String text, Actor actor, Stage stage)
        {
            id = callID;
            line = new Lines(text, actor);
            this.actor = actor;
            stage.StartLine(line);
            this.stage = stage;
        }

        public override void Start()
        {

            stage.StartLine(line);
            ready = false;
        }

        public override bool Update(GameTime t)
        {
            if (stage.player.Input.IsAButtonNewlyPressed()) { ready = true; stage.StopLine(line); }
            return ready;
        }


    }

    public class EVENTDramaticPause : PauseEVENT
    {
        float time;
        float etime;

        internal EVENTDramaticPause(float Time) { time = Time; }

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

    public class PauseEVENT : EVENT
    {
    }

    public class EVENTAcknowledgePause : PauseEVENT
    {

        private Stage Stage;

        internal EVENTAcknowledgePause(Stage stage) { Stage = stage; }


        public override bool Update(GameTime t)
        {
            if (Stage.input.IsAButtonNewlyPressed()) ready = true;
            return Ready;
        }

    }

    class EVENTMoveActor : EVENT
    {
        Actor actor;
        Vector2 destination;
        Stage Stage;

        public EVENTMoveActor(Actor Actor, Vector2 Destination, Stage stage)
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
            if ((destination - actor.Position).LengthSquared() < 50f) ready = true;
            return ready;
        }



    }

    class EVENTStageChange : EVENT
    {
        StageManager SM;
        String stageName;

        public EVENTStageChange(StageManager sM, String StageName)
        {
            SM = sM;
            stageName = StageName;

        }

        public override void Start()
        {
            SM.SetStage(stageName);
        }



    }

    class EVENTSetCameraLocation : EVENT
    {
        Camera camera;
        Vector2 location;

        public EVENTSetCameraLocation(Camera Camera, Vector2 spot)
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

    class EVENTSetCameraSubject : EVENT
    {
        Camera camera;
        Actor subject;


        public EVENTSetCameraSubject(Camera Camera, Actor Subject)
        {
            camera = Camera;
            subject = Subject;

        }


        public override void Start()
        {
            camera.setSubject(subject);
            ready = true;
        }

    }

    class EVENTChangeAmbient : EVENT
    {
        Stage stage;
        Vector3 color;

        float strength;
        float time;
        float etime;

        public EVENTChangeAmbient(Stage Stage, Vector3 Color, float Strength, float Time)
        {

            stage = Stage;
            color = Color;
            strength = Strength;
            time = Time;
            etime = 0;
        }

        public EVENTChangeAmbient(Stage Stage, Vector3 Color, float Strength)
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
            stage.AMBColor += color * t.ElapsedGameTime.Milliseconds / time;
            stage.AMBStrength += strength * t.ElapsedGameTime.Milliseconds / time;
            etime += t.ElapsedGameTime.Milliseconds;
            ready = (etime >= time);
            return ready;
        }



    }

    class EVENTMoveCamera : EVENT
    {
        Camera camera;
        Vector2 destination;
        Vector2 direction;
        float time;
        float etime;

        public EVENTMoveCamera(Camera Camera, Vector2 Destination, float Time)
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
            camera.Pan(direction * (t.ElapsedGameTime.Milliseconds / time));
            ready = (etime >= time);
            return ready;
        }
    }

    class EVENTScaleCamera : EVENT
    {
        Camera camera;
        float time;
        float etime;
        float EndScale;
        float ScaleInterval;


        public EVENTScaleCamera(Camera Camera, float Scale, float Time)
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
            camera.Zoom(ScaleInterval * t.ElapsedGameTime.Milliseconds / time);
            ready = (etime >= time);
            return ready;
        }




    }

    class EVENTPerformAction : EVENT
    {
        String action;
        Actor actor;
        float etime;
        float time;

        public EVENTPerformAction(Actor Actor, String Action)
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

    class EVENTActorExitStage : EVENT
    {
        Stage stage;
        String target;


        public EVENTActorExitStage(Stage Stage, String Target)
        {
            stage = Stage;
            target = Target;
        }

        public override void Start()
        {
            stage.removeActor(target);
            ready = true;
        }



    }

    class EVENTActorAddItem : EVENT
    {
        Actor actor;
        Item item;

        public EVENTActorAddItem(Actor Actor, Item Item)
        {
            actor = Actor;
            item = Item;
        }

        public override void Start()
        {
            actor.AddItem(item);
        }



    }

    class EVENTActorRemoveItem : EVENT
    {
        Actor actor;
        Item item;

        public EVENTActorRemoveItem(Actor Actor, Item Item)
        {
            actor = Actor;
            item = Item;
            ready = true;
        }

        public override void Start()
        {
            actor.RemoveItem(item);
            ready = true;
        }
    }

    class EVENTActorAddProperty : EVENT
    {

        Actor actor;
        String property;

        public EVENTActorAddProperty(Actor actor, String property)
        {
            this.actor = actor;
            this.property = property;
        }

        public override void Start()
        {
            actor.AddProperty(property);
            ready = true;
        }



    }

    class EVENTOpenMenu : EVENT
    {
        UserMenus.Menu target;
        UserMenus.UIManager UIM;

        public EVENTOpenMenu(UserMenus.Menu target, UserMenus.UIManager UIM)
        {
            this.target = target;
            this.UIM = UIM;
            
        }

        public override void Start()
        {
            UIM.OpenMenu(target);
            ready = true;
        }

    }

    class EVENTOpenEvent : EVENT
    {
        String nextEvent;
        Stage stage;
        
        public EVENTOpenEvent(String Next, Stage Stage)
        {
            nextEvent= Next;
            stage = Stage;
        }

        public override void Start()
        {
            ready = true;
        }

        public override void End()
        {

            stage.RunEvent(nextEvent);
            ready = false;
        }

    }


}
