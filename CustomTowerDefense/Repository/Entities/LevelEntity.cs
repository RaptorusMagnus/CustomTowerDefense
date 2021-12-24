using System;
using System.Collections.Generic;
using CustomTowerDefense.GameEngine;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.Repository.Entities
{
    /// <summary>
    /// Contains all the elements to create a level in the game.
    /// A level must have:
    ///     - a single end vortex
    ///     - a single start vortex 
    ///     - at least one wave 
    /// </summary>
    [Serializable]
    public class LevelEntity
    {
        public CoordinateEntity StartVortexCoordinate { get; set; }
        public CoordinateEntity EndVortexCoordinate { get; set; }
        
        public List<WaveEntity> Waves { get; set; }
    }
}