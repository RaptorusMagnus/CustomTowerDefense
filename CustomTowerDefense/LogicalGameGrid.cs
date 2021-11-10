using System;
using System.Runtime.InteropServices;
using CustomTowerDefense.GameObjects;

namespace CustomTowerDefense
{
    /// <summary>
    /// A zero based logical view of the game grid.
    /// Here we don't use pixels but possible locations for game tiles like:
    /// path elements, structure elements, locked locations, towers, space ships...
    /// Note that it is possible to establish a link between a grid cell and a number of pixels,
    /// when we know the size of a standard tile (e.g. 16*16; 32*32; 64*64; 96*96...) 
    /// </summary>
    public class LogicalGameGrid
    {
        public const ushort X_SIZE = 18;
        public const ushort Y_SIZE = 10;
        
        // with 64*64 tiles, a [18; 10] grid fits in our 1200 * 720 screen.
        // later we'll have a scrollable large map, but for first version, let's stick to a fixed, hard-coded grid.
        private GameObject[,] _grid = new GameObject[X_SIZE, Y_SIZE];

        public void PutGameObject(GameObject theObjectToPut, ushort x, ushort y)
        {
            if (x > X_SIZE - 1 || y > Y_SIZE - 1)
                throw new ArgumentException($"The requested position [{x}, {y}] is out of range in the logical grid which is max [{X_SIZE}, {Y_SIZE}].");

            _grid[x, y] = theObjectToPut;
        }
    }
}