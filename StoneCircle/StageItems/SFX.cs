using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StoneCircle
{
    class SFX
    {
        public Vector3 Location;
        internal Vector2 Position { get{ return new Vector2(Location.X, (Location.Y + Location.Z)/2);}}
        protected Texture2D image;
        protected String assetName;
        int ImageXIndex;
        int ImageYIndex;
        int ImageWidth;
        int ImageHeight;
        Vector2 origin;
        float time;
        protected Stage parent;
        
        int frame;
        int maxFrame;


        public SFX()
        {

        }



        public virtual void Draw(SpriteBatch theSpriteBatch, Camera camera)
        {

             theSpriteBatch.Draw(image, camera.screenadjust + (camera.Scale * (Position - camera.Location)), new Rectangle(ImageXIndex * ImageWidth, ImageYIndex * ImageHeight, ImageWidth, ImageHeight), Color.White, 0f, origin, camera.Scale, SpriteEffects.None, .2f - Location.Y / 100000f);
               
        }

        public virtual void Start()
        {


        }

        public virtual void End()
        {

        }

        public virtual void update(GameTime t)
        {
            UpdateFrame(t);


        }

        public void UpdateFrame(GameTime t)
        {
            time += t.ElapsedGameTime.Milliseconds;
            if (time >= 17) { frame++; time = 0; }
            frame %= maxFrame;

        }


    }





    class FireFly : SFX
    {
        LightSource LS;


        public FireFly(Vector3 Location, Stage Parent, String asetName)
        {

           this.Location = Location;
           parent = Parent;
           assetName = asetName;
           LS = new LightSource("Firely", Position, 300f, parent, null);
           LS.ChangeColor(Color.Pink);

        }


        public override void update(GameTime t)
        {
            LS.Update(t);
            LS.UpdatePosition(Position);
            base.update(t);
        }

        public override void Start()
        {
            parent.addLight(LS);         
                
                
        }

        public override void End()
        {

            parent.removeLight(LS);
        }

    }




}
