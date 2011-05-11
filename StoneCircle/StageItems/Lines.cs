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

using StoneCircle.Persistence;

namespace StoneCircle
{
    class Lines : ISaveable
    {
        private uint objectId;
        private Actor actor;
        private String text;
        private SpriteFont font;
        private bool talkThought;
        private Texture2D image;
        private String[] lines = new String[5];
        private int[] lineSpace = new int[5];


        public Lines() { this.objectId = IdFactory.GetNextId(); }

        public Lines(String text, Actor actor)
        {
            this.objectId = IdFactory.GetNextId();
            this.text = text;
            this.actor = actor;
        }

        public Lines(uint objectId)
        {
            this.objectId = objectId;
            IdFactory.MoveNextIdPast(objectId);
        }

        public virtual void Load(ContentManager CM)
        {
            font = CM.Load<SpriteFont>("Text");
            if (talkThought) image = CM.Load<Texture2D>("DialogueBox");
            else image = CM.Load<Texture2D>("ThoughtBox");
            ProcessText();
        }

        private void ProcessText()
        {
            Vector2 sizeLength = font.MeasureString(text);



            string[] words = text.Split(' ');

            String constructor = "";
            float lineWidth = 0f;
            int lineNum = 0;

            switch ((int)sizeLength.X / 60)
            {
                case 0: lineSpace = new int[] { 0, 0, 120, 120, 1200 }; break;
                case 1: lineSpace = new int[] { 0, 0, 120, 120, 1200 }; break;
                case 2: lineSpace = new int[] { 0, 0, 180, 120, 1200 }; break;
                case 3: lineSpace = new int[] { 0, 60, 180, 120, 1200 }; break;
                case 4: lineSpace = new int[] { 0, 60, 180, 120, 1200 }; break;
                case 5: lineSpace = new int[] { 0, 120, 180, 120, 1200 }; break;
                case 6: lineSpace = new int[] { 0, 120, 180, 180, 1200 }; break;
                case 7: lineSpace = new int[] { 0, 120, 220, 180, 1200 }; break;
                case 8: lineSpace = new int[] { 0, 180, 220, 180, 1200 }; break;
                case 9: lineSpace = new int[] { 120, 180, 220, 180, 1200 }; break;
                case 10: lineSpace = new int[] { 120, 180, 220, 180, 1200 }; break;
                case 11: lineSpace = new int[] { 120, 180, 220, 180, 1200 }; break;
                case 12: lineSpace = new int[] { 120, 180, 220, 180, 1200 }; break;

            }


            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (lineWidth + size.X < lineSpace[lineNum])
                {
                    constructor = constructor + " " + word;
                    lineWidth += size.X + spaceWidth;
                }

                else
                {
                    sizeLength = font.MeasureString(constructor);
                    lineSpace[lineNum] = (int)((image.Width - 10) - lineWidth) / 2;
                    if (constructor != null) lines[lineNum] = constructor; else lines[lineNum] = "  ";

                    lineNum++;
                    constructor = "" + word;
                    lineWidth = size.X + spaceWidth;
                }
            }

            sizeLength = font.MeasureString(constructor);
            lineSpace[lineNum] = (int)((image.Width - 10) - lineWidth) / 2;
            lines[lineNum] = constructor;


        }
        

        public virtual void Draw(SpriteBatch batch, Vector2 camera_pos, float camera_scale)
        {
            batch.Draw(image, new Vector2(683 + 20, 384 - actor.ImageHeight) + (camera_scale * (actor.Position - camera_pos)), new Rectangle(0, 0, image.Width, image.Height), new Color(1f, 1f, 1f, .8f), 0f, new Vector2(0, image.Height), camera_scale - .01f, SpriteEffects.None, 0.1f);
            for (int i = 0; i < 5; i++)
            {
                if (lines[i] != null)
                {
                    batch.DrawString(font, lines[i], new Vector2(683 + (25 + lineSpace[i])*camera_scale, 364 + (40 + (25 * i))*camera_scale - actor.ImageHeight - image.Height) + (camera_scale * (actor.Position - camera_pos)), Color.Black, 0f, Vector2.Zero, camera_scale, SpriteEffects.None, 0f);
                    batch.DrawString(font, lines[i], new Vector2(683 + (25 + lineSpace[i]) * camera_scale, 364 + (40 + (25 * i))*camera_scale - actor.ImageHeight - image.Height) + (camera_scale * (actor.Position - camera_pos)), Color.Black, 0f, Vector2.Zero, camera_scale, SpriteEffects.None, 0f);
                }
            }
        }


        public void Save(System.IO.BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            throw new NotImplementedException();
        }

        public void Load(System.IO.BinaryReader reader, SaveType type)
        {
            throw new NotImplementedException();
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            throw new NotImplementedException();
        }

        public void FinishLoad(GameManager manager)
        {
            throw new NotImplementedException();
        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            throw new NotImplementedException();
        }

        public uint GetId()
        {
            return objectId;
        }
    }

/*
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

    }*/
}

       


