using System;
using System.Collections.Generic;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense.Screens
{
    public class BuildPathScreen: GameScreen
    {
        #region Fields

        private readonly InputAction _mouseLeftCliked;
        
        private ContentManager _contentManager;
        
        float _pauseAlpha;

        private LogicalGameGridSingle _gameGrid;
        
        // Where the enemies will come from
        private Vortex _startVortex;
        
        // Where the enemies will go
        private Vortex _endVortex;

        // The path between the two vortexes
        private List<PathElement> _path;
        
        // The structure on which we can put defense towers
        private List<StructureElement> _structureElements;
        
        // for the vortex
        private float _endVortexRotationAngle;
        private float _startVortexRotationAngle;

        #endregion
        
        
        #region Initialization - Constructors

        public BuildPathScreen()
        {
            _mouseLeftCliked = new InputAction(
                Array.Empty<Buttons>(), 
                Array.Empty<Keys>(),
                new MouseButton[] {MouseButton.LeftButton},
                true);
                
            _gameGrid = new LogicalGameGridSingle(TowerDefenseGame.TILES_SIZE, 0, 0);
            
            var startVortexLogicalCoordinate = new Coordinate(0, 0);
            _startVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(startVortexLogicalCoordinate));
            _gameGrid.AddGameObject(_startVortex, startVortexLogicalCoordinate);

            var endVortexLogicalCoordinate = new Coordinate(11, 6);
            _endVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(endVortexLogicalCoordinate))
                         {
                             CurrentColorEffect = Color.Red
                         };
            _gameGrid.AddGameObject(_endVortex, endVortexLogicalCoordinate);

            // structure elements
            _structureElements = new List<StructureElement>();
            var structureElementLogicalCoordinate = new Coordinate(2, 0);
            _structureElements.Add(new StructureElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(structureElementLogicalCoordinate)));
            _gameGrid.AddGameObject(_structureElements[0], structureElementLogicalCoordinate);
            
            structureElementLogicalCoordinate = new Coordinate(11, 5);
            _structureElements.Add(new StructureElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(structureElementLogicalCoordinate)));
            _gameGrid.AddGameObject(_structureElements[1], structureElementLogicalCoordinate);
            
            structureElementLogicalCoordinate = new Coordinate(10, 5);
            _structureElements.Add(new StructureElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(structureElementLogicalCoordinate)));
            _gameGrid.AddGameObject(_structureElements[2], structureElementLogicalCoordinate);
            
            structureElementLogicalCoordinate = new Coordinate(9, 3);
            _structureElements.Add(new StructureElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(structureElementLogicalCoordinate)));
            _gameGrid.AddGameObject(_structureElements[3], structureElementLogicalCoordinate);
            
            structureElementLogicalCoordinate = new Coordinate(0, 1);
            _structureElements.Add(new StructureElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(structureElementLogicalCoordinate)));
            _gameGrid.AddGameObject(_structureElements[4], structureElementLogicalCoordinate);
            
            // We must calculate the shortest way after the structural elements.
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
                
                _path.Add(new PathElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(currentCoordinate)));
            }
        }
        
        #endregion

        #region Load/unload content

        /// <summary>
        /// Loads graphics content for this screen. Textures can be quite numerous,
        /// so we use our own local ContentManager to load it. This allows us
        /// to unload before going to another screen, whereas if we used the shared
        /// ContentManager provided by the Game class, the content would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                _contentManager ??= new ContentManager(ScreenManager.Game.Services, "Content");

                TexturesByObjectName.Add(nameof(PathElement), _contentManager.Load<Texture2D>(PathElement.ImagePathAndName));
                TexturesByObjectName.Add(nameof(Vortex), _contentManager.Load<Texture2D>(Vortex.ImagePathAndName));
                TexturesByObjectName.Add(nameof(StructureElement), _contentManager.Load<Texture2D>(StructureElement.ImagePathAndName));
            }
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            TexturesByObjectName.Clear();
            _contentManager.Unload();
        }

        #endregion

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (_mouseLeftCliked.Evaluate(input, ControllingPlayer, out var playerIndex))
            {
                _endVortex.CurrentCoordinate = new Coordinate(input.CurrentMouseState.X, input.CurrentMouseState.Y);
            }
        }

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
                TexturesByObjectName[_startVortex.GetType().Name],
                _startVortex.GetRectangle(),
                null,
                _startVortex.CurrentColorEffect,
                _startVortexRotationAngle,
                _startVortex.RotationOrigin,
                SpriteEffects.None,
                0);
            
            ScreenManager.SpriteBatch.Draw(
                TexturesByObjectName[_endVortex.GetType().Name],
                _endVortex.GetRectangle(),
                null,
                _endVortex.CurrentColorEffect,
                _endVortexRotationAngle,
                _endVortex.RotationOrigin,
                SpriteEffects.None,
                0);
            
            // We must draw all structure elements
            foreach (var structureElement in _structureElements)
            {
                ScreenManager.SpriteBatch.Draw(
                    TexturesByObjectName[structureElement.GetType().Name],
                    structureElement.CurrentCoordinate.GetVector2(),
                    Color.White);
            }
                
            
            // We must draw all path elements between the two vortexes
            foreach (var pathElement in _path)
            {
                ScreenManager.SpriteBatch.Draw(
                    TexturesByObjectName[pathElement.GetType().Name],
                    pathElement.CurrentCoordinate.GetVector2(),
                    Color.White);
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