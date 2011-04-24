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

    class Stage
    {
        private String id;
        private Texture2D background;
        private Vector2 position;
        public int max_X;
        public int max_Y;

        Dictionary<String, Actor> exists = new Dictionary<String, Actor>();
        public Dictionary<String, Actor>.ValueCollection Actors { get { return exists.Values; } }
        List<LightSource> lights = new List<LightSource>();
        List<TriggerBox> triggers = new List<TriggerBox>();
        public InputController input = new InputController();
        public Camera camera;
        
        private Texture2D terrain;

        [XmlIgnoreAttribute]
        public Player player;
        Stack<Menu> activeMenus = new Stack<Menu>();
        

        public String BGMTitle;
        public Vector3 AMBColor;
        public float AMBStrength;


        [XmlIgnoreAttribute]
        public SpriteFont font;
        Dictionary<String, Lines> conversations = new Dictionary<String, Lines>();
        List<Lines> openConversations = new List<Lines>();
        EventGroup currentEvent;

        [XmlIgnoreAttribute]
        public Dictionary<String, EventGroup> events = new Dictionary<String, EventGroup>();

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

        public Stage(StageManager SM)
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

        public void Initialize()
        {
            foreach (Actor A in exists.Values) A.Initialize();
            AM.SetSong(BGMTitle);
            if (currentEvent != null) currentEvent.Start();           
            
        }


        public void Load( ContentManager CM)
        {
            if (!loaded)
            {
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

        public void AddTrigger(TriggerBox newT) { triggers.Add(newT); }

        public void AddLines(Lines dialouge){conversations.Add(dialouge.CallID, dialouge);}

        public void AddEvent(EventGroup add)
        {
            events.Add(add.ID, add);
        }
        public void RunEvent(String next)
        {
            if (events.ContainsKey(next)) { currentEvent = events[next]; currentEvent.Start(); }
            else currentEvent = null;

        }
    
        public void RunLine(String dialouge)        {            if (conversations.ContainsKey(dialouge)) 
        { conversations[dialouge].Start(); 
            openConversations.Add(conversations[dialouge]); }   
        }



        public void RemoveDialogue(Lines dialogue)        {            openConversations.Remove(dialogue);        }

        public void RemoveDialogue(String dialogue) { openConversations.Remove(conversations[dialogue]); }

        public void addPlayer(String id, String asset_name, Vector2 starting)        {            addPlayer(new Player(id, asset_name, starting, this, input), starting);        }


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
            LightSource temp = new LightSource(id,starting,radius, this, lightSourceShader);
            addLight(temp);
        }

        public void addLight(LightSource light){ lights.Add(light);}

        public void addActor(String id, Actor actor)
        {
            exists.Add(id, actor);   
        }

        public Actor GetActor(String actor)
        {
            return exists[actor];
        }

        public void removeActor(String target)        { exists.Remove(target); }

        public void removeLight(LightSource light)     {   lights.Remove(light);    }

       


        
        public void setCamera()   {            camera.setSubject(player);        }



        public void Draw(GraphicsDevice device, SpriteBatch theSpriteBatch, RenderTarget2D shadeTemp)
        {

           device.SetRenderTarget(0, shadeTemp);
                     
            theSpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            device.Clear(Color.Ivory);
            for (int i = 0; i < 40; i++)
                {
                    for (int j = 0; j < 40; j++)
                        theSpriteBatch.Draw(terrain, ((new Vector2(i * terrain.Width, j * terrain.Height)) - camera.Location) * camera.Scale + camera.screenadjust, new Rectangle(0, 0, terrain.Width, terrain.Height), Color.White, 0f, new Vector2(0, 0), camera.Scale, SpriteEffects.None, 1f);
                }

                foreach (Actor y in exists.Values)
                {
                    y.Draw(theSpriteBatch, camera.Location, camera.Scale, 1f, font);
                   // float rotation = sun.calcRotate(y);
                   // float intensity = sun.calcIntensity(y);
                   // y.DrawShadow(theSpriteBatch, camera.Location, camera.Scale, rotation, intensity);
              
                    foreach (LightSource x in lights)
                    {
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
            Vector2 tempPlayer = (player.Position - camera.Location)*camera.Scale + camera.screenadjust;
            tempPlayer.X /= 1366; tempPlayer.Y /= 768;
               lightSourceShader.Parameters["health"].SetValue(player.CurrentLife/player.TotalLife*.707f);
               lightSourceShader.Parameters["Position"].SetValue(LPosition);
               lightSourceShader.Parameters["index"].SetValue(lights.Count);
               lightSourceShader.Parameters["player"].SetValue(tempPlayer);
               lightSourceShader.Parameters["Radius"].SetValue(Radius);
               lightSourceShader.Parameters["AMBColor"].SetValue(AMBColor);
               lightSourceShader.Parameters["AMBStrength"].SetValue(AMBStrength);
               lightSourceShader.Parameters["GLOColor"].SetValue(new Vector3(1f, .6f, -.1f));
               lightSourceShader.Parameters["fatigue"].SetValue(player.CurrentFatigue / player.TotalFatigue * .707f);
           
            input.Update();
            if (input.IsPauseMenuNewlyPressed()) SM.GM.UIManager.Pause();


            List<Lines> finished = new List<Lines>();
            foreach (Lines D in openConversations) if (D.Update(t)) finished.Add(D);
            foreach (Lines D in finished) { if(D.NextLine!=null) RunLine(D.NextLine); RemoveDialogue(D); }

            if (currentEvent != null) { if (currentEvent.Update(t)) RunEvent(currentEvent.NextEvent); }
            else
            {
                foreach (Actor x in exists.Values) // This will update all the actors, 
                //  it makes sure that nobody leaves or moves through anybody else.
                {
                    x.Update(t, exists.Values);
                    if ((int)x.Location.X < x.ImageWidth / 2) x.Location.X = x.ImageWidth / 2;
                    if ((int)x.Location.X > max_X - x.ImageWidth / 2) x.Location.X = max_X - x.ImageWidth / 2;
                    if ((int)x.Location.Y < x.ImageHeight) x.Location.Y = x.ImageHeight;
                    if ((int)x.Location.Y > max_Y) x.Location.Y = max_Y;


                }
                camera.Update(t); //Updates the camera's position. 
            }

                AM.Update(player.Location);
                foreach (TriggerBox T in triggers) T.Update(t, exists["Player"]);
             
            }


        

    }
}