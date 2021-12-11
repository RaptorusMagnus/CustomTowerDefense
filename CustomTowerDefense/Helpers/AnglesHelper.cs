using System;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Helpers
{
    public class AnglesHelper
    {
        public const float PI = (float)Math.PI;
        public const float TwoPI = (float) (Math.PI * 2);
        
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        /// <summary>
        /// In XNA, radiant angles are ranging from 0 to (2 * PI).
        /// Going above two PIs will in fact go over the zero and continue turning.
        /// Though many XNA framework methods are handling this correctly,
        /// We should keep angles in the default range to avoid possible weird behaviors in our methods. 
        /// So, this method will keep the given rotation angle between the default range.
        /// </summary>
        /// <param name="radiantAngle">Can possibly be greater than the max rotation</param>
        /// <returns></returns>
        [Pure]
        public static float NormalizeRadians2Pi(float radiantAngle)
        {
            var result = radiantAngle % TwoPI;
            return result > 0 ? result : result + TwoPI;
        }
        
        public static float GetAngleToReachTarget(Vector2 origin, Vector2 targetCoordinate)
        {
            var targetVsOriginVector = targetCoordinate - origin;
            var baseZeroDegreeVector = new Point(0, -1); // 12 o'clock == 0°, assuming that y goes from top to bottom (the case in XNA framework)

            var rawAngle = (float) (Math.Atan2(targetVsOriginVector.Y, targetVsOriginVector.X) -
                                    Math.Atan2(baseZeroDegreeVector.Y, baseZeroDegreeVector.X));

            return NormalizeRadians2Pi(rawAngle);
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
            return Math.Clamp(rotationIncrementPerStep, 0.01f, TwoPI);
        }

        /// <summary>
        /// Determines whether we must turn clockwise or anticlockwise,
        /// to reach a given rotation angle, from current rotation angle.
        /// </summary>
        /// <param name="currentRotation"></param>
        /// <param name="targetRotation"></param>
        /// <returns>Returns 1 or -1 to be easily used as a multiplier for rotation increments</returns>
        public static int GetShortestRotationDirection(float currentRotation, float targetRotation)
        {
            if(targetRotation < 0)
                targetRotation += (PI *2);
            
            if(currentRotation < 0)
                currentRotation += (PI *2);
            
            if(targetRotation < currentRotation)
                targetRotation += (PI *2);

            if(targetRotation - currentRotation <= PI)
                return 1;
            else
                return -1;
        }
    }
}