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
        public static List<string> highscores;
        public static bool debug = false;
        public static bool tas = true;
        public static bool reinit = false;

        public static int enemies;

        public static int highscore = 0;

        public static float[] fpsOptions = { 0f, 60f, 120f, 144f, 240f, 288f, 1000f };
        public static byte fpsLimit = 0;

        public static List<Vector2> res = new List<Vector2>();
        public static byte currentRes = 6;

        public static void LoadHighscores()
        {
            highscores = new List<string>();

            if (File.Exists("highscores.scores"))
            {
                using (StreamReader br = new StreamReader(File.Open("highscores.scores", FileMode.Open)))
                {
                    string hs = br.ReadLine();

                    Console.WriteLine(hs);

                    highscores.Add(hs);
                }
            }
            else
            {
                using (StreamWriter bw = new StreamWriter(File.Open("highscores.scores", FileMode.Create)))
                {
                    bw.WriteLine(00000000);
                }
            }
        }

        public static void SaveHighscores()
        {
            if (highscore > Convert.ToInt32(highscores[0]))
            {
                File.Delete("highscores.scores");
                using (StreamWriter bw = new StreamWriter(File.Open("highscores.scores", FileMode.CreateNew)))
                {
                    bw.Write(StaticValues.highscore);
                }
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
