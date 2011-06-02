using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UserMenus;

using StoneCircle.Persistence;

namespace StoneCircle
{
    class Actor : ISaveable
    {
        private uint objectId;

        protected String name;  // Name of the actor.
        public String Name { get { return name; } set { name = value; } }

        protected List<String> properties = new List<String>();


        protected int speed;
        public int Speed { get { return speed; } }

        public Vector2 screenadjust = new Vector2(684, 384);


        // These are variables that define the sprite of the actor.
        public String asset_Name;
        protected Texture2D image_map;
        public int ImageWidth;
        public int ImageHeight;
        public int ImageXindex;
        public int ImageYindex;

        public bool Active;
        public bool Interacting;

        private AIProfile currentProfile;


        //Here is the inventory
        protected Inventory inventoryMenu;
        public Inventory InventoryMenu { get { return inventoryMenu; } }
        protected List<Item> inventory = new List<Item>();
        protected Item currentItem;
        public Item CurrentItem { get { return currentItem; } set { currentItem = value; } }
        protected bool inventoryOpen;

        protected float defaultBeatTimer = 1000;
        protected float currentBeatTimer;
        protected float currentBeatTime;

        protected CollisionCylinder lowerRegion;
        protected CollisionCylinder midRegion;
        protected CollisionCylinder upperRegion;

        protected float awarenessWidth;
        protected float awarenessRange;

        protected float pGravity;


        // These variables represent the location and position of the actor. 
        protected Vector2 origin;
        public Vector3 Location;
        public Vector2 Position { get { return new Vector2(Location.X, (Location.Y - Location.Z) / 2); } }
        protected float radius;
        public float Radius { get { return radius; } }
        protected float height;

        public Vector2 RenderPosition;
        protected Vector2 facing;
        public Vector2 Facing { get { return facing; } set { facing = value; } }
        protected Vector3 updateVector;
        public Vector3 UpdateVector { get { return updateVector; } set { updateVector += value; } }

        protected Dictionary<String, Actionstate> knownActions = new Dictionary<String, Actionstate>();
        protected Actionstate current_Action;
        protected Actionstate talking = new Actionstate("Talking");
        protected Actionstate defaultAction;

        protected float totalLifeTime;
        protected float currentLife;
        public float CurrentLife { get { return currentLife; } }
        public float TotalLife { get { return totalLifeTime; } }
        public bool Bleeding { get { return properties.Contains("Bleeding"); } }

        protected float totalFatigue;
        protected float currentFatigue;
        public float CurrentFatigue { get { return currentFatigue; } }
        public float TotalFatigue { get { return totalFatigue; } }


        public List<AIOption> AIStance = new List<AIOption>();


        public CollisionCylinder Bounds { get { return new CollisionCylinder(Location, radius, height); } }


        public Stage parent;

        protected GameManager gameManager;


        public Actor(GameManager gameManager)
        {
            this.objectId = IdFactory.GetNextId();
            asset_Name = "male_select";
            speed = 100;
            Location = Vector3.Zero;
            ImageXindex = 0; ImageYindex = 0;
            totalLifeTime = 100;
            currentLife = totalLifeTime;
            totalFatigue = 100;
            currentFatigue = totalFatigue;
            currentBeatTimer = defaultBeatTimer;
            currentBeatTime = 0;
            awarenessWidth = 180;
            height = 60;
            radius = 20;

            currentProfile = new AIPFullRandom();
            this.gameManager = gameManager;
            Active = true;
        }

        public Actor(String Id, String asset_name, Vector2 starting)  // Basic constructor.
        {
            this.objectId = IdFactory.GetNextId();
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 0);
            name = Id;
            height = 60;
            radius = 60;

            currentProfile = new AIPFullRandom();
        }

        public Actor(String id, String asset_name, Vector2 starting, Stage Parent)  // Basic constructor.
        {
            this.objectId = IdFactory.GetNextId();
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 0);

