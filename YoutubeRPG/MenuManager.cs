using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YoutubeRPG
{
    public class MenuManager
    {
        private Menu _menu;

        public MenuManager()
        {
            _menu = new Menu();
            _menu.OnMenuChange += (sender, args) =>
            {
                _menu.UnloadContent();
                // Transition
                _menu = new XmlManager<Menu>().Load(_menu.Id);
                _menu?.LoadContent();
            };
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != string.Empty) _menu.Id = menuPath;
        }
        public void UnloadContent()
        {
            _menu.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            _menu.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }
    }
}
