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

        public AudioManager AM;
        public UIManager MM;
        public ContentManager CM;
        public StageManager SM;
        public Player player;
        public Camera Camera;
        



        public GameManager(ContentManager cM)
        {
            CM = cM;
            player = new Player("Player", "male_select", Vector2.Zero, new InputController(InputController.InputMode.Player1));
            
            Camera = new Camera(player, new InputController());
            
            MM = new UIManager(this, CM);
            SM = new StageManager(CM, this);
            
        }

        public void LoadContent()
        {
            MM.Load(CM);
            SM.SetStage("region1");
            
        }

        public void Initialize()
        {
            SM.Initialize();
            AM.Initialize();
            MM.Initialize();


        }

        public void Update(GameTime T)
        {   
            MM.Update(T);
           if(!MM.OpenMenus) SM.Update(T);
          
        }

        public void Draw(GraphicsDevice device, SpriteBatch batch, RenderTarget2D shadeTemp)
        {
           SM.Draw(device, batch, shadeTemp);
           batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
           MM.Draw(batch);
           batch.End();

        }



    }
}
