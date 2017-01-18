using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace YoutubeRPG
{
    public class FadeEffect : ImageEffect
    {
        public float FadeSpeed;
        public bool Increase;

        private Image _image;

        public FadeEffect()
        {
            FadeSpeed = 1;
            Increase = false;
        }

        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
            _image = image;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_image.IsActive)
            {
                var totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (!Increase) _image.Alpha -= FadeSpeed * totalSeconds;
                else _image.Alpha += FadeSpeed * totalSeconds;

                if (_image.Alpha < 0.0f)
                {
                    Increase = true;
                    _image.Alpha = 0.0f;
                }
                else if (_image.Alpha > 1.0f)
                {
                    Increase = false;
                    _image.Alpha = 1.0f;
                }
            }
            else _image.Alpha = 1.0f;
        }
    }
}
