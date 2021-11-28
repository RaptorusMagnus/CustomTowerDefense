using System;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;

namespace CustomTowerDefense.GameGrids
{
    /// <summary>
    /// Base class for all logical grids.
    /// </summary>
    public abstract  class LogicalGameGrid
    {
        /// <summary>
        /// The X size is the number of columns
        /// (do not confuse with max X since we use a zero based matrix) 
        /// </summary>
        public const ushort X_SIZE = 12;

        /// <summary>
        /// The Y size is the number of rows
        /// (do not confuse with max Y since we use a zero based matrix) 
        /// </summary>
        public const ushort Y_SIZE = 7;


        public ushort TilesSize { get; init; }
        protected ushort XOffset { get; init; }
        protected ushort YOffset { get; init; }

        #region Properties

        protected ushort MaxX => X_SIZE - 1;
        protected ushort MaxY => Y_SIZE - 1;

        #endregion


        #region Constructors

        protected LogicalGameGrid(ushort tilesSize, ushort xOffset, ushort yOffset)
        {
            TilesSize = tilesSize;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        #endregion
        
        
        /// <summary>
        /// Returns a physical top left pixel of a tile, from its logical coordinate.
        /// This is possible when we know the tiles size and the grid offset.
        /// </summary>
        /// <param name="logicalCoordinate"></param>
        /// <returns></returns>
        public Coordinate GetTopLeftPixelFromLogicalCoordinate(Coordinate logicalCoordinate)
        {
            var x = (logicalCoordinate.X * TilesSize) + XOffset;
            var y = (logicalCoordinate.Y * TilesSize) + YOffset;

            return new Coordinate(x, y);
        }
        
        /// <summary>
        /// Returns a physical tile center (pixel) from a logical coordinate.
        /// This is possible when we know the tiles size and the grid offset.
        /// </summary>
        /// <param name="logicalCoordinate"></param>
        /// <returns></returns>
        public Coordinate GetPixelCenterFromLogicalCoordinate(Coordinate logicalCoordinate)
        {
            var x = (logicalCoordinate.X * TilesSize) + (TilesSize / 2f) + XOffset;
            var y = (logicalCoordinate.Y * TilesSize) + (TilesSize / 2f) + YOffset;

            return new Coordinate(x, y);
        }
        
        
        /// <summary>
        /// Useful method to determine which logical cell is linked to a physical pixel (or mouse click) on screen.
        /// </summary>
        /// <param name="physicalCoordinate"></param>
        /// <returns>Null when the coordinate is not in the logical grid</returns>
        [CanBeNull]
        public Coordinate? GetLogicalCoordinateFromPixelCoordinate(Coordinate physicalCoordinate)
        {
            var x = (float) Math.Floor((physicalCoordinate.X - XOffset) / TilesSize);
            var y = (float) Math.Floor((physicalCoordinate.Y - YOffset) / TilesSize);

            var loGicalCoordinate = new Coordinate(x, y);

            return IsOutOfGrid(loGicalCoordinate) ? null : loGicalCoordinate;
        }
        
        /// <summary>
        /// Returns true if the given coordinate is out of the grid.
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfGrid(Coordinate coordinate)
        {
            return coordinate.X < 0 ||
                   coordinate.Y < 0 ||
                   coordinate.X > MaxX ||
                   coordinate.Y > MaxY;
        }
    }
}