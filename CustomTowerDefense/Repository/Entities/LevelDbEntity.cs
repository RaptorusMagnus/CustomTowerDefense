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
    public class LevelDbEntity
    {
        /// <summary>
        /// How much money is given to the player at the beginning of the level.
        /// </summary>
        public uint InitialPlayerMoney { get; set; }
        
        public CoordinateDbEntity StartVortexCoordinate { get; set; }
        public CoordinateDbEntity EndVortexCoordinate { get; set; }
        
        public List<WaveDbEntity> Waves { get; set; }
    }
}