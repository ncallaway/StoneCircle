using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    class Follower : Actor
    {

        AIOption follow = new AIOption();
        AIOption runaway = new AIOption();
        public Follower(String Id, Vector2 starting, Stage Parent, GameManager gameManager)
            : base(gameManager)
        {
            asset_Name = "Actor2";
            name = Id;
            Location = new Vector3(starting, 0);
            parent = Parent;
            follow.condition = new FarToActor(gameManager.Player, this);
            follow.action = new WalkToActor(this, gameManager.Player);

            runaway.condition = new MidToActor(gameManager.Player, this);
            runaway.action = new AIAction();

            AIStance.Add(follow);
            AIStance.Add(runaway);

        }

        public Follower(uint objectId) : base(objectId) { }






    }
}
