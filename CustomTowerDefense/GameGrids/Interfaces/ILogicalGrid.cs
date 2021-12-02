using System.Collections.Generic;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameGrids.Interfaces
{
    public interface ILogicalGrid
    {
        /// <summary>
        /// Returns all the objects of the grid in a flat list.
        /// </summary>
        List<GameObject> GameObjects { get; }

        /// <summary>
        /// Adds the received object at the specified location.
        /// </summary>
        /// <param name="theObjectToAdd"></param>
        /// <param name="coordinate"></param>
        void AddGameObject(GameObject theObjectToAdd, GridCoordinate coordinate);

        /// <summary>
        /// Returns true if the grid is empty at the specified location. 
        /// </summary>
        /// <param name="coordinate">The location to check in the grid</param>
        /// <returns></returns>
        bool IsEmptyAt(GridCoordinate coordinate);
    }
}