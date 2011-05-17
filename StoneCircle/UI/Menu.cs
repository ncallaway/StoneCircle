using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


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
using StoneCircle.Persistence;


namespace UserMenus
{
    class Menu : ISaveable
    {
        private uint objectId;
        protected List<MenuItem> menuitems = new List<MenuItem>();
        protected MenuItem current;
        protected Player player;
        protected UIManager parent;
        protected String fontName;
        protected SpriteFont font;
        protected String imageName;
        protected Texture2D image;
        protected int current_index;
        protected int titleX;
        protected int titleY;
        protected int x_position;
        protected int y_position;
        protected int x_spacing;
        protected int y_spacing;
        protected String title;
        protected bool loaded;
        public bool Loaded { get { return loaded; } }

        public Menu() {
            objectId = IdFactory.GetNextId();
        }

        /// <summary>
        /// Creates a menu that runs in the UIManager attached to the given GameManager.
        /// </summary>
        /// <param name="gameManager">GameManager that's hosting the desired UIManager</param>
        public Menu(GameManager gameManager)
        {
            objectId = IdFactory.GetNextId();
            parent = gameManager.UIManager;
            player = gameManager.Player;
            fontName = "Text";
            imageName = "BlankIcon";
            current_index = 0;
            x_position = 500;
            y_position = 500;
            y_spacing = 20;
            x_spacing = 0;
            title = "";
            loaded = false;
        }

        public Menu(GameManager gameManager, String ImageName, String FontName, String Title)
        {
            objectId = IdFactory.GetNextId();
            parent = gameManager.UIManager;
            player = gameManager.Player;
            fontName = FontName;
            imageName = ImageName;
            current_index = 0;
            x_position = 700;
            y_position = 400;
            y_spacing = 20;
            x_spacing = 0;
            title = Title;
            loaded = false;
        }

        public Menu(uint objectId)
        {
            this.objectId = objectId;
            IdFactory.MoveNextIdPast(objectId);
        }

        /// <summary>
        /// Add a MenuItem to this Menu.
        /// </summary>
        /// <param name="menuitem">The MenuItem to add to this Menu.</param>
        public void AddMenuItem(MenuItem menuitem) { menuitems.Add(menuitem); }

        /// <summary>
        /// Load the content related to this menu.
        /// </summary>
        /// <param name="contentManager"></param>
        /// 
        public void RemoveMenuItem(MenuItem menuitem) { menuitems.Remove(menuitem); }

        public void Load(ContentManager contentManager)
        {
            if (!loaded)
            {
                font = contentManager.Load<SpriteFont>("Fonts/" + fontName);
                image = contentManager.Load<Texture2D>("UI Images/" + imageName);
            }
             foreach (MenuItem mi in menuitems) mi.Load(contentManager);
            loaded = true;
        }

        /// <summary>
        /// Menu-specific initialization
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Allow the menu to update itself
        /// </summary>
        /// <param name="gametime">Time structure representing elapsed and total time.</param>
        public virtual void Update(GameTime gametime)
        {
            Player p = player;
            if (current == null) current = menuitems.ElementAt(0);

            player.Input.Update();
            if (player.Input.IsMoveUpNewlyPressed() || player.Input.IsDPadUpNewlyPressed()) current_index--;
            if (player.Input.IsMoveDownNewlyPressed() || player.Input.IsDPadDownNewlyPressed()) current_index++;
            if (player.Input.IsBButtonNewlyPressed()) parent.CloseMenu();
            if (current_index >= menuitems.Count) current_index = 0;
            if (current_index < 0) current_index = menuitems.Count - 1;
            current = menuitems.ElementAt(current_index);
            if (player.Input.IsAButtonNewlyPressed() && current!=null) {
                player.Input.Update();
                current.execute();
            }

        }

        /// <summary>
        /// Draw the menu on the screen (using the given SpriteBatch)
        /// </summary>
        /// <param name="batch">SpriteBatch to render with</param>
        public virtual void Draw(SpriteBatch batch)
        {
          // if(image!=null) batch.Draw(image, new Rectangle(0, 0, 1366, 768), Color.White);
            batch.Draw(image, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, image.Width, image.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            batch.DrawString(font, title, new Vector2(titleX, titleY), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .5f );
            foreach (MenuItem x in menuitems) {

                int i = menuitems.IndexOf(x);
                if (i == current_index) x.DrawText(batch, font, x_position + (i + 1) * x_spacing - 15, y_position + y_spacing * (i + 1));
                else x.DrawText(batch, font, x_position + (i+1) * x_spacing, y_position + y_spacing * (i+1));
            }

        }




        public void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            Saver.SaveSaveableList<MenuItem>(menuitems, writer, objectTable);
        }

        private class MenuInflatables
        {
            public List<uint> menuItems;
        }

        private MenuInflatables inflatables;
        public void Load(BinaryReader reader, SaveType type)
        {
            inflatables = new MenuInflatables();
            inflatables.menuItems = Loader.LoadSaveableList(reader);
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            menuitems = Loader.InflateSaveableList<MenuItem>(inflatables.menuItems, objectTable);
        }

        public virtual void FinishLoad(GameManager manager)
        {
            parent = manager.UIManager;
            player = manager.Player;
            font = parent.Font;

            fontName = "Text";
            imageName = "BlankIcon";
            
            current_index = 0;
            x_position = 400;
            y_position = 400;
            y_spacing = 20;
            x_spacing = 0;

        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> refs = new List<ISaveable>();
            foreach (MenuItem mi in menuitems)
            {
                refs.Add(mi);
            }

            return refs;
        }

        public uint GetId()
        {
            return objectId;
        }
    }


    class FullscreenMenu : Menu
    {
























    }





}
