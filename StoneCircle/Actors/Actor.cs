using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UserMenus;

namespace StoneCircle
{
    class Actor
    {
        protected String name; // Name of the actor.
        public String Name { get { return name; } }

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

        //Here is the inventory
        protected Inventory inventory;
        public Inventory Inventory { get { return inventory; } }
        protected InventoryItem currentItem;
        public InventoryItem CurrentItem { get { return currentItem; } set { currentItem = value; } }
        protected bool inventoryOpen;

        protected float defaultBeatTimer = 1000;
        protected float currentBeatTimer;
        protected float currentBeatTime;



        // These variables represent the location and position of the actor. 
        protected Vector2 origin;
        public Vector3 Location;
        public Vector2 Position { get { return new Vector2(Location.X, Location.Y - Location.Z / 2); } }
        public Vector2 RenderPosition;
        protected Vector2 facing;
        public Vector2 Facing { get { return facing; } set { facing = value; } }

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


        public Stage parent;

        protected GameManager gameManager;


        public Actor(GameManager gameManager)
        {
            asset_Name = "male_select";
            speed = 100;
            Location = Vector3.Zero;
            ImageXindex = 0; ImageYindex = 0;
            learnAction(new Actionstate("Talking"));
            learnAction(new Actionstate("Standing"));
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
            totalLifeTime = 100;
            currentLife = totalLifeTime;
            totalFatigue = 100;
            currentFatigue = totalFatigue;
            currentBeatTimer = defaultBeatTimer;
            currentBeatTime = 0;

            this.gameManager = gameManager;
            Active = true;
        }

        public Actor(String Id, String asset_name, Vector2 starting)  // Basic constructor.
        {
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 0);
            name = Id;

            learnAction(new Actionstate("Talking"));
            learnAction(new Actionstate("Standing"));
            learnAction(new Jump());
            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }

        public Actor(String id, String asset_name, Vector2 starting, Stage Parent)  // Basic constructor.
        {
            asset_Name = asset_name;
            speed = 100;
            Location = new Vector3(starting.X, starting.Y, 0);

            parent = Parent;
            name = id;

            learnAction(new Actionstate("Talking"));
            learnAction(new Actionstate("Standing"));
            learnAction(new Jump());
            learnAction(new UseItem());
            learnAction(new Walk());

            defaultAction = knownActions["Standing"];
            SetAction("Standing");
        }

        protected virtual void learnAction(Actionstate learned) { knownActions.Add(learned.id, learned); learned.Actor = this; }

        public virtual void SetAction(String nextAction)
        {
            if (nextAction == "Walking" || nextAction == "Running") {
                current_Action = knownActions[movement(nextAction)];
                //  current_Action.Reset();
            } else if (knownActions.ContainsKey(nextAction)) {
                current_Action = knownActions[nextAction];
                current_Action.Reset();
            }
        }

        public virtual void loadImage(ContentManager theContentManager) // This loads the image map of the actor and locates the origin.
        {
            image_map = theContentManager.Load<Texture2D>(asset_Name);
            if (ImageWidth == 0) ImageWidth = image_map.Width;
            if (ImageHeight == 0) ImageHeight = image_map.Height;
            origin = new Vector2(ImageWidth / 2, ImageHeight - ImageWidth / 3);

        }

        protected virtual void heartBeat()
        {   //Random rand = new Random();
            currentLife--;
            currentBeatTimer *= .98f;
        }


        public void StartBleeding()
        { properties.Add("Bleeding"); }

        public void StopBleeding() { properties.Remove("Bleeding"); }

