using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UserMenus;

namespace StoneCircle
{

    [Serializable]
    class Stage
    {
        private String id;
        [NonSerialized] private Texture2D background;
        private Vector2 position;
        public int max_X;
        public int max_Y;

        [NonSerialized] Dictionary<String, Actor> exists = new Dictionary<String, Actor>();
        public Dictionary<String, Actor>.ValueCollection Actors { get { return exists.Values; } }
        [NonSerialized] List<LightSource> lights = new List<LightSource>();
        [NonSerialized] List<Trigger> triggers = new List<Trigger>();
        [NonSerialized] public InputController input = new InputController();
        [NonSerialized] public Camera camera;

        [NonSerialized] private Texture2D terrain;
        [NonSerialized] private GameManager gameManager;

        [XmlIgnoreAttribute]
        [NonSerialized] public Player player;
        [NonSerialized] Stack<Menu> activeMenus = new Stack<Menu>();


        [NonSerialized] public String BGMTitle;
        [NonSerialized] public Vector3 AMBColor;
        [NonSerialized] public float AMBStrength;


        [XmlIgnoreAttribute]
        [NonSerialized] public SpriteFont font;
        [NonSerialized] Dictionary<String, Lines> conversations = new Dictionary<String, Lines>();
        [NonSerialized] List<Lines> openConversations = new List<Lines>();
        [NonSerialized] Event currentEvent;

        [XmlIgnoreAttribute]
        [NonSerialized] public Dictionary<String, Event> events = new Dictionary<String, Event>();

        [NonSerialized] public AudioManager AM;
        [XmlIgnoreAttribute]
        [NonSerialized] public ContentManager CM;
        [XmlIgnoreAttribute]
        [NonSerialized] public StageManager SM;
        [NonSerialized] private bool loaded;
        // LightSource sun = new LightSource("sun", new Vector2(200000, 10000), 3000000f, null, null);

        [XmlIgnoreAttribute]
        [NonSerialized] Effect ambientLightShader;
        [XmlIgnoreAttribute]
        [NonSerialized] Effect lightSourceShader;


        public Stage()
        {
            position = new Vector2(0, 0);
            max_X = 4000;
            max_Y = 4000;
            camera = new Camera(this, input);
            BGMTitle = "LXD";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            AM = new AudioManager();
            loaded = false;
        }


        public Stage(String id, StageManager SM)
        {
            position = new Vector2(0, 0);
            max_X = 4000;
            max_Y = 4000;
            camera = new Camera(this, input);
            this.SM = SM;
            BGMTitle = "LXD";
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
            
            BGMTitle = "LXD";
            AMBColor = new Vector3(1f, 1f, .4f);
            AMBStrength = .8f;
            loaded = false;
        }

        public void Initialize()
        {
            foreach (Actor A in exists.Values) A.Initialize();
            AM.SetSong(BGMTitle);
            if (currentEvent != null) currentEvent.Start();

        }


        public void Load(ContentManager CM)
        {
            if (!loaded) {
                this.CM = CM;
                terrain = CM.Load<Texture2D>("Grass");
                font = CM.Load<SpriteFont>("Text");
                player.loadImage(CM);
                ambientLightShader = CM.Load<Effect>("AmbientLight");
                lightSourceShader = CM.Load<Effect>("Effect1");
                AM.Load(CM);
                foreach (Actor x in exists.Values) x.loadImage(CM);
                loaded = true;
                foreach (Lines D in conversations.Values) D.Load(CM);

            }
        }


        public void addActor(String id, String asset_name, Vector2 starting)
        {
            Actor temp = new Actor(id, asset_name, starting);
            exists.Add(id, temp);

        }

        public void AddTrigger(Trigger newT) { triggers.Add(newT); }

        public void AddLines(Lines dialouge) { conversations.Add(dialouge.CallID, dialouge); }

        public void AddEvent(Event add)
        {
            events.Add(add.ID, add);
        }

        public void AddEvent(String callID, Event add)
        {
            events.Add(callID, add);
        }
        public void RunEvent(String next)
        {
            if (events.ContainsKey(next)) { currentEvent = events[next]; currentEvent.Start(); } else currentEvent = null;

        }

        public void RunLine(String dialouge)
        {
            if (conversations.ContainsKey(dialouge)) {
                conversations[dialouge].Start();
                openConversations.Add(conversations[dialouge]);
            }
        }



        public void RemoveDialogue(Lines dialogue) { openConversations.Remove(dialogue); }

