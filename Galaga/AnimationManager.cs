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
    class AnimationManager
    {
        static Animation explode;
        static Animation bigExplode;

        public static void LoadContent(ContentManager content)
        {
            explode = new Animation(0, 0, 64, 64, 16, new float[] { 25, 25, 25, 25, 25, 25, 25, 25 });
            explode.LoadContent(content, "explosion-8");

            bigExplode = new Animation(0, 0, 256, 256, 64, new float[] { 20, 50, 100, 100, 100, 100, 70, 70 });
            bigExplode.LoadContent(content, "bigexplosion-8");
        }

        public static void Play(int x, int y, string animation)
        {
            if (animation == "explode")
            {
                explode.pos = new Vector2(x, y);
                explode.Play();
            }
            else if (animation == "bigExplode")
            {
                bigExplode.pos = new Vector2(x, y);
                bigExplode.Play();
            }
        }

        public static void Update(GameTime gt)
        {
            explode.Update(gt);
            bigExplode.Update(gt);
        }

        public static void Draw(SpriteBatch sb)
        {
            explode.Draw(sb);
            bigExplode.Draw(sb);
        }
    }
}
