using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    class Follower: Actor
    {

        AIOption follow = new AIOption();
        AIOption runaway = new AIOption();
         public Follower(String Id, Vector2 starting, Stage Parent)
    {
        asset_Name = "Actor2";
        name = Id;
        Location = new Vector3(starting, 0);
        parent = Parent;
        follow.condition = new FarToActor(parent.SM.GM.player, this);
        follow.action = new WalkToActor(this, parent.SM.GM.player);

        runaway.condition = new MidToActor(parent.SM.GM.player, this);
         runaway.action = new AIAction();

        AIStance.Add(follow);
        AIStance.Add(runaway);
        Facing = new Vector2(1, 0);

    }


        



    }
}
