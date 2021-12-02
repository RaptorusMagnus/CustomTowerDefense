using System;
using System.Collections.Generic;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.Helpers
{
    public class ShortestPathHelper
    {
        // Placeholder for the calculated path (not thread safe)
        private int[,] _pathLengths;

        public List<GridCoordinate> FindPath(LogicalGameGridSingle gameGrid, GridCoordinate startCoordinate, GridCoordinate endCoordinate)
        {
            //Initialize the path length array
            _pathLengths = new int[LogicalGameGrid.X_SIZE, LogicalGameGrid.Y_SIZE];
            for (int y = 0; y < _pathLengths.GetLength(1); y++)
            {
                for (int x = 0; x < _pathLengths.GetLength(0); x++)
                {
                    _pathLengths[x, y] = int.MaxValue;
                }
            }
            
            //Begin at the start Coordinate
            _pathLengths[startCoordinate.X, startCoordinate.Y] = 0;
            FindPath_Spread(gameGrid, startCoordinate);

            //Once done, backtrack from the end Coordinate
            List<GridCoordinate> result = FindPath_Trace(gameGrid, endCoordinate);

            //Only return the path if it contains the start Coordinate
            if (result.Contains(startCoordinate)) {
                return result;
            }

            return new List<GridCoordinate>();
        }

        
        private void FindPath_Spread(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            FindPath_Spread(gameGrid, coordinate, coordinate.TopSibling);
            FindPath_Spread(gameGrid, coordinate, coordinate.LeftSibling);
            FindPath_Spread(gameGrid, coordinate, coordinate.RightSibling);
            FindPath_Spread(gameGrid, coordinate, coordinate.BottomSibling);
        }

        private void FindPath_Spread(LogicalGameGridSingle gameGrid, GridCoordinate start, GridCoordinate target)
        {
            var directlyReachableCoordinates = GetDirectlyReachableCoordinates(gameGrid, start);
            
            //Abort if no movement is allowed
            if (!directlyReachableCoordinates.Contains(target)) {
                return;
            }

            //Get current path lengths
            int coordinateLength = FindPath_GetPathLength(gameGrid, start);
            int targetLength = FindPath_GetPathLength(gameGrid, target);

            //Use length if it improves target
            if (coordinateLength + 1 < targetLength)
            {
                _pathLengths[target.X, target.Y] = coordinateLength + 1;
                FindPath_Spread(gameGrid, target);
            }
        }

        private int FindPath_GetPathLength(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            return IsReachableCoordinate(gameGrid, coordinate) ? _pathLengths[coordinate.X, coordinate.Y] : int.MaxValue;
        }

        private List<GridCoordinate> FindPath_Trace(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            int coordinateLength = FindPath_GetPathLength(gameGrid, coordinate);
            
            //Find the sibling paths
            int topLength = IsReachableCoordinate(gameGrid, coordinate.TopSibling)
                ? FindPath_GetPathLength(gameGrid, coordinate.TopSibling)
                : int.MaxValue;
            int leftLength = IsReachableCoordinate(gameGrid, coordinate.LeftSibling)
                ? FindPath_GetPathLength(gameGrid, coordinate.LeftSibling)
                : int.MaxValue;
            int rightLength = IsReachableCoordinate(gameGrid, coordinate.RightSibling)
                ? FindPath_GetPathLength(gameGrid, coordinate.RightSibling)
                : int.MaxValue;
            int bottomLength = IsReachableCoordinate(gameGrid, coordinate.BottomSibling)
                ? FindPath_GetPathLength(gameGrid, coordinate.BottomSibling)
                : int.MaxValue;

            //Calculate the lowest path length
            int lowestLength =
                Math.Min(coordinateLength,
                    Math.Min(topLength,
                        Math.Min(leftLength,
                            Math.Min(rightLength, bottomLength))));

            if (lowestLength == int.MaxValue)
                return new List<GridCoordinate>();
            
            //Add each possible path
            var possiblePaths = new List<GridCoordinate>();
            if (topLength == lowestLength){
                possiblePaths.Add(coordinate.TopSibling);
            }
            if (leftLength == lowestLength){
                possiblePaths.Add(coordinate.LeftSibling);
            }
            if (rightLength == lowestLength) {
                possiblePaths.Add(coordinate.RightSibling);
            }
            if (bottomLength == lowestLength) {
                possiblePaths.Add(coordinate.BottomSibling);
            }

            //Continue through a random possible path
            var result = new List<GridCoordinate>();
            if (possiblePaths.Any()) {
                result = FindPath_Trace(gameGrid, possiblePaths.First());
            }

            //Add the Coordinate itself, then return
            result.Add(coordinate);
            return result;
        }
        
        public static List<GridCoordinate> GetDirectlyReachableCoordinates(LogicalGameGridSingle gameGrid, GridCoordinate fromLocation)
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
        
        public static bool IsReachableCoordinate(LogicalGameGridSingle gameGrid, GridCoordinate coordinate)
        {
            if (gameGrid.IsOutOfGrid(coordinate))
                return false;
            
            var content = gameGrid.GetContentAt(coordinate);

            return content == null ||
                   content.PreciseObjectType == PreciseObjectType.Vortex;
        }
    }
}