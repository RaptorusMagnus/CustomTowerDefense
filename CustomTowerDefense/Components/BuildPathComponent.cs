using System.Collections.Generic;
using CustomTowerDefense.Components.Shared;
using CustomTowerDefense.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CustomTowerDefense.Components
{
    public class BuildPathComponent: DrawableGameComponent, IParentComponent
    {
        private TowerDefenseGame _towerDefenseGame; 
        private readonly BackgroundComponent _backgroundComponent;
        private readonly PathGridComponent _pathGridComponent;
        
        public BuildPathComponent(TowerDefenseGame game) : base(game)
        {
            _towerDefenseGame = game;
            _backgroundComponent = new BackgroundComponent(game);
            _pathGridComponent = new PathGridComponent(game);
        }

        public override void Update(GameTime gameTime)
        {
            if (_towerDefenseGame.IsNewKey(Keys.Escape))
                _towerDefenseGame.SwitchBackToMainMenu();

            base.Update(gameTime);
        }

        public List<GameComponent> ChildComponents => GetChildComponents();
        
        private List<GameComponent> GetChildComponents()
        {
            return new() { _backgroundComponent, _pathGridComponent };
        }
    }
}