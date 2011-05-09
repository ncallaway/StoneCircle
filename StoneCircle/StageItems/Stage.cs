using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UserMenus;

using StoneCircle.Persistence;

namespace StoneCircle
{


    class Stage : ISaveable
    {
        private uint objectId;

        private String id;
        private Texture2D background;
        private Vector2 position;
        public int max_X;
        public int max_Y;

        internal String Id { get { return id; } }

        Dictionary<String, Actor> exists = new Dictionary<String, Actor>();
        public Dictionary<String, Actor>.ValueCollection Actors { get { return exists.Values; } }
        List<LightSource> lights = new List<LightSource>();
        List<Trigger> triggers = new List<Trigger>();
        public InputController input = new InputController();
        public Camera camera;

        private Texture2D terrain;
        private GameManager gameManager;

        [XmlIgnoreAttribute]
        public Player player;
        Stack<Menu> activeMenus = new Stack<Menu>();


        public String BGMTitle;
        public Vector3 AMBColor;
        public float AMBStrength;


        [XmlIgnoreAttribute]
        
        public SpriteFont font;
        List<Lines> openConversations = new List<Lines>();
        List<EVENT> currentEvents = new List<EVENT>();
        

        [XmlIgnoreAttribute]
        public Dictionary<String, EVENT> events = new Dictionary<String, EVENT>();

        public AudioManager AM;
        [XmlIgnoreAttribute]
        public ContentManager CM;
        [XmlIgnoreAttribute]
        public StageManager SM;
        private bool loaded;
        // LightSource sun = new LightSource("sun", new Vector2(200000, 10000), 3000000f, null, null);

        [XmlIgnoreAttribute]
        Effect ambientLightShader;
        [XmlIgnoreAttribute]
        Effect lightSourceShader;
        [XmlIgnoreAttribute]

        Effect statusShader;

        Texture2D DeathScreen;


        public Stage()
        {
            position = new Vector2(0, 0);
            max_X = 4000;
            max_Y = 4000;
            camera = new Camera(this, input);
            BGMTitle = "FlowerWaltz";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            AM = new AudioManager();
            loaded = false;
        }

        public Stage(uint id)
        {
            this.objectId = id;
            IdFactory.MoveNextIdPast(id);
        }


        public Stage(String id, StageManager SM)
        {
            this.objectId = IdFactory.GetNextId();
            this.id = id;
            position = new Vector2(0, 0);
            max_X = 4000;
            max_Y = 4000;
            camera = new Camera(this, input);
            this.SM = SM;
            BGMTitle = "FlowerWaltz";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            AM = new AudioManager();
            loaded = false;
        }

        public Stage(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.SM = gameManager.StageManager;
            this.AM = gameManager.AudioManager;
            position = new Vector2(0, 0);
            max_X = 4000;
            max_Y = 4000;
            camera = new Camera(this, input);

            BGMTitle = "FlowerWaltz";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            loaded = false;
        }

        public void Initialize()
        {
            foreach (Actor A in exists.Values) A.Initialize();
            AM.SetSong(BGMTitle);
            foreach (EVENT E in currentEvents) E.Start();

        }


        public void Load(ContentManager CM)
        {
            if (!loaded)
            {
                this.CM = CM;
                terrain = CM.Load<Texture2D>("Grass");
                font = CM.Load<SpriteFont>("Text");
                player.loadImage(CM);
                ambientLightShader = CM.Load<Effect>("AmbientLight");
                lightSourceShader = CM.Load<Effect>("Effect1");
                statusShader = CM.Load<Effect>("AmbientLight");
                AM.Load(CM);
                foreach (Actor x in exists.Values) x.loadImage(CM);
                loaded = true;
                foreach (Lines D in openConversations) D.Load(CM);
                openConversations.Clear();
                DeathScreen = CM.Load<Texture2D>("RedScreenOfDeath");

            }
        }


        public void addActor(String id, String asset_name, Vector2 starting)
        {
            Actor temp = new Actor(id, asset_name, starting);
            exists.Add(id, temp);
            temp.parent = this;
        }

        public void AddTrigger(Trigger newT) { triggers.Add(newT); }

        public void AddEVENT(EVENT add)
        {
            events.Add(add.ID, add);
        }

        public void AddEVENT(String callID, EVENT add)
        {
            events.Add(callID, add);
        }
        public void RunEvent(String next)
        {
            if (events.ContainsKey(next)) { EVENT eventInstance = (events[next]); eventInstance.Reset(); eventInstance.Start();  currentEvents.Add(eventInstance); }

        }

        public void StartLine(Lines dialogue) { openConversations.Add(dialogue); }
        public void StopLine(Lines dialogue) { openConversations.Remove(dialogue); }

        public void addPlayer(String id, String asset_name, Vector2 starting) { addPlayer(new Player(id, asset_name, starting, gameManager, input), starting); }


        public void addPlayer(Player Player, Vector2 starting)
        {
            player = Player;
            player.Location = new Vector3(starting.X, starting.Y, 1);
            player.parent = this;
            exists.Add("Player", player);
        }

        public void RemovePlayer() { exists.Remove("Player"); }


        public void addLight(String id, Vector2 starting, float radius)
        {
            LightSource temp = new LightSource(id, starting, radius, this, lightSourceShader);
            addLight(temp);
        }

