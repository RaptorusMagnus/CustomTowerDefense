using System;
using System.Collections.Generic;
using CustomTowerDefense.GameEngine;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class WaveDbEntity
    {
        /// <inheritdoc cref="Wave.IntroductionMessage"/>
        public string IntroductionMessage { get; set; }
        
        /// <inheritdoc cref="Wave.VortexSpeed"/>
        public float VortexSpeed { get; set; }
        
        /// <inheritdoc cref="Wave.Elements"/>
        public List<WaveElementDbEntity> Elements { get; set; }

        public Wave ToWave()
        {
            var returnedWave = new Wave();

            returnedWave.IntroductionMessage = IntroductionMessage;
            returnedWave.VortexSpeed = VortexSpeed;
            
            foreach (var waveElementDbEntity in Elements)
            {
                if (waveElementDbEntity.RepeatNumber > GameConstants.MaxRepeatValueForWaveElement)
                {
                    throw new Exception($"Impossible to repeat a wave element more than {GameConstants.MaxRepeatValueForWaveElement} times.");
                }
                    
                for (var i = 1; i <= waveElementDbEntity.RepeatNumber; i++)
                {
                    returnedWave.Elements.Add(waveElementDbEntity.ToWaveElement());
                }
            }

            return returnedWave;
        }
    }
}