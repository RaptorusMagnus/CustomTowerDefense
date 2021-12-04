﻿using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense.Screens.BuildPath
{
    public class BuildPathScreen: GameScreen
    {
        #region Private Fields

        private ushort _numberOfBlocsAvailable;
        
        private BuildPathActionButtonType? _currentActiveActionButton;
        
        private readonly InputAction _mouseLeftClicked;
        
        private ContentManager _contentManager;
        
        private float _pauseAlpha;

        private readonly LogicalGameGridMultiple _gameGrid;
        
        // Where the enemies will come from
        private readonly Vortex _startVortex;
        private readonly GridCoordinate _startVortexLogicalCoordinate;
        
        // Where the enemies will go
        private readonly Vortex _endVortex;
        private readonly GridCoordinate _endVortexLogicalCoordinate;

        // The path between the two vortexes
        private List<PathElement> _path;
        
        // Colors for elements that may turn red and progressively go back to white.
        private Color _pathColor = Color.White;
        private Color _numberOfElementsInfoColor = Color.White;

        private readonly List<BuildPathActionButtonType> _interfaceButtonTypes;
        
        #endregion
        
        
        #region Initialization - Constructors

        public BuildPathScreen()
        {
            _mouseLeftClicked = new InputAction(
                Array.Empty<Buttons>(), 
                Array.Empty<Keys>(),
                new [] {MouseButton.LeftButton},
                true);
                
            _gameGrid = new LogicalGameGridMultiple(TowerDefenseGame.TILES_SIZE, 0, 0);
            
            _startVortexLogicalCoordinate = new GridCoordinate(0, 0);
            _startVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(_startVortexLogicalCoordinate));
            _gameGrid.AddGameObject(_startVortex, _startVortexLogicalCoordinate);

            _endVortexLogicalCoordinate = new GridCoordinate(11, 6);
            _endVortex = new Vortex(_gameGrid.GetPixelCenterFromLogicalCoordinate(_endVortexLogicalCoordinate))
                         {
                             CurrentColorEffect = Color.Red
                         };
            _gameGrid.AddGameObject(_endVortex, _endVortexLogicalCoordinate);

            RecomputeShortestPath();

            // Interface buttons for current screen.
            _currentActiveActionButton = BuildPathActionButtonType.StructureElement;
            _interfaceButtonTypes = new List<BuildPathActionButtonType>
                                    {
                                        BuildPathActionButtonType.DoubleGunsTurret,
                                        BuildPathActionButtonType.StructureElement
                                    };

            //
            //         ______
            //            _\ _~-\___
            //    =  = ==(____AA____D
            //                \_____\___________________,-~~~~~~~`-.._
            //                /     o O o o o o O O o o o o o o O o  |\_
            //                `~-.__        ___..----..                  )
            //                      `---~~\___________/------------`````
            //                      =  ===(_________D
            //  
            // TODO: This hard-coded value is just a test, it will later be function of the level and game rules
            _numberOfBlocsAvailable = 18;
            
            SpawnNextSpaceShip();
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
                TexturesByObjectName.Add(nameof(SmallScoutShip), _contentManager.Load<Texture2D>(SmallScoutShip.ImagePathAndName));
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

        
        //                  .----.
        //    .---------.   | == |
        //    |.-"""""-.|   |----|
        //    ||       ||   | == |
        //    ||       ||   |----|
        //    |'-.....-'|   |::::|
        //    `"")---(""`   |___.|-
        //                /       /
        //    /:::::::::::\     /
        //   /:::=======:::\   \`\
        //   `"""""""""""""`    '-'
        // TODO: split into separate methods for each processing, and if possible in separate class.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (!_mouseLeftClicked.Evaluate(input, ControllingPlayer, out var playerIndex))
                return;
            
            var clickedCoordinate = new Coordinate(input.CurrentMouseState.X, input.CurrentMouseState.Y);
            var logicalCoordinate = _gameGrid.GetLogicalCoordinateFromPixelCoordinate(clickedCoordinate);

            // Is the clicked location in the logical grid ?
            if (logicalCoordinate != null)
            {
                ProcessClickOnTheLogicalGameGrid(logicalCoordinate.Value);
                return;
            }
                
            // Is the clicked location on an action button ?
            var clickedButton = GetActionButtonClicked(clickedCoordinate);

            if (clickedButton == null)
                return;
                
            switch (clickedButton.Value)
            {
                case BuildPathActionButtonType.DoubleGunsTurret:
                    _currentActiveActionButton = BuildPathActionButtonType.DoubleGunsTurret;
                    break;
                case BuildPathActionButtonType.StructureElement:
                    _currentActiveActionButton = BuildPathActionButtonType.StructureElement;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clickedButton), $"Unhandled button type: {clickedButton.Value}");
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

                PumpSpaceShipOutOfVortex();
                
                // When the path or any other element is not of the correct color (the case after an error),
                // we set it progressively back to white.
                _pathColor = FaderHelper.GetNextFadeBackToWhiteColor(_pathColor);
                _numberOfElementsInfoColor = FaderHelper.GetNextFadeBackToWhiteColor(_numberOfElementsInfoColor);
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
                    currentGameObject.GetScaledRectangle(currentGameObject.Scale),
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

            DrawInterfaceButtons(_interfaceButtonTypes);

            DrawInfoBar();
            
            ScreenManager.SpriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        
        #endregion  Update and draw
        
        #region Private Methods

        private BuildPathActionButtonType? GetActionButtonClicked(Coordinate coordinate)
        {
            if (coordinate.X >= TowerDefenseGame.ASPECT_RATIO_WIDTH - _gameGrid.TilesSize)
            {
                if (coordinate.Y <= 64)
                {
                    // First button clicked
                    return BuildPathActionButtonType.DoubleGunsTurret;
                }
                if (coordinate.Y <= 128)
                {
                    // Second button clicked
                    return BuildPathActionButtonType.StructureElement;
                }
            }

            return null;
        }


        private static Type GetObjectTypeFromButtonType(BuildPathActionButtonType buttonType)
        {
            return buttonType switch
            {
                BuildPathActionButtonType.DoubleGunsTurret => typeof(DeffenseTurretDoubleGuns),
                BuildPathActionButtonType.StructureElement => typeof(StructureElement),
                _ => throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null)
            };
        }

        private static BuildPathActionButtonType GetButtonTypeFromObjectType(Type buttonType)
        {
            if (buttonType == typeof(DeffenseTurretDoubleGuns)) return BuildPathActionButtonType.DoubleGunsTurret;
            if (buttonType == typeof(StructureElement)) return BuildPathActionButtonType.StructureElement;

            throw new Exception($"Unhandled button type: {buttonType}");
        }
        
        private void DrawInterfaceButtons(IEnumerable<BuildPathActionButtonType> buttonsList)
        {
            DrawInterfaceButtons(buttonsList.Select(GetObjectTypeFromButtonType));
        }

        private void DrawInterfaceButtons(IEnumerable<Type> buttonsList)
        {
            // To apply a dimmed color effect on the non selected buttons 
            var dimmedColorEffect = new Color(128, 128, 128);
            var currentOffsetY = _gameGrid.TilesSize / 2;
            var xPosition = TowerDefenseGame.ASPECT_RATIO_WIDTH - (_gameGrid.TilesSize / 2);
            const float buttonScale = 0.75f;
            
            foreach (var currentButtonObjectType in buttonsList)
            {
                var currentButtonCoordinate = new Coordinate(xPosition, currentOffsetY);
                var currentButton = (GameObject)Activator.CreateInstance(currentButtonObjectType, currentButtonCoordinate);

                if (currentButton == null)
                    throw new Exception($"Impossible to generate a button from current type: {currentButtonObjectType}");
                
                if (_currentActiveActionButton != GetButtonTypeFromObjectType(currentButtonObjectType))
                    currentButton.CurrentColorEffect = dimmedColorEffect;
                
                ScreenManager.SpriteBatch.Draw(
                    TexturesByObjectName[currentButton.GetType().Name],
                    currentButton.GetScaledRectangle(buttonScale),
                    null,
                    currentButton.CurrentColorEffect,
                    currentButton.RotationAngle,
                    currentButton.RotationOrigin,
                    SpriteEffects.None,
                    0f);
                
                currentOffsetY += _gameGrid.TilesSize;
            }
        }

        private void DrawInfoBar()
        {
            var font = ScreenManager.Font;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var bottomLeftCorner = new Vector2(0, viewport.Height);
            var availableStructureElementsText = $"Structure elements: {_numberOfBlocsAvailable}";
            var textSize = font.MeasureString(availableStructureElementsText);
            var textPosition = bottomLeftCorner - new Vector2(0, textSize.Y);
            
            ScreenManager.SpriteBatch.DrawString(font, availableStructureElementsText, textPosition, _numberOfElementsInfoColor);
        }
        
        private List<GridCoordinate> RecomputeShortestPath()
        {
            var singleObjectPerLocationGrid = _gameGrid.GetLogicalGameGridSingle(typeof(StructureElement));
            var shortestPathHelper = new ShortestPathHelper();
            var path = shortestPathHelper.FindPath(singleObjectPerLocationGrid, _startVortexLogicalCoordinate, _endVortexLogicalCoordinate);
            
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

        /// <summary>
        /// Spawns a new spaceship at the beginning of the path (the start vortex), when necessary. 
        /// </summary>
        private void SpawnNextSpaceShip()
        {
            // TODO: take the spaceship from a given list (based on the level), and not that hard-coded value.
            var newSpaceShip = new SmallScoutShip(_gameGrid.GetPixelCenterFromLogicalCoordinate(_startVortexLogicalCoordinate))
            {
                Scale = 0.1f,
                RotationAngle = _startVortex.RotationAngle
            };

            // The new space ship must be small, and must turn with the start vortex

            _gameGrid.AddGameObject(newSpaceShip, _startVortexLogicalCoordinate);
        }

        /// <summary>
        /// Does the animation of the space ship going out of the vortex.
        /// Which is, scaling up to normal size and turn at same speed than the vortex.
        /// </summary>
        private void PumpSpaceShipOutOfVortex()
        {
            var objectInTheVortex = _gameGrid.GetContentAt(_startVortexLogicalCoordinate)
                                              ?.FirstOrDefault(o => o.PreciseObjectType != PreciseObjectType.Vortex);

            if (objectInTheVortex != null)
            {
                objectInTheVortex.RotationAngle = _startVortex.RotationAngle;
                if (objectInTheVortex.Scale < 1)
                {
                    objectInTheVortex.Scale += 0.005f;
                }
                else
                {
                    _gameGrid.RemoveObjectAt(objectInTheVortex, _startVortexLogicalCoordinate);
                    SpawnNextSpaceShip();
                }
            }
        }

        private void ProcessClickOnTheLogicalGameGrid(GridCoordinate logicalCoordinate)
        {
            var gameObjects = _gameGrid.GetContentAt(logicalCoordinate);

            switch (_currentActiveActionButton)
            {
                case BuildPathActionButtonType.StructureElement when _gameGrid.IsEmptyAt(logicalCoordinate):
                    AddStructureOnEmptyLocation(logicalCoordinate);
                    break;
                case BuildPathActionButtonType.StructureElement:
                {
                    if (gameObjects?.Count > 1)
                    {
                        // We cannot do anything when there are several objects already in the cell.
                        // Generally a construction on the top of an existing structure element. 
                        break;
                    }

                    if (gameObjects?.First() is StructureElement)
                    {
                        // We had a structure element already, we remove it when the user clicks on it again.
                        _gameGrid.RemoveObjectAt(gameObjects.First(), logicalCoordinate);
                        _numberOfBlocsAvailable++;
                        RecomputeShortestPath();
                    }

                    break;
                }
                case BuildPathActionButtonType.DoubleGunsTurret when !_gameGrid.IsEmptyAt(logicalCoordinate):
                    if (gameObjects?.Count > 1)
                    {
                        // It must be possible to remove an existing turret
                        if (gameObjects[1].GetType() == typeof(DeffenseTurretDoubleGuns))
                        {
                            _gameGrid.RemoveObjectAt(gameObjects[1], logicalCoordinate);
                        }
                        
                        // We cannot do anything when there are several objects already in the cell.
                        // Generally a construction on the top of an existing structure element. 
                        break;
                    }
                    
                    if (gameObjects?.First() is StructureElement)
                    {
                        // there is a structure element, we can build a turret on it
                        var newTurret = new DeffenseTurretDoubleGuns(_gameGrid.GetPixelCenterFromLogicalCoordinate(logicalCoordinate));

                        _gameGrid.AddGameObject(newTurret, logicalCoordinate);
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles the necessary actions when a structure element is added on an empty location.
        /// As the naming suggest, no test is done, so the target location is supposed empty.
        /// </summary>
        /// <param name="logicalCoordinate"></param>
        private void AddStructureOnEmptyLocation(GridCoordinate logicalCoordinate)
        {
            if (_numberOfBlocsAvailable <= 0)
            {
                // we turn the related info text to red, so that the user understands why we don't put a new structure elment
                _numberOfElementsInfoColor = Color.Red;
                return;
            }
            
            // The location is free, we may add a new structure element.
            var newStructureElement = new StructureElement(_gameGrid.GetPixelCenterFromLogicalCoordinate(logicalCoordinate));

            _gameGrid.AddGameObject(newStructureElement, logicalCoordinate);
                    
            var path = RecomputeShortestPath();

            // Does the added structure element break the path?
            if (path == null || path.Count == 0)
            {
                _gameGrid.RemoveObjectAt(newStructureElement, logicalCoordinate);
                return;
            }

            _numberOfBlocsAvailable--;
        }
        
        #endregion Private Methods
    }
}