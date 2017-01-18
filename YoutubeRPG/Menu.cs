using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace YoutubeRPG
{
    public class Menu
    {

        public event EventHandler OnMenuChange;

        public string Axis;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;

        private int _itemNumber;
        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnMenuChange?.Invoke(this, null);
            }
        }

        private void AlignMenuItems()
        {
            var dimensions = Vector2.Zero;
            foreach (var item in Items)
                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X) / 2, (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2);
            foreach (var item in Items)
            {
                switch (Axis)
                {
                    case "X":
                        item.Image.Position = new Vector2(dimensions.X, (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRect.Height) / 2);
                        break;
                    case "Y":
                        item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - item.Image.SourceRect.Width) / 2, dimensions.Y);
                        break;
                }

                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }
        }

        public Menu()
        {
            _id = string.Empty;
            _itemNumber = 0;
            Effects = string.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            var effects = Effects.Split(':');
            foreach (var item in Items)
            {
                item.Image.LoadContent();
                foreach (var effect in effects) item.Image.ActivateEffect(effect);
            }
            AlignMenuItems();
        }
        public void UnloadContent()
        {
            foreach (var item in Items) item.Image.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            if (Axis == "X")
            {
                if (InputManager.Instance.KeyPressed(Keys.Right))
                    _itemNumber++;
                if (InputManager.Instance.KeyPressed(Keys.Left))
                    _itemNumber--;
            }
            else if (Axis == "Y")
            {
                if (InputManager.Instance.KeyPressed(Keys.Down))
                    _itemNumber++;
                if (InputManager.Instance.KeyPressed(Keys.Up))
                    _itemNumber--;
            }
            if (_itemNumber < 0) _itemNumber = 0;
            else if (_itemNumber > Items.Count -1) _itemNumber = Items.Count - 1;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Image.IsActive = i == _itemNumber;
                Items[i].Image.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Items) item.Image.Draw(spriteBatch);
        }
    }
}
