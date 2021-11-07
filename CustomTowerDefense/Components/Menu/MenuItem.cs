using System;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    public class MenuItem
    {
        public string Text;
        public Vector2 Position;
        public float TextSize;
        public Action MenuAction { get; }

        public MenuItem(string text, Vector2 position, Action menuAction)
        {
            this.Text = text;
            this.Position = position;
            TextSize = 0.6f;
            MenuAction = menuAction;
        }
    }
}