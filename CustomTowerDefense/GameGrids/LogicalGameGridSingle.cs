using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameGrids.Interfaces;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;

namespace CustomTowerDefense.GameGrids
{
    /// <summary>
    /// A zero based, single element per location, logical view of the game grid.
    /// Here we don't use pixels but possible locations for game tiles like:
    /// path elements, structure elements, locked locations, towers,...
    /// Note that it is possible to establish a link between a grid cell and a number of pixels,
    /// when we know the size of a standard tile (e.g. 16*16; 32*32; 64*64; 96*96...)
    ///
    /// Note that in the game we could have several ships on one location (when they overtake each others for instance),
    /// but this is a transitional special case. For structure elements we'll manage only one element per location.
    /// </summary>
    public class LogicalGameGridSingle: LogicalGameGrid, ILogicalGrid
    {
        // with 64*64 tiles, a [18; 10] grid fits in our 1200 * 720 screen.
        // later we'll have a scrollable large map, but for first version, let's stick to a fixed, hard-coded grid.
        private readonly GameObject[,] _grid;

        #region Properties

        /// <inheritdoc cref="ILogicalGrid.GameObjects"/>
        public List<GameObject> GameObjects => _grid.OfType<GameObject>().ToList();

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
            : base(tilesSize, xOffset, yOffset)
        {
            _grid = new GameObject[X_SIZE, Y_SIZE];
        }

        #endregion


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

            return _grid[(ushort) coordinate.X, (ushort) coordinate.Y];
        }

        public void RemoveObjectAt(Coordinate coordinate)
        {
            if (IsOutOfGrid(coordinate))
                throw new ArgumentException($"The received coordinate [{coordinate.X}, {coordinate.Y}] is out of the logical grid.");
            
            _grid[(ushort)coordinate.X, (ushort)coordinate.Y] = null;
        }
        
        public void RemoveObjectAt(ushort x, ushort y)
        {
            RemoveObjectAt(new Coordinate(x, y));
        }

        public void AddGameObject([NotNull] GameObject theObjectToAdd, Coordinate coordinate)
        {
            // For the "Single object per cell" version of the grid, we do not allow piling up.
            if (GetContentAt(coordinate) != null)
                throw new Exception($"There is already an object at location {coordinate}, impossible to add the {theObjectToAdd.GetType().Name}");

            _grid[(ushort)coordinate.X, (ushort)coordinate.Y] = theObjectToAdd;
        }

        /// <inheritdoc cref="ILogicalGrid.IsEmptyAt"/>
        public bool IsEmptyAt(Coordinate coordinate)
        {
            return GetContentAt(coordinate) == null;
        }

        public void AddGameObject([NotNull] GameObject theObjectToAdd, ushort x, ushort y)
        {
            AddGameObject(theObjectToAdd, new Coordinate(x, y));
        }


    }
}