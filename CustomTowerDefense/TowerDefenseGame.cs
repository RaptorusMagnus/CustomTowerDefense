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
        
        #endregion

        private RenderTarget2D _renderTarget;
        
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
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            
            // must be at the end because base.Initialize is calling LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: load all the textures in a dedicated method or class
            _backgroundSprite = Content.Load<Texture2D>("Starfield_Background");

            // Nowadays, even a low cost phone is now capable of displaying 1080p resolution,
            // so let's take that as a basis. 
            _renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            
            // The background must be painted first,
            // so don't put code above this line, unless you know what you are doing.
            FillBackgroundWithBackgroundSprites();

            _spriteBatch.End();
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

            while(currentBgSpritePosition.X < _graphics.PreferredBackBufferWidth)
            {
                // we paint a full column from top to bottom.
                currentBgSpritePosition.Y = 0;
                while (currentBgSpritePosition.Y < _graphics.PreferredBackBufferHeight)
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