using System;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Components.Shared
{
    /// <summary>
    /// Simple animated background with a ship flying more or less randomly.
    /// TODO: Would be nice to code nice standard rotation intelligence in the game object (not here)
    /// TODO: the movements would depend on the game object characteristics.
    /// </summary>
    public class BackgroundFlyingShips: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private Texture2D _smallScoutSprite;
        
        SmallScoutShip _smallScoutShip = new SmallScoutShip(new Coordinate(500, 500));
        private float xDirection = 0;
        private float yDirection = 0;
        private bool goUp = false;
        private bool goRight = true;
        
        public BackgroundFlyingShips(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _smallScoutSprite = _towerDefenseGame.Content.Load<Texture2D>(_smallScoutShip.TextureImagePath);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_smallScoutShip.CurrentCoordinate.X > TowerDefenseGame.ASPECT_RATIO_WIDTH)
            {
                goRight = false;
            }
            else if (_smallScoutShip.CurrentCoordinate.X < 0)
            {
                goRight = true;
            }
            
            if (goUp)
            {
                if (yDirection > -1)
                {
                    yDirection -= 0.02f;
                }
                else
                {
                    goUp = false;
                }
            }
            else
            {
                if (yDirection < 1)
                {
                    yDirection += 0.02f;
                }
                else
                {
                    goUp = true;
                }
            }

            var xMove = (float)Math.Cos(yDirection) * 3;

            if (!goRight)
                xMove *= -1;
            
            _smallScoutShip.Move(new Vector2(xMove, yDirection));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_smallScoutSprite == null)
                LoadContent();
            
            _towerDefenseGame.SpriteBatch.Begin();
            
            _towerDefenseGame.SpriteBatch.Draw(
                _smallScoutSprite,
                _smallScoutShip.GetRectangle(),
                null,
                _smallScoutShip.CurrentColorEffect,
                _smallScoutShip.RotationAngle,
                _smallScoutShip.RotationVector,
                SpriteEffects.None,
                0);
            
            _towerDefenseGame.SpriteBatch.End();
            
            base.Draw(gameTime);
        }
        
    }
}