using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YoutubeRPG
{
    public class GameScreen
    {
        protected ContentManager Content;
        [XmlIgnore]
        public Type Type;

        public string XmlPath;

        public GameScreen()
        {
            Type = GetType();
            XmlPath = "../../../../Load/" + Type.ToString().Replace("YoutubeRPG.", "") + ".xml";
        }

        public virtual void LoadContent()
        {
            Content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            Content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
