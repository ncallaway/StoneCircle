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


using StoneCircle;
namespace UserMenus{

   class Inventory : RingMenu
    {
       public AudioManager AM;
       

        public Inventory(Player Owner)
        {
            player = Owner;
            title = player.Name + " Inventory";
            current_index = 0;

        }

        public override void Update(GameTime gametime)
        {
            MenuItem last = current;
            player.Input.Update();
            if (player.Input.RStickPosition().LengthSquared() > .5f && menuitems.Count > 0)
            {
                
                    degree = 450 + 180 / menuitems.Count + (int)MathHelper.ToDegrees((float)Math.Atan2(player.Input.RStickPosition().Y, player.Input.RStickPosition().X) + .001f);
                    current_index = (int)Math.Floor((double)((degree % 360) / (360 / menuitems.Count + 1)));
                    current = menuitems.ElementAt(current_index);

                   // if (last != current) AM.PlayEffect("menuBeep");
                    player.CurrentItem = (InventoryItem)menuitems.ElementAt(current_index);
                   
              
            }
            List<InventoryItem> remove = new List<InventoryItem>();
            foreach (InventoryItem Item in menuitems) if (Item.Quantity <= 0) remove.Add(Item);
            foreach (InventoryItem Item in remove) RemoveItem(Item);
            if (!player.Input.IsRightBumperPressed()) { player.parent.SM.GM.MM.CloseMenu(); }
       

        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(image, new Rectangle((int)player.Position.X - (int)player.parent.camera.Location.X + 683 - 75, (int)player.Position.Y - (int)player.parent.camera.Location.Y - 90 + 384, 150, 60), new Rectangle(0, 0, 80, 80), Color.White, 0f, Vector2.Zero, SpriteEffects.None, .1f);
            if (current != null)
            {

                if (player.CurrentItem.Quantity > 1) batch.DrawString(font, current.Id + " x" + player.CurrentItem.Quantity, new Vector2((int)player.Position.X - 50 - (int)player.parent.camera.Location.X + 683, (int)player.Position.Y - (int)player.parent.camera.Location.Y - 80 + 384), Color.Black);

                else batch.DrawString(font, current.Id, new Vector2((int)player.Position.X - 50 - (int)player.parent.camera.Location.X + 683, (int)player.Position.Y - (int)player.parent.camera.Location.Y - 80 + 384), Color.Black);

            }

            else batch.DrawString(font, "Empty", new Vector2((int)player.Position.X - 50 - (int)player.parent.camera.Location.X + 683, (int)player.Position.Y - (int)player.parent.camera.Location.Y - 80 + 384), Color.Black);
            
            int dFplayer = 360;
            if (menuitems.Count > 0) dFplayer = 360 / menuitems.Count;

            foreach (MenuItem x in menuitems)
            {
                int i = menuitems.IndexOf(x);
                if (x != current) x.Draw(batch, font, (int)player.Position.X - (int)player.parent.camera.Location.X + 683 + (int)(150 * Math.Sin(MathHelper.ToRadians(i * dFplayer))), (int)player.Position.Y - (int)player.parent.camera.Location.Y - player.ImageHeight / 3 + 384 - (int)(150 * Math.Cos(MathHelper.ToRadians(i * dFplayer))));
                else x.Draw(batch, font, (int)player.Position.X - (int)player.parent.camera.Location.X + 683 + (int)(150 * Math.Sin(MathHelper.ToRadians(i * dFplayer))), (int)player.Position.Y - (int)player.parent.camera.Location.Y - player.ImageHeight / 3 + 384 - (int)(150 * Math.Cos(MathHelper.ToRadians(i * dFplayer))), Color.Gold);
            
            }

        }

        public void RemoveItem(InventoryItem remove) { menuitems.Remove(remove); }
        public void AddItem(InventoryItem add)
        {
            bool notadded = true;
            foreach (InventoryItem II in menuitems) { if (II.Id == add.Id) { II.AddQuantity(add.Quantity); notadded = false; } }
            if (notadded) menuitems.Add(add);
        }

    }
}
