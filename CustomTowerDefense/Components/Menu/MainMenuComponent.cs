using System.Collections.Generic;
using CustomTowerDefense.Interfaces;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Components.Menu
{
    /// <summary>
    /// Specific menu for main actions in the game.
    /// The main menu must know its content and is in charge of dispatching actions.
    /// </summary>
    public class MainMenuComponent: DrawableGameComponent, IParentComponent
    {
        private readonly MenuItemsComponent _menuItemsComponent;
        
        public MainMenuComponent(TowerDefenseGame game) : base(game)
        {
            var menuItems = new MenuItemsComponent(game, new Vector2(250, 250), Color.Red, Color.Yellow, 80);
            menuItems.AddItem("START");
            menuItems.AddItem("TOP SCORE");
            menuItems.AddItem("CREDITS");
            menuItems.AddItem("QUIT");
            
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

        public List<GameComponent> ChildComponents => GetChildComponents();

        private List<GameComponent> GetChildComponents()
        {
            return new() { _menuItemsComponent };
        }
    }
}