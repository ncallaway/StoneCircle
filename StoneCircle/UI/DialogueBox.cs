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

using StoneCircle;


namespace UserMenus
{
    class DialogueBox
    {
        String displayText;
        Color textColor;
        Color boxColor;
        Color borderColor;
        Vector2 position;
        SpriteFont font;

        bool talking;
        Texture2D talkBox;
        Texture2D thoughtBox;
        Actor owner;

        public DialogueBox()
        { }

        public DialogueBox(Actor Owner)
        {

            owner = Owner;
            displayText = "I am " + owner.Name;
            textColor = Color.Black;
            boxColor = Color.White;
            borderColor = Color.Black;
            position = owner.Position;

        }
             

        public void Load(ContentManager CM)
        {
            talkBox = CM.Load<Texture2D>("DialogueBox");
            thoughtBox = CM.Load<Texture2D>("ThoughtBox");
            font = CM.Load<SpriteFont>("text");

        }
   
        public void openBox(String text, bool talking)
        {
            position = owner.Position;
            displayText = WrapText(text, 100);
            this.talking = talking;
        }

        private string WrapText(string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return sb.ToString();
        }
   
        public void Draw(SpriteBatch batch, Vector2 camera_pos, float camera_scale)
        {
            if (talking) batch.Draw(talkBox, new Vector2(683 + 20, 384 - owner.ImageHeight) + (camera_scale * (position - camera_pos)), new Rectangle(0, 0, talkBox.Width, talkBox.Height), new Color(1f, 1f, 1f, .9f), 0f, new Vector2(0, talkBox.Height), camera_scale, SpriteEffects.None, 0.1f);
            else batch.Draw(thoughtBox, new Vector2(683 + 20, 384 - owner.ImageHeight) + (camera_scale * (position - camera_pos)), new Rectangle(0, 0, thoughtBox.Width, thoughtBox.Height), new Color(1f, 1f, 1f, .9f), 0f, new Vector2(0, thoughtBox.Height), camera_scale, SpriteEffects.None, 0.1f);
           
            batch.DrawString(font, displayText, new Vector2(683 + 100, 394 - owner.ImageHeight - talkBox.Height) + (camera_scale * (position - camera_pos)), Color.Black, 0f, Vector2.Zero, camera_scale, SpriteEffects.None, 0f);
        }



    }
}
