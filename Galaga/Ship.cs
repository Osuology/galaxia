using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Galaxia
{
    class Ship : Sprite
    {
        public uint health;

        public int cooldown = 0;

        public List<Bullet> bullets;

        private Texture2D bulleT;

        public Ship(int x, int y, int width, int height) : base(x, y, width, height)
        {
            health = 3;

            bullets = new List<Bullet>();
        }

        public override void LoadContent(ContentManager content, string path)
        {
            bulleT = content.Load<Texture2D>("Textures/bullet");
            base.LoadContent(content, path);
        }

        public override void Update(Camera cam)
        {
            if (cooldown > 0)
                cooldown--;

            for (int i = bullets.Count - 1; i >= 0 ; i--)
            {
                bullets[i].Update(cam);

                if (bullets[i].delete == true)
                    bullets.Remove(bullets[i]);
            }
            
            if (Input.KeyDown(Keys.LeftShift))
            {
                vel.X = 0;

                if (Input.KeyPressed(Keys.D))
                    pos.X += 45;
                else if (Input.KeyPressed(Keys.A))
                    pos.X -= 45;
            }
            else
            {
                if (Input.KeyDown(Keys.D))
                    vel.X = 3;
                else if (Input.KeyDown(Keys.A))
                    vel.X = -3;
                else
                    vel.X = 0;
            }

            if (Input.KeyPressed(Keys.Space))
            {
                Shoot();
            }

            if (health <= 0)
            {
                StaticValues.Gamestate = 3;
                visible = false;

                AnimationManager.Play((int)pos.X, (int)pos.Y, "explode");
                SoundManager.PlaySound("explode");
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Bullet bull in bullets)
                bull.Draw(sb);

            base.Draw(sb);
        }

        public void Damage(int amount)
        {
            if (cooldown == 0)
            {
                health -= (uint)amount;
                SoundManager.PlaySound("hit2");
                cooldown = 60;
            }
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet((int)pos.X + 31, (int)pos.Y - 32, 2, 32, new Vector2(0, -25));
            bullet.SetTexture(bulleT);
            bullets.Add(bullet);
            SoundManager.PlaySound("laser", -0.25f);
        }
    }

    class Bullet : Sprite
    {
        public bool delete = false;

        public Bullet(int x, int y, int width, int height, Vector2 velocity) : base(x, y, width, height)
        {
            vel = velocity;
        }

        public void SetTexture(Texture2D texture)
        {
            tex = texture;
        }

        public override void LoadContent(ContentManager content, string path)
        {
            base.LoadContent(content, path);
        }

        public override void Update(Camera cam)
        {
            if (pos.Y < 0 - cam.pos.Y)
                delete = true;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
