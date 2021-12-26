using System;
using System.Collections.Generic;
using CustomTowerDefense.GameEngine;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Repository.Entities;

namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Object representation of the data that will be serialized.
    /// </summary>
    [Serializable]
    public class RepositoryData
    {
        public PlayerDbEntity Player;

        public List<LevelDbEntity> Levels;
        
        /// <summary>
        /// Returns a sample object fully filled, to make serialization tests,
        /// and produce a valid sample file that can be manually updated.
        /// </summary>
        /// <returns></returns>
        public static RepositoryData GetSampleObject()
        {
            var player = new PlayerDbEntity
            {
                Level = 1,
                WaveNumber = 1
            };
            
            var waveLevel1 = new WaveDbEntity
                                {
                                    Elements = new List<WaveElementDbEntity>
                                    {
                                        new WaveElementDbEntity
                                        {
                                            RepeatNumber = 10,
                                            DelayBeforeCreation = 0,
                                            SpaceShipType = PreciseObjectType.SmallScoutShip
                                        }
                                    }
                                };
                
            var level1 = new LevelDbEntity
            {
                StartVortexCoordinate = new CoordinateDbEntity{ X= 0, Y=0},
                EndVortexCoordinate = new CoordinateDbEntity{ X= 11, Y=6},
                Waves = new List<WaveDbEntity>{waveLevel1}
            };
            
            return new RepositoryData
            {
                Player = player,
                Levels = new List<LevelDbEntity>{level1}
            };
        }
    }
}