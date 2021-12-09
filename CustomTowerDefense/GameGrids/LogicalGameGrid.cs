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
        private int _minPhysicalX;
        private int _minPhysicalY;
        private int _maxPhysicalX;
        private int _maxPhysicalY;
        
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


        public ushort TilesSize { get; }
        protected ushort XOffset { get; }
        protected ushort YOffset { get; }

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
            _minPhysicalX = 0 + xOffset;
            _minPhysicalY = 0 + yOffset;
            _maxPhysicalX = _minPhysicalX + (tilesSize * X_SIZE);
            _maxPhysicalY = _minPhysicalY + (tilesSize * Y_SIZE);
        }

        #endregion
        
        
        /// <summary>
        /// Returns a physical top left pixel of a tile, from its logical coordinate.
        /// This is possible when we know the tiles size and the grid offset.
        /// </summary>
        /// <param name="logicalCoordinate"></param>
        /// <returns></returns>
        public Coordinate GetTopLeftPixelFromLogicalCoordinate(GridCoordinate logicalCoordinate)
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
        public Coordinate GetPixelCenterFromLogicalCoordinate(GridCoordinate logicalCoordinate)
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
        public GridCoordinate? GetLogicalCoordinateFromPixelCoordinate(Coordinate physicalCoordinate)
        {
            var x = (ushort) Math.Floor((physicalCoordinate.X - XOffset) / TilesSize);
            var y = (ushort) Math.Floor((physicalCoordinate.Y - YOffset) / TilesSize);

            var logicalCoordinate = new GridCoordinate(x, y);

            return IsOutOfGrid(logicalCoordinate) ? null : logicalCoordinate;
        }
        
        /// <summary>
        /// Returns true if the given coordinate is out of the grid.
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfGrid(GridCoordinate coordinate)
        {
            return coordinate.X > MaxX ||
                   coordinate.Y > MaxY;
        }

        public bool IsOutOfPhysicalGrid(Coordinate coordinate)
        {
            return coordinate.X < _minPhysicalX ||
                   coordinate.X > _maxPhysicalX ||
                   coordinate.Y < _minPhysicalY ||
                   coordinate.Y > _maxPhysicalY;
        }
    }
}