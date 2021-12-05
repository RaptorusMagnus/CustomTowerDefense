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
            List<GridCoordinate> path,
            LogicalGameGridMultiple logicalGameGrid)
            : base(coordinate, width, height, preciseObjectType, speed)
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
            if (CurrentPathIndex == Path.Count - 1)
            {
                // we don't move further when the last path index is reached.
                return;
            }

            // We must handle possible rotations when next path coordinate is not in front of the spaceship.
            var currentCoordinate = GetCurrentCoordinateAsVector();
            var targetPhysicalLocation = _logicalGameGrid.GetPixelCenterFromLogicalCoordinate(Path[CurrentPathIndex + 1]).GetVector2();
            var angleToReachTarget = AnglesHelper.GetAngleToReachTarget(currentCoordinate, targetPhysicalLocation);
            
            var distanceToTarget = Vector2.Distance(currentCoordinate, targetPhysicalLocation);

            var numberOfCyclesToReachTarget = distanceToTarget / Speed;
            
            // Are we in front of the next path location ?
            var rotationDifference = RotationAngle - angleToReachTarget;

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
            
            // Special actions must be undertaken when we reach the next path coordinate
            // Note that we trigger the transition before actually reaching the precise center of next path coordinate,
            // because we need to start turning before reaching the center to avoid very steep rotations.
            if (distanceToTarget <= _logicalGameGrid.TilesSize / 2.5f)
            {
                CurrentPathIndex++;
                
                // We must tell the grid that we have just changed our logical coordinate.
                _logicalGameGrid.MoveObjectLogically(this, Path[CurrentPathIndex - 1], Path[CurrentPathIndex]);
                
                // when reaching the end of the path, we must make sure we don't go further than the target location.
                if (CurrentPathIndex == Path.Count - 1)
                {
                    CurrentCoordinate = _logicalGameGrid.GetPixelCenterFromLogicalCoordinate(Path.Last());
                }
            }
        }
    }
}