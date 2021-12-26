using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameEngine;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Repository.Entities;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;

namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Utility class to load waves and check their content against some business rules.
    /// Do not put any wave execution code here: this class must only load an check.
    /// </summary>
    public class WaveRepository: BaseRepository
    {
        /// <summary>
        /// Gets the number of waves defined for the received level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ushort GetNumberOfWaves(ushort level)
        {
            if (level == 0 || level >= RepositoryData.Levels.Count)
                return 0;
            
            return (ushort) (RepositoryData.Levels[level - 1].Waves?.Count ?? 0);
        }

        /// <summary>
        /// Returns the wave corresponding to the received level and wave number.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="waveNumber"></param>
        /// <returns></returns>
        [CanBeNull]
        public Wave GetWave(ushort level, ushort waveNumber)
        {
            if (level == 0 || level >= RepositoryData.Levels.Count)
                return null;

            var levelWaves = RepositoryData.Levels[level - 1].Waves ?? new List<WaveDbEntity>();
            
            if (waveNumber == 0 || waveNumber >= levelWaves.Count)
                return null;

            return levelWaves[waveNumber - 1].ToWave();
        }
    }
}