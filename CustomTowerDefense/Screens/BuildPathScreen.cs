using System;
using System.Collections.Generic;
using System.Diagnostics;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Screens
{
    public class BuildPathScreen: GameScreen
    {
        #region Fields
        
        private ContentManager _contentManager;
        
        // Textures used in this specific screen 
        private Texture2D _pathElementTile;
        private Texture2D _vortexTile;
        
        float _pauseAlpha;

        private LogicalGameGridSingle _gameGrid;
        
        // Where the enemies will come from
        private Vortex _startVortex;
        
        // Where the enemies will go
        private Vortex _endVortex;

        // The path between the two vortexes
        private List<PathElement> _path;
        
        // for the vortex
        private float _endVortexRotationAngle = 0f;
        private float _startVortexRotationAngle = 0f;

        #endregion
        
        
        #region Initialization - Constructors

        public BuildPathScreen()
        {
            _gameGrid = new LogicalGameGridSingle(TowerDefenseGame.TILES_SIZE, 0, 0);
            
            var startVortexLogicalCoordinate = new Coordinate(0, 0);
            _startVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(startVortexLogicalCoordinate));
            _gameGrid.AddGameObject(_startVortex, startVortexLogicalCoordinate);

            var endVortexLogicalCoordinate = new Coordinate(11, 0);
            _endVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(endVortexLogicalCoordinate))
                         {
                             CurrentColorEffect = Color.Red
                         };
            _gameGrid.AddGameObject(_endVortex, endVortexLogicalCoordinate);

            var path = ShortestPathHelper.GetShortestPath(_gameGrid, startVortexLogicalCoordinate, endVortexLogicalCoordinate);

            if (path == null)
                throw new Exception("The end vortex is unreachable and this should never be the case at initialization.");

            _path = new List<PathElement>();
            
            foreach (var currentCoordinate in path.Coordinates)
            {
                // The full path includes the two vortexes but we don't want them to build the visual in between path elements. 
                if (currentCoordinate.Equals(startVortexLogicalCoordinate) ||
                    currentCoordinate.Equals(endVortexLogicalCoordinate))
                {
                    continue;
                }
                
                _path.Add(new PathElement(_gameGrid.GetPixelCenterFromLogicalCoordinate(currentCoordinate)));
            }
        }
        
        #endregion

        #region Load/unload content

        /// <summary>
        /// Loads graphics content for this screen. The background texture can be quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (_contentManager == null)
                    _contentManager = new ContentManager(ScreenManager.Game.Services, "Content");

                _pathElementTile = _contentManager.Load<Texture2D>(@"Sprites\PathElement01");
                _vortexTile = _contentManager.Load<Texture2D>(@"Sprites\vortex_64_64");
            }
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            _contentManager.Unload();
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
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            _pauseAlpha = coveredByOtherScreen ? Math.Min(_pauseAlpha + 1f / 32, 1) : Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                _startVortexRotationAngle -= 0.01f;
                _endVortexRotationAngle += 0.01f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // we must not clear the background here because we want a standard background screen applied.
            ScreenManager.SpriteBatch.Begin();
                
            ScreenManager.SpriteBatch.Draw(
                _vortexTile,
                _startVortex.GetRectangle(),
                null,
                _startVortex.CurrentColorEffect,
                _startVortexRotationAngle,
                _startVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            ScreenManager.SpriteBatch.Draw(
                _vortexTile,
                _endVortex.GetRectangle(),
                null,
                _endVortex.CurrentColorEffect,
                _endVortexRotationAngle,
                _endVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            // We must draw all path elements between the two vortexes
            foreach (var pathElement in _path)
            {
                ScreenManager.SpriteBatch.Draw(
                    _pathElementTile,
                    pathElement.GetRectangle(),
                    null,
                    pathElement.CurrentColorEffect,
                    0f,
                    pathElement.RotationVector,
                    SpriteEffects.None,
                    0);
            }
            
            ScreenManager.SpriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}