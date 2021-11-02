using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    public class MenuItem
    {
        public string Text;
        public Vector2 Position;
        public float Size;

        public MenuItem(string text, Vector2 position)
        {
            this.Text = text;
            this.Position = position;
            Size = 0.6f;
        }
    }
}