using System.Transactions;
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

        #region Our game sprites
        
        private Texture2D _backgroundSprite;
        private Texture2D _spaceship0002;
        
        #endregion

        #region Graphical constants

        private const int ASPECT_RATIO_WIDTH = 1200;
        private const int ASPECT_RATIO_HEIGHT = 720;

        #endregion

        private RenderTarget2D _renderTarget;
        private float _scale = 0.44444f;
        
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
            _spaceship0002 = Content.Load<Texture2D>(@"Sprites\Spaceship_0002_small");

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
            
            _spriteBatch.Draw(_spaceship0002, Vector2.Zero, Color.White);
            
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