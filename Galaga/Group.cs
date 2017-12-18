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
    class Group
    {
        public List<Enemy> enemies;

        static uint[,] group1 = {
            {1, 1, 1, 1, 1, 1, 1 }
            };
        static uint[,] group2 = {
            {0, 0, 0, 0, 0, 0, 0 }
            };
        static uint[,] group3 = {
            {0, 1, 0, 1, 0, 1, 0 }
            };
        static uint[,] group4 = {
            {1, 0, 1, 0, 1, 0, 1 }
            };
        static uint[,] group5 = {
            {0, 0, 1, 1, 1, 0, 0 }
            };

        public static List<uint[,]> groups;

        public static void InitGroups()
        {
            groups = new List<uint[,]> { };

            groups.Add(group1);
            groups.Add(group2);
            groups.Add(group3);
            groups.Add(group4);
            groups.Add(group5);
        }

        public double timer = 0;

        int x = 0;
        int z = 0;

        int enemy1 = 0;

        public bool allDead = false;

        public Group(uint type, int offset)
        {
            x = groups[(int)type].GetLength(0);
            z = groups[(int)type].GetLength(1);

            enemies = new List<Enemy>();
            for (int i = 0; i <= groups[(int)type].GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= groups[(int)type].GetLength(1) - 1; j++)
                {
                    enemies.Add(new Enemy(400 + (68 * j), (int)(0 - (offset)*524) + (172 * i), 64, 64, groups[(int)type][i,j], i+j));
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            AnimationManager.LoadContent(content);
            foreach (Enemy en in enemies)
            {
                if (en.aiType == 0)
                    en.LoadContent(content, "enemy");
                else if (en.aiType == 1)
                    en.LoadContent(content, "enemy2");
            }
        }

        public void Update(ref Ship ship, GameTime gt, ref List<Highscore> scores)
        {
            timer -= gt.ElapsedGameTime.TotalMilliseconds;

            AnimationManager.Update(gt);

            if (timer <= 0)
            {
                if (enemies.Count >= 1)
                {
                    if (enemies.First().fullOnScreen)
                        enemies.First().ai = true;

                    if (enemies.Last().fullOnScreen)
                        enemies.Last().ai = true;
                }

                timer = 500;
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(ref ship, gt, ref scores);

                if (enemies[i].dead == true)
                {
                    enemies.Remove(enemies[i]);
                }

                if (enemies.Count == 0)
                    allDead = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            AnimationManager.Draw(sb);
            foreach (Enemy en in enemies)
                en.Draw(sb);
        }
    }
}
