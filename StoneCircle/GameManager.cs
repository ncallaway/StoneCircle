﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

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

using StoneCircle.Persistence;

namespace StoneCircle
{
    /// <summary>
    /// Top Level Manager for StoneCircle. This class is the driver of update and draw calls to all of the
    /// game subcomponents. The GameManager also provides access to many key components of the Game (such as
    /// other Managers, the Camera, the Player, etc.
    /// 
    /// Right now the GameManager holds a lot of "global state". 
    /// </summary>
    public class GameManager
    {

        internal AudioManager AudioManager { get { return audioManager; } }
        internal UIManager UIManager { get { return uiManager; } }
        internal ContentManager ContentManager { get { return contentManager; } }
        public  StageManager StageManager { get { return stageManager; } }

        internal Player Player { get { return player; } }
        internal Camera Camera { get { return camera; } }

        private Player player;
        private Camera camera;

        private AudioManager audioManager;
        private UIManager uiManager;
        private ContentManager contentManager;
        private StageManager stageManager;
        private DeviceManager deviceManager;

        /// <summary>
        /// Creates a new game manager object (including the important sub-manager objects, like the UIManager). This
        /// also attaches the given contentManager to the new GameManager.
        /// </summary>
        /// <param name="contentManager">The ContentManager that the new GameManager should use</param>
        public GameManager(ContentManager contentManager)
        {
            /* Other managers are dependant on these fields being ready */
            this.contentManager = contentManager;
            player = new Player("Player", "male_select", Vector2.Zero, this, new InputController(InputController.InputMode.Player1));
            camera = new Camera(Player, new InputController());

            uiManager = new UIManager(this);
            stageManager = new StageManager(this);
            audioManager = new AudioManager();
            deviceManager = new DeviceManager();
        }

        public void LoadContent()
        {
            stageManager.LoadContent(contentManager);
            UIManager.Load(ContentManager);
            UIManager.OpenMenu(UIManager.MainMenu);
        }

        public void Initialize()
        {
            StageManager.Initialize();
            AudioManager.Initialize();
            UIManager.Initialize();
        }

        private bool saveRequested;
        private bool loadRequested;

        private void saveGame(Stream outputStream)
        {
            BinaryWriter binary = new BinaryWriter(outputStream);
            Saver.Save(StageManager, binary, SaveType.FULL);
            binary.Close();
        }

        private void loadGame(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream);
            Loader.LoadResponse response = Loader.Load(reader, SaveType.FULL);

            stageManager = (StageManager)response.root;
            player = stageManager.GetLoadingPlayer();

            foreach (ISaveable saveable in response.objects)
            {
                if (saveable == stageManager)
                {
                    continue;
                }
                saveable.FinishLoad(this);
            }

            stageManager.FinishLoad(this);
        }

        public void Update(GameTime T)
        {
            deviceManager.Update();

            if ((loadRequested || saveRequested)) {
                if (deviceManager.HasStorageContainer(PlayerIndex.One)) {
                    StorageContainer saveContainer = deviceManager.GetStorageContainer(PlayerIndex.One);

                    if (loadRequested)
                    {
                        Stream s = saveContainer.OpenFile("Stages.bin", FileMode.Open);
                        loadGame(s);
                        loadRequested = false;
                    }
                    else
                    {
                        Stream s = saveContainer.OpenFile("Stages.bin", FileMode.Create);
                        saveGame(s);
                        saveRequested = false;
                    }
                }
            }

            if (Player.Input.IsLeftBumperNewlyPressed()&& false)
            {
#if XBOX
                /* Request the container */
                saveRequested = true;
                deviceManager.RequestStorageContainer(PlayerIndex.One);
#else
                Stream stream = File.Open("Stages.bin", FileMode.Create);
                saveGame(stream);
                stream.Close();
#endif
                
            }

            if (Player.Input.IsRightBumperNewlyPressed() &&  false)
            {
#if XBOX
                loadRequested = true;
                deviceManager.RequestStorageContainer(PlayerIndex.One) ;
#else
                Stream stream = File.Open("Stages.bin", FileMode.Open);
                loadGame(stream);
                stream.Close();
#endif
            }

            UIManager.Update(T);
            if (!UIManager.OpenMenus) StageManager.Update(T);
        }

        public void Draw(GraphicsDevice device, SpriteBatch batch, RenderTarget2D shadeTemp)
        {
          if(StageManager.OpenStage != null)  StageManager.Draw(device, batch, shadeTemp);
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, DepthStencilState.Default, RasterizerState.CullNone, null);
            UIManager.Draw(batch);
            batch.End();

        }



    }




    class FullScreeenMenu : Menu
    {

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(image, new Rectangle(0,0, 1366, 768), Color.White);

            foreach(MenuItem MI in menuitems)
            {
               


            }
        }




    }
}
