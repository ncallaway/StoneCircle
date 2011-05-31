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
        private Texture2D terrainPallette;
        private Vector2 position;
        public int MaxX;
        public int MaxY;

        internal String Id { get { return id; } }

        Dictionary<String, Actor> exists = new Dictionary<String, Actor>();
        public Dictionary<String, Actor>.ValueCollection Actors { get { return exists.Values; } }

        List<LightSource> lights = new List<LightSource>();
        List<Trigger> triggers = new List<Trigger>();
        public InputController input = new InputController();
        public Camera camera;

        private GameManager gameManager;

        private float frame;
        private float time;

        private VertexPositionTexture[] ScreenVertices = new VertexPositionTexture[3];
 

        [XmlIgnoreAttribute]
        public Player player;
        Stack<Menu> activeMenus = new Stack<Menu>();
        

        public String BGMTitle;
        public Vector3 AMBColor;
        public float AMBStrength;

        protected int regionsWide;
        protected int regionsHigh;
        protected Texture2D[,] regions;

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
        Effect TerrainMapper;
        [XmlIgnoreAttribute]
        Effect lightSourceShader;
        [XmlIgnoreAttribute]
        Effect statusShader;

        

        Texture2D DeathScreen;
        Texture2D LightMap;

        public Stage()
        {
            regionsWide = 1;
            regionsHigh = 1;

            position = new Vector2(0, 0);
            MaxX = regionsWide * 8192;
            MaxY = regionsHigh * 8192;
            camera = new Camera(this, input);
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = 0;
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
            regionsWide = 1;
            regionsHigh = 1;

            this.objectId = IdFactory.GetNextId();
            this.id = id;
            position = new Vector2(0, 0);
            MaxX = regionsWide * 8192;
            MaxY = regionsHigh * 8192;
            camera = new Camera(this, input);
            this.SM = SM;
            BGMTitle = "FlowerWaltz";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            AM = new AudioManager();
            loaded = false;
            CreateScreenRenderObject();
        }

        public Stage(GameManager gameManager)
        {
            regionsWide = 1;
            regionsHigh = 1;

            this.gameManager = gameManager;
            this.SM = gameManager.StageManager;
            this.AM = gameManager.AudioManager;
            position = new Vector2(0, 0);
            MaxX = regionsWide * 8192;
            MaxY = regionsHigh * 8192;
            camera = new Camera(this, input);

            regionsWide = 1;
            regionsHigh = 1;
         
            BGMTitle = "FlowerWaltz";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            loaded = false;

            CreateScreenRenderObject();
        }

        public void adjustRegionCounts(int x, int y)
        {
            regionsWide = x;
            regionsHigh = y;

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
                terrainPallette = CM.Load<Texture2D>("texturePallette");
                font = CM.Load<SpriteFont>("Fonts/Text");

                player.loadImage(CM);
                TerrainMapper = CM.Load<Effect>("Shaders/TerrainMapper");
                lightSourceShader = CM.Load<Effect>("Shaders/Effect1");
                statusShader = CM.Load<Effect>("Shaders/StatusShader");
                AM.Load(CM);
                foreach (Actor x in exists.Values) x.loadImage(CM);
                loaded = true;
                foreach (Lines D in openConversations) D.Load(CM);
                openConversations.Clear();
                DeathScreen = CM.Load<Texture2D>("RedScreenOfDeath");
                LightMap = CM.Load<Texture2D>("LightMapBase");
                regions = new Texture2D[regionsWide, regionsHigh];
                for (int i = 0; i < regionsWide; i++)
                {
                    for (int j = 0; j < regionsHigh; j++)
                    {
                        regions[i, j] = CM.Load<Texture2D>("Levels/" + id + "_" + i + "_" + j + "IMG");

                    }
                }
                
            }
        }


        public void addActor(String id, String asset_name, Vector2 starting)
        {
            Actor temp = new Actor(id, asset_name, starting);
            exists.Add(id, temp);
            temp.parent = this;
        }

        public void AddTrigger(Trigger newT) { triggers.Add(newT); }

        public void AddActor(String id)
        {   Actor temp = SM.MainCharacters[id];
            exists.Add(id, temp);
            temp.parent = this;

        }

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
            addPlayer(Player);
        }

        public void addPlayer(Player Player)
        {
            player = Player;
            player.parent = this;
            exists.Add("Player", player);
        }

        public void RemovePlayer() { exists.Remove("Player"); }

        public void CreateScreenRenderObject()
        {
            ScreenVertices[0].Position = Vector3.Zero;
            ScreenVertices[0].TextureCoordinate = Vector2.Zero;
            ScreenVertices[1].Position = new Vector3(1,1,0);
            ScreenVertices[1].TextureCoordinate = Vector2.One;
            ScreenVertices[2].Position = new Vector3(0,1,0);
            ScreenVertices[2].TextureCoordinate = Vector2.UnitY;
            
                        
        }


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

        public void Draw(GraphicsDevice device, SpriteBatch theSpriteBatch, RenderTarget2D heightMap)
        {   
            frame ++;


            device.SetRenderTarget(heightMap);
            device.Textures[1] = terrainPallette;
            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, null, null, null, TerrainMapper);
            
            for (int i = 0; i < regionsWide; i++)
            {
                for (int j = 0; j < regionsHigh; j++)
                {
                    theSpriteBatch.Draw(regions[i, j], (4096 * new Vector2(i, j) - camera.Location) * camera.Scale + camera.screenadjust, Color.White);
                 
                }

            } theSpriteBatch.End();

            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null,DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);

            foreach (Actor y in exists.Values)
            {

             
            Vector2 drawable = y.Position - camera.Location;
            if (Math.Abs(drawable.X) < 1.2 * camera.screenadjust.X && Math.Abs(drawable.Y) < 1.5 * camera.screenadjust.Y)
            {
                y.Draw(theSpriteBatch, camera.Location, camera.Scale, 1f, font);
                foreach (LightSource x in lights)
                {
                    float rotation = x.calcRotate(y);
                    float intensity = x.calcIntensity(y);
                    y.DrawShadow(theSpriteBatch, camera.Location, camera.Scale, rotation, intensity);
                }

            }
            }

            theSpriteBatch.End();
            device.SetRenderTarget(null);

            

            device.Textures[2] = LightMap;

            theSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, lightSourceShader);
            theSpriteBatch.Draw(heightMap, Vector2.Zero, Color.White);
            theSpriteBatch.End();

            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, statusShader);
            theSpriteBatch.Draw(DeathScreen, Vector2.Zero, Color.White);
            theSpriteBatch.End();


            theSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
            foreach (Lines D in openConversations) D.Draw(theSpriteBatch, camera.Location, camera.Scale);

            theSpriteBatch.DrawString(font, "" + player.Location, 200 * Vector2.One, Color.White);

            theSpriteBatch.DrawString(font, "" + time, 200 * Vector2.One + 10 * Vector2.UnitY, Color.White);
            theSpriteBatch.End();

        }


        public void Update(GameTime t)
        {
            time += (float) t.ElapsedGameTime.Seconds + (float) t.ElapsedGameTime.Milliseconds / 1000;
            Vector2[] LPosition = new Vector2[6];
            float[] Radius = new float[6];
            foreach (LightSource l in lights)
            {
                l.Update(t);
                Vector2 temp = camera.screenadjust + camera.Scale * (l.Location - camera.Location);
                temp.X /= 1366; temp.Y /= 768 * 2;
                LPosition[lights.IndexOf(l)] = temp;
                Radius[lights.IndexOf(l)] = l.Radius * camera.Scale;
            }
            Vector2 tempPlayer = (player.Position - camera.Location) * camera.Scale + camera.screenadjust;
            tempPlayer.X /= 1366; tempPlayer.Y /= 768;
            statusShader.Parameters["health"].SetValue(player.CurrentLife / player.TotalLife * .707f);
            statusShader.Parameters["fatigue"].SetValue(player.CurrentFatigue / player.TotalFatigue * .707f);
           
            lightSourceShader.Parameters["Position"].SetValue(LPosition);
            lightSourceShader.Parameters["index"].SetValue(lights.Count);
             lightSourceShader.Parameters["Radius"].SetValue(Radius);
            lightSourceShader.Parameters["AMBColor"].SetValue(AMBColor);
            
            lightSourceShader.Parameters["AMBStrength"].SetValue(AMBStrength);
            lightSourceShader.Parameters["GLOColor"].SetValue(new Vector3(1f, .6f, -.1f));
          
          

            input.Update();
            if (input.IsPauseMenuNewlyPressed()) gameManager.UIManager.Pause();

            List<EVENT> finishedEvents = new List<EVENT>();
            foreach (EVENT E in currentEvents) { if (E.Update(t)) finishedEvents.Add(E); }
            foreach (EVENT E in finishedEvents) { E.End(); currentEvents.Remove(E); }
            
            foreach (Actor x in exists.Values) // This will update all the actors, it makes sure that nobody leaves or moves through anybody else.
            {
                if (x.Active) { x.Update(t, exists.Values); }
                //if ((int)x.Location.X < x.ImageWidth / 2) x.Location.X = x.ImageWidth / 2;
                //if ((int)x.Location.X > MaxX - x.ImageWidth / 2) x.Location.X = MaxX - x.ImageWidth / 2;
                //if ((int)x.Location.Y < x.ImageHeight) x.Location.Y = x.ImageHeight;
                //if ((int)x.Location.Y > MaxY) x.Location.Y = MaxY;


            }
            if (camera.Active) camera.Update(t); //Updates the camera's position. 


            AM.Update(player.Location);
            foreach (Trigger T in triggers) if (T.CheckCondition()) { T.UpdateAvailability(); RunEvent(T.Target); }

        }

        public void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            writer.Write(id);
            writer.Write(MaxX);
            writer.Write(MaxY);
            writer.Write(AMBStrength);
            Saver.SaveVector3(AMBColor, writer);

            writer.Write(regionsWide);
            writer.Write(regionsHigh);

            List<ISaveable> eventsList = new List<ISaveable>();
            foreach (KeyValuePair<String, EVENT> pair in events) {
                if (pair.Key != pair.Value.ID)
                {
                    pair.Value.ID = pair.Key;
                }
                eventsList.Add(pair.Value);
            }

            List<ISaveable> actorsList = new List<ISaveable>();
            foreach (KeyValuePair<String, Actor> pair in exists)
            {
                if (pair.Key != pair.Value.Name)
                {
                    pair.Value.Name = pair.Key;
                }
                actorsList.Add(pair.Value);
            }

            Saver.SaveSaveableList(eventsList, writer, objectTable);

            Saver.SaveSaveableList<Lines>(openConversations, writer, objectTable);

            Saver.SaveSaveableList(actorsList, writer, objectTable);
        }

        private StageInflatables inflatables;

        public void Load(BinaryReader reader, SaveType type)
        {
            id = reader.ReadString();
            MaxX = reader.ReadInt32();
            MaxY = reader.ReadInt32();
            AMBStrength = reader.ReadSingle();
            AMBColor = Loader.LoadVector3(reader);

            regionsWide = reader.ReadInt32();
            regionsHigh = reader.ReadInt32();

            inflatables = new StageInflatables();

            inflatables.eventsList = Loader.LoadSaveableList(reader);
            inflatables.openConversationsList = Loader.LoadSaveableList(reader);
            inflatables.actorsList = Loader.LoadSaveableList(reader);
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            if (inflatables != null)
            {
                events = new Dictionary<String, EVENT>();
                foreach (uint objectId in inflatables.eventsList)
                {
                    EVENT inflatedEVENT = (EVENT)objectTable[objectId];
                    if (inflatedEVENT != null)
                    {
                        events.Add(inflatedEVENT.ID, inflatedEVENT);
                    }
                }

                exists = new Dictionary<String, Actor>();
                foreach (uint objectId in inflatables.actorsList)
                {
                    Actor inflatedActor = (Actor)objectTable[objectId];
                    if (inflatedActor != null)
                    {
                        exists.Add(inflatedActor.Name, inflatedActor);
                    }
                }
                openConversations = Loader.InflateSaveableList<Lines>(inflatables.openConversationsList, objectTable);
            }
        }

        private class StageInflatables
        {
            public List<uint> eventsList;
            public List<uint> openConversationsList;
            public List<uint> actorsList;
        }

        public void FinishLoad(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.SM = gameManager.StageManager;
            this.AM = gameManager.AudioManager;
            camera = new Camera(this, input);
        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> refs = new List<ISaveable>();
            foreach (EVENT s in events.Values)
            {
                refs.Add(s);
            }
            foreach (Lines l in openConversations)
            {
                refs.Add(l);
            }
            foreach (Actor a in exists.Values)
            {
                refs.Add(a);
            }
            return refs;
        }

        public uint GetId()
        {
            return objectId;
        }


        
    }
}