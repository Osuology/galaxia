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
    class Boss : Sprite
    {
        public bool intro = true;
        public bool ai = false;
        public bool dead = false;

        public int phase = 1;

        public double timer = 0;
        public double timer2 = 0;
        public double timer3 = 0;
        public double cooldown = 0;

        public int posNum = 1;

        public uint health = 180;

        public List<Missile> missiles;
        public List<Bullet> bullets;

        public Blackhole hole;

        private Texture2D missileTex;
        private Texture2D bulletTex;

        public Boss(int x, int y, int width, int height) : base(x, y, width, height)
        {
            pos.Y -= 25*4;

            missiles = new List<Missile>();
            bullets = new List<Bullet>();
            hole = new Blackhole(400, 656, 64, 64);
        }

        public override void LoadContent(ContentManager content, string path)
        {
            missileTex = content.Load<Texture2D>("Textures/missile");
            bulletTex = content.Load<Texture2D>("Textures/lazer2");

            hole.LoadContent(content, "blackhole");

            base.LoadContent(content, path);
        }

        public void Update(GameTime gt, ref Ship ship, ref List<Highscore> scores)
        {
            int num = 180 - (int)health;

            phase = (int)Math.Floor(num / 60.0) + 1;

            if (ai)
            {
                AI(gt, ref ship);
            }
            else if (intro)
            {
                Intro();
            }

            foreach (Missile missile in missiles)
            {
                missile.Update(ref ship);
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                if (bullets[i].hitbox.Intersects(ship.hitbox))
                {
                    ship.Damage(1);

                    bullets.RemoveAt(i);
                }
            }

            for (int i = ship.bullets.Count - 1; i >= 0; i--)
            {
                if (ship.bullets[i].hitbox.Intersects(hitbox))
                {
                    Damage(60);
                    ship.bullets.Remove(ship.bullets[i]);
                }
            }

            if (health <= 0)
            {
                AnimationManager.Play(hitbox.X, hitbox.Y + 64, "bigExplode");
                SoundManager.PlaySound("bigExplode");
                dead = true;

                scores.Add(new Highscore(10000000, (int)pos.X + 200, (int)pos.Y + 300));

                StaticValues.highscore += 10000000;
            }

            base.Update();
        }

        public void AI(GameTime gt, ref Ship ship)
        {
            timer -= gt.ElapsedGameTime.TotalMilliseconds;
            timer2 -= gt.ElapsedGameTime.TotalMilliseconds;
            cooldown -= gt.ElapsedGameTime.TotalMilliseconds;

            //vel = Vector2.Zero;

            if (phase == 1)
                Phase1(gt, ref ship);
            else if (phase == 2)
                Phase2(gt, ref ship);
            else if (phase == 3)
                Phase3(gt, ref ship);
        }

        public void Intro()
        {
            vel.Y = 2;

            if (pos.Y == -100)
            {
                vel.Y = 0;

                intro = false;
                ai = true;
            }
        }

        public void Phase1(GameTime gt, ref Ship ship)
        {
            Vector2 pos1 = new Vector2(528, -100);
            Vector2 pos2 = new Vector2(643, -100);
            Vector2 pos3 = new Vector2(752, -100);

            Vector2 center = new Vector2(pos.X + 128, pos.Y);

            Vector2[] poss = { pos1, pos2, pos3 };

            if (center == poss[posNum-1])
            {
                timer3 += gt.ElapsedGameTime.TotalMilliseconds;

                if (timer3 >= 333)
                {
                    posNum++;
                    timer3 = 0;
                }

                if (posNum > 3)
                    posNum = 1;

                vel.X = 0;
                timer = 0;
            }
            else if (poss[posNum-1].X > center.X)
            {
                vel.X = 5;
                TripleShootLazer();
            }
            else if (poss[posNum-1].X < center.X)
            {
                vel.X = -5;
                TripleShootLazer();
            }
        }

        public void Phase2(GameTime gt, ref Ship ship)
        {
            int xRouteDown = 880;
            int xRouteUp = 400;

            int yRouteLeft = 256;
            int yRouteRight = -100;

            if (pos.Y == yRouteRight && pos.X + hitbox.Width < xRouteDown)
            {
                vel.X = 5;
                vel.Y = 0;
            }
            else if (pos.X + hitbox.Width >= xRouteDown && pos.Y + hitbox.Height < yRouteLeft)
            {
                vel.X = 0;
                vel.Y = 5;

                ShootTwoLazers();
            }
            else if (pos.Y + hitbox.Height == yRouteLeft && pos.X > xRouteUp)
            {
                vel.X = -5;
                vel.Y = 0;

                ShootMissile();
            }
            else if (pos.X == xRouteUp && pos.Y > yRouteRight)
            {
                vel.X = 0;
                vel.Y = -5;

                ShootTwoLazers();
            }
        }

        public void Phase3(GameTime gt, ref Ship ship)
        {
            int xRouteDown = 880;
            int xRouteUp = 400;

            int yRouteLeft = 256;
            int yRouteRight = -100;

            if (pos.Y == yRouteRight && pos.X + hitbox.Width < xRouteDown)
            {
                vel.X = 5;
                vel.Y = 0;
            }
            else if (pos.X + hitbox.Width >= xRouteDown && pos.Y + hitbox.Height < yRouteLeft)
            {
                vel.X = 0;
                vel.Y = 5;

                ShootMissile();
            }
            else if (pos.Y + hitbox.Height == yRouteLeft && pos.X > xRouteUp)
            {
                vel.X = -5;
                vel.Y = 0;
            }
            else if (pos.X == xRouteUp && pos.Y > yRouteRight)
            {
                vel.X = 0;
                vel.Y = -5;

                TripleShootLazer();
            }

            hole.Update(ref ship, gt);
        }

        public void ShootMissile()
        {
            Missile miss = new Missile((int)pos.X + 128, (int)pos.Y + 256, 16, 32);
            miss.SetTexture(missileTex);

            if (timer <= 0)
            {
                missiles.Add(miss);
                timer = 200;
            }
        }

        public void TripleShootLazer()
        {
            Bullet bullet = new Bullet((int)pos.X + 128, (int)pos.Y + 256, 8, 26, new Vector2(0, 16));
            bullet.SetTexture(bulletTex);

            if (timer < 300 && timer2 <= 0)
            {
                bullets.Add(bullet);
                timer += 100;
                timer2 = 66;
            }
        }

        public void ShootLazer()
        {
            Bullet bullet = new Bullet((int)pos.X + 128, (int)pos.Y + 256, 16, 32, new Vector2(0, 12));
            bullet.SetTexture(bulletTex);

            if (timer2 <= 0)
            {
                bullets.Add(bullet);
                timer2 = 66;
            }
        }

        public void ShootTwoLazers()
        {
            Bullet bullet = new Bullet((int)pos.X + 48, (int)pos.Y + 256, 16, 32, new Vector2(0, 12));
            Bullet bullet2 = new Bullet((int)pos.X + 208, (int)pos.Y + 256, 16, 32, new Vector2(0, 12));
            bullet.SetTexture(bulletTex);
            bullet2.SetTexture(bulletTex);

            if (timer2 <= 0)
            {
                bullets.Add(bullet);
                bullets.Add(bullet2);
                timer2 = 66;
            }
        }

        public void Damage(int amount)
        {
            SoundManager.PlaySound("hit2");

            if (cooldown <= 0)
            {
                health -= (uint)amount;
                cooldown = 2;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Missile missile in missiles)
                missile.Draw(sb);
            foreach (Bullet bullet in bullets)
                bullet.Draw(sb);

            if (phase == 3)
                hole.Draw(sb);

            base.Draw(sb);
        }
    }

    class Missile : Sprite
    {
        public bool dead = false;

        public Missile(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void LoadContent(ContentManager content, string path)
        {
            base.LoadContent(content, path);
        }

        public void SetTexture(Texture2D texture)
        {
            tex = texture;
        }

        public void Update(ref Ship ship)
        {
            if (!dead)
            {
                vel.Y = 15;

                Vector2 pos1 = ship.pos;

                Vector2 center = new Vector2(pos.X + 32, pos.Y + 32);

                Vector2 shipCenter = new Vector2(pos1.X + 32, pos1.Y + 32);

                if (center.X < shipCenter.X + 8 && center.X > shipCenter.X - 8)
                {
                    vel.X = 0;
                }
                else if (center.X > shipCenter.X + 8)
                {
                    vel.X = -2.5f;
                }
                else if (center.X < shipCenter.X - 8)
                {
                    vel.X = 2.5f;
                }

                if (hitbox.Intersects(ship.hitbox))
                {
                    dead = true;
                    AnimationManager.Play((int)pos.X, (int)pos.Y, "explode");
                    SoundManager.PlaySound("explode");
                    ship.Damage(1);
                }

                for (int i = ship.bullets.Count - 1; i >= 0; i--)
                {
                    if (hitbox.Intersects(ship.bullets[i].hitbox))
                    {
                        dead = true;
                        AnimationManager.Play((int)pos.X, (int)pos.Y, "explode");
                        SoundManager.PlaySound("explode");

                        ship.bullets.RemoveAt(i);
                    }
                }
            }
            else
            {
                vel = Vector2.Zero;
                visible = false;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