        public virtual void Initialize()
        { }

        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 camera_pos, float camera_scale, float intensity, SpriteFont font) // Draws the sprite and shadow of actor in relation to camera.
        {
            theSpriteBatch.Draw(image_map, screenadjust + (camera_scale * (Position - camera_pos)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), new Color(intensity, intensity, intensity, 1f), 0f, origin, camera_scale, SpriteEffects.None, .2f - Location.Y / 100000f);
            theSpriteBatch.DrawString(font, current_Action.ID + "  " + current_Action.Frame, screenadjust + (camera_scale * (Position - camera_pos) - new Vector2(ImageWidth / 2, ImageHeight + 15)), Color.White);
        }


        public virtual void DrawShadow(SpriteBatch theSpriteBatch, Vector2 camera_pos, float camera_scale, float rotation, float intensity)
        {
            Vector2 renderTarget = screenadjust + (camera_scale * (Position - camera_pos));
            if (rotation < Math.PI / 2) theSpriteBatch.Draw(image_map, new Rectangle((int)renderTarget.X, (int)renderTarget.Y, (int)(ImageWidth * camera_scale), (int)(ImageHeight * (intensity + 1) * camera_scale)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), new Color(0f, 0f, 0f, intensity), rotation, origin, SpriteEffects.None, .2f - (Location.Y - 2) / 100000f);
            else theSpriteBatch.Draw(image_map, new Rectangle((int)renderTarget.X, (int)renderTarget.Y, (int)(ImageWidth * camera_scale), (int)(ImageHeight * (intensity + 1) * camera_scale)), new Rectangle(ImageXindex * ImageWidth, ImageYindex * ImageHeight, ImageWidth, ImageHeight), new Color(0f, 0f, 0f, intensity), rotation, origin, SpriteEffects.None, .2f - (Location.Y - 2) / 100000f);
        }


        public virtual BoundingBox GetBounds()   // Returns the bounding box of a non-moving actor for collision detection.
        {
            Vector3 min = new Vector3(Location.X - ImageWidth / 3, Location.Y - ImageHeight / 4, Location.Z);
            Vector3 max = new Vector3(Location.X + ImageWidth / 3, Location.Y + ImageHeight / 4, Location.Z + ImageHeight);
            return new BoundingBox(min, max);
        }


        public virtual BoundingBox GetBounds(Vector3 update)// Returns the bounding box of a moving actor for collision detection.
        {
            Vector3 min = new Vector3(Location.X + update.X - ImageWidth / 2, Location.Y + update.Y - ImageWidth / 2, Location.Z + update.Z);
            Vector3 max = new Vector3(Location.X + update.X + ImageWidth / 2, Location.Y + update.Y + ImageWidth / 2, Location.Z + update.Z + ImageHeight);
            return new BoundingBox(min, max);
        }


        public virtual void ApplyAction(Actionstate affected, Actor affector)
        {
            switch (affected.ID) {
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
            if (Bleeding && currentLife > 0) {
                currentBeatTime -= t.ElapsedGameTime.Milliseconds;
                if (currentBeatTime < 0) {
                    heartBeat();
                    currentBeatTime = currentBeatTimer;
                }
            }

            if (currentLife <= 0) SetAction("Dead");
            if (currentFatigue <= 0) SetAction("Unconcious");
            if (currentFatigue < totalFatigue) currentFatigue += .33f;


            current_Action.Update(t, targets);
            SetAction(ChooseAction(t, targets));
        }

        protected virtual String movement(String input)
        {
            if (properties.Contains("LegInjury")) return "Limping";
            else return input;
        }

        public virtual String ChooseAction(GameTime t, Dictionary<String, Actor>.ValueCollection actor_list)
        {

            foreach (AIOption AIO in AIStance) {
                if (AIO.condition.Condition) return AIO.action.ActionReturn();

            }


            return "";
        }

        public virtual void ActionUpdate(GameTime t, Dictionary<String, Actor>.ValueCollection actor_list)
        { current_Action.Update(t, actor_list); }

        public virtual void Move(Vector3 update) // Changes location of actor.
        {
            Location += update;
        }


        protected virtual String GetAIAction()
        {
            foreach (AIOption AIO in AIStance) {
                if (AIO.condition.Condition) return (AIO.action.ActionReturn());

            }
            return "Walking";
        }

        public bool HasProperty(String Property)
        {
            return properties.Contains(Property);
        }


        public virtual void AddAIOption(AIOption next)
        {
            AIStance.Add(next);
        }
    }
}