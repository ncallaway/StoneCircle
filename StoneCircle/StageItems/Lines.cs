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
    class Lines
    {
        Actor actor;
        String text;
        protected String callID;
        public String CallID { get { return callID; } }
        String nextLine;
        String actorID;
        public String ActorID { get { return actorID; } }
        public String NextLine { get { return nextLine; } }
        SpriteFont font;
        LineType type;
        int time;
        Stage stage;
        bool talkThought;
        Texture2D image;




        public enum LineType
        {
            Timed,
            Player
        }

        public Lines() { }

        public Lines(String call, String nextID, String Text)
        {
            text = Text;
            callID = call;
            nextLine = nextID;
            type = LineType.Timed;
            talkThought = true;

        }

        public Lines(String call, String nextID, String Text, Stage Stage)
        {
            text = Text;
            callID = call;
            nextLine = nextID;
            stage = Stage;
            type = LineType.Timed;
            talkThought = true;
        }

        public Lines(String call, String nextID, String Text, Stage Stage, bool Player)
        {
            text = Text;
            callID = call;
            nextLine = nextID;
            stage = Stage;
            type = LineType.Player;
            this.actor = stage.player;
        }


        public virtual void Start()
        {
            time = 0;
            actor = stage.GetActor(actorID);
            actor.Talking();
        }

        public Lines(String call, String nextID, String Text, Stage Stage, LineType Type)
        {
            text = Text;
            callID = call;
            nextLine = nextID;
            stage = Stage;
            type = Type;
            talkThought = true;
        }

        public Lines(String call, String nextID, String ActorID, String Text, Stage Stage, LineType Type)
        {
            text = Text;
            callID = call;
            nextLine = nextID;
            stage = Stage;
            type = Type;
            talkThought = true;
            actorID = ActorID;
        }
        
        public virtual bool Update(GameTime t)
        {
            switch (type)
            {
                case LineType.Timed:
                    if (time == 0) time = t.TotalGameTime.Seconds;
                    if (t.TotalGameTime.Seconds - time >= 3)
                    {
                        time = t.TotalGameTime.Seconds;
                        actor.DefaultAction();
                        return true;
                    }
                    break;
                case LineType.Player:
                    if (stage.player.Input.IsAButtonNewlyPressed()) { actor.DefaultAction(); return true; }
                    break;

            }
            return false;
        }


        public virtual void Load(ContentManager CM)
        {
            font = CM.Load<SpriteFont>("Text");
            if (talkThought) image = CM.Load<Texture2D>("DialogueBox");
            else image = CM.Load<Texture2D>("ThoughtBox");
            text = ProcessText();


        }
        private string ProcessText()
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();
            float maxLineWidth = 100;
            float lineWidth = 0f;
            int lineNum = 0;

            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);
                switch (lineNum)
                {
                    case 0: maxLineWidth = 90; break;
                    case 1: maxLineWidth = 180; break;
                    case 2: maxLineWidth = 230; break;
                    case 3: maxLineWidth = 180; break;
                    case 4: maxLineWidth = 90; break;
                }
                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    lineNum++;
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return sb.ToString();
        }


        public virtual void Draw(SpriteBatch batch, Vector2 camera_pos, float camera_scale)
        {
            batch.Draw(image, new Vector2(683 + 20, 384 - actor.ImageHeight) + (camera_scale * (actor.Position - camera_pos)), new Rectangle(0, 0, image.Width, image.Height), new Color(1f, 1f, 1f, .8f), 0f, new Vector2(0, image.Height), camera_scale-.01f, SpriteEffects.None, 0.1f);

            batch.DrawString(font, text, new Vector2(683 + 25, 364 + 40 - actor.ImageHeight - image.Height) + (camera_scale * (actor.Position - camera_pos)), Color.Black, 0f, Vector2.Zero, camera_scale, SpriteEffects.None, 0f);
            batch.DrawString(font, text, new Vector2(683 + 25, 364 + 40 - actor.ImageHeight - image.Height) + (camera_scale * (actor.Position - camera_pos)), Color.Black, 0f, Vector2.Zero, camera_scale, SpriteEffects.None, 0f);
        }


    }


    class LinesMenu : Lines 
    {
        RingMenu menu;
        UIManager UI;

        public LinesMenu(String ID, RingMenu Menu, UIManager uI) { callID = ID; menu = Menu; UI = uI; }

        public override bool Update(GameTime t)
        {
            return true;
        }

        public override void Load(ContentManager CM)
        {
            menu.Load(CM);
        }

        public override void Start()
        {
            UI.OpenMenu(menu);
        }

        public override void Draw(SpriteBatch batch, Vector2 camera_pos, float camera_scale)
        { }

    }
}

       


