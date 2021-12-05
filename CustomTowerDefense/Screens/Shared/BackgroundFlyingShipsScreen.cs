using System;
using System.Collections.Generic;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Screens.Shared
{
    /// <summary>
    /// Simple animated background with a ship flying more or less randomly.
    /// TODO: Would be nice to code nice standard rotation intelligence in the game object (not here)
    /// TODO: the movements would depend on the game object characteristics.
    /// </summary>
    public class BackgroundFlyingShipsScreen: GameScreen
    {
        #region Fields
        
        private Texture2D _smallScoutSprite;

        private SmallScoutShip _smallScoutShip;
        
        private float xDirection = 0;
        private float yDirection = 0;
        private bool goUp = false;
        private bool goRight = true;

        #endregion

        #region Initialisation - Constructors

        public BackgroundFlyingShipsScreen()
        {
            var gameGrid = new LogicalGameGridMultiple(TowerDefenseGame.TILES_SIZE, 0, 0);
            var path = new List<GridCoordinate>();
            _smallScoutShip = new SmallScoutShip(new Coordinate(200, 200), path, gameGrid);
        }


        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                _smallScoutSprite = ScreenManager.Game.Content.Load<Texture2D>(SmallScoutShip.ImagePathAndName);
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // We do not test the IsActive property here because this screen is a background
            // It is not actually the active screen but must always be updated 
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
        }

        public override void Draw(GameTime gameTime)
        {
            // We don't clear the screen because this background animation is just a layer above the static background.
            ScreenManager.SpriteBatch.Begin();
            
            ScreenManager.SpriteBatch.Draw(
                _smallScoutSprite,
                _smallScoutShip.GetRectangle(),
                null,
                _smallScoutShip.CurrentColorEffect,
                _smallScoutShip.RotationAngle,
                _smallScoutShip.RotationOrigin,
                SpriteEffects.None,
                0);
            
            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}