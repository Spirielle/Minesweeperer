using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
    public GameObject   tilePrefab;
    public static int   numberOfColumns = 10;
    public static int   numberOfRows = 4;
    public float        distanceBetweenTiles = 1.0f;
    public static Tile[,] tiles;

    void Start () {
        tiles = new Tile[numberOfColumns, numberOfRows];
        CreateTiles();
    }

    //  y / Rows
    //      ___________________
    //    2 |   |   |   |
    //      |___|___|___|______
    //    1 |   |   |   |
    //      |___|___|___|______
    //    0 |   |   |   |
    //      |___|___|___|______
    //    0   0   1   2   ...           x / Columns
    //

    // Update is called once per frame
    void CreateTiles() {
        float xOffset = 0.0f;
        float yOffset = 0.0f;

        //These loop instantiate tiles from left to right then top to bottom
        for (int row = 0; row < numberOfRows; ++row)
        {
            for (int column = 0; column < numberOfColumns; ++column)
            {
                var instance = (GameObject)Instantiate(tilePrefab, new Vector2(transform.position.x + xOffset, transform.position.y + yOffset), transform.rotation);
                tiles[column, row] = instance.GetComponent<Tile>();
                xOffset += distanceBetweenTiles;
            }
            //Reset the x offset for the next row
            xOffset = 0.0f;
            yOffset += distanceBetweenTiles;
        }
    }

    public static void UncoverMines()
    {
        foreach (Tile tile in tiles)
            if (tile.mine)
                tile.LoadSprite();
    }

    // Find out if a mine is at the coordinates
    public static bool MineAt(int x, int y)
    {
        // Coordinates in range? Then check for mine.
        if (x >= 0 && y >= 0 && x < numberOfColumns && y < numberOfRows)
            return tiles[x, y].mine;
        return false;
    }

    // Count adjacent mines for an element
    public static int CalculateNbOfAdjacentMines(int x, int y)
    {
        int count = 0;

        if (MineAt(x    , y + 1)) ++count; // top
        if (MineAt(x + 1, y + 1)) ++count; // top-right
        if (MineAt(x + 1, y    )) ++count; // right
        if (MineAt(x + 1, y - 1)) ++count; // bottom-right
        if (MineAt(x    , y - 1)) ++count; // bottom
        if (MineAt(x - 1, y - 1)) ++count; // bottom-left
        if (MineAt(x - 1, y    )) ++count; // left
        if (MineAt(x - 1, y + 1)) ++count; // top-left

        return count;
    }

    // Flood Fill empty elements
    public static void FloodFillUncover(int x, int y, bool[,] visited)
    {
        // Coordinates in Range?
        if (x >= 0 && y >= 0 && x < numberOfColumns && y < numberOfRows)
        {
            // visited already?
            if (visited[x, y])
                return;

            CalculateNbOfAdjacentMines(x, y);

            // uncover element
            tiles[x, y].LoadSprite();

            // close to a mine? then no more work needed here
            if (tiles[x, y].NbOfAdjacentMines > 0)
                return;

            // set visited flag
            visited[x, y] = true;

            // recursion
            FloodFillUncover(x - 1, y, visited);
            FloodFillUncover(x + 1, y, visited);
            FloodFillUncover(x, y - 1, visited);
            FloodFillUncover(x, y + 1, visited);
        }
    }
}
