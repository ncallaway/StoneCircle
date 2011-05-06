using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneCircle
{
    class Dead: Actionstate
    {


        public Dead()
        {
            id = "Dead";
        }
        
        public override void  Update(Microsoft.Xna.Framework.GameTime t, Dictionary<string,Actor>.ValueCollection targets)
        {
 	        Actor.AddProperty("Dead");
        }
    
        
    }

    class Unconcious : Actionstate
    {
        public Unconcious()
        {
            id = "Unconcious";

        }

        public override void Update(Microsoft.Xna.Framework.GameTime t, Dictionary<string, Actor>.ValueCollection targets)
        {
            Actor.AddProperty("Unconcious");
        }





    }

}
