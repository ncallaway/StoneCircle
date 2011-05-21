using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StoneCircle
{
    abstract class CollisionStructure
    {
        protected Vector3 location;
        public Vector3 Location { get { return location; } set { location = value; } }
        protected float radius, height;
        public float LocationZ { get { return location.Z; } set { location.Z = value; } }
        internal float Radius { get { return radius; } set { radius = value; } }
        internal float Height { get { return height; } set { height = value; } }


        public virtual bool Intersects(CollisionStructure CS)
        {
            return false;
        }

    }

    class CollisionCylinder : CollisionStructure
    {


        public CollisionCylinder(Vector3 location, float radius, float height)
        {

            this.location = location;
            this.radius = radius;
            this.height = height;

        }


        public bool IntersectsType(CollisionCylinder target)
        {
            Vector3 difference = location - target.location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if ((distance < radius * radius || distance < target.Radius * target.Radius) && (difference.Z < target.Height || difference.Z > -height)) return true;
            else return false;


        }

        public bool IntersectsType(CollisionArc target)
        {
            return target.IntersectsType(this);

        }

        public Vector3 Intersection(CollisionArc target)
        {
            return -target.Intersection(this);
        }

        public Vector3 Intersection(CollisionCylinder target)
        {
            Vector3 difference = location - target.location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;

            if ((distance < (radius + target.Radius) * (radius + target.Radius)))
            {

                if ((target.LocationZ + target.Height) > LocationZ - 10 && location.Z > target.LocationZ)
                {
                    return new Vector3(0, 0, location.Z - target.Height - target.location.Z);
                }
                else if (location.Z + Height > target.LocationZ && location.Z < target.LocationZ)
                    return new Vector3(0, 0, target.location.Z - Height - location.Z);



                else
                {
                    difference.Z = 0;
                    difference.Normalize();
                    return difference * (float)(Math.Sqrt(distance) - radius - target.Radius);
                }
            }


            else return Vector3.Zero;

        }




    }


    class CollisionArc
    {
        Vector3 location;
        public Vector3 Location { get { return location; } set { location = value; } }
        public float LocationZ { get { return location.Z; } set { location.Z = value; } }
        float radius, height;
        internal float Radius { get { return radius; } set { radius = value; } }
        internal float Height { get { return height; } set { height = value; } }
        private float centerAngle;
        public float CenterAngle { get { return centerAngle; } set { centerAngle = value; } }

        private float angleWidth;
        public float AngleWidth { get { return angleWidth; } set { angleWidth = value; } }

        public CollisionArc(Vector3 location, float radius, float height, float centralAngle, float AngleWidth)
        {

            this.location = location;
            this.radius = radius;
            this.height = height;
            centerAngle = centralAngle;
            this.angleWidth = AngleWidth;
        }




        public bool IntersectsType(CollisionCylinder target)
        {
            Vector3 difference = location - target.Location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if ((distance < (radius + target.Radius) * (radius + target.Radius)) && (difference.Z < target.Height || difference.Z > -height))
            {
                float angle = (float)Math.Atan2(-difference.Y, -difference.X);

                return (angle < centerAngle + angleWidth / 2 && angle > centerAngle - angleWidth / 2);
            }
            else return false;


        }

        public bool IntersectsType(CollisionArc target)
        {
            Vector3 difference = location - target.Location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;
            if ((distance < radius * radius || distance < target.Radius * target.Radius) && (difference.Z > target.Height || difference.Z < -height))
            {
                float angle = (float)Math.Atan2(-difference.Y, -difference.X);

                return (angle < centerAngle + angleWidth / 2 && angle > centerAngle - angleWidth / 2);
            }
            else return false;



        }

        private float mod(float target)
        {
            return(float)( (target + 2 *  Math.PI) % Math.PI);
        }

        public Vector3 Intersection(CollisionCylinder target)
        {
            Vector3 difference = location - target.Location;
            float distance = difference.X * difference.X + difference.Y * difference.Y;

            float angle = (float)Math.Atan2(-difference.Y, -difference.X);
            if ((distance < (radius + target.Radius) * (radius + target.Radius))&&          
                (angle > centerAngle + AngleWidth / 2 && angle < centerAngle - AngleWidth / 2))
                {
                    if ((target.LocationZ + target.Height) > LocationZ - 10 && location.Z > target.LocationZ)
                    {
                        return new Vector3(0, 0, location.Z - target.Height - target.Location.Z);
                    }
                    else if (location.Z + Height > target.LocationZ && location.Z < target.LocationZ)
                        return new Vector3(0, 0, target.LocationZ - Height - location.Z);



                    else
                    {
                        difference.Z = 0;
                        difference.Normalize();
                        return difference * (float)(Math.Sqrt(distance) - radius - target.Radius);
                    }
                }
            

            else return Vector3.Zero;

        }
    }


    class CollisionBox: CollisionStructure
    {   
        float depth;
        float width;

        public CollisionBox(Vector3 location, float radius, float height, float width)
        {

            this.location = location;
            depth = radius;
            this.height = height;
            this.width = width;
        }


        public bool IntersectsType(CollisionCylinder target)
        {
            if(target.Location.X < location.X - width/2)
            {

                if(target.Location.Y < location.Y - depth/2)
                {
                    Vector3 difference = location - target.Location + new Vector3(-width/2, - depth/2, 0);
                     float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return distance < target.Radius * target.Radius;
            
                }

                else if ( target.Location.Y > location.Y + depth / 2){

                    Vector3 difference = location - target.Location + new Vector3(-width/2, + depth/2, 0);
                     float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return distance < target.Radius * target.Radius;


                }

                else return (target.Location.X > location.X - width/2 - target.Radius);

            }
            else if(target.Location.X > location.X + width/2)
            {
                 if(target.Location.Y < location.Y - depth/2)
                {
                    Vector3 difference = location - target.Location + new Vector3(-width/2, - depth/2, 0);
                     float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return distance < target.Radius * target.Radius;
            
                }

                else if ( target.Location.Y > location.Y + depth / 2){

                    Vector3 difference = location - target.Location + new Vector3(-width/2, + depth/2, 0);
                    float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return distance < target.Radius * target.Radius;


                }

                else return (target.Location.X < location.X + width/2 + target.Radius);
                
            }
            else return (target.Location.Y < location.Y + depth/2 + target.Radius || target.Location.Y > Location.Y - depth/2 - target.Radius);

                
           

        }

        
        public Vector3 Intersection(CollisionCylinder target)
        {
            if (target.Location.X < location.X - width / 2)
            {

                if (target.Location.Y < location.Y - depth / 2)
                {
                    Vector3 difference = location - target.Location + new Vector3(-width / 2, -depth / 2, 0);
                    float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return difference;

                }

                else if (target.Location.Y > location.Y + depth / 2)
                {

                    Vector3 difference = location - target.Location + new Vector3(-width / 2, +depth / 2, 0);
                    float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return difference;


                }

                else return (target.Location.X - location.X - width / 2 - target.Radius) * Vector3.UnitY;

            }
            else if (target.Location.X > location.X + width / 2)
            {
                if (target.Location.Y < location.Y - depth / 2)
                {
                    Vector3 difference = location - target.Location + new Vector3(-width / 2, -depth / 2, 0);
                    float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return difference;

                }

                else if (target.Location.Y > location.Y + depth / 2)
                {

                    Vector3 difference = location - target.Location + new Vector3(-width / 2, +depth / 2, 0);
                    float distance = difference.X * difference.X + difference.Y * difference.Y;
                    return difference;


                }

                else return (target.Location.X - location.X - width / 2 - target.Radius) * Vector3.UnitY;

            }
            else return (target.Location.Y - location.Y + depth / 2 + target.Radius) * Vector3.UnitX;


        }
    }
}



        