using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense
{
    /// <summary>
    /// A game scene (which can be thought of as a game screen), represents a consistent set of components,
    /// glued together to create a full feature within the whole application. 
    /// </summary>
    public class GameScene
    {
        #region Private fields

        private readonly List<GameComponent> _components;
        private readonly TowerDefenseGame _towerDefenseGame;

        #endregion

        #region Constructors

        public GameScene(TowerDefenseGame mainGame, params GameComponent[] components)
        {
            _towerDefenseGame = mainGame;
            _components = new List<GameComponent>();
            
            foreach (var component in components)
            {
                AddComponent(component);
            }
        }

        #endregion
        
        
        public void AddComponent(GameComponent component)
        {
            if (_components.Contains(component))
                return;
            
            _components.Add(component);

            // The component must as well be registered in the main game
            // Since several scenes may use the same components, we must make a test to avoid adding the same ones. 
            if (!_towerDefenseGame.Components.Contains(component))
            {
                _towerDefenseGame.Components.Add(component);
            }
        }

        public void RemoveComponent(GameComponent component)
        {
            if (!_components.Contains(component))
                return;

            _components.Remove(component);
        }
        
        public List<GameComponent> GetComponents()
        {
            return _components.ToList();
        }
    }
}