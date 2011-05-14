using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserMenus;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using StoneCircle;

namespace UserMenus
{
    class UIManager
    {
        private Stack<Menu> currentMenus = new Stack<Menu>();
        public bool OpenMenus { get { return currentMenus.Count != 0; } }

        private SpriteFont font;
        public SpriteFont Font { get { return font; } }

        /// <summary>
        /// Gets the GameManager that this UIManager is attached to.
        /// </summary>
        public GameManager GameManager { get { return gameManager; } }
        private GameManager gameManager;

        private ContentManager contentManager;
        private RingMenu pauseMenu1;
        private Player player;

        private Menu mainMenu;
        public Menu MainMenu { get {return mainMenu;}}

        public UIManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            contentManager = gameManager.ContentManager;

            }


        public void Initialize()
        {
           
            pauseMenu1.Initialize();
        }


        public void Load(ContentManager CM)
        {
            font = CM.Load<SpriteFont>("Fonts/Text");
            mainMenu = new Menu(gameManager, "Wallpaper", "Text", "Main Menu");
            mainMenu.AddMenuItem(new ChangeLevelItem("Village", gameManager.StageManager, this, "New Game"));

            pauseMenu1 = new RingMenu(gameManager);
            pauseMenu1.AddMenuItem(new ChangeLevelItem("region1", gameManager.StageManager, this, "Region 1"));
            pauseMenu1.AddMenuItem(new ChangeLevelItem("region2", gameManager.StageManager, this, "Region 2"));
            pauseMenu1.Load(CM);

        }




        public void CloseMenu()
        {
            currentMenus.Pop();
            // if (currentMenus.Count == 0) Manager.AM.ResumeSounds();

        }

        public void Pause()
        {
            currentMenus.Push(pauseMenu1);
        }


        public void OpenMenu(Menu menu)
        {
            if (!menu.Loaded) {  menu.Initialize();menu.Load(contentManager); }
            
            currentMenus.Push(menu);
        }

        public void Update(GameTime t)
        {
            if (currentMenus.Count != 0) currentMenus.Peek().Update(t);

        }

        public void Draw(SpriteBatch batch)
        {
            if (currentMenus.Count != 0) currentMenus.Peek().Draw(batch);
        }


    }
}
