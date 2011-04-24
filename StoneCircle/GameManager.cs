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

        public Player Player { get { return player; } }
        public Camera Camera { get { return camera; } }

        private Player player;
        private Camera camera;

        private AudioManager audioManager;
        private UIManager uiManager;
        private ContentManager contentManager;
        private StageManager stageManager;

        /// <summary>
        /// Creates a new game manager object (including the important sub-manager objects, like the UIManager). This
        /// also attaches the given contentManager to the new GameManager.
        /// </summary>
        /// <param name="contentManager">The ContentManager that the new GameManager should use</param>
        public GameManager(ContentManager contentManager)
        {
            /* Other managers are dependant on these fields being ready */
            this.contentManager = contentManager;
            player = new Player("Player", "male_select", Vector2.Zero, new InputController(InputController.InputMode.Player1));
            camera = new Camera(Player, new InputController());

            uiManager = new UIManager(this);
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
            if (!UIManager.OpenMenus) StageManager.Update(T);
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
