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

    class PWalk : Actionstate
    {

   // public new string id;


        public PWalk(BoundingBox effect)
        {
            EffectBox = effect;
            id = "Walking";
        }

        public PWalk()
        {
            EffectBox = new BoundingBox(new Vector3(0, 0, 0), new Vector3(40, 40, 80));
            id = "Walking";
        }

        public override void Update(Player player, List<Actor> targets, KeyboardState keys, GameTime t)
        {

            Vector2 dir = Vector2.Zero;
            Vector2 update = Vector2.Zero;

            if (keys.IsKeyDown(Keys.Up)) dir += new Vector2(0, -1);
            if (keys.IsKeyDown(Keys.Down)) dir += new Vector2(0, 1);
            if (keys.IsKeyDown(Keys.Left)) dir += new Vector2(-1, 0);
            if (keys.IsKeyDown(Keys.Right)) dir += new Vector2(1, 0);
            update = (float)t.ElapsedGameTime.TotalSeconds * player.speed * 2 * dir;

            BoundingBox CheckBox = player.GetBounds(update);
            Boolean legal = true;

            foreach (Actor y in targets)
            {
                if ((CheckBox.Intersects(y.GetBounds()) && !player.Equals(y))) //Collision Detection. Ideally reduces movement to outside collision bounds.
                {
                    legal = false;
               }
            }
            if (legal) player.location += update;
            if(!(keys.IsKeyDown(Keys.Up)||keys.IsKeyDown(Keys.Down)||keys.IsKeyDown(Keys.Left)||keys.IsKeyDown(Keys.Right))) player.current_Action = player.standing;
        }




    }

}
