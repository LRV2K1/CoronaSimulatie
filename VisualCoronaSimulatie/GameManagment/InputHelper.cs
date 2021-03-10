using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.GameManagment
{   
    public enum MouseButton
    {
        Left,
        Right,
        Middel,
        None
    }

    public class InputHelper
    {
        protected MouseState currentMouseState, previousMouseState;
        protected KeyboardState currentKeyboardState, previousKeyboardState;
        protected Vector2 scale, offset;
        protected char textbuffer;

        public InputHelper()
        {
            //initialize states
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;
            currentMouseState = Mouse.GetState();
            previousMouseState = currentMouseState;
            scale = Vector2.One;
            offset = Vector2.Zero;
            textbuffer = '\0';
        }

        /// <summary>
        /// Update all input states.
        /// </summary>
        public virtual void Update()
        {
            //read new states
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// The scale of the MouseMovement.
        /// </summary>
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// The offset for the MousePosition.
        /// </summary>
        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        /// <summary>
        /// Gives the current mouse position on the screen.
        /// </summary>
        public Vector2 MousePosition
        {
            get { return (new Vector2(currentMouseState.X, currentMouseState.Y) - offset) / scale; }
        }

        /// <summary>
        /// Returns true when the mousebutton has just been pressed.
        /// </summary>
        public bool MouseButtonPressed(MouseButton m)
        {
            switch (m)
            {
                case (MouseButton.Left):
                    return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case (MouseButton.Right):
                    return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                case (MouseButton.Middel):
                    return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true when the mousebutton is held down.
        /// </summary>
        public bool MouseButtonDown(MouseButton m)
        {
            if (m == MouseButton.Left)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (m == MouseButton.Right)
            {
                return currentMouseState.RightButton == ButtonState.Pressed;
            }
            else if (m == MouseButton.Middel)
            {
                return currentMouseState.MiddleButton == ButtonState.Pressed;
            }
            else return false;
        }

        /// <summary>
        /// Returns true when the mousebutton has just been released.
        /// </summary>
        public bool MouseButtonReleased(MouseButton m)
        {
            if (m == MouseButton.Left)
            {
                return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (m == MouseButton.Right)
            {
                return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
            }
            else if (m == MouseButton.Middel)
            {
                return currentMouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed;
            }
            else return false;
        }

        /// <summary>
        /// Returns true when the scrolwheel has just been moved up.
        /// </summary>
        public bool ScrolUp()
        {
            return currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Returns true when the scrolwheel has just been moved down.
        /// </summary>
        public bool ScrolDown()
        {
            return currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Returns true when the key has just been pressed.
        /// </summary>
        public bool KeyPressed(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
        }

        /// <summary>
        /// Returns true when the key is held down.
        /// </summary>
        public bool IsKeyDown(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Returns true when the key has just been released.
        /// </summary>
        public bool KeyReleased(Keys k)
        {
            return currentKeyboardState.IsKeyUp(k) && previousKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Returns true when any key is pressed.
        /// </summary>
        public bool AnyKeyPressed
        {
            get { return currentKeyboardState.GetPressedKeys().Length > 0 && previousKeyboardState.GetPressedKeys().Length == 0; }
        }

        /// <summary>
        /// Updates the textbuffer.
        /// </summary>
        public void TextInput(object sneder, TextInputEventArgs args)
        {
            textbuffer = args.Character;
        }

        /// <summary>
        /// Read the text that is stored in the textbuffer.
        /// </summary>
        public char GetText()
        {
            char text = textbuffer;
            textbuffer = '\0';
            return text;
        }
    }
}
