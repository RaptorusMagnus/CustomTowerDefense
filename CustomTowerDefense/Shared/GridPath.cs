using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.Helpers;
using JetBrains.Annotations;

namespace CustomTowerDefense.Shared
{
    /// <summary>
    /// Non cycling, non discontinuous path within a grid;
    /// This class is basically a list of coordinates, but with the added value of checking that the path is valid;
    /// </summary>
    public class GridPath
    {
        #region Private members
        
        private ushort _minX;
        private ushort _minY;
        private ushort _maxX;
        private ushort _maxY;

        #endregion

        #region Properties

        public List<Coordinate> Coordinates { get; }

        /// <summary>
        /// Number of movements necessary to reach the end, when starting from the start position.
        /// </summary>
        public ushort NecessaryMoves => (ushort) (Coordinates.Count - 1);

        #endregion
        
        #region Constructors
        
        public GridPath(ushort minX, ushort minY, ushort maxX, ushort maxY)
        {
            _minX = minX;
            _minY = minY;
            _maxX = maxX;
            _maxY = maxY;
            Coordinates = new List<Coordinate>();
        }

        private GridPath(ushort minX, ushort minY, ushort maxX, ushort maxY, IEnumerable<Coordinate> coordinates)
            : this (minX, minY, maxX, maxY)
        {
            // For performance sake, we don't use the AddCoordinate method. This way, we avoid unnecessary tests.
            // This is a private constructor, values are known to be valid.
            foreach (var coordinate in coordinates)
            {
                Coordinates.Add(new Coordinate(coordinate.X, coordinate.Y));
            }
        }
        
        #endregion

        public void AddCoordinate(Coordinate newCoordinate)
        {
            if (Coordinates.Contains(newCoordinate))
            {
                throw new ArgumentException($"Coordinate [{newCoordinate.X}, {newCoordinate.Y}] is already in the path, and the path must be non cycling.");
            }

            if (newCoordinate.X > _maxX ||
                newCoordinate.Y > _maxY ||
                newCoordinate.X < _minX ||
                newCoordinate.Y < _minY )
            {
                throw new ArgumentException($"Coordinate [{newCoordinate.X}, {newCoordinate.Y}] is out of the grid. Impossible to add it to the path");
            }

            if (Coordinates.Count > 0)
            {
                // we must check that the added coordinate is in the continuity of previous path
                var pathEnd = Coordinates.Last();

                var isDirectlyReachable =
                    pathEnd.RightSibling.Equals(newCoordinate) ||
                    pathEnd.LeftSibling.Equals(newCoordinate) ||
                    pathEnd.TopSibling.Equals(newCoordinate) ||
                    pathEnd.BottomSibling.Equals(newCoordinate);

                if (!isDirectlyReachable)
                {
                    throw new ArgumentException($"You are trying to add a non continuous coordinate {newCoordinate} to the following path : {string.Join("; ", Coordinates)} ");
                }
            }
            
            Coordinates.Add(newCoordinate);
        }

        public void AddPath(GridPath thePathToAdd)
        {
            foreach (var pathCoordinate in thePathToAdd.Coordinates)
            {
                AddCoordinate(pathCoordinate);
            }
        }

        
        
        /// <summary>
        /// Efficient way to get the end of the path. 
        /// </summary>
        [CanBeNull]
        public Coordinate? GetPathEnd()
        {
            return Coordinates.Count > 0 ? Coordinates.Last() : null;
        }

        public GridPath GetClone()
        {
            return new GridPath(_minX, _minX, _maxX, _maxY, Coordinates);
        }
    }
}