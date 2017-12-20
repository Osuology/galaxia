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
    class Wave
    {
        List<Group> groups;

        public int enemies = 0;

        public int last = 0;

        public Boss boss;

        public bool allDead = false;

        public Wave()
        {
            Group group1 = new Group(1, 0);
            Group group2 = new Group(2, 1);
            Group group3 = new Group(0, 2);
            Group group4 = new Group(3, 3);

            Group group5 = new Group(4, 4);
            Group group6 = new Group(0, 5);
            Group group7 = new Group(3, 6);
            Group group8 = new Group(4, 7);

            groups = new List<Group>();

            groups.Add(group1); 
            groups.Add(group2);
            groups.Add(group3);
            groups.Add(group4);
            groups.Add(group5);
            groups.Add(group6);
            groups.Add(group7);
            groups.Add(group8);

            foreach (Group group in groups)
            {
                enemies += group.enemies.Count;
            }

            boss = new Boss(400, -256, 256, 256);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Group group in groups)
            {
                group.LoadContent(content);
            }

            boss.LoadContent(content, "boss");
        }

        public void Update(ref Ship ship, GameTime gt, ref List<Highscore> scores, ref List<Bullet> bullets)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Update(ref ship, gt, ref scores, ref bullets);

                if (groups[last].allDead && i > last)
                    foreach (Enemy enemy in groups[i].enemies)
                    {
                        if (!enemy.fullOnScreen)
                            enemy.pos.Y += 8;
                        else
                            last = i;
                    }

                foreach (Enemy enemy in groups[i].enemies)
                {
                    if (enemy.hitbox.Intersects(ship.hitbox))
                    {
                        enemy.health = 0;
                        ship.Damage(1);
                    }
                }
            }

            if (groups[0].allDead  && groups[1].allDead && groups[2].allDead && groups[3].allDead && groups[4].allDead && groups[5].allDead && groups[6].allDead && groups[7].allDead)
            {
                allDead = true;
            }

            if (allDead && !boss.dead)
                boss.Update(gt, ref ship, ref scores);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!boss.dead)
                boss.Draw(sb);

            foreach (Group group in groups)
                group.Draw(sb);
        }
    }
}
