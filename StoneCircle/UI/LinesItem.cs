using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoneCircle;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UserMenus
{
    class LinesItem : MenuItem
    {
        String nextEvent;
        UIManager UI;
        Stage stage;



        public LinesItem(Stage Stage, String Next, String Icon, String id, UIManager uI)
        {
            nextEvent = Next;
            stage = Stage;
            this.id =id;
            iconName = Icon;
            color = Color.NavajoWhite;
            UI = uI;

        }

        public LinesItem(Stage Stage, String Next, bool yesNo, UIManager uI)
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
            stage.RunLine(nextEvent);
            UI.CloseMenu();

        }

          
    }
}
