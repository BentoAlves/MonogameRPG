using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace YoutubeRPG
{
    public class InputManager
    {
        KeyboardState _currentKeyState, _prevKeyState;

        private static InputManager _instance;

        public static InputManager Instance
        {
            get { return _instance ?? (_instance = new InputManager()); }
        }

        public void Update()
        {
            _prevKeyState = _currentKeyState;
            if (!ScreenManager.Instance.IsTransitioning) _currentKeyState = Keyboard.GetState();
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_currentKeyState.IsKeyDown(key) && _prevKeyState.IsKeyUp(key)) return true;
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_currentKeyState.IsKeyUp(key) && _prevKeyState.IsKeyDown(key)) return true;
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_currentKeyState.IsKeyDown(key)) return true;
            }
            return false;
        }
    }
}
