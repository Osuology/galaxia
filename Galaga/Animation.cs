using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxia
{
    class Animation : Sprite
    {
        float[] _frameAdvanceSecs;

        Rectangle source;

        double elaspedMs = 0;

        int currentFrame = 0;
        int frames;

        int _frameSize;

        public Animation(int x, int y, int width, int height, int frameSquareSize, float[] frameAdvanceSecs) : base(x, y, width, height)
        {
            _frameAdvanceSecs = frameAdvanceSecs;
            frames = frameAdvanceSecs.Length;

            source = new Rectangle(0, 0, frameSquareSize, frameSquareSize);
            _frameSize = frameSquareSize;

            visible = false;
        }

        public override void LoadContent(ContentManager content, string path)
        {
            base.LoadContent(content, path);
        }

        public void Update(GameTime gt)
        {
            if (visible)
            {
                elaspedMs += gt.ElapsedGameTime.TotalMilliseconds;

                if (elaspedMs >= _frameAdvanceSecs[currentFrame])
                {
                    elaspedMs = 0;
                    currentFrame++;
                    if (currentFrame > frames - 1)
                    {
                        currentFrame = 0;
                        visible = false;
                    }
                    source = new Rectangle(0, _frameSize * currentFrame, _frameSize, _frameSize);
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (visible)
                sb.Draw(tex, hitbox, source, Color.White);
        }

        public void Play()
        {
            visible = true;
        }
    }
}
