using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense.Components.Menu
{
    public class MenuItemsComponent : DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private Vector2 _position;
        private List<MenuItem> _items;
        
        private Color _itemColor;
        private Color _selectedItemColor;
        private int _textSize;
        
        public MenuItem SelectedItem;
        

        public MenuItemsComponent(TowerDefenseGame towerDefenseGame, Vector2 position, Color itemColor, Color selectedItemColor, int textSize):
             base(towerDefenseGame)
        {
            this._towerDefenseGame = towerDefenseGame;
            _position = position;
            _itemColor = itemColor;
            _selectedItemColor = selectedItemColor;
            _textSize = textSize;
            _items = new List<MenuItem>();
            SelectedItem = null;
        }

        public void AddItem(string text)
        {
            // setting up the position according to the item's collection index
            var position = new Vector2(_position.X, _position.Y + _items.Count * _textSize);
            var menuItem = new MenuItem(text, position);
            
            _items.Add(menuItem);
            
            // selecting the first item when none is selected
            SelectedItem ??= menuItem;
        }
        
        public void SelectNext()
        {
            var index = _items.IndexOf(SelectedItem);

            SelectedItem = (index < _items.Count - 1) ? _items[index + 1] : _items[0];
        }
        
        public void SelectPrevious()
        {
            var index = _items.IndexOf(SelectedItem);

            SelectedItem = index > 0 ? _items[index - 1] : _items.Last();
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_towerDefenseGame.NewKey(Keys.Up))
                SelectPrevious();
            else if (_towerDefenseGame.NewKey(Keys.Down))
                SelectNext();
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}