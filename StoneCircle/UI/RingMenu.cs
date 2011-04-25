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


namespace  UserMenus
{

    class RingMenu : Menu
    {
        protected int degree;
        protected AudioManager audioManager;
        protected GameManager gameManager;

        public RingMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.parent = gameManager.UIManager;
            player = gameManager.Player;
            audioManager = gameManager.AudioManager;
        }

        public override void Initialize()
        {
            
            font = parent.Font;
            current_index = 0;
            title = "";

            
        }


        




        public override void Update(GameTime gametime)
        {
            MenuItem last = current;
            player.Input.Update();
                    if (player.Input.LStickPosition().LengthSquared() > .5f)
                    {
                         degree =450 + 180/menuitems.Count + (int)MathHelper.ToDegrees((float)Math.Atan2(player.Input.LStickPosition().Y , player.Input.LStickPosition().X));
                        current_index = (int) Math.Floor( (double)((degree % 360) / (360 / menuitems.Count + 1)));
                        current = menuitems.ElementAt(current_index);
                       // if (last != current) AM.PlayEffect("menuBeep");
                    }
                    else current = null;
                        
                    
                    if (player.Input.IsBButtonNewlyPressed() || player.Input.IsPauseMenuNewlyPressed()) parent.CloseMenu();
                    if (player.Input.IsAButtonNewlyPressed())
                    {
                        player.Input.Update();
                        if(current!= null) current.execute();
                    }
            
                }
        

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(image, new Rectangle((int)player.Position.X - (int)player.parent.camera.Location.X + 683 - 100, (int)player.Position.Y - (int)player.parent.camera.Location.Y - 90 + 384, 200, 90), new Rectangle(0,0, 80,80), Color.Beige, 0f, Vector2.Zero, SpriteEffects.None, .1f);
            if(current != null) batch.DrawString(font, current.Id, new Vector2((int)player.Position.X - 95 - (int)player.parent.camera.Location.X + 683, (int)player.Position.Y - (int)player.parent.camera.Location.Y  - 80 + 384), Color.Black);

            int dFactor = 360;
            if ( menuitems.Count > 0) dFactor = 360 / menuitems.Count;
            
            foreach (MenuItem x in menuitems)
            {
                int i = menuitems.IndexOf(x);
                if (x != current) x.Draw(batch, font, (int)player.Position.X - (int)player.parent.camera.Location.X + 683 + (int)(150 * Math.Sin(MathHelper.ToRadians(i * dFactor))), (int)player.Position.Y - (int)player.parent.camera.Location.Y - player.ImageHeight / 3 + 384 - (int)(150 * Math.Cos(MathHelper.ToRadians(i * dFactor))));
                else x.Draw(batch, font, (int)player.Position.X - (int)player.parent.camera.Location.X + 683 + (int)(150 * Math.Sin(MathHelper.ToRadians(i * dFactor))), (int)player.Position.Y - (int)player.parent.camera.Location.Y - player.ImageHeight / 3 + 384 - (int)(150 * Math.Cos(MathHelper.ToRadians(i * dFactor))) , Color.Gold);
            }

        }
    





    }
}
