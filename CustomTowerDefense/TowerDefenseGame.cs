using CustomTowerDefense.Screens.Menu;
using CustomTowerDefense.Screens.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense
{
    /// <summary>
    /// Main Class with standard MonoGame structure.
    /// Keep method content as short as possible: the complex tasks must be moved to other classes and components.
    /// </summary>
    public class TowerDefenseGame: Game
    {
        private readonly GraphicsDeviceManager _graphics;

        #region Graphical constants

        // For standard 1.77777 aspect ratio
        public const int ASPECT_RATIO_WIDTH = 1200;
        public const int ASPECT_RATIO_HEIGHT = 720;
        
        /// <summary>
        /// Standard tiles size in this game.
        /// </summary>
        public const int TILES_SIZE = 64;

        #endregion

        private float _scale;
        
        private readonly ScreenManager _screenManager;

        public TowerDefenseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            // Create the screen factory and add it to the Services
            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            
            // Create the screen manager component.
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            // On Windows and Xbox we just add the initial screens
            AddInitialScreens();
        }

        #region private methods
        
        private void AddInitialScreens()
        {
            // Activate the first screens.
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new BackgroundFlyingShipsScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        #endregion
    }
}