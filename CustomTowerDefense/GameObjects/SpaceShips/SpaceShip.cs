using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    /// <summary>
    /// Base class for all spaceships, with specific behaviors.
    /// </summary>
    public abstract class SpaceShip: MoveableGameObject
    {
        #region Private members

        // To code some behaviors we need information concerning the surroundings  
        private LogicalGameGridMultiple _logicalGameGrid;

        #endregion
        
        
        #region Protected members

        /// <summary>
        /// All spaceships must follow a known path (no free move),
        /// this this the specificity of tower defense games.
        /// </summary>
        protected List<GridCoordinate> Path { get; }
        
        /// <summary>
        /// We don't want to recompute endlessly current path index from physical location,
        /// so we keep current index, and we easily know that the next index is the target location.
        /// </summary>
        protected ushort CurrentPathIndex { get; private set; }
        
        public SpaceshipAction CurrentAction { get; private set; }

        #endregion

        #region Constructors

        protected SpaceShip(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType,
            float speed,
            int drawOrder,
            List<GridCoordinate> path,
            LogicalGameGridMultiple logicalGameGrid)
            : base(coordinate, width, height, preciseObjectType, speed, drawOrder)
        {
            Path = path;
            CurrentPathIndex = 0;
            _logicalGameGrid = logicalGameGrid;
            CurrentAction = SpaceshipAction.GoingOutOfVortex;
        }

        #endregion

        /// <summary>
        /// Requests the ship to go on doing what it was doing previously (whatever it was):
        /// continue its move along the path, or keep getting out of the vortex, or keep going in the end vortex,...
        /// </summary>
        public void DoCurrentAction()
        {
            switch (CurrentAction)
            {
                case SpaceshipAction.GoingOutOfVortex:
                    break;
                case SpaceshipAction.GoingInVortex:
                    break;
                case SpaceshipAction.MoveToNextPathLocation:
                    FollowPath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Changes current action of the spaceship, so that it will now follow the path.
        /// </summary>
        public void StartFollowingPath()
        {
            CurrentAction = SpaceshipAction.MoveToNextPathLocation;
            FollowPath();
        }
        
        /// <summary>
        /// All spaceships must be able to follow the given path.
        /// This method can be call at each game update phase, it does the necessary actions to follow the path.
        /// </summary>
        protected void FollowPath()
        {
            var lastPathIndex = Path.Count - 1;
            var targetPathIndex = CurrentPathIndex == lastPathIndex ? lastPathIndex : CurrentPathIndex + 1;
                
            var targetPhysicalLocation = _logicalGameGrid.GetPixelCenterFromLogicalCoordinate(Path[targetPathIndex]).GetVector2();

            var currentCoordinate = GetCurrentCoordinateAsVector();
            var distanceToTarget = Vector2.Distance(currentCoordinate, targetPhysicalLocation);

            // Every good thing has an end!
            // When reaching the end of the path and when physically in the end vortex, we must stop following the path.
            if (CurrentPathIndex == lastPathIndex &&
                distanceToTarget <= Speed)
            {
                CurrentAction = SpaceshipAction.GoingInVortex;
                Coordinate = new Coordinate(targetPhysicalLocation.X, targetPhysicalLocation.Y);
                return;
            }
            
            // We must handle possible rotations when next path coordinate is not in front of the spaceship.
            var angleToReachTarget = AnglesHelper.GetAngleToReachTarget(currentCoordinate, targetPhysicalLocation);
            var rotationDifference = RotationAngle - angleToReachTarget;

            // We must know many cycles we have before reaching the target, to compute the best trajectory. 
            var numberOfCyclesToReachTarget = distanceToTarget / Speed;
            
            if (Math.Abs(rotationDifference) < 0.1f || numberOfCyclesToReachTarget <= 1)
            {
                // we don't spread the rotation on next moves when the difference is so small that we would not see the difference.
                // Or, when it's the last move to reach the target (we won't have another cycle to do so)
                RotationAngle = angleToReachTarget;
            }
            else
            {
                // several cycles will be necessary, and the angle is significant let's make a progressive turn
                var rotationIncrementPerStep = AnglesHelper.GetRotationIncrementPerStep(rotationDifference, numberOfCyclesToReachTarget);

                if (RotationAngle > angleToReachTarget)
                {
                    RotationAngle -= rotationIncrementPerStep;
                }
                else
                {
                    RotationAngle += rotationIncrementPerStep;
                }
            }

            // Now that the angle is correct, lets move in that direction!
            var rotationVector = AnglesHelper.AngleToVector(RotationAngle);
            Move(rotationVector * Speed);
            
            // Note that we trigger trajectory transition before actually reaching the precise center of next path coordinate,
            // because we need to start turning before reaching the center, to avoid very steep/weird rotations.
            // 0.63 is an empirical number. After several tests, it gives nice results.
            // A higher value gives a smooth trajectory with large curves. A lower value keeps the spaceship more strictly on the path.
            // /!\ don't mess to much with it because a wrong value can lead to some weird trajectories,
            // and even bugs when the spaceships miss their target due to a bad trajectory.
            // If you face trajectory bugs their are 3 parameters to keep in mind:
            // pilotAnticipationForTurning, the GetRotationIncrementPerStep method and the test on rotationDifference in this method.
            float pilotAnticipationForTurning = _logicalGameGrid.TilesSize * 0.63f;
            
            //
            //          _,
            //     -==<' `\
            //         ) /
            //        / (_.
            //       | ,-,`\
            //        \\  \ \
            //         `\, \ \
            //          ||\ \`|,
            //         _|| `=`-'
            //
            // TODO: We should clearly not move to next logical cell in the grid when we are not physically in.
            // TODO: pilot anticipation should apply only for the trajectory computation not for cell transitions. This will lead to bugs for sure.
            // Special actions must be undertaken when we reach the next path coordinate
            if (CurrentPathIndex < lastPathIndex &&
                distanceToTarget <= pilotAnticipationForTurning)
            {
                CurrentPathIndex++;
                
                // We must tell the grid that we have just changed our logical coordinate.
                _logicalGameGrid.MoveObjectLogically(this, Path[CurrentPathIndex - 1], Path[CurrentPathIndex]);
            }
        }
    }
}