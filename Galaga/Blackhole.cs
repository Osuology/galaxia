using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Galaxia
{
    class Blackhole : Sprite
    {
        const float gravity = 9.81f;

        public Rectangle source;

        int frame = 0;

        double timer = 0;

        public Blackhole(int x, int y, int width, int height) : base(x, y, width, height)
        {
            source = new Rectangle(0, 0, 16, 16);
        }

        public override void LoadContent(ContentManager content, string path)
        {
            base.LoadContent(content, path);
        }

        public void Update(ref Ship ship, GameTime gt)
        {
            timer += gt.ElapsedGameTime.TotalSeconds;

            if (timer >= 1f/10f)
            {
                frame++;
                if (frame >= 3)
                {
                    frame = 0;
                }
                timer = 0;
            }

            source = new Rectangle(0, 16 * frame, 16, 16);

            float distance = pos.X - ship.pos.X;

            if (distance == 0)
                distance = 1;

            if (ship.cooldown == 0)
                ship.pos.X += 300 / distance;

            if (hitbox.Intersects(ship.hitbox))
            {
                ship.Damage(1);
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, hitbox, source, Color.White);
        }
    }
}
