using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
   
    class CollisionCylinder 
    {
        Vector3 location;
        public Vector3 Location { get { return location; } set { location = value; } }
        float radius, height;
        internal float Radius { get { return radius; } set { radius = value; } }
        internal float Height { get { return height; } set { height = value; } }
      
        public CollisionCylinder(Vector3 location, float radius, float height)
        {

            this.location = location;
            this.radius = radius;
            this.height = height;

        }


        public bool Intersects(CollisionCylinder target)
        {
            Vector3 difference = location - target.location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if ((distance < radius * radius || distance < target.Radius * target.Radius) && (difference.Z < target.Height || difference.Z > -height)) return true;
            else return false;


        }

        public Vector3 Intersection(CollisionCylinder target)
        {
            Vector3 difference = location - target.location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if (distance < radius * radius || distance < target.Radius * target.Radius)
            {
                if (target.location.Z < location.Z)
                {
                    if (target.location.Z + target.Height < location.Z) return Vector3.Zero;
                    else return new Vector3(0, 0, location.Z - target.Height - target.location.Z);
                }
                else
                {
                    if (location.Z + Height < target.location.Z) return Vector3.Zero;
                    else return new Vector3(0, 0, target.location.Z - Height - location.Z);
                }

            }
            else if (distance < (radius + target.Radius) * (radius + target.Radius))
            {
                difference.Z = 0;
                difference.Normalize();
                return difference * (float)(Math.Sqrt(distance) - radius - target.Radius);


            }

            else return Vector3.Zero;



        }




    }


    class CollisionArc
    {
        Vector3 location;
        public Vector3 Location { get { return location; } }
        float radius, height;
        internal float Radius { get { return radius; }  set { radius = value; } }
        internal float Height { get { return height; }  set { height = value; } }
        private float centerAngle;
        public float CenterAngle { get { return centerAngle; } set { centerAngle = value; } }

        private float angleWidth;
        public float AngleWidth { get { return angleWidth; } set { angleWidth = value; } }

        public CollisionArc(Vector3 location, float radius, float height, float centralAngle, float AngleWidth)
        {

            this.location = location;
            this.radius = radius;
            this.height = height;

        }




        public bool Intersects(CollisionCylinder target)
        {
            Vector3 difference = location - target.Location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if ((distance < radius * radius || distance < target.Radius * target.Radius) && (difference.Z < target.Height || difference.Z > -height)) 
            {
             float angle = (float)Math.Atan2(difference.Y, difference.X);
   
                return (angle < centerAngle + angleWidth/2 && angle > centerAngle - angleWidth/2);
            }            else return false;


        }

        public Vector3 Intersection(CollisionCylinder target)
        {
            Vector3 difference = location - target.Location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if (distance < radius * radius || distance < target.Radius * target.Radius)
            {
                float angle = (float)Math.Atan2(difference.Y, difference.X);
   
                if (target.Location.Z < location.Z)
                {
                    if (target.Location.Z + target.Height < location.Z) return Vector3.Zero;
                    else return new Vector3(0, 0, location.Z - target.Height - target.Location.Z);
                }
                else
                {
                    if (location.Z + Height < target.Location.Z) return Vector3.Zero;
                    else return new Vector3(0, 0, target.Location.Z - Height - location.Z);
                }

            }
            else if (distance < (radius + target.Radius) * (radius + target.Radius))
            {
                difference.Z = 0;
                difference.Normalize();
                return difference * (float)(Math.Sqrt(distance) - radius - target.Radius);


            }

            else return Vector3.Zero;



        }




    }
}
