using System;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YoutubeRPG
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        [XmlIgnore]
        public Vector2 Dimensions { private set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }

        readonly XmlManager<GameScreen> _xmlGameScreenManager;

        GameScreen _currentScreen, _newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; set; }

        public static ScreenManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new XmlManager<ScreenManager>().Load("../../../../Load/ScreenManager.xml");
                return _instance;
            }
        }

        public void ChangeScreens(string screenName)
        {
            _newScreen = (GameScreen)Activator.CreateInstance(Type.GetType($"YoutubeRPG.{screenName}"));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        private void Transition(GameTime gameTime)
        {
            if (!IsTransitioning) return;
            Image.Update(gameTime);
            if (Image.Alpha == 1.0f)
            {
                _currentScreen.UnloadContent();
                _currentScreen = _newScreen;
                _xmlGameScreenManager.Type = _currentScreen.Type;
                if (File.Exists(_currentScreen.XmlPath)) _currentScreen = _xmlGameScreenManager.Load(_currentScreen.XmlPath);
                _currentScreen.LoadContent();
            }
            else if (Image.Alpha == 0.0f)
            {
                Image.IsActive = false;
                IsTransitioning = false;
            }
        }

        private ScreenManager()
        {
            Dimensions = new Vector2(640, 480);
            _currentScreen = new SplashScreen();
            _xmlGameScreenManager = new XmlManager<GameScreen> { Type = _currentScreen.Type };
            _currentScreen = _xmlGameScreenManager.Load("../../../../Load/SplashScreen.xml");
        }

        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            _currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            _currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
            if (IsTransitioning) Image.Draw(spriteBatch);
        }
    }
}
