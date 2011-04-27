using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace StoneCircle
{
    [Serializable]
    class LightSource
    {
        protected Vector2 location;
        protected float radius;
        protected String id;
        protected Color color;
        public Vector3 LightColor {get{return color.ToVector3();}}
        public float Radius { get { return radius/1366; } }
        public Vector2 Location { get { return new Vector2(location.X, location.Y); } }
        
        [NonSerialized] Effect light;
        private Stage parent;

        public LightSource() { }

        public LightSource(String Id, Vector2 starting, float Radius, Stage Parent, Effect Light)
        {
            id = Id;
            location = starting;
            radius = Radius;
            parent = Parent;
            color = Color.Orange;
            //light = Light.Clone(Light.GraphicsDevice);
           
        }



        public bool inLight(BoundingBox targetBox)
        {
            BoundingSphere lightSphere = new BoundingSphere(new Vector3(location.X, location.Y, 0), radius);
            return lightSphere.Intersects(targetBox);

        }

        public float calcRotate(Actor target)
        {
            float x_diff = location.X - target.Location.X;
            float y_diff = location.Y - target.Location.Y;
            double tan = (double)x_diff / y_diff;
            double rot_angle = -Math.Atan(tan);
            if (y_diff < 0) rot_angle = Math.PI + rot_angle ;
            return (float)rot_angle;

        }

        public virtual void Update(GameTime t)
        {   Random rand = new Random();
                color = new Color(color.ToVector3() - rand.Next(-1,2) * .2f * Vector3.UnitX);
                radius += rand.Next(-2,3);
           
        }


        public void LightStart()
        {
           // light.Begin();
           // light.CurrentTechnique.Passes[0].Begin();

        }

        public void LightStop()
        {
           // light.End();

           // light.CurrentTechnique.Passes[0].End();
        }


        public float calcIntensity(Actor target)
        {
            Vector2 dist = location - target.Position;
            return .6f - (dist.Length() / radius);

        }


        
        

    }






    [Serializable]
    class ActorLightSource : LightSource
    {
        [NonSerialized] Actor owner;

        public override void Update(GameTime t)
        {
            location = new Vector2(owner.Location.X, owner.Location.Y);
            base.Update(t);


        }

        public ActorLightSource(Actor Owner)
        {
            radius = 250f;
            owner = Owner;
            location = new Vector2(owner.Location.X, owner.Location.Y);
        }

        public ActorLightSource(Actor Owner, float Strength)
        {
            radius = Strength;
            owner = Owner;
            location = new Vector2(owner.Location.X, owner.Location.Y);
        }



    }

}
