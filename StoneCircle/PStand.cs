using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace StoneCircle
{
    class PStand: Actionstate
    {
        public new string id;

        public PStand()
        {
            id = "stand";
        }


        public override void Update(Player player, List<Actor> targets, KeyboardState keys, GameTime t)
       {   if(keys.IsKeyDown(Keys.Up)||keys.IsKeyDown(Keys.Down)||keys.IsKeyDown(Keys.Left)||keys.IsKeyDown(Keys.Right)) player.current_Action = player.walk;
           
       }



    }
}
