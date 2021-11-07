using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    public class MainMenuComponent: DrawableGameComponent
    {
        private TowerDefenseGame _towerDefenseGame;
        private MenuItemsComponent _menuItemsComponent;
        
        public MainMenuComponent(TowerDefenseGame game, MenuItemsComponent menuItems) : base(game)
        {
            _towerDefenseGame = game;
            _menuItemsComponent = menuItems;
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