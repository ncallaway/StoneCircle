using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using StoneCircle;
using StoneCircle.Persistence;

namespace UserMenus
{
    class MenuItem : ISaveable
    {
        protected String id;
        public String Id { get { return id; } }
        protected String display;
        protected Color color;
        protected String iconName;
        protected Texture2D icon;

        private uint objectId;



        public MenuItem(String ID)
        {
            objectId = IdFactory.GetNextId();
            id = ID;
            color = Color.NavajoWhite;
            iconName = "BlankIcon";
        }

        public MenuItem()
        {
            objectId = IdFactory.GetNextId();
            id = "Default ID";
            display = "Default";
            color = Color.NavajoWhite;

            iconName = "BlankIcon";
        }

        public MenuItem(uint objectId)
        {
            this.objectId = objectId;
            IdFactory.MoveNextIdPast(objectId);
        }



        public void Load(ContentManager CM)
        {
            icon = CM.Load<Texture2D>("UI Images/" + iconName);
        }



      

        public virtual void execute()
        {
            // Various Implementation things go here.


        }

        public virtual void Draw(SpriteBatch batch, SpriteFont font,  int x_pos, int y_pos)
        {
            batch.Draw(icon, new Rectangle(x_pos, y_pos, 60, 60), new Rectangle(0, 0, 80, 80),color, 0f, new Vector2(30,30), SpriteEffects.None, 0f);
            batch.DrawString(font, display, new Vector2(x_pos, y_pos), color);

        }

        public virtual void Draw(SpriteBatch batch, SpriteFont font, int x_pos, int y_pos, Color color)
        {
            batch.Draw(icon, new Rectangle(x_pos, y_pos, 60, 60), new Rectangle(0, 0, 80, 80), color, 0f, new Vector2(30, 30), SpriteEffects.None, 0f);
              batch.DrawString(font, display,  new Vector2(x_pos, y_pos), color);

        }



        public virtual void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            Saver.SaveString(id, writer);
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
            writer.Write(color.A);
            Saver.SaveString(iconName, writer);
        }

        public virtual void Load(BinaryReader reader, SaveType type)
        {
            id = Loader.LoadString(reader);
            color = new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            iconName = Loader.LoadString(reader);
        }

        public virtual void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            /* no-op*/
        }

        public virtual void FinishLoad(GameManager manager)
        {
            /* no-op */
        }

        public virtual List<ISaveable> GetSaveableRefs(SaveType type)
        {
            return null;
        }

        public uint GetId()
        {
            return objectId;
        }
    }

    class StateConditionItem : MenuItem
    {
        String stateCondition;
        StageManager stageManager;

        public StateConditionItem(String StateCondition, StageManager SM)
        {
            stateCondition = StateCondition;
            stageManager = SM;
        }

        public override void execute()
        {
            stageManager.SetCondition(stateCondition);
            
        }

    }

    class InventoryItem : MenuItem
    {
        public Item item;

        public InventoryItem(Item Item)
        {
            item = Item;
            if (item != null) { id = item.Name; iconName = item.IconName; }
            else
            {
                id = "Empty";
                iconName = "BlankIcon";
            }
        }





    }

    class CloseMenuButton : MenuItem
    {
        UIManager manager;

        public CloseMenuButton(UIManager Manager)
        {
            manager = Manager;

            id = "Close Menu";
        }



        public override void execute()
        {
            manager.CloseMenu();
        }




    }

    class EventItem : MenuItem
    {
        String nextEvent;
        UIManager UI;
        Stage stage;

        public EventItem(Stage Stage, String Next, String Icon, String id, UIManager uI)
        {
            nextEvent = Next;
            stage = Stage;
            this.id = id;
            iconName = Icon;
            color = Color.NavajoWhite;
            UI = uI;

        }

        public EventItem(Stage Stage, String Next, bool yesNo, UIManager uI)
        {
            nextEvent = Next;
            stage = Stage;


            if (yesNo) { iconName = "ThumbsUpIcon"; id = "Yes"; }
            else { iconName = "ThumbsDownIcon"; id = "No"; }

            color = Color.Beige;
            UI = uI;
        }

        public EventItem(uint objectId) : base(objectId) { }

        public override void execute()
        {
            stage.RunEvent(nextEvent);
            UI.CloseMenu();
        }

        public override List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> parentRefs =  base.GetSaveableRefs(type);
            if (parentRefs == null)
                parentRefs = new List<ISaveable>();
            parentRefs.Add(stage);
            return parentRefs;
        }

        public override void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            base.Save(writer, type, objectTable);
            Saver.SaveString(nextEvent, writer);
            writer.Write(objectTable[stage]);
        }

        private uint stageId;
        public override void Load(BinaryReader reader, SaveType type)
        {
            base.Load(reader, type);
            nextEvent = Loader.LoadString(reader);
            stageId = reader.ReadUInt32();
        }

        public override void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            base.Inflate(objectTable);
            stage = (Stage)objectTable[stageId];
        }

        public override void FinishLoad(GameManager manager)
        {
            base.FinishLoad(manager);
            UI = manager.UIManager;
        }


    }

    class ChangeLevelItem : MenuItem
    {
        StageManager SM;
        UIManager UI;

        public enum LevelState
        {
            Unavailable,
            Unknown,
            Available,
            Current

        }

        public ChangeLevelItem(String targetName, StageManager sM, UIManager uI, String display)
        {
            id = targetName;
            SM = sM;
            UI = uI;
            this.display = display;

        }


        public override void execute()
        {
            SM.SetStage(id);
            UI.CloseMenu();
        }
    }

    class ChangeBGMItem : MenuItem
    {
        AudioManager AM;
        UIManager UI;

        public ChangeBGMItem(String targetName, AudioManager aM, UIManager uI)
        {
            id = targetName;
            AM = aM;
            UI = uI;
            color = Color.NavajoWhite;

        }


        public override void execute()
        {
            AM.SetSong(id);
            UI.CloseMenu();
        }
    }
}
