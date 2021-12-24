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
        public PlayerEntity Player;

        public List<LevelEntity> Levels;
        
        /// <summary>
        /// Returns a sample object fully filled to make serialization tests.
        /// </summary>
        /// <returns></returns>
        public static RepositoryData GetSampleObject()
        {
            var player = new PlayerEntity {Level = 2};
            var waveLevel1 = new WaveEntity
                                {
                                    Elements = new List<WaveElementEntity>
                                    {
                                        new WaveElementEntity
                                        {
                                            RepeatNumber = 10,
                                            DelayBeforeCreation = 0,
                                            SpaceShipType = PreciseObjectType.SmallScoutShip
                                        }
                                    }
                                };
                
            var level1 = new LevelEntity
            {
                StartVortexCoordinate = new CoordinateEntity{ X= 0, Y=0},
                EndVortexCoordinate = new CoordinateEntity{ X= 11, Y=6},
                Waves = new List<WaveEntity>{waveLevel1}
            };
            
            return new RepositoryData
            {
                Player = player,
                Levels = new List<LevelEntity>{level1}
            };
        }
    }
}