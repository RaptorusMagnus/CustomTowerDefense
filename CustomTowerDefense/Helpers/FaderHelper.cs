using System;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Helpers
{
    /// <summary>
    /// Utility class to facilitate fade effects
    /// </summary>
    public static class FaderHelper
    {
        private const ushort STANDARD_FADING_SPEED = 3;
        public static Color GetNextFadeBackToWhiteColor(Color currentColor, ushort fadeSpeed = STANDARD_FADING_SPEED)
        {
            // When the color is not white (the case after an error), we set it progressively back to white. 
            if (currentColor == Color.White)
                return Color.White;
            
            return new Color(Math.Clamp(currentColor.R + fadeSpeed, 0, 255),
                             Math.Clamp(currentColor.G + fadeSpeed, 0, 255),
                             Math.Clamp(currentColor.B + fadeSpeed, 0, 255),
                             currentColor.A);
        }
    }
}