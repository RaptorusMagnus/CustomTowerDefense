using System;
using System.Collections.Generic;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;

namespace CustomTowerDefense
{
    /// <summary>
    /// A zero based, single element per location, logical view of the game grid.
    /// Here we don't use pixels but possible locations for game tiles like:
    /// path elements, structure elements, locked locations, towers, space ships...
    /// Note that it is possible to establish a link between a grid cell and a number of pixels,
    /// when we know the size of a standard tile (e.g. 16*16; 32*32; 64*64; 96*96...)
    ///
    /// Note that in the game we could have several ships on one location (when they overtake each others for instance),
    /// but this is a transitional special case. For structure elements we'll manage only one element per location.
    /// </summary>
    public class LogicalGameGridSingle
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
        
        // with 64*64 tiles, a [18; 10] grid fits in our 1200 * 720 screen.
        // later we'll have a scrollable large map, but for first version, let's stick to a fixed, hard-coded grid.
        private GameObject[,] _grid;

        private ushort _tilesSize;
        private ushort _xOffset;
        private ushort _yOffset;
        
        #region Properties

        public ushort MaxX => X_SIZE - 1;
        public ushort MaxY => Y_SIZE - 1;

        #endregion

        
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilesSize">To be able to switch from logical coordinates to pixel coordinates</param>
        /// <param name="xOffset">When the logical grid is not physically displayed in the top left corner</param>
        /// <param name="yOffset">When the logical grid is not physically displayed in the top left corner</param>
        /// 
        public LogicalGameGridSingle(ushort tilesSize, ushort xOffset, ushort yOffset)
        {
            _tilesSize = tilesSize;
            _xOffset = xOffset;
            _yOffset = yOffset;
            
            _grid = new GameObject[X_SIZE, Y_SIZE];
        }

        #endregion

        /// <summary>
        /// Returns a physical tile center (pixel) from a logical coordinate.
        /// This is possible when we know the tiles size and the grid offset.
        /// </summary>
        /// <param name="logicalCoordinate"></param>
        /// <returns></returns>
        public Coordinate GetPixelCenterFromLogicalCoordinate(Coordinate logicalCoordinate)
        {
            var x = (logicalCoordinate.X * _tilesSize) + (_tilesSize / 2f) + _xOffset;
            var y = (logicalCoordinate.Y * _tilesSize) + (_tilesSize / 2f) + _yOffset;

            return new Coordinate(x, y);
        }
        
        /// <summary>
        /// Returns the grid content at the specified location.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>Null when there is nothing at this location</returns>
        /// <exception cref="ArgumentException">When the coordinate is not located in the grid.</exception>
        [CanBeNull]
        public GameObject GetContentAt(Coordinate coordinate)
        {
            if (IsOutOfGrid(coordinate))
                throw new ArgumentException($"The received coordinate [{coordinate.X}, {coordinate.Y}] is out of the logical grid.");

            return _grid[(int)coordinate.X, (int)coordinate.Y];
        }

        public void AddGameObject(GameObject theObjectToAdd, Coordinate coordinate)
        {
            AddGameObject(theObjectToAdd, (ushort)coordinate.X, (ushort)coordinate.Y);
        }
        
        public void AddGameObject(GameObject theObjectToAdd, ushort x, ushort y)
        {
            if (x > X_SIZE - 1 || y > Y_SIZE - 1)
                throw new ArgumentException($"The requested position [{x}, {y}] is out of range in the logical grid which is max [{MaxX}, {MaxY}].");

            // we allow piling up, so we don't check that the location is free.  
            _grid[x, y] = theObjectToAdd;
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