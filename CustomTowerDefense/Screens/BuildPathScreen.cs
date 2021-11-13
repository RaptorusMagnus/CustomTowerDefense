using System;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Screens
{
    public class BuildPathScreen: GameScreen
    {
        #region Fields
        
        private ContentManager _contentManager;
        
        // Textures used in this specific screen 
        private Texture2D _pathElementTile;
        private Texture2D _vortexTile;
        
        float _pauseAlpha;
        
        // Where the enemies will come from
        private Vortex _startVortex;
        
        // Where the enemies will go
        private Vortex _endVortex;
        
        // for the vortex
        private float _endVortexRotationAngle = 0f;
        private float _startVortexRotationAngle = 0f;

        #endregion
        
        
        #region Initialization - Constructors

        public BuildPathScreen()
        {
            _startVortex = new Vortex(new Coordinate(32, 32));
            _endVortex = new Vortex(new Coordinate(768, 32));
            _endVortex.CurrentColorEffect = Color.Red;
        }
        
        #endregion
        
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

                _pathElementTile = _contentManager.Load<Texture2D>(@"Sprites\PathElement01");
                _vortexTile = _contentManager.Load<Texture2D>(@"Sprites\vortex_64_64");
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
                _startVortexRotationAngle -= 0.01f;
                _endVortexRotationAngle += 0.01f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            
            ScreenManager.SpriteBatch.Begin();
                
            ScreenManager.SpriteBatch.Draw(
                _vortexTile,
                _startVortex.GetRectangle(),
                null,
                _startVortex.CurrentColorEffect,
                _startVortexRotationAngle,
                _startVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            ScreenManager.SpriteBatch.Draw(
                _vortexTile,
                _endVortex.GetRectangle(),
                null,
                _endVortex.CurrentColorEffect,
                _endVortexRotationAngle,
                _endVortex.RotationVector,
                SpriteEffects.None,
                0);
            
            ScreenManager.SpriteBatch.Draw(_pathElementTile, new Vector2(65, 0), Color.White);
            
            ScreenManager.SpriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}