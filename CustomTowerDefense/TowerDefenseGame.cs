using System;
using System.Transactions;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense
{
    /// <summary>
    /// Main Class with standard MonoGame structure.
    /// Keep method content as short as possible: the complex tasks must be moved to other classes.
    /// </summary>
    public class TowerDefenseGame: Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _defaultFont;
        
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
        
        #region to scratch
        
        // TODO: this is just a test: the loading process must be elsewhere 
        SmallScoutShip _smallScoutShip = new SmallScoutShip(new Coordinate(500, 500));
        private StructureElement _structureElement = new StructureElement(new Coordinate(200, 200));
        private float xDirection = 0;
        private float yDirection = 0;
        private bool goUp = false;
        private float _globalRotationAngle = 0f;
        
        #endregion
        
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
            _graphics.ApplyChanges();
            
            // must be at the end because base.Initialize is calling LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load all the textures in a dedicated method or class
            _backgroundSprite = Content.Load<Texture2D>(@"Sprites\Starfield_Background");
            _smallScoutSprite = Content.Load<Texture2D>(@$"Sprites\{_smallScoutShip.TextureImagePath}");
            _structureElementSprite = Content.Load<Texture2D>(@$"Sprites\{_structureElement.TextureImagePath}");
            _defaultFont = Content.Load<SpriteFont>(@"Fonts\defaultFont");

            // Nowadays, even a low cost smartphone is capable of displaying 1080p resolution,
            // so let's take that as a basis. 
            _renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
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
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            
            // The background must be painted first,
            // so don't put code above this line, unless you know what you are doing.
            FillBackgroundWithBackgroundSprites();
            
            _spriteBatch.Draw(
                _smallScoutSprite,
                _smallScoutShip.GetRectangle(),
                null,
                _smallScoutShip.CurrentColorEffect,
                _smallScoutShip.RotationAngle,
                _smallScoutShip.RotationVector,
                SpriteEffects.None,
                0);
            
            _spriteBatch.Draw(
                _structureElementSprite,
                _structureElement.GetRectangle(),
                null,
                _structureElement.CurrentColorEffect,
                _globalRotationAngle,
                _structureElement.RotationVector,
                SpriteEffects.None,
                0);
            
            
            _spriteBatch.DrawString(_defaultFont, "Text test", new Vector2(100, 100), Color.White);
            
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            #region Specific scale rendering

            _scale = 1f / (1080f / _graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
        
        #endregion Standard Monogame methods

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
                    _spriteBatch.Draw(_backgroundSprite, currentBgSpritePosition, Color.White);
                    currentBgSpritePosition.Y += _backgroundSprite.Height;
                }
                
                // we move to next column
                currentBgSpritePosition.X += _backgroundSprite.Width;
            }
        }
    }
}