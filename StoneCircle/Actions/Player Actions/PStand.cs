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
    class PStand: PAction
    {
        public PStand(InputController Input)
        {
            input = Input;
            id = "Standing";
        }


        public override void Update(GameTime t, Dictionary<String,Actor>.ValueCollection targets)
        {
            
       }



    }
}
