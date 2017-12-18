using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxia
{
    public static class Input
    {
        //Setup mouses, and keyboards.
        public static MouseState mouse;
        public static MouseState oldMouse;
        public static KeyboardState kb;
        public static KeyboardState oldKb;

        //Enum for classifying individual mouse buttons consisely.
        public enum MouseButton
        {
            Left,
            Right,
            Middle,
            LeftRight,
            LeftMiddle,
            RightMiddle
        }

        //Method for updating mouses and keyboards.
        public static void Update()
        {
            mouse = Mouse.GetState();
            kb = Keyboard.GetState();
        }

        //Method for updating old versions, which are for detecting releases/presses.
        public static void OldUpdate()
        {
            oldMouse = mouse;
            oldKb = kb;
        }

        //Method for detecting a key release, returns value.
        public static bool KeyReleased(Keys key)
        {
            if (kb.IsKeyUp(key) && oldKb.IsKeyDown(key))
                return true;
            else
                return false;
        }

        //Method for detecting a key press, returns value.
        public static bool KeyPressed(Keys key)
        {
            if (kb.IsKeyDown(key) && oldKb.IsKeyUp(key))
                return true;
            else
                return false;
        }

        //Method for detecting if a key is up, returns value.
        public static bool KeyUp(Keys key)
        {
            if (kb.IsKeyUp(key))
                return true;
            else
                return false;
        }

        //Method for detecting if a key is down, returns value.
        public static bool KeyDown(Keys key)
        {
            if (kb.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public static bool MouseButtonReleased(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }
            else if (button == MouseButton.LeftMiddle)
            {
                if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed && mouse.MiddleButton == ButtonState.Released && oldMouse.MiddleButton == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 GetMousePos()
        {
            Vector2 pos = mouse.Position.ToVector2();
            return pos / StaticValues.scale;
        }
    }
}
