﻿using System;
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

        public void AddItem(string text, Action menuAction)
        {
            // setting up the position according to the item's collection index
            var position = new Vector2(_position.X, _position.Y + _items.Count * _textSize);
            var menuItem = new MenuItem(text, position, menuAction);
            
            _items.Add(menuItem);
            
            // selecting the first item when none is selected
            SelectedItem ??= menuItem;
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
            if (_towerDefenseGame.IsNewKey(Keys.Up))
                SelectPrevious();
            else if (_towerDefenseGame.IsNewKey(Keys.Down))
                SelectNext();
            else if (_towerDefenseGame.IsNewKey(Keys.Enter))
                DoMenuAction();
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            _towerDefenseGame.SpriteBatch.Begin();
            
            foreach (MenuItem item in _items)
            {
                Color color = _itemColor;
                
                if (item == SelectedItem)
                    color = _selectedItemColor;
                
                _towerDefenseGame.SpriteBatch.DrawString(_towerDefenseGame.DefaultFont, item.Text, item.Position, color);
            }
            
            _towerDefenseGame.SpriteBatch.End();
        }

        #region Private methods

        private void SelectNext()
        {
            var index = _items.IndexOf(SelectedItem);

            SelectedItem = (index < _items.Count - 1) ? _items[index + 1] : _items[0];
        }

        private void SelectPrevious()
        {
            var index = _items.IndexOf(SelectedItem);

            SelectedItem = index > 0 ? _items[index - 1] : _items.Last();
        }

        private void DoMenuAction()
        {
            SelectedItem.MenuAction?.Invoke();
        }
        
        #endregion

    }
}