        public void RemoveDialogue(String dialogue) { openConversations.Remove(conversations[dialogue]); }

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
            exists.Add(id, actor);
        }

        public Actor GetActor(String actor)
        {
            return exists[actor];
        }

        public void removeActor(String target) { exists.Remove(target); }

        public void removeLight(LightSource light) { lights.Remove(light); }





        public void setCamera() { camera.setSubject(player); }



        public void Draw(GraphicsDevice device, SpriteBatch theSpriteBatch, RenderTarget2D shadeTemp)
        {

            device.SetRenderTarget(0, shadeTemp);

            theSpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            device.Clear(Color.Ivory);
            for (int i = 0; i < 40; i++) {
                for (int j = 0; j < 40; j++)
                    theSpriteBatch.Draw(terrain, ((new Vector2(i * terrain.Width, j * terrain.Height)) - camera.Location) * camera.Scale + camera.screenadjust, new Rectangle(0, 0, terrain.Width, terrain.Height), Color.White, 0f, new Vector2(0, 0), camera.Scale, SpriteEffects.None, 1f);
            }

            foreach (Actor y in exists.Values) {
                y.Draw(theSpriteBatch, camera.Location, camera.Scale, 1f, font);
                // float rotation = sun.calcRotate(y);
                // float intensity = sun.calcIntensity(y);
                // y.DrawShadow(theSpriteBatch, camera.Location, camera.Scale, rotation, intensity);

                foreach (LightSource x in lights) {
                    float rotation = x.calcRotate(y);
                    float intensity = x.calcIntensity(y);
                    y.DrawShadow(theSpriteBatch, camera.Location, camera.Scale, rotation, intensity);
                }


            }

            theSpriteBatch.End();
            device.SetRenderTarget(0, null);
            Texture2D ShaderTexture = shadeTemp.GetTexture();
            //device.SetRenderTarget(0, shadeTemp);
            theSpriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            lightSourceShader.Begin();

            lightSourceShader.CurrentTechnique.Passes[0].Begin();
            theSpriteBatch.Draw(ShaderTexture, Vector2.Zero, Color.White);
            theSpriteBatch.End();


            lightSourceShader.CurrentTechnique.Passes[0].End();
            lightSourceShader.End();

            theSpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            foreach (Lines D in openConversations) D.Draw(theSpriteBatch, camera.Location, camera.Scale);
            theSpriteBatch.End();

        }


        public void Update(GameTime t)
        {
            Vector2[] LPosition = new Vector2[6];
            float[] Radius = new float[6];
            foreach (LightSource l in lights) {
                l.Update(t);
                Vector2 temp = l.Location + camera.screenadjust - camera.Location;
                temp.X /= 1366; temp.Y /= 768;
                LPosition[lights.IndexOf(l)] = temp;
                Radius[lights.IndexOf(l)] = l.Radius;
            }
            Vector2 tempPlayer = (player.Position - camera.Location) * camera.Scale + camera.screenadjust;
            tempPlayer.X /= 1366; tempPlayer.Y /= 768;
            lightSourceShader.Parameters["health"].SetValue(player.CurrentLife / player.TotalLife * .707f);
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


            List<Lines> finished = new List<Lines>();
            foreach (Lines D in openConversations) if (D.Update(t)) finished.Add(D);
            foreach (Lines D in finished) { if (D.NextLine != null) RunLine(D.NextLine); RemoveDialogue(D); }

            if (currentEvent != null) { if (currentEvent.Update(t)) currentEvent = null; }
                      
                foreach (Actor x in exists.Values) // This will update all the actors, 
                //  it makes sure that nobody leaves or moves through anybody else.
                {
                    if (x.Active)  { x.Update(t, exists.Values);  }
                    if ((int)x.Location.X < x.ImageWidth / 2) x.Location.X = x.ImageWidth / 2;
                    if ((int)x.Location.X > max_X - x.ImageWidth / 2) x.Location.X = max_X - x.ImageWidth / 2;
                    if ((int)x.Location.Y < x.ImageHeight) x.Location.Y = x.ImageHeight;
                    if ((int)x.Location.Y > max_Y) x.Location.Y = max_Y;


                }
                if(camera.Active) camera.Update(t); //Updates the camera's position. 
            

            AM.Update(player.Location);
            foreach (Trigger T in triggers) if (T.CheckCondition()) { T.UpdateAvailability(); RunEvent(T.Target); }

        }




    }
}