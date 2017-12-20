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
    class Enemy : Sprite
    {
        public uint health;

        public uint aiType = 0;

        public int ID;

        int cooldown = 0;
        double shootTimer = 0;

        public bool dead = false;
        public bool ai = false;
        
        Texture2D bulleT;

        int something = 0;

        public Enemy(int x, int y, int width, int height, uint ai, int id) : base(x, y, width, height)
        {
            aiType = ai;

            if (aiType == 0)
                health = 2;
            else if (aiType == 1)
                health = 1;

            ID = id;
        }

        public override void LoadContent(ContentManager content, string path)
        {
            bulleT = content.Load<Texture2D>("Textures/lazer2");

            base.LoadContent(content, path);
        }

        public void Update(ref Ship ship, GameTime gt, ref List<Highscore> scores, ref List<Bullet> bullets)
        {
            something++;
            if (cooldown > 0)
                cooldown--;

            for (int i = ship.bullets.Count - 1; i >= 0; i--)
            {
                if (ship.bullets[i].hitbox.Intersects(hitbox))
                {
                    Damage(1);
                    ship.bullets.Remove(ship.bullets[i]);
                }
            }

            if (ai)
            {
                if (aiType == 0)
                {
                    vel.Y = 4.5f;
                    Vector2 pos1 = ship.pos;

                    Vector2 center = new Vector2(pos.X + 32, pos.Y + 32);

                    Vector2 shipCenter = new Vector2(pos1.X + 32, pos1.Y + 32);

                    shootTimer -= gt.ElapsedGameTime.TotalMilliseconds;

                    if (shootTimer < 0)
                        shootTimer = 0;

                    if (center.X < shipCenter.X + 8 && center.X > shipCenter.X - 8)
                    {
                        vel.X = 0;

                        if (shootTimer == 0)
                        {
                            Shoot(ref bullets);
                            shootTimer = 250;
                        }
                    }
                    else if (center.X > shipCenter.X + 8)
                    {
                        vel.X = -2.5f;
                    }
                    else if (center.X < shipCenter.X - 8)
                    {
                        vel.X = 2.5f;
                    }
                }
                else if (aiType == 1)
                {
                    Vector2 pos1 = ship.pos;

                    Vector2 center = new Vector2(pos.X + 32, pos.Y + 32);

                    Vector2 shipCenter = new Vector2(pos1.X + 32, pos1.Y + 32);
                    
                    if (vel == null || vel == Vector2.Zero)
                    {
                        float xDif = center.X - shipCenter.X;
                        float yDif = center.Y - shipCenter.Y;

                        if (xDif < 0)
                            xDif *= -1;

                        float ratio = xDif / yDif;

                        if (center.X > shipCenter.X)
                            vel = new Vector2(15 * ratio, 15);
                        else if (center.X < shipCenter.X)
                            vel = new Vector2(-(15 * ratio), 15);
                    }
                }
            }

            if (health <= 0)
            {
                SoundManager.PlaySound("explode");
                AnimationManager.Play((int)pos.X, (int)pos.Y, "explode");
                dead = true;

                scores.Add(new Highscore(100, (int)pos.X + 48, (int)pos.Y));

                StaticValues.highscore += 100;
            }

            if (pos.Y > 720)
                dead = true;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public void Damage(int amount)
        {
            if (cooldown == 0)
            {
                health -= (uint)amount;
                SoundManager.PlaySound("hit2");
                cooldown = 2;
            }
        }

        public void Shoot(ref List<Bullet> bullets)
        {
            Bullet bullet = new Bullet((int)pos.X + 31, (int)pos.Y + 32, 2, 32, new Vector2(0, 20));
            bullet.SetTexture(bulleT);
            bullets.Add(bullet);
            SoundManager.PlaySound("laser", -1f);
        }
    }
}
