using System;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Screens.Shared
{
    public class BackgroundScreen: GameScreen
    {
        private ContentManager _contentManager;
        private Texture2D _backgroundSprite;
        
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        
        /// <summary>
        /// Loads graphics content for this screen. The background texture can be quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (_contentManager == null)
                    _contentManager = new ContentManager(ScreenManager.Game.Services, "Content");

                _backgroundSprite = _contentManager.Load<Texture2D>(@"Sprites\Starfield_Background");
            }
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            _contentManager.Unload();
        }
        
        #region Update and Draw
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;

            spriteBatch.Begin();
            
            FillBackgroundWithBackgroundSprites(spriteBatch, viewport.Width, viewport.Height);
            
            spriteBatch.End();
        }
        
        #endregion (Update and draw)

        #region Private methods
        
        /// <summary>
        /// Fill the background with the necessary number of sprites depending on the screen size.
        /// we could apply a patchwork of various sprites in the background (and it would be done in this method)
        /// but in our case the task is easy, because we only have a single sprite to repeat. 
        /// </summary>
        private void FillBackgroundWithBackgroundSprites(SpriteBatch spriteBatch, int viewportWidth, int viewportHeight)
        {
            var currentBgSpritePosition = new Vector2(0, 0);

            while(currentBgSpritePosition.X < viewportWidth)
            {
                // we paint a full column from top to bottom.
                currentBgSpritePosition.Y = 0;
                while (currentBgSpritePosition.Y < viewportHeight)
                {
                    spriteBatch.Draw(_backgroundSprite, currentBgSpritePosition, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                    
                    currentBgSpritePosition.Y += _backgroundSprite.Height;
                }
                
                // we move to next column
                currentBgSpritePosition.X += _backgroundSprite.Width;
            }
        }
        
        #endregion
    }
}