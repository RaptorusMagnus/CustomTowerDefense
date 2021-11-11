using System;
using System.Collections.Generic;
using CustomTowerDefense.Components;
using CustomTowerDefense.Components.Menu;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Interfaces;
using CustomTowerDefense.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense
{
    /// <summary>
    /// Main Class with standard MonoGame structure.
    /// Keep method content as short as possible: the complex tasks must be moved to other classes and components.
    /// </summary>
    public class TowerDefenseGame: Game
    {
        private readonly GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch;
        public SpriteFont DefaultFont;
        

        #region Graphical constants

        // For standard 1.77777 aspect ratio
        public const int ASPECT_RATIO_WIDTH = 1200;
        public const int ASPECT_RATIO_HEIGHT = 720;

        #endregion

        public RenderTarget2D RenderTarget;
        private float _scale;
        
        #region all game scenes available in the game

        private GameScene _currentScene;
        private GameScene _mainMenuScene;
        public GameScene BuildPathComponent;
        
        #endregion

        public KeyboardState KeyboardState;
        public KeyboardState PreviousKeyboardState;
        
        public TowerDefenseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #region Standard Monogame methods
        
        protected override void Initialize()
        {
            Window.Title = "Tower defense";
            
            // We set a standard, wide screen, aspect ratio
            _graphics.PreferredBackBufferWidth = ASPECT_RATIO_WIDTH;
            _graphics.PreferredBackBufferHeight = ASPECT_RATIO_HEIGHT;
            _graphics.IsFullScreen = false;
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            _graphics.ApplyChanges();
            
            _mainMenuScene = new GameScene(this, new MainMenuComponent(this));
            BuildPathComponent = new GameScene(this, new BuildPathComponent(this));

            // By default, to avoid any conflict, we disable everything
            DisableAllComponents();
            
            // must be at the end because base.Initialize is calling LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            DefaultFont = Content.Load<SpriteFont>(@"Fonts\defaultFont");

            // Nowadays, even a low cost smartphone is capable of displaying 1080p resolution,
            // so let's take that as a basis. 
            RenderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            
            SwitchScene(_mainMenuScene);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (_currentScene == _mainMenuScene &&
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            // Important to handle keyboard inputs correctly see IsNewKey() method
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.SetRenderTarget(null);

            #region Specific scale rendering

            _scale = 1f / (1080f / _graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            
            SpriteBatch.Draw(RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

            SpriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
        
        #endregion Standard Monogame methods

        #region public methods accessible from other components
        
        /// <summary>
        /// Returns true when the given key is marked as newly pressed.
        /// Meaning that it was not pressed before, but is now down.
        /// This method is useful to avoid multiple identical actions on a single long key press. 
        /// </summary>
        /// <param name="key">The key we want to check</param>
        public bool IsNewKey(Keys key)
        {
            return KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        public void SwitchBackToMainMenu()
        {
            SwitchScene(_mainMenuScene);
        }
        
        /// <summary>
        /// Switches from current scene to the received scene.
        /// Unused components are disabled and the requested ones are enabled.
        /// </summary>
        /// <param name="scene"></param>
        public void SwitchScene(GameScene scene)
        {
            var usedComponents = scene.GetComponents();
            
            foreach (var component in Components)
            {
                var gameComponent = (GameComponent) component;
                var isUsed = usedComponents.Contains(gameComponent);
                ChangeComponentState(gameComponent, isUsed);
            }

            _currentScene = scene;
            
            // After a scene switch we reset the keyboard state to start with a brand new situation.
            PreviousKeyboardState = KeyboardState;
        }
        
        #endregion public methods

        #region private methods

        private void ChangeComponentState(GameComponent component, bool enabled)
        {
            component.Enabled = enabled;

            if (component is DrawableGameComponent drawableGameComponent)
            {
                drawableGameComponent.Visible = enabled;
            }
        }

        private void DisableAllComponents()
        {
            foreach (var gameComponent in Components)
            {
                var component = (GameComponent) gameComponent;
                ChangeComponentState(component, false);
            }
        }

        #endregion
    }
}