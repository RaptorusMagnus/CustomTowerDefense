namespace CustomTowerDefense
{
    /// <summary>
    /// The logical view of the game grid.
    /// Here we don't use pixels but possible locations for game tiles like:
    /// path elements, structure elements, locked locations, towers, space ships...
    /// Note that it is possible to establish a link between a grid cell and a number of pixels,
    /// when we know the size of a standard tile (e.g. 16*16; 32*32; 64*64; 96*96...) 
    /// </summary>
    public class TilesGrid
    {
        // with 64*64 tiles, a [18; 10] grid fits in our 1200 * 720 screen.
    }
}