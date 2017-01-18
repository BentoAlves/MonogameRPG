using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YoutubeRPG
{
    [Serializable]
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public bool IsActive;

        [XmlIgnore]
        public Texture2D Texture;
        Vector2 _origin;
        ContentManager _content;
        RenderTarget2D _renderTarget;
        SpriteFont _font;
        readonly Dictionary<string, ImageEffect> _effectList;
        public string Effects;

        public FadeEffect FadeEffect;

        private void SetEffect<T>(ref T effect)
        {
            if (effect == null) effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                var imageEffect = effect as ImageEffect;
                if (imageEffect != null)
                {
                    imageEffect.IsActive = true;
                    var obj = this;
                    imageEffect.LoadContent(ref obj);
                }
            }
            _effectList.Add(effect.GetType().ToString().Replace("YoutubeRPG.", ""), effect as ImageEffect);
        }

        public void ActivateEffect(string effect)
        {
            if (!_effectList.ContainsKey(effect)) return;
            _effectList[effect].IsActive = true; 
            var obj = this;
            _effectList[effect].LoadContent(ref obj);
        }

        public void DeactivateEffect(string effect)
        {
            if (_effectList.ContainsKey(effect))
            {
                _effectList[effect].IsActive = false;
                _effectList[effect].UnloadContent();
            }
        } 

        public Image()
        {
            Path = Text = Effects = string.Empty;
            FontName = "Fonts/Orbitron";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            _effectList = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent()
        {
            _content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Path != string.Empty) Texture = _content.Load<Texture2D>(Path);

            _font = _content.Load<SpriteFont>(FontName);

            var dimensions = Vector2.Zero; 

            if(Texture != null) dimensions.X += Texture.Width; 
            dimensions.X += _font.MeasureString(Text).X;

            dimensions.Y = Texture != null ? Math.Max(Texture.Height, _font.MeasureString(Text).Y) : _font.MeasureString(Text).Y;

            if (SourceRect == Rectangle.Empty) SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y); 
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if(Texture != null) ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.DrawString(_font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = _renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect(ref FadeEffect);

            if (Effects != string.Empty)
            {
                var split = Effects.Split(':');
                foreach (var item in split) ActivateEffect(item);
            }
        }

        public void UnloadContent()
        {
            _content.Unload();
            foreach (var effect in _effectList) DeactivateEffect(effect.Key);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in _effectList)
            {
                if(effect.Value.IsActive) effect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, 0.0f, _origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
