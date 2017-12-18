using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    class Stars
    {
        //Texture 1x1, a single pixel.
        Texture2D pixel;

        //positions of the stars.
        List<Vector2> positions;

        //Constructor.
        public Stars(GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            Color[] data = { Color.White };
            pixel.SetData<Color>(data);

            Random rand = new Random();

            //Initialize and assign star positions.
            positions = new List<Vector2>();
            for (int i = 0; i < 50; i++)
            {
                positions.Add(new Vector2(rand.Next((int)(400), (int)(880)), rand.Next(0, (int)(720))));
            }
        }

        public void Update(Camera cam)
        {
            Random rand = new Random();

            //Randomly shift 5 stars' positions.
            for (int i = 0; i < 5; i++)
                positions[rand.Next(0, positions.Count)] = new Vector2(rand.Next((int)(400), (int)(880)), rand.Next(0, (int)(720)));
        }

        public void Draw(SpriteBatch sb, Camera cam)
        {
            //Draw all stars.
            foreach (Vector2 pos in positions)
            {
                sb.Draw(pixel, new Rectangle((int)pos.X, (int)(pos.Y - cam.pos.Y), 1, 1), Color.White);
            }
        }
    }
}
