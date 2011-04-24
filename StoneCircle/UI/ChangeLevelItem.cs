using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StoneCircle; 

namespace UserMenus
{
    class ChangeLevelItem: MenuItem
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
}
