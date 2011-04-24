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

namespace StoneCircle
{
    class TextBox
    {
        String displayText;
        Color textColor;
        Color boxColor;
        Color borderColor;
        Vector2 position;
        Vector2 dimensions;
        SpriteFont font;
        Stage parent;
        Texture2D drawBox;

        public TextBox()
        {
            displayText = "Hello, How are you? This is supposed to be a very long text bit/spiel thing so I'm looking forwrd to seeing it in action and doing other stuff Yay!!";
            textColor = Color.Black;
            boxColor = Color.White;
            borderColor = Color.Black;

            position = new Vector2(600, 150);
            dimensions = new Vector2(drawBox.Width, drawBox.Height);
        }



        public TextBox(Stage region)
        {
            displayText = "Hello, How are you? This is supposed to be a very long text bit/spiel thing so I'm looking forwrd to seeing it in action and doing other stuff Yay!!";
            textColor = Color.Black;
            boxColor = Color.White;
            borderColor = Color.Black;

            parent = region;
            position = new Vector2(600, 150);
            dimensions = new Vector2(300, 100);


            font = parent.font;
            drawBox = parent.CM.Load<Texture2D>("DialogueBox");

        }

        public void ProcessText(String fullText)
        {

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



        public TextBox(String Text, Stage region)
        {
            displayText = Text;
            textColor = Color.Black;
            boxColor = Color.White;
            borderColor = Color.Black;

            parent = region;
            position = new Vector2(600, 150);
            dimensions = new Vector2(300, 100);

            font = parent.font;
            drawBox = parent.CM.Load<Texture2D>("DialogueBox");


        }

        public void Draw(SpriteBatch batch)
        {
            displayText = WrapText(displayText, 100);
          //  batch.Draw(drawBox, new Rectangle((int) position.X, (int) position.Y, (int)(position.X + dimensions.X), (int)(position.Y + dimensions.Y)), borderColor);
            batch.Draw(drawBox, new Rectangle((int)position.X + 5, (int)position.Y + 5, (int)drawBox.Width, (int)(drawBox.Height)), boxColor);
            batch.DrawString(font, displayText, new Vector2(position.X + 10, position.Y + 10), textColor);



        }



    }
}
