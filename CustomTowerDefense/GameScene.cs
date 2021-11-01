using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense
{
    // TODO: https://www.ictdemy.com/csharp/monogame/csharp-programming-games-monogame-tetris/tetris-in-monogame-game-scene-management
    public class GameScene
    {
        private List<GameComponent> _components;
        private TowerDefenseGame _towerDefenseGame;
        
        
        public void AddComponent(GameComponent component)
        {
            _components.Add(component);
            if (!_towerDefenseGame.Components.Contains(component))
                _towerDefenseGame.Components.Add(component);
        }
    }
}