using System;
using System.Collections.Generic;
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
        
        #region Our game sprites
        
        private Texture2D _backgroundSprite;
        private Texture2D _smallScoutSprite;
        private Texture2D _structureElementSprite;
        
        #endregion

        #region Graphical constants

        private const int ASPECT_RATIO_WIDTH = 1200;
        private const int ASPECT_RATIO_HEIGHT = 720;

        #endregion

        private RenderTarget2D _renderTarget;
        private float _scale = 0.44444f;
        
        #region all game scenes available in the game

        private GameScene _mainMenuScene;
        
        #endregion
        
        #region to scratch
        
        // TODO: this is just a test: the loading process must be elsewhere 
        SmallScoutShip _smallScoutShip = new SmallScoutShip(new Coordinate(500, 500));
        private StructureElement _structureElement = new StructureElement(new Coordinate(200, 200));
        private float xDirection = 0;
        private float yDirection = 0;
        private bool goUp = false;
        private float _globalRotationAngle = 0f;
        
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

            // By default, to avoid any conflict, we disable everything
            DisableAllComponents();
            
            // must be at the end because base.Initialize is calling LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load all the textures in a dedicated method or class
            _backgroundSprite = Content.Load<Texture2D>(@"Sprites\Starfield_Background");
            _smallScoutSprite = Content.Load<Texture2D>(@$"Sprites\{_smallScoutShip.TextureImagePath}");
            _structureElementSprite = Content.Load<Texture2D>(@$"Sprites\{_structureElement.TextureImagePath}");
            DefaultFont = Content.Load<SpriteFont>(@"Fonts\defaultFont");

            // Nowadays, even a low cost smartphone is capable of displaying 1080p resolution,
            // so let's take that as a basis. 
            _renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            
            SwitchScene(_mainMenuScene);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Important to handle keyboard inputs correctly see IsNewKey() method
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            #region To scratch

            _globalRotationAngle += 0.02f;

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

            var xMove = (float)Math.Cos(yDirection) * 2;
            
            
            _smallScoutShip.Move(new Vector2(xMove, yDirection));

            #endregion
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            
            // The background must be painted first,
            // so don't put code above this line, unless you know what you are doing.
            FillBackgroundWithBackgroundSprites();
            
            SpriteBatch.Draw(
                _smallScoutSprite,
                _smallScoutShip.GetRectangle(),
                null,
                _smallScoutShip.CurrentColorEffect,
                _smallScoutShip.RotationAngle,
                _smallScoutShip.RotationVector,
                SpriteEffects.None,
                0);
            
            SpriteBatch.Draw(
                _structureElementSprite,
                _structureElement.GetRectangle(),
                null,
                _structureElement.CurrentColorEffect,
                _globalRotationAngle,
                _structureElement.RotationVector,
                SpriteEffects.None,
                0);
            
            
            SpriteBatch.DrawString(DefaultFont, "Text test", new Vector2(100, 100), Color.White);
            
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            #region Specific scale rendering

            _scale = 1f / (1080f / _graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            
            SpriteBatch.Draw(_renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

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
            
            // After a scene switch we reset the keyboard state to start with a brand new situation.
            PreviousKeyboardState = KeyboardState;
        }
        
        #endregion public methods

        #region private methods

        /// <summary>
        /// Fill the background with the necessary number of sprites depending on the screen size.
        /// we could apply a patchwork of various sprites in the background (and it would be done in this method)
        /// but in our case the task is easy, because we only have a single sprite to repeat. 
        /// </summary>
        private void FillBackgroundWithBackgroundSprites()
        {
            var currentBgSpritePosition = new Vector2(0, 0);

            while(currentBgSpritePosition.X < _renderTarget.Width)
            {
                // we paint a full column from top to bottom.
                currentBgSpritePosition.Y = 0;
                while (currentBgSpritePosition.Y < _renderTarget.Height)
                {
                    SpriteBatch.Draw(_backgroundSprite, currentBgSpritePosition, Color.White);
                    currentBgSpritePosition.Y += _backgroundSprite.Height;
                }
                
                // we move to next column
                currentBgSpritePosition.X += _backgroundSprite.Width;
            }
        }
        
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