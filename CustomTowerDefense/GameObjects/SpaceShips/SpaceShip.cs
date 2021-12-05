using System.Collections.Generic;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;

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
        protected ushort CurrentPathIndex { get; }

        #endregion

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
        }
        
        protected void FollowPath()
        {
            var targetPhysicalLocation = _logicalGameGrid.GetPixelCenterFromLogicalCoordinate(Path[CurrentPathIndex]).GetVector2();
            var angleToReachTarget = AnglesHelper.GetAngleToReachTarget(GetCurrentCoordinateAsVector(), targetPhysicalLocation);
        }
    }
}