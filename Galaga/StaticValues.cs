using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    public static class StaticValues
    {
        public static bool Close = false;
        public static byte Gamestate = 0;
        public static Vector2 scale = Vector2.One;
        public static List<uint> highscores;
        public static bool debug = false;
        public static bool tas = true;
        public static bool reinit = false;

        public static int highscore = 0;

        public static short[] fpsOptions = { 0, 60, 120, 144, 240, 288, 1000 };
        public static byte fpsLimit = 0;

        public static List<Vector2> res = new List<Vector2>();
        public static byte currentRes = 6;

        public static void LoadHighscores()
        {
            highscores = new List<uint>();

            if (File.Exists("highscores.scores"))
            {
                using (BinaryReader br = new BinaryReader(File.Open("highscores.scores", FileMode.Open)))
                {
                    var hs = br.Read();
                    Console.WriteLine(hs);

                    highscores.Add((uint)hs);
                }
            }
            else
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open("highscores.scores", FileMode.CreateNew)))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        bw.Write(00000000000);
                    }
                }
            }
        }

        public static void SaveHighscores()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open("highscores.scores", FileMode.Append)))
            {
                bw.Write(StaticValues.highscore);
            }
        }

        public static void LoadResolutions()
        {
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                res.Add(new Vector2(mode.Width, mode.Height));
            }
        }
    }
}
