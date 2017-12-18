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
    class Highscore
    {
        static SpriteFont font;

        public Vector2 pos;
        public Vector2 vel;

        public int alpha = 255;

        public uint amount;

        public Highscore(uint _amount, int x, int y)
        {
            pos = new Vector2(x, y);
            vel = new Vector2(0.1f, -1f);

            amount = _amount;
        }

        public static void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/juib");
        }

        public void Update(GameTime gt)
        {
            pos += vel;

            alpha -= gt.ElapsedGameTime.Milliseconds;
        }
        
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, amount.ToString(), pos, new Color(255, 255, 255, alpha));
        }
    }
}
