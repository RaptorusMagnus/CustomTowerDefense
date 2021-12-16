using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameEngine
{
    /// <summary>
    /// Utility class to handle common tasks on waves.
    /// </summary>
    public class WaveHandler
    {
        private Wave _wave;
        private ushort _currentElementIndex;
        private TimeSpan _gameTimePreviousElement;

        public WaveHandler(Wave wave)
        {
            if (wave == null || !wave.Elements.Any())
                throw new Exception("The received wave is empty. It must contain at least one element.");

            _wave = wave;
            _currentElementIndex = 0;
            _gameTimePreviousElement = TimeSpan.Zero;
        }

        /// <summary>
        /// Pops the next wave element if it is ready.
        /// Returns null otherwise.
        /// When the pop action succeeds, the wave index goes to the next element.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public WaveElement PopNextWaveElementIfReady(GameTime gameTime)
        {
            if (_currentElementIndex >= _wave.Elements.Count)
                return null;

            if (_gameTimePreviousElement == TimeSpan.Zero)
            {
                _gameTimePreviousElement = gameTime.TotalGameTime;
            }
            
            var currentElement = _wave.Elements[_currentElementIndex];

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