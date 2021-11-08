using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Components
{
    public class TilesGridComponent: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private Texture2D _pathElementTile;
        
        public TilesGridComponent(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
        }

        protected override void LoadContent()
        {
            _pathElementTile = _towerDefenseGame.Content.Load<Texture2D>(@"Sprites\PathElement01");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _towerDefenseGame.SpriteBatch.Begin();

            _towerDefenseGame.SpriteBatch.Draw(_pathElementTile, Vector2.Zero, Color.White);
            
            _towerDefenseGame.SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}