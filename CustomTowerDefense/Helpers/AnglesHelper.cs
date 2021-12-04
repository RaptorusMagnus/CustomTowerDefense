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
    }
}