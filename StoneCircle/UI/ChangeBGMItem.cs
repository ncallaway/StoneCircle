using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using StoneCircle; 

namespace UserMenus
{
    class ChangeBGMItem: MenuItem
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
