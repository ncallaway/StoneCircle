using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using UserMenus;

namespace StoneCircle
{
    class GameManager
    {

        public AudioManager AudioManager { get { return audioManager; } }
        public UIManager UIManager { get { return uiManager; } }
        public ContentManager ContentManager { get { return contentManager; } }
        public StageManager StageManager { get { return stageManager; } }

        public Player player;
        public Camera Camera;

        private AudioManager audioManager;
        private UIManager uiManager;
        private ContentManager contentManager;
        private StageManager stageManager;
        
        public GameManager(ContentManager cM)
        {
            contentManager = cM;
            player = new Player("Player", "male_select", Vector2.Zero, new InputController(InputController.InputMode.Player1));
            
            Camera = new Camera(player, new InputController());
            
            uiManager = new UIManager(this, ContentManager);
            stageManager = new StageManager(ContentManager, this);
        }

        public void LoadContent()
        {
            UIManager.Load(ContentManager);
            StageManager.SetStage("region1");
        }

        public void Initialize()
        {
            StageManager.Initialize();
            AudioManager.Initialize();
            UIManager.Initialize();


        }

        public void Update(GameTime T)
        {   
            UIManager.Update(T);
           if(!UIManager.OpenMenus) StageManager.Update(T);
          
        }

        public void Draw(GraphicsDevice device, SpriteBatch batch, RenderTarget2D shadeTemp)
        {
           StageManager.Draw(device, batch, shadeTemp);
           batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
           UIManager.Draw(batch);
           batch.End();

        }



    }
}
