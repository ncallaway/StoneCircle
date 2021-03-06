﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;

using StoneCircle.Persistence;

namespace StoneCircle
{

    class EVENTGroup : EVENT
    {
        protected List<EVENT> EVENTs = new List<EVENT>();

        public EVENTGroup() { }
        public EVENTGroup(uint objectId) : base(objectId) { }


        public void AddEVENT(EVENT add) { EVENTs.Add(add); }

        public override void Reset()
        {
            ready = false;
            foreach (EVENT E in EVENTs) E.Reset();
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = base.GetSaveableRefs(type);
            if (parentRefs == null)
            {
                parentRefs = new List<ISaveable>();
            }

            foreach (EVENT e in EVENTs)
            {
                parentRefs.Add(e);
            }

            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);

            List<ISaveable> eventList = new List<ISaveable>();
            foreach (EVENT e in EVENTs)
            {
                eventList.Add(e);
            }

            Saver.SaveSaveableList(eventList, writer, objectTable);
        }

        private EVENTGroupInflatables inflatables;

        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new EVENTGroupInflatables();
            inflatables.eventsList = Loader.LoadSaveableList(reader);
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            if (inflatables != null)
            {
                EVENTs = Loader.InflateSaveableList<EVENT>(inflatables.eventsList, objectTable);
            }
        }

        private class EVENTGroupInflatables
        {
            public List<uint> eventsList;
        }
        
    }

    class ParallelEVENTGroup : EVENTGroup
    {
        public ParallelEVENTGroup(String callID)
        {
            id = callID;
        }

        public ParallelEVENTGroup() { }
        public ParallelEVENTGroup(uint objectId) : base(objectId) { }



        public override void Start()
        {
            foreach (EVENT E in EVENTs) E.Start();
        }


        public override bool Update(GameTime t)
        {
            bool Ready = true;
            foreach (EVENT E in EVENTs) if (!E.Ready) { E.Update(t);  Ready = (Ready && E.Ready); }
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

        public SerialEVENTGroup(uint objectId) : base(objectId) { }

        public override void Start()
        {
            index = 0;
            if (EVENTs.Count > 0)
            {
                currentEVENT = EVENTs[index];
                currentEVENT.Start();
            }
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

    public class EVENT : ISaveable
    {
        protected String id;
        public String ID { get { return id; } set { id = value; } }
        protected bool ready;
        public bool Ready { get { return ready; } }

        private uint objectId;

        public EVENT()
        {
            objectId = IdFactory.GetNextId();
        }

        public EVENT(uint objectId)
        {
            this.objectId = objectId;
            IdFactory.MoveNextIdPast(objectId);
        }

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


        public virtual void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            writer.Write(this.id == null);
            if (this.id != null)
            {
                writer.Write(this.id);
            }
            writer.Write(this.ready);
        }

        public virtual void Load(BinaryReader reader, SaveType type)
        {
            bool idNull = reader.ReadBoolean();

            this.id = null;
            if (!idNull)
            {
                this.id = reader.ReadString();
            }

            this.ready = reader.ReadBoolean();
        }

        public virtual void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            /* no-op */
        }

        public virtual void FinishLoad(GameManager manager)
        {
            /* no-op */
        }

        public virtual List<ISaveable> GetSaveableRefs(SaveType type)
        {
            return null;
        }

        public uint GetId()
        {
            return this.objectId;
        }
    }

    class EVENTStateConditionSet : EVENT
    {
        StageManager SM;
        String StateCondition;

        public EVENTStateConditionSet(String sc, StageManager sm)
        {
            SM = sm;
            StateCondition = sc;

        }

        public EVENTStateConditionSet(uint objectId) : base(objectId) { }

        public override void Start()
        {
            SM.SetCondition(StateCondition);
            ready = true;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            Saver.SaveString(StateCondition, writer);
        }

        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            StateCondition = Loader.LoadString(reader);
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            SM = manager.StageManager;
        }
    }

    class EVENTPlayerDeactivate : EVENT
    {
        Player player;
        public EVENTPlayerDeactivate(Player player)
        { this.player = player; }

        public EVENTPlayerDeactivate(uint objectId) : base(objectId) { }


        public override void Start()
        {
            player.Active = false;
            
                ready = true;
        }

        public override bool Update(GameTime t)
        {
            ready = true;
            return ready;
        }

    }

    class EVENTPlayerReactivate : EVENT
    {

        Player player;
        public EVENTPlayerReactivate(Player player)
        { this.player = player; }

        public EVENTPlayerReactivate(uint objectId) : base(objectId) { }


        public override void Start()
        {
            player.Active = true;
           // ready = true;
        }

        public override bool Update(GameTime t)
        {
            ready = true;
            return ready;
        }

    }

    class EVENTStageDeactivate : EVENT
    {
        Stage stage;

        public EVENTStageDeactivate(Stage stage)
        { this.stage = stage; }

        public EVENTStageDeactivate(uint objectId) : base(objectId) { }

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

        public EVENTStageReactivate(uint objectId) : base(objectId) { }


        public override void Start()
        {
            foreach (Actor A in stage.Actors) A.Active = false;
            ready = true;
        }

    }

    public class EVENTCameraDeactivate : EVENT
    {

        Stage cameraContainer;

        internal EVENTCameraDeactivate(Stage cameraContainer)
        { this.cameraContainer = cameraContainer; }

        public EVENTCameraDeactivate(uint objectId) : base(objectId) { }


        public override void Start()
        {
            cameraContainer.camera.Active = false;
            ready = true;
        }

        public override bool Update(GameTime t)
        {
            ready = true;
            return ready;
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRefs.Add(cameraContainer);
            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[cameraContainer]);
        }

        private uint camContainerId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            camContainerId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            cameraContainer = (Stage)objectTable[camContainerId];
        }

    }

    public class EVENTCameraReactivate : EVENT
    {

        Stage cameraContainer;

        internal EVENTCameraReactivate(Stage cameraContainer)
        { this.cameraContainer = cameraContainer; }

        public EVENTCameraReactivate(uint objectId) : base(objectId) { }

        public override void Start()
        {
            cameraContainer.camera.Active = true;
            ready = true;
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRefs.Add(cameraContainer);
            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[cameraContainer]);
        }

        private uint camContainerId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            camContainerId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            cameraContainer = (Stage)objectTable[camContainerId];
        }

    }

    public class EVENTDialogueTimed : EVENT
    {
        private Lines line;
        private Actor actor;
        private float etime;
        float time;
        const float TEXTTIME = 5000f;

        public EVENTDialogueTimed(uint objectId) : base(objectId) { }

        internal EVENTDialogueTimed(String text, Actor actor, Stage stage, float Time)
        {
            line = new Lines(text, actor);
            this.actor = actor;
            stage.StartLine(line);
            time = Time;
        }

        internal EVENTDialogueTimed(String text, Actor actor, Stage stage)
        {
            line = new Lines(text, actor);
            this.actor = actor;
            stage.StartLine(line);

            time = TEXTTIME;
        }

        internal EVENTDialogueTimed(String callID, String text, Actor actor)
        {
            id = callID;
            line = new Lines(text, actor);
            this.actor = actor;
            actor.parent.StartLine(line);
            time = TEXTTIME;
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

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[line]);
            writer.Write(objectTable[actor]);
            writer.Write(etime);
            writer.Write(time);

        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs =  base.GetSaveableRefs(type);
            if (parentRefs == null)
            {
                parentRefs = new List<ISaveable>();
            }
            parentRefs.Add(line);
            parentRefs.Add(actor);

            return parentRefs;
        }

        private DialogInflatables inflatables;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new DialogInflatables();
            inflatables.lineId = reader.ReadUInt32();
            inflatables.actorId = reader.ReadUInt32();
            etime = reader.ReadSingle();
            time = reader.ReadSingle();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            line = (Lines)objectTable[inflatables.lineId];
            actor = (Actor)objectTable[inflatables.actorId];
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            actor.parent.StartLine(line);
        }

        private class DialogInflatables
        {
            public uint lineId;
            public uint actorId;
        }

    }

    public class EVENTDialogueConfirmed : EVENT
    {
        private Lines line;
        private Stage stage;

        public EVENTDialogueConfirmed(uint objectId) : base(objectId) { }

        internal EVENTDialogueConfirmed(String text, Actor actor, Stage stage)
        {
            line = new Lines(text, actor);
            this.stage = stage;
            stage.StartLine(line);
        }

        internal EVENTDialogueConfirmed(String callID, String text, Actor actor, Stage stage)
        {
            id = callID;
            line = new Lines(text, actor);
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
            if (stage.input.IsAButtonNewlyPressed()) { ready = true; stage.StopLine(line); }
            return ready;
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = base.GetSaveableRefs(type);
            if (parentRefs == null) { parentRefs = new List<ISaveable>(); }

            parentRefs.Add(line);
            parentRefs.Add(stage);

            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[line]);
            writer.Write(objectTable[stage]);
        }

        private uint lineId;
        private uint stageId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            lineId = reader.ReadUInt32();
            stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            line = (Lines)objectTable[lineId];
            stage = (Stage)objectTable[stageId];
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            stage.StartLine(line);
        }


    }

    public class EVENTDramaticPause : PauseEVENT
    {
        float time;
        float etime;

        internal EVENTDramaticPause(float Time) { time = Time; }

        public EVENTDramaticPause(uint objectId) : base(objectId) { }

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
        public PauseEVENT() { }
        public PauseEVENT(uint objectId) : base(objectId) { }
    }

    public class EVENTAcknowledgePause : PauseEVENT
    {

        private Stage Stage;

        internal EVENTAcknowledgePause(Stage stage) { Stage = stage; }

        public EVENTAcknowledgePause(uint objectId) : base(objectId) { }


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

        public EVENTMoveActor(uint objectId) : base(objectId) { }

        public override void Start()
        {
           actor.SetAction("Walking");
        }

        public override bool Update(GameTime t)
        {   Vector2 test = (new Vector2(destination.X - actor.Location.X, destination.Y - actor.Location.Y));

            actor.UpdateFacing(test);

            actor.ActionUpdate(t, Stage.Actors);
            if ((test).LengthSquared() < 2500f)
            {
                ready = true;
                actor.SetAction("Standing");
            }
            return ready;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[actor]);
            writer.Write(destination.X); writer.Write(destination.Y);
            writer.Write(objectTable[Stage]);
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRef = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRef.Add(actor);
            parentRef.Add(Stage);
            return parentRef;
        }

        private DialogInflatables inflatables;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new DialogInflatables();
            inflatables.actorId = reader.ReadUInt32();
            destination = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            inflatables.stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            actor = (Actor)objectTable[inflatables.actorId];
            Stage = (Stage)objectTable[inflatables.stageId];
        }

        private class DialogInflatables
        {
            public uint actorId;
            public uint stageId;
        }
        //Actor actor;
        //Vector2 destination;
        //Stage Stage;



    }

    class EVENTMoveActorToPlayer : EVENT
    {
        Actor actor;
        Vector2 destination;
        Stage Stage;

        public EVENTMoveActorToPlayer(Actor Actor, Vector2 Destination, Stage stage)
        {
            destination = Destination;
            actor = Actor;
            ready = false;
            Stage = stage;
        }

        public EVENTMoveActorToPlayer(uint objectId) : base(objectId) { }

        public override void Start()
        {

            actor.UpdateFacing(new Vector2(Stage.player.Location.X -actor.Location.X, Stage.player.Location.Y- actor.Location.Y));
             actor.SetAction("Walking");
        }

        public override bool Update(GameTime t)
        {
            actor.UpdateFacing(new Vector2(Stage.player.Location.X - actor.Location.X, Stage.player.Location.Y - actor.Location.Y));
            
            actor.ActionUpdate(t, Stage.Actors);
            if ((Stage.player.Location - actor.Location).LengthSquared() < 2500f)
            {
                ready = true;
                actor.SetAction("Standing");
            }
            return ready;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[actor]);
            writer.Write(destination.X); writer.Write(destination.Y);
            writer.Write(objectTable[Stage]);
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRef = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRef.Add(actor);
            parentRef.Add(Stage);
            return parentRef;
        }

        private DialogInflatables inflatables;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new DialogInflatables();
            inflatables.actorId = reader.ReadUInt32();
            destination = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            inflatables.stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            actor = (Actor)objectTable[inflatables.actorId];
            Stage = (Stage)objectTable[inflatables.stageId];
        }

        private class DialogInflatables
        {
            public uint actorId;
            public uint stageId;
        }
        //Actor actor;
        //Vector2 destination;
        //Stage Stage;



    }


    class EVENTWarpActor : EVENT
    {
        Actor actor;
        Vector3 destination;
        Stage Stage;

        public EVENTWarpActor(Actor Actor, Vector3 Destination, Stage stage)
        {
            destination = Destination;
            actor = Actor;
            ready = false;
            Stage = stage;
        }

        public EVENTWarpActor(uint objectId) : base(objectId) { }

        public override void Start()
        {
            actor.Location = destination;
            ready = true;

        }

        public override bool Update(GameTime t)
        {
            return ready;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[actor]);
            writer.Write(destination.X); writer.Write(destination.Y);
            writer.Write(objectTable[Stage]);
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRef = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRef.Add(actor);
            parentRef.Add(Stage);
            return parentRef;
        }

        private DialogInflatables inflatables;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new DialogInflatables();
            inflatables.actorId = reader.ReadUInt32();
            destination = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            inflatables.stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            actor = (Actor)objectTable[inflatables.actorId];
            Stage = (Stage)objectTable[inflatables.stageId];
        }

        private class DialogInflatables
        {
            public uint actorId;
            public uint stageId;
        }
    }

    class EVENTPartialWarpActor : EVENT
    {
        Actor actor;
        Vector3 destination;
        Stage Stage;

        public EVENTPartialWarpActor(Actor Actor, Vector3 Destination, Stage stage)
        {
            destination = Destination;
            actor = Actor;
            ready = false;
            Stage = stage;
        }

        public EVENTPartialWarpActor(uint objectId) : base(objectId) { }

        public override void Start()
        {
            actor.Location += destination;
            ready = true;

        }

        public override bool Update(GameTime t)
        {
            return ready;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[actor]);
            writer.Write(destination.X); writer.Write(destination.Y);
            writer.Write(objectTable[Stage]);
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRef = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRef.Add(actor);
            parentRef.Add(Stage);
            return parentRef;
        }

        private DialogInflatables inflatables;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            inflatables = new DialogInflatables();
            inflatables.actorId = reader.ReadUInt32();
            destination = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            inflatables.stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            actor = (Actor)objectTable[inflatables.actorId];
            Stage = (Stage)objectTable[inflatables.stageId];
        }

        private class DialogInflatables
        {
            public uint actorId;
            public uint stageId;
        }
    }

    class EVENTStageChange : EVENT
    {
        StageManager SM;
        String stageName;
        Vector2 destination;

        public EVENTStageChange(StageManager sM, String StageName, Vector2 destination)
        {
            SM = sM;
            stageName = StageName;
            this.destination = destination;
        }

        public EVENTStageChange(uint objectId) : base(objectId) { }

        public override void Start()
        {
            SM.SetStage(stageName, destination);
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            Saver.SaveString(stageName, writer);
        }

        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            stageName = Loader.LoadString(reader);
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            SM = manager.StageManager;
        }



    }

    class EVENTSetCameraLocation : EVENT
    {
        Stage cameraContainer;
        Vector2 location;

        public EVENTSetCameraLocation(uint objectId) : base(objectId) { }

        public EVENTSetCameraLocation(Stage CameraContainer, Vector2 spot)
        {
            cameraContainer = CameraContainer;
            location = spot;

        }

        public override void Start()
        {
            cameraContainer.camera.Location = location;
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

        public EVENTSetCameraSubject(uint objectId) : base(objectId) { }


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

        public EVENTChangeAmbient(uint objectId) : base(objectId) { }



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

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = Saver.ConstructSaveableList(base.GetSaveableRefs(type));
            parentRefs.Add(stage);
            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[stage]);
            Saver.SaveVector3(color, writer);
            writer.Write(strength);
            writer.Write(time);
            writer.Write(etime);
        }

        private uint stageId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            stageId = reader.ReadUInt32();
            color = Loader.LoadVector3(reader);
            strength = reader.ReadSingle();
            time = reader.ReadSingle();
            etime = reader.ReadSingle();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            stage = (Stage)objectTable[stageId];
        }



    }

    class EVENTMoveCamera : EVENT
    {
        Stage cameraContainer;
        Vector2 destination;
        Vector2 direction;
        float time;
        float etime;

        public EVENTMoveCamera(uint objectId) : base(objectId) { }

        public EVENTMoveCamera(Stage CameraContainer, Vector2 Destination, float Time)
        {
            cameraContainer = CameraContainer;
            destination = Destination;
            time = Time;
            etime = 0;
        }

        public override void Start() { cameraContainer.camera.Active = false; direction = destination - cameraContainer.camera.Location; }

        public override bool Update(GameTime t)
        {
            etime += t.ElapsedGameTime.Milliseconds;
            cameraContainer.camera.Pan(direction * (t.ElapsedGameTime.Milliseconds / time));
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

        public EVENTScaleCamera(uint objectId) : base(objectId) { }

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

        public EVENTPerformAction(uint objectId) : base(objectId) { }

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

        public EVENTActorExitStage(uint objectId) : base(objectId) { }

        public override void Start()
        {
            stage.removeActor(target);
            ready = true;
        }



    }

    class EVENTActorEnterStage : EVENT
    {

        Stage stage;
        String target;
        StageManager SM;
        Vector2 destination;



        public EVENTActorEnterStage(Stage Stage, String Target, StageManager SM, Vector2 destination)
        {
            this.SM = SM;
            stage = Stage;
            target = Target;
            this.destination = destination;

        }

        public EVENTActorEnterStage(uint objectId) : base(objectId) { }

        public override void Start()
        {
            stage.AddActor(SM.MainCharacters[target], destination );
            ready = true;
        }

        public override bool Update(GameTime t)
        {
            ready = true;
            return ready;
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

        public EVENTActorAddItem(uint objectId) : base(objectId) { }

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

        public EVENTActorRemoveItem(uint objectId) : base(objectId) { }

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

        public EVENTActorAddProperty(uint objectId) : base(objectId) { }

        public override void Start()
        {
            actor.AddProperty(property);
            ready = true;
        }



    }

    class EVENTActorRemoveProperty : EVENT
    {

        Actor actor;
        String property;

        public EVENTActorRemoveProperty(Actor actor, String property)
        {
            this.actor = actor;
            this.property = property;
        }

        public EVENTActorRemoveProperty(uint objectId) : base(objectId) { }

        public override void Start()
        {
            actor.RemoveProperty(property);
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

        public EVENTOpenMenu(uint objectId) : base(objectId) { }

        public override void Start()
        {
            ready = true;
            UIM.OpenMenu(target);
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            writer.Write(objectTable[target]);
        }

        uint targetId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            targetId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            target = (UserMenus.Menu)objectTable[targetId];
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs = base.GetSaveableRefs(type);
            if (parentRefs == null)
            {
                parentRefs = new List<ISaveable>();
            }

            parentRefs.Add(target);
            return parentRefs;
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            UIM = manager.UIManager;
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

        public EVENTOpenEvent(uint objectId) : base(objectId) { }

        public override void Start()
        {
            ready = true;
        }

        public override void End()
        {

            stage.RunEvent(nextEvent);
            ready = false;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            Saver.SaveString(nextEvent, writer);
            writer.Write(objectTable[stage]);
        }

        private uint stageId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            nextEvent = Loader.LoadString(reader);
            stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            stage = (Stage) objectTable[stageId];
        }
    }

    class EVENTStateConditionONEVENT : EVENT
    {
        String stateCondition;
        StageManager SM;
        EVENT targetEvent;


        public EVENTStateConditionONEVENT(String SC, EVENT Target, StageManager sm)
        {
            stateCondition = SC;
            SM = sm;
            targetEvent = Target;


        }


        public override void Start()
        {

            if(SM.CheckCondition(stateCondition)) targetEvent.Start();
            else ready = true;
        }

        public override bool Update(GameTime t)
        {
            ready = targetEvent.Update(t);
            return ready;
        }

    }

    class EVENTActorEquipItem : EVENT
    {
         Stage stage;
        String target;
        StageManager SM;
        Item item;



        public EVENTActorEquipItem(Stage Stage, String Target, StageManager SM, Item next)
        {
            this.SM = SM;
            stage = Stage;
            target = Target;
            item = next;

        }

        public EVENTActorEquipItem(uint objectId) : base(objectId) { }

        public override void Start()
        {
            Actor Target = stage.GetActor(target);
            if (Target == null) Target = SM.MainCharacters[target];
            if (Target.CurrentItem != null) Target.CurrentItem.OnUnequipItem();
            Target.CurrentItem = item;
            if (Target.CurrentItem != null) Target.CurrentItem.OnEquipItem();
            ready = true;
        }




    }

}
