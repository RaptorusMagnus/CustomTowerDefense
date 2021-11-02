using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    public class MainMenuComponent: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        
        public MainMenuComponent(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}