        public void addLight(LightSource light) { lights.Add(light); }

        public void addActor(String id, Actor actor)
        {
            actor.parent = this;
            exists.Add(id, actor);
        }

        public void AddActor(Actor actor, Vector2 starting)
        {
            actor.parent = this;
            exists.Add(actor.Name, actor);
            actor.Location = new Vector3(starting, 0);
        }

        public Actor GetActor(String actor)
        {
            return exists[actor];
        }

        public void removeActor(String target) { exists[target].UnInitialize(); exists.Remove(target); }

        public void removeLight(LightSource light) { lights.Remove(light); }

        public void setCamera() { camera.setSubject(player); }

        public void Draw(GraphicsDevice device, SpriteBatch theSpriteBatch, RenderTarget2D shadeTemp)
        {

            device.SetRenderTarget(shadeTemp);
            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);

            device.Clear(Color.Ivory);
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                    theSpriteBatch.Draw(terrain, ((new Vector2(i * terrain.Width, j * terrain.Height)) - camera.Location) * camera.Scale + camera.screenadjust, new Rectangle(0, 0, terrain.Width, terrain.Height), Color.White, 0f, new Vector2(0, 0), camera.Scale, SpriteEffects.None, 1f);
            }

            foreach (Actor y in exists.Values)
            {
                y.Draw(theSpriteBatch, camera.Location, camera.Scale, 1f, font);


                foreach (LightSource x in lights)
                {
                    float rotation = x.calcRotate(y);
                    float intensity = x.calcIntensity(y);
                    y.DrawShadow(theSpriteBatch, camera.Location, camera.Scale, rotation, intensity);
                }


            }

            theSpriteBatch.End();
            device.SetRenderTarget(null);
            theSpriteBatch.Begin(0, BlendState.Opaque, null, null, null, lightSourceShader);
            theSpriteBatch.Draw(shadeTemp, Vector2.Zero, Color.White);
            theSpriteBatch.End();
            theSpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, statusShader);
            theSpriteBatch.Draw(DeathScreen, Vector2.Zero, Color.White);
            theSpriteBatch.End();
            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);
            foreach (Lines D in openConversations) D.Draw(theSpriteBatch, camera.Location, camera.Scale);
            theSpriteBatch.End();

        }


        public void Update(GameTime t)
        {   
            Vector2[] LPosition = new Vector2[6];
            float[] Radius = new float[6];
            foreach (LightSource l in lights)
            {
                l.Update(t);
                Vector2 temp = camera.screenadjust + camera.Scale * (l.Location - camera.Location);
                temp.X /= 1366; temp.Y /= 768;
                LPosition[lights.IndexOf(l)] = temp;
                Radius[lights.IndexOf(l)] = l.Radius * camera.Scale;
            }
            Vector2 tempPlayer = (player.Position - camera.Location) * camera.Scale + camera.screenadjust;
            tempPlayer.X /= 1366; tempPlayer.Y /= 768;
            statusShader.Parameters["health"].SetValue(player.CurrentLife / player.TotalLife * .707f);
            lightSourceShader.Parameters["Position"].SetValue(LPosition);
            lightSourceShader.Parameters["index"].SetValue(lights.Count);
            lightSourceShader.Parameters["player"].SetValue(tempPlayer);
            lightSourceShader.Parameters["Radius"].SetValue(Radius);
            lightSourceShader.Parameters["AMBColor"].SetValue(AMBColor);
            lightSourceShader.Parameters["AMBStrength"].SetValue(AMBStrength);
            lightSourceShader.Parameters["GLOColor"].SetValue(new Vector3(1f, .6f, -.1f));
            lightSourceShader.Parameters["fatigue"].SetValue(player.CurrentFatigue / player.TotalFatigue * .707f);

            input.Update();
            if (input.IsPauseMenuNewlyPressed()) gameManager.UIManager.Pause();

            List<EVENT> finishedEvents = new List<EVENT>();
            foreach (EVENT E in currentEvents) { if (E.Update(t)) finishedEvents.Add(E); }
            foreach (EVENT E in finishedEvents) { E.End(); currentEvents.Remove(E); }

            foreach (Actor x in exists.Values) // This will update all the actors, 
            //  it makes sure that nobody leaves or moves through anybody else.
            {
                if (x.Active) { x.Update(t, exists.Values); }
                if ((int)x.Location.X < x.ImageWidth / 2) x.Location.X = x.ImageWidth / 2;
                if ((int)x.Location.X > max_X - x.ImageWidth / 2) x.Location.X = max_X - x.ImageWidth / 2;
                if ((int)x.Location.Y < x.ImageHeight) x.Location.Y = x.ImageHeight;
                if ((int)x.Location.Y > max_Y) x.Location.Y = max_Y;


            }
            if (camera.Active) camera.Update(t); //Updates the camera's position. 


            AM.Update(player.Location);
            foreach (Trigger T in triggers) if (T.CheckCondition()) { T.UpdateAvailability(); RunEvent(T.Target); }

        }

        public void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            writer.Write(id);
        }

        public void Load(BinaryReader reader, SaveType type)
        {
            id = reader.ReadString();
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            /* no-op*/
        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            return null;
        }

        public uint GetId()
        {
            return objectId;
        }
    }
}