using System;
using CustomTowerDefense.Shared;
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

        public static float GetAngleToReachTarget(Vector2 origin, Vector2 targetCoordinate)
        {
            var targetVsOriginVector = targetCoordinate - origin;
            var baseZeroDegreeVector = new Point(0, -1); // 12 o'clock == 0°, assuming that y goes from top to bottom (the case in XNA framework)
            
            return (float) (Math.Atan2(targetVsOriginVector.Y, targetVsOriginVector.X) - Math.Atan2(baseZeroDegreeVector.Y, baseZeroDegreeVector.X));
        }
        
        
        public static float GetAngleFromTargetSiblingTile(GridCoordinate start, GridCoordinate target)
        {
            if (target == start.RightSibling)
                return MathHelper.PiOver2;
            if (target == start.BottomSibling)
                return MathHelper.Pi;
            if (target == start.LeftSibling)
                return -MathHelper.PiOver2;
            //if (target == start.TopSibling)
            return 0;
        }
        
        /// <summary>
        /// Gets the correct rotation increment to be applied in order to turn smoothly,
        /// depending on the total rotation to do and the number of cycles left to do that rotation.
        /// </summary>
        /// <param name="rotationDifference"></param>
        /// <param name="numberOfCyclesToReachTarget"></param>
        /// <returns></returns>
        public static float GetRotationIncrementPerStep(float rotationDifference, float numberOfCyclesToReachTarget)
        {
            // First we get a base increment (that would be correct for a constant long turn)
            var rotationIncrementPerStep = Math.Abs(rotationDifference / numberOfCyclesToReachTarget);

            // But, we want a more pronounced rotation at the beginning, when the rotation difference is high.
            // The sinus function will help us having that smoothly decreasing intensity.
            const float  rotationAccelerator = 2f;
            rotationIncrementPerStep += (rotationIncrementPerStep * (float) (rotationAccelerator * Math.Sin(Math.Abs(rotationDifference))));

            // In all cases we don't want a rotation increment too small, otherwise we ship is going too far to the sides.
            return Math.Clamp(rotationIncrementPerStep, 0.01f, 10f);
        }
    }
}