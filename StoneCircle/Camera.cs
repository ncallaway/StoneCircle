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
    class Camera
    {
        private Vector2 location;
        public Vector2 Location { get { return location; } set { location = value; } }

        public bool Active;

        private Actor subject;
        public Vector2 screenadjust = new Vector2(684, 384);
        List<Actor> seen = new List<Actor>();
        private float scale = 1;
        public float Scale {get{ return scale;}}
        InputController input = new InputController();
        Stage currentStage;

        private int maxX;
        private int maxY;
        public Camera()
        {
            
        }

        public Camera(Stage Parent, InputController Input)
        {
            input = Input;
            currentStage = Parent;
            maxX = currentStage.max_X;
            maxY = currentStage.max_Y;
        }


        public Camera(Actor new_subject)
        {
            setSubject(new_subject);
        }

        public void Zoom(float value)
        {
            scale += value;
        }

        public void Pan(Vector2 direction)
        {
            location += direction;
        }

        public Camera(Actor new_subject, InputController Input)
        {
            setSubject(new_subject);
            input = Input;

        }

       

        public void setSubject(Actor new_subject)
        {
            subject = new_subject;
            location = subject.Position;
        }


        public void Update(GameTime t)
        {
                Vector2 dir = Vector2.Zero;

               if (!input.IsRightBumperPressed()) dir = input.RStickPosition();
                if ((subject.Position - location).LengthSquared() < 300 && !(dir.LengthSquared() > .2f)) location = subject.Position;
                else if (dir.LengthSquared() <= .2f ) {                  
                    
                    dir = subject.Position-location;
                    dir.Normalize();
                    dir *= 3;
                        
                                     
                  }
                location += (float)t.ElapsedGameTime.TotalSeconds * 300 * 2 * dir;
                if (scale > 1.5f)  scale = 1.5f; 
                if (scale < .3)  scale = .3f;
                if (scale < 1.05 && scale > .95) scale = 1f;
                else scale = (scale + 19) / 20;

                if ((int)location.X < 0 + screenadjust.X) location.X = screenadjust.X;
                if ((int)location.X > (int)(maxX - screenadjust.X)) location.X = maxX - screenadjust.X;
                if ((int)location.Y < 0 + screenadjust.Y) location.Y = screenadjust.Y;
                if ((int)location.Y > (int)(maxY - screenadjust.Y)) location.Y = maxY - screenadjust.Y;
        }


    }
}
