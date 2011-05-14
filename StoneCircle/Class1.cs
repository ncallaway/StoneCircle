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
        public Vector3 Location { get { return location; } }
        float radius, height;
        public float Radius { get { return radius; } }
        public float Height { get { return height; } }

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

}
