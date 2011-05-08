using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using StoneCircle;

namespace UserMenus
{
    class MenuItem
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

        public override void execute()
        {
            stage.RunEvent(nextEvent);
            UI.CloseMenu();

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

        public ChangeLevelItem(String targetName, StageManager sM, UIManager uI)
        {
            id = targetName;
            SM = sM;
            UI = uI;


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
