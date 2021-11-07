using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    public class MenuItem
    {
        public string Text;
        public Vector2 Position;
        public float TextSize;

        public MenuItem(string text, Vector2 position)
        {
            this.Text = text;
            this.Position = position;
            TextSize = 0.6f;
        }
    }
}