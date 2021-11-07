using System;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Helpers
{
    public class AnglesHelper
    {
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }
    }
}