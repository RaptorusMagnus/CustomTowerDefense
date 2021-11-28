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
    /// A zero based, multiple elements per location, logical view of the game grid.
    /// Here we don't use pixels but possible locations for game tiles like:
    /// path elements, structure elements, locked locations, towers,...
    /// Note that it is possible to establish a link between a grid cell and a number of pixels,
    /// when we know the size of a standard tile (e.g. 16*16; 32*32; 64*64; 96*96...)
    ///
    /// Note that in the game we could have several ships on one location (when they overtake each others for instance),
    /// but this is a transitional special case. For structure elements we'll manage only one element per location.
    /// </summary>
    public class LogicalGameGridMultiple: LogicalGameGrid, ILogicalGrid
    {
        // with 64*64 tiles, a [18; 10] grid fits in our 1200 * 720 screen.
        // later we'll have a scrollable large map, but for first version, let's stick to a fixed, hard-coded grid.
        private readonly List<GameObject>[,] _grid;

        /// <inheritdoc cref="ILogicalGrid.GameObjects"/>
        public List<GameObject> GameObjects
        {
            get
            {
                var returnedList = new List<GameObject>();
                foreach (var objectsList in _grid)
                {
                    if (objectsList != null)
                    {
                        returnedList.AddRange(objectsList);                        
                    }
                }

                return returnedList;
            }
        }

        #region Constructors

        public LogicalGameGridMultiple(ushort tilesSize, ushort xOffset, ushort yOffset)
            : base(tilesSize, xOffset, yOffset)
        {
            _grid = new List<GameObject>[X_SIZE, Y_SIZE];
        }

        #endregion

        /// <summary>
        /// Returns the grid content at the specified location.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="mustCheckCoordinateValidity">Can be set to false when you are sure that your coordinate is valid (avoids unnecessary tests)</param>
        /// <returns>Null when there is nothing at this location</returns>
        /// <exception cref="ArgumentException">When the coordinate is not located in the grid.</exception>
        [CanBeNull]
        public List<GameObject> GetContentAt(Coordinate coordinate, bool mustCheckCoordinateValidity = true)
        {
            if (mustCheckCoordinateValidity && IsOutOfGrid(coordinate))
                throw new ArgumentException($"The received coordinate [{coordinate.X}, {coordinate.Y}] is out of the logical grid.");

            return _grid[(ushort) coordinate.X, (ushort) coordinate.Y];
        }
        
        public void AddGameObject([NotNull] GameObject theObjectToAdd, Coordinate coordinate)
        {
            if (IsOutOfGrid(coordinate))
                throw new ArgumentException($"The received coordinate [{coordinate.X}, {coordinate.Y}] is out of the logical grid.");

            _grid[(ushort) coordinate.X, (ushort) coordinate.Y] ??= new List<GameObject>();
            _grid[(ushort) coordinate.X, (ushort) coordinate.Y].Add(theObjectToAdd);
        }
        
        public void RemoveObjectAt([NotNull] GameObject theObjectToRemove, Coordinate coordinate)
        {
            if (IsOutOfGrid(coordinate))
                throw new ArgumentException($"The received coordinate [{coordinate.X}, {coordinate.Y}] is out of the logical grid.");
            
            var gridCell = _grid[(ushort) coordinate.X, (ushort) coordinate.Y];
            
            if (gridCell == null || !gridCell.Any())
                return;

            gridCell.Remove(theObjectToRemove);
        }
        
        /// <inheritdoc cref="ILogicalGrid.IsEmptyAt"/>
        public bool IsEmptyAt(Coordinate coordinate)
        {
            var contentAt = GetContentAt(coordinate);
            
            return contentAt == null || !contentAt.Any();
        }

        /// <summary>
        /// Gets a single object per location grid from current grid,
        /// by taking only the first element of the correct type for each cell.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public LogicalGameGridSingle GetLogicalGameGridSingle(Type objectType)
        {
            var gridSingle = new LogicalGameGridSingle(TilesSize, XOffset, YOffset);

            for (var x = 0; x <= MaxX; x++)
            {
                for (var y = 0; y <= MaxY; y++)
                {
                    var coordinate = new Coordinate(x, y);
                    
                    var objectOfCorrectType =
                        GetContentAt(coordinate, mustCheckCoordinateValidity: false)
                            ?.FirstOrDefault(o => o.GetType() == objectType);

                    if (objectOfCorrectType != null)
                    {
                        gridSingle.AddGameObject(objectOfCorrectType, coordinate);
                    }
                }
            }
            
            return gridSingle;
        }
    }
}