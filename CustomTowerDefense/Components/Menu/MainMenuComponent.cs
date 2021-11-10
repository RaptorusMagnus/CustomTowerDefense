using System.Collections.Generic;
using CustomTowerDefense.Components.Shared;
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
        private readonly BackgroundComponent _backgroundComponent;
        private readonly BackgroundFlyingShips _backgroundFlyingShips;
        
        public MainMenuComponent(TowerDefenseGame game) : base(game)
        {
            var menuItems = new MenuItemsComponent(game, new Vector2(250, 250), Color.Red, Color.Yellow, 80);
            menuItems.AddItem("START", null);
            menuItems.AddItem("BUILD PATH", () => game.SwitchScene(game.BuildPathComponent));
            menuItems.AddItem("TOP SCORE", null);
            menuItems.AddItem("CREDITS", null);
            menuItems.AddItem("QUIT", Game.Exit);

            _backgroundComponent = new BackgroundComponent(game);
            _backgroundFlyingShips = new BackgroundFlyingShips(game);
            
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
            return new() { _backgroundComponent, _backgroundFlyingShips, _menuItemsComponent };
        }
    }
}