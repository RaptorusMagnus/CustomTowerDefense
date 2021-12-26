using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameEngine
{
    public class Wave
    {
        #region Private fields

        private ushort _currentElementIndex;
        
        private TimeSpan _gameTimePreviousElement;

        #endregion
        
        /// <summary>
        /// Message that will be displayed to the user at the beginning of the wave.
        /// </summary>
        public string IntroductionMessage { get; set; }
        
        /// <summary>
        /// How fast the vortex is turning.
        /// The faster is turns the faster spaceships are going out.
        /// standard speed is 0.01f
        /// </summary>
        public float VortexSpeed { get; set; }

        public List<WaveElement> Elements { get; }

        #region Constructors

        public Wave()
        {
            Elements = new List<WaveElement>();
            
            _currentElementIndex = 0;
            _gameTimePreviousElement = TimeSpan.Zero;
        }

        #endregion

        /// <summary>
        /// Pops the next wave element if it is ready.
        /// Returns null otherwise.
        /// When the pop action succeeds, the wave index goes to the next element.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public WaveElement PopNextWaveElementIfReady(GameTime gameTime)
        {
            if (_currentElementIndex >= Elements.Count)
                return null;

            if (_gameTimePreviousElement == TimeSpan.Zero)
            {
                _gameTimePreviousElement = gameTime.TotalGameTime;
            }
            
            var currentElement = Elements[_currentElementIndex];

            if (IsNextElementReadyToGo(gameTime, currentElement))
            {
                _currentElementIndex++;
                _gameTimePreviousElement = gameTime.TotalGameTime;
                return currentElement;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the next wave element must go out.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="waveElement"></param>
        /// <returns></returns>
        private bool IsNextElementReadyToGo(GameTime gameTime, WaveElement waveElement)
        {
            return (gameTime.TotalGameTime - _gameTimePreviousElement).TotalMilliseconds >= waveElement.DelayBeforeCreation;
        }
        
    }
}