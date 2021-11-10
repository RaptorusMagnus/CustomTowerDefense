using System.Xml.Linq;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Components
{
    public class TilesGridComponent: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private Texture2D _pathElementTile;
        private Texture2D _vortexTile;
        
        // Where the enemies will come from
        private Vortex _startVortex;
        
        // Where the enemies will go
        private Vortex _endVortex;
        
        // for the vortex
        private float _globalRotationAngle = 0f;
        private float _startVortexRotationAngle = 0f;
        
        public TilesGridComponent(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
        }

        public override void Initialize()
        {
            _startVortex = new Vortex(new Coordinate(32, 32));
            _endVortex = new Vortex(new Coordinate(1168, 32));
            _endVortex.CurrentColorEffect = Color.Red;
        }

        protected override void LoadContent()
        {
            _pathElementTile = _towerDefenseGame.Content.Load<Texture2D>(@"Sprites\PathElement01");
            _vortexTile = _towerDefenseGame.Content.Load<Texture2D>(@"Sprites\vortex_64_64");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _startVortexRotationAngle -= 0.01f;
            _globalRotationAngle += 0.01f;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_vortexTile == null)
            {
                LoadContent();
            }
            
            _towerDefenseGame.SpriteBatch.Begin();
            
            _towerDefenseGame.SpriteBatch.Draw(
                _vortexTile,
                _startVortex.GetRectangle(),
                null,
                _startVortex.CurrentColorEffect,
                _startVortexRotationAngle,
                _startVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            _towerDefenseGame.SpriteBatch.Draw(
                _vortexTile,
                _endVortex.GetRectangle(),
                 null,
                _endVortex.CurrentColorEffect,
                _globalRotationAngle,
                _endVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            _towerDefenseGame.SpriteBatch.Draw(_pathElementTile, new Vector2(65, 0), Color.White);
            
            _towerDefenseGame.SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}