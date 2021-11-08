using CustomTowerDefense.Components.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.Components.Shared
{
    public class BackgroundComponent: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private Texture2D _backgroundSprite;
        
        public BackgroundComponent(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _backgroundSprite = _towerDefenseGame.Content.Load<Texture2D>(@"Sprites\Starfield_Background");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _towerDefenseGame.SpriteBatch.Begin();
            
            FillBackgroundWithBackgroundSprites();
            
            _towerDefenseGame.SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        #region Private methods
        
        /// <summary>
        /// Fill the background with the necessary number of sprites depending on the screen size.
        /// we could apply a patchwork of various sprites in the background (and it would be done in this method)
        /// but in our case the task is easy, because we only have a single sprite to repeat. 
        /// </summary>
        private void FillBackgroundWithBackgroundSprites()
        {
            var currentBgSpritePosition = new Vector2(0, 0);

            while(currentBgSpritePosition.X < _towerDefenseGame.RenderTarget.Width)
            {
                // we paint a full column from top to bottom.
                currentBgSpritePosition.Y = 0;
                while (currentBgSpritePosition.Y < _towerDefenseGame.RenderTarget.Height)
                {
                    _towerDefenseGame.SpriteBatch.Draw(_backgroundSprite, currentBgSpritePosition, Color.White);
                    currentBgSpritePosition.Y += _backgroundSprite.Height;
                }
                
                // we move to next column
                currentBgSpritePosition.X += _backgroundSprite.Width;
            }
        }
        
        #endregion
    }
}