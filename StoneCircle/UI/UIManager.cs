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

        private Texture2D image;
        public Texture2D Image { get {return image;}}

        public GameManager GameManager { get { return manager; } }
        private GameManager manager;
        public ContentManager CM;


        private RingMenu pauseMenu1;

        public Player player;


        public UIManager(GameManager gameManager)
        {
            manager = gameManager;
            CM = gameManager.ContentManager;

            player = gameManager.Player;
            pauseMenu1 = new RingMenu(this);
            pauseMenu1.addMenuItem(new ChangeLevelItem("region1", manager.StageManager, this));
            pauseMenu1.addMenuItem(new ChangeLevelItem("region2", manager.StageManager, this));
            
            
        }


        public void Initialize(){

            pauseMenu1.Initialize();
           

        
        }


        public void Load(ContentManager CM)
        {
            font = CM.Load<SpriteFont>("Text");
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
           // Manager.AM.PauseSounds();
            currentMenus.Push(menu);
        }

        public void Update(GameTime t)
        {   if(currentMenus.Count != 0) currentMenus.Peek().Update(t);
           
        }

        public void Draw(SpriteBatch batch)
        {
            if(currentMenus.Count !=0) currentMenus.Peek().Draw(batch);
           

        }




    }
}
