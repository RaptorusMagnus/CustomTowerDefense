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
        private Coordinate _startVortexLogicalCoordinate;
        
        // Where the enemies will go
        private Vortex _endVortex;
        private Coordinate _endVortexLogicalCoordinate;

        // The path between the two vortexes
        private List<PathElement> _path;
        
        // The structure on which we can put defense towers
        private List<StructureElement> _structureElements;

        private Color _pathColor = Color.White;
        
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
            
            _startVortexLogicalCoordinate = new Coordinate(0, 0);
            _startVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(_startVortexLogicalCoordinate));
            _gameGrid.AddGameObject(_startVortex, _startVortexLogicalCoordinate);

            _endVortexLogicalCoordinate = new Coordinate(11, 6);
            _endVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(_endVortexLogicalCoordinate))
                         {
                             CurrentColorEffect = Color.Red
                         };
            _gameGrid.AddGameObject(_endVortex, _endVortexLogicalCoordinate);

            var defenseTowerLogicalCoordinate = new Coordinate(3, 3);
            var defenseTower = new DeffenseTurretDoubleGuns(_gameGrid.GetPixelCenterFromLogicalCoordinate(defenseTowerLogicalCoordinate));
            _gameGrid.AddGameObject(defenseTower, defenseTowerLogicalCoordinate);
            
            RecomputeShortestPath();
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
                TexturesByObjectName.Add(nameof(DeffenseTurretDoubleGuns), _contentManager.Load<Texture2D>(DeffenseTurretDoubleGuns.ImagePathAndName));
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
                var clickedCoordinate = new Coordinate(input.CurrentMouseState.X, input.CurrentMouseState.Y);
                var logicalCoordinate = _gameGrid.GetLogicalCoordinateFromPixelCoordinate(clickedCoordinate);

                // Is the clicked location in the logical grid ?
                if (logicalCoordinate != null)
                {
                    var gameObject = _gameGrid.GetContentAt(logicalCoordinate.Value);
                    
                    if (gameObject == null)
                    {
                        // The location is free, we may add a new structure element.
                        var newStructureElement = new StructureElement(_gameGrid.GetPixelCenterFromLogicalCoordinate(logicalCoordinate.Value));
                        _gameGrid.AddGameObject(newStructureElement, logicalCoordinate.Value);
                        
                        var path = RecomputeShortestPath();

                        // Does the added structure element break the path?
                        if (path == null || path.Count == 0)
                        {
                            _gameGrid.RemoveObjectAt(logicalCoordinate.Value);
                        }
                    }
                    else if (gameObject is StructureElement)
                    {
                        // We had a structure element already, we remove it when the user clicks on it again.
                        _gameGrid.RemoveObjectAt(logicalCoordinate.Value);
                        var path = RecomputeShortestPath();
                    }
                }
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
                // Vortexes must turn
                _startVortex.RotationAngle -= 0.01f;
                _endVortex.RotationAngle += 0.01f;

                // When the path is not white (the case after an error), we set it progressively back to white. 
                if (_pathColor != Color.White)
                {
                    _pathColor = new Color(Math.Clamp(_pathColor.R + 3, 0, 255),
                                           Math.Clamp(_pathColor.G + 3, 0, 255),
                                           Math.Clamp(_pathColor.B + 3, 0, 255),
                                           _pathColor.A);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // we must not clear the background here because we want a standard background screen applied.
            ScreenManager.SpriteBatch.Begin();

            foreach (var currentGameObject in _gameGrid.GameObjects)
            {
                ScreenManager.SpriteBatch.Draw(
                    TexturesByObjectName[currentGameObject.GetType().Name],
                    currentGameObject.GetRectangle(),
                    null,
                    currentGameObject.CurrentColorEffect,
                    currentGameObject.RotationAngle,
                    currentGameObject.RotationOrigin,
                    SpriteEffects.None,
                    0);
            }
            
            // We must draw all path elements between the two vortexes
            foreach (var pathElement in _path)
            {
                ScreenManager.SpriteBatch.Draw(
                    TexturesByObjectName[pathElement.GetType().Name],
                    pathElement.CurrentCoordinate.GetVector2(),
                    _pathColor);
            }
            
            ScreenManager.SpriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private List<Coordinate> RecomputeShortestPath()
        {
            // We must calculate the shortest way after the structural elements.
            //var path = ShortestPathHelper.GetShortestPath(_gameGrid, _startVortexLogicalCoordinate, _endVortexLogicalCoordinate);

            var shortestPathHelper = new ShortestPathHelper();
            var path = shortestPathHelper.FindPath(_gameGrid, _startVortexLogicalCoordinate, _endVortexLogicalCoordinate);
            
            if (path == null || path.Count == 0)
            {
                // It is impossible to reach the end vortex, we must display an error.
                _pathColor = Color.Red;
                return null;
            }
            
            _path = new List<PathElement>();
            
            foreach (var currentCoordinate in path)
            {
                // The full path includes the two vortexes but we don't want them to build the visual in between path elements. 
                if (currentCoordinate.Equals(_startVortexLogicalCoordinate) ||
                    currentCoordinate.Equals(_endVortexLogicalCoordinate))
                {
                    continue;
                }
                
                _path.Add(new PathElement(_gameGrid.GetTopLeftPixelFromLogicalCoordinate(currentCoordinate)));
            }

            return path;
        }
        
        #endregion
    }
}