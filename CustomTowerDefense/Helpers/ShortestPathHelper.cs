using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.Helpers
{
    /// <summary>
    /// Provides some useful methods to solve the common problem of finding the shortest path,
    /// from one grid cell to another.
    /// This helper is greatly inspired from the the following article, written by Daniel Saidi (https://github.com/danielsaidi):
    /// https://danielsaidi.com/blog/2010/02/22/find-the-shortest-path-between-two-tiles-in-a-grid-in-xna
    /// </summary>
    public class ShortestPathHelper
    {
        // To store the calculated path length (not thread safe)
        private int[,] _pathLengths;

        /// <summary>
        /// Finds the shortest path from the start coordinate to the end coordinate, within the received game grid.
        /// </summary>
        /// <param name="gameGrid"></param>
        /// <param name="startCoordinate"></param>
        /// <param name="endCoordinate"></param>
        /// <returns></returns>
        public List<GridCoordinate> FindPath(LogicalGameGridSingle gameGrid, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
        {
            // we initialize the path length array.
            // By default the move cost is extremely high because we don't know the path yet.
            _pathLengths = new int[LogicalGameGrid.X_SIZE, LogicalGameGrid.Y_SIZE];
            
            for (int y = 0; y < _pathLengths.GetLength(1); y++)
            {
                for (int x = 0; x < _pathLengths.GetLength(0); x++)
                {
                    _pathLengths[x, y] = int.MaxValue;
                }
            }
            
            // We begin at the start Coordinate,
            // and the cost to reach it, is equal to zero, because we are currently at the start coordinate.
            _pathLengths[startCoordinate.X, startCoordinate.Y] = 0;
            
            // We call the recursive method exploring all paths reachable from the start position.
            ExploreAllReachableSiblings(gameGrid, startCoordinate);

            // Once done, backtrack from the end Coordinate
            var result = FindShortestPathAfterExploration(gameGrid, endCoordinate);

            return result.Contains(startCoordinate) ? result : new List<GridCoordinate>();
        }

        
        private void ExploreAllReachableSiblings(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            ExploreOneReachableSibling(gameGrid, coordinate, coordinate.TopSibling);
            ExploreOneReachableSibling(gameGrid, coordinate, coordinate.LeftSibling);
            ExploreOneReachableSibling(gameGrid, coordinate, coordinate.RightSibling);
            ExploreOneReachableSibling(gameGrid, coordinate, coordinate.BottomSibling);
        }

        private void ExploreOneReachableSibling(LogicalGameGridSingle gameGrid, GridCoordinate start, GridCoordinate target)
        {
            var directlyReachableCoordinates = GetDirectlyReachableCoordinates(gameGrid, start);
            
            // Abort if no movement is allowed
            if (!directlyReachableCoordinates.Contains(target)) {
                return;
            }

            // Get current path lengths
            int coordinateLength = GetPathLength(gameGrid, start);
            int targetLength = GetPathLength(gameGrid, target);

            // Use length if it improves target
            if (coordinateLength + 1 < targetLength)
            {
                _pathLengths[target.X, target.Y] = coordinateLength + 1;
                ExploreAllReachableSiblings(gameGrid, target);
            }
        }

        private int GetPathLength(LogicalGameGridSingle gameGrid, GridCoordinate targetCoordinate)
        {
            return IsReachableCoordinate(gameGrid, targetCoordinate) ? _pathLengths[targetCoordinate.X, targetCoordinate.Y] : int.MaxValue;
        }

        /// <summary>
        /// All paths cost have been explored, we now have to chose the best one.
        /// </summary>
        /// <param name="gameGrid"></param>
        /// <param name="targetCoordinate"></param>
        /// <returns></returns>
        private List<GridCoordinate> FindShortestPathAfterExploration(LogicalGameGridSingle gameGrid, GridCoordinate targetCoordinate)
        {
            var coordinateLength = GetPathLength(gameGrid, targetCoordinate);
            
            // Find the sibling paths
            var topLength =  GetPathLength(gameGrid, targetCoordinate.TopSibling);
            var leftLength = GetPathLength(gameGrid, targetCoordinate.LeftSibling);
            var rightLength = GetPathLength(gameGrid, targetCoordinate.RightSibling);
            var bottomLength = GetPathLength(gameGrid, targetCoordinate.BottomSibling);

            // Calculate the lowest path length
            int lowestLength =
                Math.Min(coordinateLength,
                    Math.Min(topLength,
                        Math.Min(leftLength,
                            Math.Min(rightLength, bottomLength))));

            if (lowestLength == int.MaxValue)
                return new List<GridCoordinate>();
            
            // Add each possible path
            var possiblePaths = new List<GridCoordinate>();
            if (topLength == lowestLength){
                possiblePaths.Add(targetCoordinate.TopSibling);
            }
            if (leftLength == lowestLength){
                possiblePaths.Add(targetCoordinate.LeftSibling);
            }
            if (rightLength == lowestLength) {
                possiblePaths.Add(targetCoordinate.RightSibling);
            }
            if (bottomLength == lowestLength) {
                possiblePaths.Add(targetCoordinate.BottomSibling);
            }

            // Among all equivalent paths, we arbitrarily take the first one.
            // We don't use a random here to have the same behavior.
            var result = new List<GridCoordinate>();
            if (possiblePaths.Any())
            {
                result = FindShortestPathAfterExploration(gameGrid, possiblePaths.First());
            }

            // Add the Coordinate itself, then return
            result.Add(targetCoordinate);
            return result;
        }

        private static List<GridCoordinate> GetDirectlyReachableCoordinates(LogicalGameGridSingle gameGrid, GridCoordinate fromLocation)
        {
            // we can only move up/down, and left/right, no diagonal moves allowed.
            var returnedList = new List<GridCoordinate>();
            

            if (IsReachableCoordinate(gameGrid, fromLocation.LeftSibling))
            {
                returnedList.Add(fromLocation.LeftSibling);
            }
            
            if (IsReachableCoordinate(gameGrid, fromLocation.RightSibling))
            {
                returnedList.Add(fromLocation.RightSibling);
            }
            
            if (IsReachableCoordinate(gameGrid, fromLocation.TopSibling))
            {
                returnedList.Add(fromLocation.TopSibling);
            }
            
            if (IsReachableCoordinate(gameGrid, fromLocation.BottomSibling))
            {
                returnedList.Add(fromLocation.BottomSibling);
            }
            
            return returnedList;
        }

        private static bool IsReachableCoordinate(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            if (gameGrid.IsOutOfGrid(coordinate))
                return false;
            
            var content = gameGrid.GetContentAt(coordinate);

            return content == null ||
                   content.PreciseObjectType == PreciseObjectType.Vortex;
        }
    }
}