using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    class Sprite
    {
        //A vector2 for position, so we can use floats.
        public Vector2 pos;
        //A vector2 for velocity, so we can scale speed to resolution easily.
        public Vector2 vel;
        //We have to use rectangle, which uses ints; we convert.
        public Rectangle hitbox;

        //Texture for the sprite.
        public Texture2D tex;

        //Boolean to trigger visibility.
        public bool visible = true;

        //Boolean to show if sprite is on screen.
        public bool onScreen = false;

        //Boolean to show if sprite is fully on screen.
        public bool fullOnScreen = false;

        //Constructor, we need a position (x and y) and we need the width and height of the object.
        public Sprite(int x, int y, int width, int height)
        {
            //Scale position and set local for argument.
            pos = new Vector2(x, y);

            //Check if position is within window.
            if (new Rectangle(0, 0, 1280, 720).Contains(pos))
            {
                fullOnScreen = true;
            }
            else if (new Rectangle(0, 0, 1280, 720).Intersects(hitbox))
            {
                onScreen = true;
            }
            else
            {
                fullOnScreen = false;
                onScreen = false;
            }

            //Scale size and set local from argument/
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(width), (int)(height));
        }

        //Load texture when we need to.
        public virtual void LoadContent(ContentManager content, string path)
        {
            //Load from textures folder.
            tex = content.Load<Texture2D>("Textures/" + path);
        }

        //Update sprite.
        public virtual void Update()
        {
            //Add velocity to position.
            pos.X += vel.X;
            pos.Y += vel.Y;

            //Check if position is within window.
            if (new Rectangle(0, 0, 1280, 720).Contains(pos))
            {
                fullOnScreen = true;
            }
            else if (new Rectangle(0, 0, 1280, 720).Intersects(hitbox))
            {
                onScreen = true;
            }
            else
            {
                fullOnScreen = false;
                onScreen = false;
            }

            //Define both inner screen boundaries.
            int left = (int)(400);
            int right = (int)((880 - hitbox.Width));

            if (pos.X < left)
                pos.X = left;
            else if (pos.X > right)
                pos.X = right;

            //Convert position into rectangle int.
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, hitbox.Width, hitbox.Height);
        }

        //Update sprite with camera option.
        public virtual void Update(Camera cam)
        {
            //Add velocity to position.
            pos.X += vel.X;
            pos.Y += vel.Y;

            //Check if position is within window.
            if (new Rectangle((int)(0 - cam.pos.X), (int)(0 - cam.pos.Y), 1280, 720).Contains(pos))
            {
                fullOnScreen = true;
            }
            else if (new Rectangle((int)(0 - cam.pos.X), (int)(0 - cam.pos.Y), 1280, 720).Intersects(hitbox))
            {
                onScreen = true;
            }
            else
            {
                fullOnScreen = false;
                onScreen = false;
            }

            //Define both inner screen boundaries.
            int left = (int)(400);
            int right = (int)((880 - hitbox.Width));

            if (pos.X < left)
                pos.X = left;
            else if (pos.X > right)
                pos.X = right;

            //Convert position into rectangle int.
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, hitbox.Width, hitbox.Height);
        }

        //Draw the sprite.
        public virtual void Draw(SpriteBatch sb)
        {
            if (visible)
                sb.Draw(tex, hitbox, Color.White);
        }
    }
}
