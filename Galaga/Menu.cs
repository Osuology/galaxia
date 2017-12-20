using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    public class MenuManager
    {
        //Galaga logo
        public Texture2D logo;
        //Gameover texture
        public Texture2D gameOver;
        //Congratulations texture
        public Texture2D beatGame;
        //Highscore texture
        public Texture2D highscore;
        //Side panels (for arcade feel)
        public Texture2D panel;
        public Texture2D inverted_panel;

        //Exit button
        public Button button;

        //Light switch
        public Switch switcha;

        //Main menu controls
        Menu mainMenu;
        //Options menu controls
        Menu optionsMenu;
        //Highscore menu controls
        Menu highscoreMenu;
        //In-game menu
        Menu ingameMenu;
        //Death menu
        Menu deathMenu;

        //Timer for timing fps
        public double timer;
        //Number for how many frames have passed
        public uint frames = 0;
        //Default value for fps.
        public uint fpsCounter = 60;

        public double timer2 = 0;

        Ship ship;

        Wave wave;

        List<Highscore> scores;

        List<Bullet> bullets;

        Texture2D lives;
        Rectangle liveSource;

        //Manager constructor.
        public MenuManager(Camera cam)
        {
            //Show button at top right corner
            button = new Button(1248, 0, 32, 32, "X");
            //Show switch in top left corner
            switcha = new Switch(0, 0, 103, 176);

            //Center all menus, initialize their options
            mainMenu = new Menu(3, new string[] { "Start Game", "High Scores", "Options" }, 640, 270);
            optionsMenu = new Menu(3, new string[] { "Switch Resolution:", "Switch FPS Limit:", "Back to Main Menu" }, 640, 270);
            highscoreMenu = new Menu(2, new string[] { "", "Back to Main Menu" }, 640, 270);
            ingameMenu = new Menu(2, new string[] { "Unpause", "Back to Main Menu" }, 640, 270);
            deathMenu = new Menu(2, new string[] { "Quit", "" }, 640, 270);

            bullets = new List<Bullet>();

            ship = new Ship(640 - 32, 720 - 64, 64, 64);

            wave = new Wave();

            scores = new List<Highscore>();

            liveSource = new Rectangle(0, 0, 34, 34);
        }

        //Load graphical content.
        public void LoadContent(ContentManager content)
        {
            //Load all textures needed
            logo = content.Load<Texture2D>("Textures/galaxia");
            gameOver = content.Load<Texture2D>("Textures/game over");
            beatGame = content.Load<Texture2D>("Textures/beatgame");
            highscore = content.Load<Texture2D>("Textures/high_score");
            panel = content.Load<Texture2D>("Textures/panel");
            inverted_panel = content.Load<Texture2D>("Textures/inverted_panel");

            //Load content of various classes
            button.LoadContent(content, "Textures/button", "Fonts/juib");
            switcha.LoadContent(content, "Textures/light_switch", "Fonts/juib");
            mainMenu.LoadContent(content);
            optionsMenu.LoadContent(content);
            highscoreMenu.LoadContent(content);
            ingameMenu.LoadContent(content);
            deathMenu.LoadContent(content);

            ship.LoadContent(content, "ship");
            wave.LoadContent(content);

            Highscore.LoadContent(content);
            
            lives = content.Load<Texture2D>("Textures/lives");
        }

        //Update all objects
        public void Update(Camera cam, GameTime gt)
        {
            StaticValues.enemies = wave.enemies;

            if (Input.KeyReleased(Keys.F1))
                StaticValues.debug = !StaticValues.debug;

            if (Input.KeyDown(Keys.LeftAlt))
                if (Input.KeyReleased(Keys.F4))
                    StaticValues.Close = true;

            button.Update();
            switcha.Update();

            liveSource = new Rectangle(0, 0, 17 * ((int)ship.health-1), 16);

            if (StaticValues.Gamestate == 0)
            {
                mainMenu.Update();
            }
            else if (StaticValues.Gamestate == 1)
            {
                ship.Update(cam, gt);

                wave.Update(ref ship, gt, ref scores, ref bullets);

                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    bullets[i].Update();

                    if (ship.hitbox.Intersects(bullets[i].hitbox))
                    {
                        ship.Damage(1);
                        bullets.Remove(bullets[i]);
                    }
                }

                if (wave.boss.dead)
                    timer2 += gt.ElapsedGameTime.TotalMilliseconds;

                if (timer2 >= 1000)
                    StaticValues.Gamestate = 4;

                if (Input.KeyPressed(Keys.Escape))
                    StaticValues.Gamestate = 2;
            }
            else if (StaticValues.Gamestate == 2)
            {
                ingameMenu.Update(cam);

                if (Input.KeyPressed(Keys.Escape))
                    StaticValues.Gamestate = 1;
            }
            else if (StaticValues.Gamestate == 3)
            {
                deathMenu.Update();
            }
            else if (StaticValues.Gamestate == 5)
                optionsMenu.Update();
            else if (StaticValues.Gamestate == 6)
                highscoreMenu.Update();

            for (int i = scores.Count - 1; i >= 0; i--)
            {
                scores[i].Update(gt);

                if (scores[i].alpha <= 0)
                    scores.RemoveAt(i);
            }
        }
        
        public void Draw(SpriteBatch sb, Camera cam, GameTime gt)
        {
            timer += gt.ElapsedGameTime.TotalMilliseconds;
            frames++;

            //Draw both panels where they should be, with the current style.
            if (switcha.on)
            {
                sb.Draw(panel, new Rectangle(0, 0 - (int)cam.pos.Y, panel.Width, panel.Height), Color.White);
                sb.Draw(panel, new Rectangle(880, 0 - (int)cam.pos.Y, panel.Width, panel.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                sb.Draw(inverted_panel, new Rectangle(0, 0 - (int)cam.pos.Y, panel.Width, panel.Height), Color.White);
                sb.Draw(inverted_panel, new Rectangle(880, 0 - (int)cam.pos.Y, panel.Width, panel.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }

            //Draw style control and exit button.
            button.Draw(sb, (int)cam.pos.Y);
            switcha.Draw(sb, (int)cam.pos.Y);

            //If we are at the main menu...
            if (StaticValues.Gamestate == 0)
            {
                //Draw logo in center.
                sb.Draw(logo, new Rectangle(1280 / 2, 720 / 4, logo.Width * 3, logo.Height * 3), null, Color.White, 0f, new Vector2(logo.Width / 2, logo.Height / 2), SpriteEffects.None, 0f);
                //And draw the main menu options.
                mainMenu.Draw(sb);
            }
            else if (StaticValues.Gamestate == 1)
            {
                ship.Draw(sb);

                wave.Draw(sb);

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(sb);
                }

                sb.Draw(lives, new Vector2(400, 704), liveSource, Color.White);
            }
            else if (StaticValues.Gamestate == 2)
            {
                ship.Draw(sb);
                wave.Draw(sb);

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(sb);
                }

                ingameMenu.Draw(sb);
                sb.Draw(lives, new Vector2(400, 704), liveSource, Color.White);
            }
            else if (StaticValues.Gamestate == 3)
            {
                ship.Draw(sb);

                wave.Draw(sb);

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(sb);
                }

                deathMenu.Draw(sb, cam);

                sb.Draw(gameOver, new Rectangle(1280 / 2, 720 / 4, (int)(gameOver.Width * 2.5), (int)(gameOver.Height * 2.5)), null, Color.White, 0f, new Vector2(gameOver.Width / 2, gameOver.Height / 2), SpriteEffects.None, 0f);
            }
            else if (StaticValues.Gamestate == 4)
            {
                ship.Draw(sb);

                sb.Draw(beatGame, new Rectangle(1280 / 2, 720 / 4, (int)(beatGame.Width * 2.5), (int)(beatGame.Height * 2.5)), null, Color.White, 0f, new Vector2(beatGame.Width / 2, beatGame.Height / 2), SpriteEffects.None, 0f);
            }
            else if (StaticValues.Gamestate == 5)
            {
                //Draw logo in center.
                sb.Draw(logo, new Rectangle(1280 / 2, 720 / 4, logo.Width * 3, logo.Height * 3), null, Color.White, 0f, new Vector2(logo.Width / 2, logo.Height / 2), SpriteEffects.None, 0f);

                optionsMenu.Draw(sb);

                if (StaticValues.fpsOptions[StaticValues.fpsLimit] > 0 && StaticValues.fpsOptions[StaticValues.fpsLimit] < 1000)
                sb.DrawString(highscoreMenu.font, StaticValues.fpsOptions[StaticValues.fpsLimit].ToString(), new Vector2(760, 330), Color.White);
                else if (StaticValues.fpsOptions[StaticValues.fpsLimit] == 0)
                    sb.DrawString(highscoreMenu.font, "VSync", new Vector2(776, 330), Color.White);
                else
                    sb.DrawString(highscoreMenu.font, "Unlimited", new Vector2(776, 330), Color.White);

                sb.DrawString(highscoreMenu.font, StaticValues.res[StaticValues.currentRes].ToString(), new Vector2(640, 296), Color.White);

            }
            else if (StaticValues.Gamestate == 6)
            {
                for (int i = 0; i < StaticValues.highscores.Count; i++)
                {
                    sb.DrawString(highscoreMenu.font, StaticValues.highscores[i].ToString(), new Vector2(640 - highscoreMenu.font.MeasureString(StaticValues.highscores[i].ToString()).X/2, 256 + (64 * i)), Color.White);
                }

                sb.Draw(highscore, new Rectangle(1280 / 2, 720 / 4, (int)(highscore.Width * 2.5), (int)(highscore.Height * 2.5)), null, Color.White, 0f, new Vector2(highscore.Width / 2, highscore.Height / 2), SpriteEffects.None, 0f);

                highscoreMenu.Draw(sb);
            }

            foreach (Highscore score in scores)
            {
                score.Draw(sb);
            }

            if (switcha.on)
            {
                sb.DrawString(mainMenu.font, StaticValues.highscore + " score.", new Vector2(200 - (mainMenu.font.MeasureString(StaticValues.highscore + " scores.").X / 2), 720 - mainMenu.font.MeasureString(StaticValues.highscore + " score.").Y), Color.Black);

                sb.DrawString(mainMenu.font, "A or D to move.", new Vector2(1080 - mainMenu.font.MeasureString("A or D to move.").X/2, 128), Color.Black);

                sb.DrawString(mainMenu.font, "Space to shoot.", new Vector2(1080 - mainMenu.font.MeasureString("Space to shoot.").X/2, 128 + 64), Color.Black);

                sb.DrawString(mainMenu.font, "Left shift + A or D to dodge.", new Vector2(1080 - mainMenu.font.MeasureString("Left shift + A or D to dodge.").X/2, 256), Color.Black);

                sb.DrawString(mainMenu.font, "Escape to go to menu.", new Vector2(1080 - mainMenu.font.MeasureString("Escape to go to menu.").X/2, 256 + 64), Color.Black);
            }
            else
            {
                sb.DrawString(mainMenu.font, StaticValues.highscore + " score.", new Vector2(200 - (mainMenu.font.MeasureString(StaticValues.highscore + " scores.").X / 2), 720 - mainMenu.font.MeasureString(StaticValues.highscore + " score.").Y), Color.White);

                sb.DrawString(mainMenu.font, "A or D to move.", new Vector2(1080 - mainMenu.font.MeasureString("A or D to move.").X / 2, 128), Color.White);

                sb.DrawString(mainMenu.font, "Space to shoot.", new Vector2(1080 - mainMenu.font.MeasureString("Space to shoot.").X / 2, 128 + 64), Color.White);

                sb.DrawString(mainMenu.font, "Left shift + A or D to dodge.", new Vector2(1080 - mainMenu.font.MeasureString("Left shift + A or D to dodge.").X / 2, 256), Color.White);

                sb.DrawString(mainMenu.font, "Escape to go to menu.", new Vector2(1080 - mainMenu.font.MeasureString("Escape to go to menu.").X / 2, 256 + 64), Color.White);
            }

            if (timer > 1000)
            {
                fpsCounter = frames;
                timer = 0;
                frames = 0;
            }
            if (StaticValues.debug && switcha.on)
            {
                sb.DrawString(mainMenu.font, fpsCounter + " fps.", Vector2.Zero, Color.Black);
                sb.DrawString(mainMenu.font, ship.bullets.Count + " lazers.", new Vector2(0, 72), Color.Black);
                sb.DrawString(mainMenu.font, wave.enemies + " enemies.", new Vector2(0, 144), Color.Black);
            }
            else if (StaticValues.debug)
            {
                sb.DrawString(mainMenu.font, fpsCounter + " fps.", Vector2.Zero, Color.White);
                sb.DrawString(mainMenu.font, ship.bullets.Count + " lazers.", new Vector2(0, 72), Color.White);
                sb.DrawString(mainMenu.font, wave.enemies + " enemies.", new Vector2(0, 144), Color.White);
            }
        }
    }

    public class Menu
    {
        List<TextOption> texts;
        public SpriteFont font;

        List<string> stuff;
        List<Vector2> locations;

        public Menu(int num, string[] text, int x, int y)
        {
            texts = new List<TextOption>();
            stuff = new List<string>();
            locations = new List<Vector2>();

            for (int i = 0; i < num; i++)
            {
                texts.Add(new TextOption(x, y + (60*i), 0, 0, text[i]));
            }
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/juib");

            foreach (TextOption Object in texts)
            {
                Object.LoadContent(content, "Textures/none", "Fonts/juib");
                Object.hitbox.X -= (int)(Object.font.MeasureString(Object._text).X) / 2;

                Object.hitbox = new Rectangle(Object.hitbox.X, Object.hitbox.Y, (int)(Object.font.MeasureString(Object._text).X), (int)(Object.font.MeasureString(Object._text).Y));
            }
        }

        public void AddText(string text, int x, int y)
        {
            stuff.Add(text);
            locations.Add(new Vector2(x, y));
        }

        public void Update()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                if (texts[i].hitbox.Contains(Input.GetMousePos()))
                {
                    texts[i].select = true;
                }
                else
                    texts[i].select = false;

                texts[i].Update();
            }
        }

        public void Update(Camera cam)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                if (texts[i].hitbox.Contains(Input.GetMousePos()))
                {
                    texts[i].select = true;
                }
                else
                    texts[i].select = false;

                texts[i].Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (MenuObject Object in texts)
            {
                Object.Draw(sb);
            }
            for (int i = 0; i < locations.Count; i++)
            {
                sb.DrawString(font, stuff[i], locations[i], Color.White);
            }
        }

        public void Draw(SpriteBatch sb, Camera cam)
        {
            foreach (MenuObject Object in texts)
            {
                Object.Draw(sb);
            }
            for (int i = 0; i < locations.Count; i++)
            {
                sb.DrawString(font, stuff[i], locations[i] - cam.pos, Color.White);
            }
        }
    }

    //One of the four menu objects.
    public class Switch : MenuObject
    {
        //Public param for judging whether it's on or off.
        public bool on = false;

        public Switch(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void LoadContent(ContentManager content, string path, string fontPath)
        {
            base.LoadContent(content, path, fontPath);
        }

        public override void Update()
        {
            if (hitbox.Contains(Input.GetMousePos()))
            {
                if (Input.MouseButtonReleased(Input.MouseButton.Left))
                {
                    select = !select;
                    on = !on;
                    Function();
                }
            }

            base.Update();
        }

        public void Draw(SpriteBatch sb, int posY)
        {
            if (select)
            {
                sb.Draw(tex, new Rectangle(hitbox.X, hitbox.Y - posY, hitbox.Width, hitbox.Height), new Rectangle(0, 0, 103, 176), Color.White);
            }
            else
            {
                sb.Draw(tex, new Rectangle(hitbox.X, hitbox.Y - posY, hitbox.Width, hitbox.Height), new Rectangle(0, 176, 103, 176), new Color(225, 225, 225));
            }

            base.Draw(sb);
        }
    }

    //One of the four menu objects.
    public class Button : MenuObject
    {
        string message;

        public Button(int x, int y, int width, int height, string _message) : base(x, y, width, height)
        {
            message = _message;
        }

        public override void LoadContent(ContentManager content, string path, string fontPath)
        {
            base.LoadContent(content, path, fontPath);
        }

        public override void Update()
        {
            if (hitbox.Contains(Input.GetMousePos()))
            {
                select = true;

                if (Input.MouseButtonReleased(Input.MouseButton.Left))
                {
                    Function();
                }
            }
            else
            {
                select = false;
            }

            base.Update();
        }

        public void Draw(SpriteBatch sb, int posY)
        {
            if (select)
            {
                sb.Draw(tex, new Rectangle(hitbox.X, hitbox.Y - posY, hitbox.Width, hitbox.Height), Color.White);
                sb.DrawString(font, message, new Vector2(((hitbox.X + hitbox.Width/2)-font.MeasureString(message).X/2), (hitbox.Y + hitbox.Height/2) - (font.MeasureString(message).Y/2)), Color.Black);
            }
            else
            {
                sb.Draw(tex, new Rectangle(hitbox.X, hitbox.Y - posY, hitbox.Width, hitbox.Height), new Color(225, 225, 225));
                sb.DrawString(font, message, new Vector2(((hitbox.X + hitbox.Width / 2) - font.MeasureString(message).X / 2) + 1, (hitbox.Y + hitbox.Height / 2) - (font.MeasureString(message).Y / 2)), new Color(20, 20, 20));
            }

            base.Draw(sb);
        }

        public override void Function()
        {
            if (message == "X")
            {
                StaticValues.Close = true;
            }

            base.Function();
        }
    }

    //One of the four menu objects.
    public class TextOption : MenuObject
    {
        //Stores the string we display.
        public string _text;

        public TextOption(int x, int y, int width, int height, string text) : base(x, y, width, height)
        {
            _text = text;
        }

        public override void LoadContent(ContentManager content, string path, string fontPath)
        {
            base.LoadContent(content, path, fontPath);
            hitbox = new Rectangle(hitbox.X, hitbox.Y, (int)(font.MeasureString(_text).X), (int)(font.MeasureString(_text).Y));
        }

        public override void Update()
        {
            if (select)
            {
                if (Input.MouseButtonReleased(Input.MouseButton.Left))
                {
                    Function();
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (select)
            {
                sb.DrawString(font, _text, hitbox.Location.ToVector2(), Color.White, 0f, Vector2.One, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                sb.DrawString(font, _text, hitbox.Location.ToVector2(), new Color(200, 200, 200), 0f, Vector2.One, 1f, SpriteEffects.None, 0f);
            }
            base.Draw(sb);
        }

        public override void Function()
        {
            if (_text == "Start Game")
            {
                StaticValues.Gamestate = 1;
            }
            else if (_text == "High Scores")
            {
                StaticValues.Gamestate = 6;
                StaticValues.LoadHighscores();
            }
            else if (_text == "Options")
            {
                StaticValues.Gamestate = 5;
            }
            else if (_text == "Back to Main Menu")
            {
                StaticValues.Gamestate = 0;
            }
            else if (_text == "Unpause")
            {
                StaticValues.Gamestate = 1;
            }
            else if (_text == "Quit")
            {
                StaticValues.Close = true;
            }
            else if (_text == "Switch FPS Limit:")
            {
                StaticValues.fpsLimit++;
                if (StaticValues.fpsLimit > (StaticValues.fpsOptions.Length - 1))
                {
                    StaticValues.fpsLimit = 0;
                }
            }
            else if (_text == "Switch Resolution:")
            {
                StaticValues.currentRes++;
                if (StaticValues.currentRes > (StaticValues.res.Count - 1))
                {
                    StaticValues.currentRes = 0;
                }
            }
            else if (_text == "Restart")
            {
                StaticValues.reinit = true;
            }

            base.Function();
        }
    }

    //Base menu object class.
    public class MenuObject
    {
        //Bool for telling whether object is selected.
        public bool select = false;

        //Font for any text.
        public SpriteFont font;

        //Texture for respective element.
        public Texture2D tex;

        //Hitbox
        public Rectangle hitbox;

        //Bool to destruct from menu.
        public bool destruct = false;

        //Constructor
        public MenuObject(int x, int y, int width, int height)
        {
            hitbox = new Rectangle(x, y, width, height);
        }

        //Loads graphical content.
        public virtual void LoadContent(ContentManager content, string path, string fontPath)
        {
            tex = content.Load<Texture2D>(path);
            font = content.Load<SpriteFont>(fontPath);
        }

        //Update controls
        public virtual void Update()
        {

        }

        //Draw object.
        public virtual void Draw(SpriteBatch sb)
        {

        }

        //Function of the menu object when it is activated.
        public virtual void Function()
        {

        }
    }
}