            parent = Parent;
            name = id;
            learnAction(new Actionstate("Talking"));
            learnAction(new Stand());
            learnAction(new Walk());
            learnAction(new Limp());
            learnAction(new Jump());
            learnAction(new Run());
            learnAction(new UseItem());
            learnAction(new Dead());
            learnAction(new Unconcious());
            learnAction(new FallForward());
            learnAction(new StandUp());
            learnAction(new Prone());
            learnAction(new Rest());

            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }

        public Actor(uint objectId)
        {
            this.objectId = objectId;
            IdFactory.MoveNextIdPast(this.objectId);
        }

        protected virtual void learnAction(Actionstate learned) { knownActions.Add(learned.id, learned); learned.Actor = this; }

        public virtual void SetAction(String nextAction)
        {
            if (current_Action == null) current_Action = knownActions[nextAction];
            if (nextAction != null && nextAction != current_Action.ID)
            {
                if (nextAction == "Walking" || nextAction == "Running")
                {
                    current_Action = knownActions[movement(nextAction)];
                    //  current_Action.Reset();
                }
                else if (knownActions.ContainsKey(nextAction))
                {
                    current_Action = knownActions[nextAction];
                    current_Action.Reset();
                }
            }
        }

        public virtual void SetAction(String nextAction, int frame)
        {
            if (nextAction == "Walking" || nextAction == "Running")
            {
                current_Action = knownActions[movement(nextAction)];
                //  current_Action.Reset();
            }
            else if (knownActions.ContainsKey(nextAction))
            {
                current_Action = knownActions[nextAction];
                current_Action.Frame = frame;
            }
        }


        public virtual void loadImage(ContentManager theContentManager) // This loads the image map of the actor and locates the origin.
        {
            image_map = theContentManager.Load<Texture2D>(asset_Name);
            //if (ImageWidth == 0) 
                ImageWidth = image_map.Width;
           // if (ImageHeight == 0) 
                ImageHeight = image_map.Height;
            origin = new Vector2(ImageWidth / 2, ImageHeight - ImageWidth / 3);

        }

        public virtual void AttackResponse(Attack attack)
        {

        }

        protected virtual void heartBeat()
        {   //Random rand = new Random();
            currentLife--;
            currentBeatTimer *= .98f;
        }

        public virtual void Initialize()
        { }

        public virtual void UnInitialize() { }

        public virtual void UpdateFacing(Vector2 newFacing)
        {
            newFacing.Normalize();
            facing = newFacing;
             if (Math.Abs(facing.X) > Math.Abs(facing.Y)) { if (facing.X > 0) ImageYindex = 0; else ImageYindex = 1; }
            else { if (facing.Y > 0) ImageYindex = 2; else ImageYindex = 3; }
        }


        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 camera_pos, float camera_scale, float intensity, SpriteFont font) // Draws the sprite and shadow of actor in relation to camera.
        {
                theSpriteBatch.Draw(image_map, screenadjust + (camera_scale * (Position - camera_pos)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), Color.White, 0f, origin, camera_scale, SpriteEffects.None, .2f - Location.Y / 100000f);
                theSpriteBatch.DrawString(font, name, screenadjust + (camera_scale * (Position - camera_pos) - new Vector2(ImageWidth / 2, ImageHeight + 15)), Color.White);
              
        }


        public virtual void DrawShadow(SpriteBatch theSpriteBatch, Vector2 camera_pos, float camera_scale, float rotation, float intensity)
        {
            Vector2 shadowTarget = screenadjust + (camera_scale * (Position - camera_pos));
            if (rotation < Math.PI / 2) theSpriteBatch.Draw(image_map, new Rectangle((int)shadowTarget.X, (int)shadowTarget.Y, (int)(ImageWidth * camera_scale), (int)(ImageHeight * (intensity + 1) * camera_scale)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), new Color(0f, 0f, 0f, intensity), rotation, origin, SpriteEffects.None, .2f - (Location.Y - 2) / 100000f);
            else theSpriteBatch.Draw(image_map, new Rectangle((int)shadowTarget.X, (int)shadowTarget.Y, (int)(ImageWidth * camera_scale), (int)(ImageHeight * (intensity + 1) * camera_scale)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), new Color(0f, 0f, 0f, intensity), rotation, origin, SpriteEffects.None, .2f - (Location.Y - 2) / 100000f);
        }


        public virtual CollisionCylinder GetBounds(Vector3 update)// Returns the bounding box of a moving actor for collision detection.
        {
            return new CollisionCylinder(Location + update, radius, height);
        }


        public virtual void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID)
            {
                case "Interact":

                    break;

                case "Nothing":
                    break;

                default:
                    break;

            }
        }

        public void Talking() { current_Action = talking; }
        public void DefaultAction() { current_Action = defaultAction; }

        public virtual void Update(GameTime t, Dictionary<String, Actor>.ValueCollection targets) // Updates position, vestigial remnants of player update. 
        {
            updateVector = Vector3.Zero;
            if (Location.Z < 0) { Location.Z = 0; pGravity = 0; }
            if (Location.Z > 0) {pGravity += 2;
                updateVector += pGravity * -2 * Vector3.UnitZ;
            }
            Interacting = false;

            if (Bleeding && currentLife > 0)
            {
                currentBeatTime -= t.ElapsedGameTime.Milliseconds;
                if (currentBeatTime < 0)
                {
                    heartBeat();
                    currentBeatTime = currentBeatTimer;
                }
            }

            if (currentLife <= 0) SetAction("Dead");
            if (currentFatigue <= 0) SetAction("Unconcious");
            if (currentFatigue < totalFatigue) currentFatigue += .33f;


            current_Action.Update(t, targets);
            Move();
            SetAction(ChooseAction(t, targets));

        }

        protected virtual String movement(String input)
        {
            if (properties.Contains("LegInjury")) return "Limping";
            else return input;
        }

        public virtual String ChooseAction(GameTime t, Dictionary<String, Actor>.ValueCollection actor_list)
        {

            List<String> actionList = new List<String>();

            if(current_Action.AvailableHigh.AButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.AButton))actionList.Add(current_Action.AvailableHigh.AButton);
            if (current_Action.AvailableHigh.BButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.BButton)) actionList.Add(current_Action.AvailableHigh.BButton);
            if (current_Action.AvailableHigh.YButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.YButton)) actionList.Add(current_Action.AvailableHigh.XButton);
            if (current_Action.AvailableHigh.XButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.XButton)) actionList.Add(current_Action.AvailableHigh.YButton);
            if (current_Action.AvailableHigh.NoButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.NoButton)) actionList.Add(current_Action.AvailableHigh.NoButton);
            if (current_Action.AvailableHigh.LStickAction != null && knownActions.ContainsKey(current_Action.AvailableHigh.LStickAction)) actionList.Add(current_Action.AvailableHigh.LStickAction);

            if (current_Action.AvailableHigh.AButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.AButton)) actionList.Add(current_Action.AvailableLow.AButton);
            if (current_Action.AvailableHigh.BButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.BButton)) actionList.Add(current_Action.AvailableLow.BButton);
            if (current_Action.AvailableHigh.XButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.XButton)) actionList.Add(current_Action.AvailableLow.XButton);
            if (current_Action.AvailableHigh.YButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.YButton)) actionList.Add(current_Action.AvailableLow.YButton);
            if (current_Action.AvailableHigh.NoButton != null && knownActions.ContainsKey(current_Action.AvailableHigh.NoButton)) actionList.Add(current_Action.AvailableLow.NoButton);
            if (current_Action.AvailableHigh.LStickAction != null && knownActions.ContainsKey(current_Action.AvailableHigh.LStickAction)) actionList.Add(current_Action.AvailableLow.LStickAction);

            int totalSum = 0;

            if (actionList.Count == 0) return current_Action.ID;

            foreach(String Action in actionList)
            {
              if(Action != null && currentProfile.ActionPriorities.ContainsKey(Action)) totalSum += currentProfile.ActionPriorities[Action];
               

            }
            Random actRand = new Random();

            int index = 0; 
            int selector = actRand.Next(totalSum);

            while(selector > 0)
            {
                if(actionList[index] != null && currentProfile.ActionPriorities.ContainsKey(actionList[index])) selector -= currentProfile.ActionPriorities[actionList[index]];
                if(selector >0) index ++;

            }

            if (current_Action.ID == actionList[index]) return "";
            return actionList[index];
        }

        public virtual void ActionUpdate(GameTime t, Dictionary<String, Actor>.ValueCollection actor_list)
        {

            updateVector = Vector3.Zero;
            if (Location.Z < 0) { Location.Z = 0; pGravity = 0; }
            if (Location.Z > 0)
            {
                pGravity += 2;
                updateVector += pGravity * -2 * Vector3.UnitZ;
            }
            Interacting = false;

            current_Action.Update(t, actor_list);
            Move();
        
        
        }

        public virtual void Move() // Changes location of actor.
        {
            Location += UpdateVector;
        }

        public virtual void AdjustUpdateVector(Vector3 adjustment)
        {

            UpdateVector += adjustment;
        }


        protected virtual String GetAIAction()
        {
            foreach (AIOption AIO in AIStance)
            {
                if (AIO.condition.Condition) return (AIO.action.ActionReturn());

            }
            return "Walking";
        }

        public bool HasProperty(String Property)
        {
            return properties.Contains(Property);
        }
        public bool DoesNotHaveProperty(String Property) { return !HasProperty(Property); }
        public void AddProperty(String Property) { if (!properties.Contains(Property)) properties.Add(Property); }
        public void RemoveProperty(String Property) { if (properties.Contains(Property)) properties.Remove(Property); }

        public virtual void AddAIOption(AIOption next)
        {
            AIStance.Add(next);
        }

        public virtual void AddItem(Item item)
        {
            inventory.Add(item);
            item.II.Load(gameManager.ContentManager);
            inventoryMenu.AddMenuItem(item.II);

        }

        public void RemoveItem(Item item)
        {
            inventoryMenu.RemoveMenuItem(item.II);
            inventory.Remove(item);

        }



        public virtual void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            writer.Write(name);
            writer.Write(asset_Name);
            
            writer.Write(Active);
            writer.Write(Interacting); // bool
            Saver.SaveStringList(properties, writer);
            writer.Write(currentBeatTimer); // single
            writer.Write(currentBeatTime); // single

            Saver.SaveVector3(Location, writer);
            Saver.SaveVector2(RenderPosition, writer);
            Saver.SaveVector2(facing, writer);

            writer.Write(speed); // single
            writer.Write(totalLifeTime); // single
            writer.Write(currentLife); // single
        
            writer.Write(totalFatigue); // single
            writer.Write(currentFatigue); // single
            writer.Write(parent != null);
            if (parent != null)
            {
                writer.Write(objectTable[parent]);
            }
        }

        public virtual void Load(BinaryReader reader, SaveType type)
        {
            name = reader.ReadString();
            asset_Name = reader.ReadString();
            Active = reader.ReadBoolean();
            Interacting = reader.ReadBoolean();
            properties = Loader.LoadStringList(reader);
            currentBeatTimer = reader.ReadSingle();
            currentBeatTime = reader.ReadSingle();

            Location = Loader.LoadVector3(reader);
            RenderPosition = Loader.LoadVector2(reader);
            facing = Loader.LoadVector2(reader);

            speed = reader.ReadInt32();
            totalLifeTime = reader.ReadSingle();
            currentLife = reader.ReadSingle();
            totalFatigue = reader.ReadSingle();
            currentFatigue = reader.ReadSingle();
            loadStage = reader.ReadBoolean();
            if (loadStage)
            {
                stageId = reader.ReadUInt32();
            }
        }

        private uint stageId;
        private bool loadStage;
        public virtual void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            if (loadStage)
            {
                parent = (Stage)objectTable[stageId];
            }
        }

        public virtual void FinishLoad(GameManager manager)
        {
            this.gameManager = manager;

            learnAction(new Actionstate("Talking"));
            learnAction(new Actionstate("Standing"));
            learnAction(new Jump());
            learnAction(new UseItem());
            learnAction(new Walk());

            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }

        public virtual List<ISaveable> GetSaveableRefs(SaveType type)
        {
            return null;
        }

        public uint GetId()
        {
            return objectId;
        }
    }

    class Character : Actor
    {
        public Character(GameManager gameManager)
            : base(gameManager)
        {
            speed = 100;
            learnAction(new Actionstate("Talking"));
            learnAction(new Stand());
            learnAction(new Walk());
            learnAction(new Limp());
            learnAction(new Jump());
            learnAction(new Run());
            learnAction(new UseItem());
            learnAction(new Dead());
            learnAction(new Unconcious());
            learnAction(new FallForward());
            learnAction(new StandUp());
            learnAction(new Prone());
            learnAction(new Rest());
            learnAction(new HighBlock());
            learnAction(new LowBlock());
            learnAction(new MidBlock());

            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }
        public Character(String Id, String asset_name, Vector2 starting,  GameManager gameManager)
            : base(gameManager)  // Basic constructor.
        {
            
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 1);
            name = Id; learnAction(new Actionstate("Talking"));
            learnAction(new Stand());
            learnAction(new Walk());
            learnAction(new Limp());
            learnAction(new UseItem());
            learnAction(new Dead());
            learnAction(new Unconcious());
            learnAction(new FallForward());
            learnAction(new StandUp());
            learnAction(new Prone());
            learnAction(new Rest());
            learnAction(new HighBlock());
            learnAction(new LowBlock());
            learnAction(new MidBlock());

            defaultAction = knownActions["Standing"];
            SetAction("Standing");
            Active = true;
        }

        public Character(uint objectId) : base(objectId) { }

        public override void loadImage(ContentManager theContentManager) // This loads the image map of the actor and locates the origin.
        {
            image_map = theContentManager.Load<Texture2D>("Characters/" + asset_Name);
            if (ImageWidth == 0) ImageWidth = image_map.Width;
            if (ImageHeight == 0) ImageHeight = image_map.Height;
            ImageWidth = 128;
            ImageHeight = 128;

            origin = new Vector2(ImageWidth / 2, ImageHeight - ImageWidth / 3);

        }

        public override void AttackResponse(Attack attack)
        {
            Vector3 diff = attack.Actor.Location - Location;
            float attackAngle = (float)Math.Atan2(diff.Y, diff.X);
            float lowerBound = (float)Math.Atan2(Facing.Y, Facing.X) - MathHelper.ToRadians(awarenessWidth/2);
            float upperBound = (float)Math.Atan2(Facing.Y, Facing.X) + MathHelper.ToRadians(awarenessWidth / 2);
            if (upperBound > Math.PI && attackAngle < 0) attackAngle += 2 * (float)Math.PI;
            if (upperBound < -Math.PI && attackAngle > 0) attackAngle -= 2 * (float)Math.PI;
            if (attackAngle < upperBound && attackAngle > lowerBound) SetAction("MidBlock");
            else AddProperty("Bleeding");
            
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            updateVector = Vector3.Zero;
            if (Location.Z < 0) Location.Z = 0;
            if (Location.Z > 2) updateVector += Vector3.UnitZ * -2;

            Interacting = false;

            if (Bleeding && currentLife > 0)
            {
                currentBeatTime -= t.ElapsedGameTime.Milliseconds;
                if (currentBeatTime < 0)
                {
                    heartBeat();
                    currentBeatTime = currentBeatTimer;
                }
            }

            if (currentLife <= 0) SetAction("Dead");
            if (currentFatigue <= 0) SetAction("Unconcious");
            if (currentFatigue < totalFatigue) currentFatigue += .33f;


            current_Action.Update(t, targets);
            Move();
            SetAction(ChooseAction(t, targets));

        }



    }


    class SetProp : Actor
    {
        public SetProp(uint objectId) : base(objectId) { }
        
        public SetProp(String Id, String asset_name, Vector2 starting,  GameManager gameManager)
            : base(gameManager)  // Basic constructor.
        {
            
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 0);
            name = Id;
            learnAction( new Actionstate("Standing"));
            defaultAction = knownActions["Standing"];
            SetAction("Standing");

        }

        public SetProp(GameManager gameManager)
            : base(gameManager)
        {
            learnAction(new Actionstate("Standing"));
            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }

        public override void Update(GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            updateVector = Vector3.Zero;
            if (Location.Z < 0) Location.Z = 0;
            if (Location.Z > 2) updateVector += Vector3.UnitZ * -2;

            Interacting = false;
            
        }

        public override void loadImage(ContentManager theContentManager) // This loads the image map of the actor and locates the origin.
        {
            image_map = theContentManager.Load<Texture2D>("Set Props/" + asset_Name);
             ImageWidth = image_map.Width;
             ImageHeight = image_map.Height;
            
            origin = new Vector2(ImageWidth / 2, ImageHeight - ImageWidth / 3);
        }



    }

}
