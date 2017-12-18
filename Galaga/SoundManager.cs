using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    static class SoundManager
    {
        static SoundEffect hit2;
        static SoundEffect laser;
        static SoundEffect explode;
        static SoundEffect bigExplode;

        public static void LoadContent(ContentManager content)
        {
            hit2 = content.Load<SoundEffect>("Sounds/hit2");
            laser = content.Load<SoundEffect>("Sounds/lazer2");
            explode = content.Load<SoundEffect>("Sounds/explode3");
            bigExplode = content.Load<SoundEffect>("Sounds/bigexplode");
        }

        public static void PlaySound(string name)
        {
            if (name == "hit2")
                hit2.Play(0.25f, 0f, 0f);
            else if (name == "laser")
                laser.Play(0.25f, 0f, 0f);
            else if (name == "explode")
                explode.Play(0.45f, 0f, 0f);
            else if (name == "bigExplode")
                bigExplode.Play(0.65f, 0f, 0f);
        }

        public static void PlaySound(string name, float pitch)
        {
            if (name == "hit2")
                hit2.Play(0.25f, pitch, 0f);
            else if (name == "laser")
                laser.Play(0.25f, pitch, 0f);
            else if (name == "explode")
                explode.Play(0.45f, pitch, 0f);
            else if (name == "bigExplode")
                bigExplode.Play(0.65f, pitch, 0f);
        }
    }
}
