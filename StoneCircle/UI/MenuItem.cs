using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UserMenus
{
    public class MenuItem
    {
        protected String id;
        public String Id { get { return id; } }
        protected Color color;
        protected String iconName;
        protected Texture2D icon;



        public MenuItem(String ID)
        {
            id = ID;
            color = Color.NavajoWhite;
            iconName = "BlankIcon";
        }


        public void Load(ContentManager CM)
        {
            icon = CM.Load<Texture2D>(iconName);
        }

        public MenuItem()
        {
            id = "Default ID";

            color = Color.NavajoWhite;

            iconName = "BlankIcon";
        }


      

        public virtual void execute()
        {
            // Various Implementation things go here.


        }

        public virtual void Draw(SpriteBatch batch, SpriteFont font,  int x_pos, int y_pos)
        {
            batch.Draw(icon, new Rectangle(x_pos, y_pos, 60, 60), new Rectangle(0, 0, 80, 80),color, 0f, new Vector2(30,30), SpriteEffects.None, 0f);
          //  batch.DrawString(font, id, new Vector2(x_pos, y_pos), color);

        }

        public virtual void Draw(SpriteBatch batch, SpriteFont font, int x_pos, int y_pos, Color color)
        {
            batch.Draw(icon, new Rectangle(x_pos, y_pos, 60, 60), new Rectangle(0, 0, 80, 80), color, 0f, new Vector2(30, 30), SpriteEffects.None, 0f);
            //  batch.DrawString(font, id, new Vector2(x_pos, y_pos), color);

        }


    }
}
