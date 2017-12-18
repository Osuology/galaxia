using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    public class Camera
    {
        public Matrix matrix;
        public Vector2 pos;
        Viewport viewport;

        public Camera(Viewport view)
        {
            viewport = view;

            matrix = Matrix.CreateTranslation(view.X, view.Y, 0);

            pos = new Vector2(matrix.Translation.X, matrix.Translation.Y);
        }

        public void SetPos(int x, int y)
        {
            matrix.Translation = new Vector3(x, y, 0);
            pos = new Vector2(matrix.Translation.X, matrix.Translation.Y);
        }
    }
}
