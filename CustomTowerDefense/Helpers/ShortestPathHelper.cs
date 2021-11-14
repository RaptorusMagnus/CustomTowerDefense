using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;

namespace CustomTowerDefense.Helpers
{
    /// <summary>
    /// Specific implementation of the shortest path algorithm, for our game.
    /// </summary>
    public static class ShortestPathHelper
    {
        /// <summary>
        /// Returns the the shortest path to go from start to end (included).
        /// Returns null when it is impossible to reach the end.
        /// </summary>
        /// <returns>Null when it is impossible to reach the end.</returns>
        [CanBeNull]
        public static GridPath GetShortestPath(LogicalGameGridSingle gameGrid, Coordinate start, Coordinate end)
        {
            var currentPath = new GridPath(0, 0, gameGrid.MaxX, gameGrid.MaxY);
            currentPath.AddCoordinate(start);
            var alreadyKnownPaths = new Dictionary<Coordinate, GridPath>();
            var shortestAdditionalPath = GetShortestAdditionalPath(gameGrid, end, currentPath, alreadyKnownPaths);

            if (shortestAdditionalPath == null)
                return  null;
            
            currentPath.AddPath(shortestAdditionalPath);
            
            return currentPath;
        }
        
        /// <summary>
        /// Recursive method finding the shortest path to reach the end coordinate.
        /// </summary>
        [CanBeNull]
        private static GridPath GetShortestAdditionalPath(
            LogicalGameGridSingle gameGrid,
            Coordinate end,
            GridPath currentPath,
            Dictionary<Coordinate, GridPath> alreadyKnownPaths)
        {
            // a path already exists; its end is our starting point to find the shortest way
            var start = currentPath.GetPathEnd();

            if (start == null)
                throw new ArgumentException("The received path must at least contain the starting point.");

            var directlyReachableCoordinates = GetDirectlyReachableCoordinates(gameGrid, start.Value);

            // Exit condition when nothing is reachable from the starting point.
            if (directlyReachableCoordinates.Count == 0)
                return null;
            
            // Exit condition the starting point is connected to the end.
            if (directlyReachableCoordinates.Contains(end))
            {
                var additionalPath = new GridPath(0, 0, gameGrid.MaxX, gameGrid.MaxY);
                additionalPath.AddCoordinate(end);
                
                if (!alreadyKnownPaths.ContainsKey(start.Value))
                {
                    alreadyKnownPaths.Add(start.Value, additionalPath);
                    //Console.WriteLine($"Path is now known from {start.Value} --> {string.Join("; ", additionalPath.Coordinates)}");
                }
                
                return additionalPath;
            }

            // No exit condition, so let's prepare the recursive call.
            // To avoid looping we never take a reachable sibling that is already in the path.
            var siblingsToTest = directlyReachableCoordinates.Except(currentPath.Coordinates).ToList();
            
            GridPath thePathToKeep = null;
            
            foreach (var currentSiblingCoordinate in siblingsToTest)
            {
                GridPath currentSiblingPath;

                if (alreadyKnownPaths.ContainsKey(currentSiblingCoordinate))
                {
                    currentSiblingPath = alreadyKnownPaths[currentSiblingCoordinate];
                }
                else
                {
                    // for the recursive call we must add current sibling in the existing path.
                    var currentPathClone = currentPath.GetClone();
                    currentPathClone.AddCoordinate(currentSiblingCoordinate);
            
                    currentSiblingPath = GetShortestAdditionalPath(gameGrid, end, currentPathClone, alreadyKnownPaths);
                }
                
                if (currentSiblingPath != null &&
                    (
                        thePathToKeep == null ||
                        currentSiblingPath.NecessaryMoves < thePathToKeep.NecessaryMoves - 1 // -1 because because sibling was added 
                    ))
                {
                    thePathToKeep = new GridPath(0, 0, gameGrid.MaxX, gameGrid.MaxY);
                    thePathToKeep.AddCoordinate(currentSiblingCoordinate);
                    thePathToKeep.AddPath(currentSiblingPath);
                }
            }

            // if a valid short path has been found, we keep it to avoid computing it again.
            if (thePathToKeep != null && !alreadyKnownPaths.ContainsKey(start.Value))
            {
                alreadyKnownPaths.Add(start.Value, thePathToKeep);
                //Console.WriteLine($"Path is now known from {start.Value} --> {string.Join("; ", thePathToKeep.Coordinates)}");
            }
            
            return thePathToKeep;
        }
        
        public static List<Coordinate> GetDirectlyReachableCoordinates(LogicalGameGridSingle gameGrid, Coordinate fromLocation)
        {
            // we can only move up/down, and left/right, no diagonal moves allowed.
            var returnedList = new List<Coordinate>();
            

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

        public static bool IsReachableCoordinate(LogicalGameGridSingle gameGrid, Coordinate coordinate)
        {
            if (gameGrid.IsOutOfGrid(coordinate))
                return false;
            
            var content = gameGrid.GetContentAt(coordinate);

            return content == null ||
                   content.PreciseObjectType == PreciseObjectType.Vortex;
        }
    }
}