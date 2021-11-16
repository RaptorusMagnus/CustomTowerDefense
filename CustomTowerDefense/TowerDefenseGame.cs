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
        
        private ScreenManager _screenManager;

        public TowerDefenseGame()
        {
            // The initialization of the graphics device manager must be done in the constructor,
            // because it is requested by the Run() method called right away in the Program Class.
            // But the modifications done on the options of the GraphicsDeviceManager must be done in the Initialize() method.
            // Otherwise they are overwritten.
            _graphics = new GraphicsDeviceManager(this);
        }
        
        /// <summary>
        /// The initialise method is the first one called by the Run() method,
        /// And some  objects like the GraphicsDeviceManager are not correctly initialized until we call the base.Initialize.
        /// So, this method is the good place to initialise the game and possibly change graphical options.
        /// </summary>
        protected override void Initialize()
        {
            // Root directory is used by the Screen manager, we must initialize it before. 
            Content.RootDirectory = "Content";
            
            // As it is built the screen manager must be added before the base.initialise,
            // because the base.Initialize is calling the loadContent and the sprite batch is not ready yet.
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            
            // Must be done first to initialize the GraphicsDeviceManager
            base.Initialize();

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            
            _graphics.PreferredBackBufferWidth = ASPECT_RATIO_WIDTH;
            _graphics.PreferredBackBufferHeight = ASPECT_RATIO_HEIGHT;
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