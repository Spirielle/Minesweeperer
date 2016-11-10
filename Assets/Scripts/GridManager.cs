using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO Julian: Create GameManager to check for win and lose conditions. 

public class GridManager : MonoBehaviour {
    public GameObject       tilePrefab;
    public int              numberOfColumns = 15;
    public int              numberOfRows = 15;
    public float            distanceBetweenTiles = 1.0f;
    public int              numberOfBombs = 20;
    public Tile[,]  tiles;
    public Sprite[]         tileSprites = new Sprite[9];
    public Sprite           mineSprite;
    public Sprite           flagSprite;
    public Sprite           defaultSprite;
    private GameObject      grid;

    void Awake () {
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
        grid = new GameObject("Grid");
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

                instance.transform.parent = grid.transform;
            }
            //Reset the x offset for the next row
            xOffset = 0.0f;
            yOffset += distanceBetweenTiles;
        }

        //randomize the bomb placement
        for(int i = 0; i < numberOfBombs; i++)
        {

            //determines if a bomb has been placed this iteration
            bool bombNotPlaced = true;

            //repeats as long as a bomb has not been placed
            while (bombNotPlaced)
            {

                //get a random tile
                int randomColumn = Random.Range(0, numberOfColumns);
                int randomRow = Random.Range(0, numberOfRows);

                //if it doesn't already have a bomb place one
                if(!tiles[randomColumn, randomRow].mine)
                {
                    tiles[randomColumn, randomRow].mine = true;
                    bombNotPlaced = false;
                }
            }
        }
    }

    //discovers a mine by changing the sprite to mine
    public void UncoverMines()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.mine)
            {
                tile.LoadSprite();
                tile.discovered = true;
            }
        }
             
    }

    // Find out if a mine is at the coordinates
    public bool MineAt(int x, int y)
    {
        // Coordinates in range? Then check for mine.
        if (TileIsInRange(x, y))
            return tiles[x, y].mine;
        return false;
    }

    // Count adjacent mines for an element
    public int CalculateNbOfAdjacentMines(int x, int y)
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

    public List<Tile> AdjacentTiles(int x, int y)
    {
        var adjacents = new List<Tile>();
        int tx, ty;
        
        tx = x; ty = y + 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x + 1; ty = y + 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x + 1; ty = y;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x + 1; ty = y - 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x; ty = y - 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x -1; ty = y - 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x - 1; ty = y;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        tx = x - 1; ty = y + 1;
        if (TileIsInRange(tx, ty))
            adjacents.Add(tiles[tx, ty]);

        return adjacents;
    }

    public List<Tile> CoveredTiles()
    {
        var coveredTiles = new List<Tile>();
        for (int row = 0; row < numberOfRows; ++row)
            for (int column = 0; column < numberOfColumns; ++column)
                if (tiles[column, row].isCovered())
                    coveredTiles.Add(tiles[column, row]);

        return coveredTiles;
    }

    bool TileIsInRange(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < numberOfColumns && y < numberOfRows);
    }

    // Flood Fill empty elements
    public void FloodFillUncover(int x, int y, bool[,] visited)
    {
        // Coordinates in Range?
        if (x >= 0 && y >= 0 && x < numberOfColumns && y < numberOfRows)
        {
            // visited already?
            if (visited[x, y])
                return;

            // uncover element
            if (!tiles[x, y].flagSet)
            {
                tiles[x, y].LoadSprite();
            }

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

            //Recursion in corners
            FloodFillUncover(x - 1, y - 1, visited);
            FloodFillUncover(x + 1, y + 1, visited);
            FloodFillUncover(x + 1, y - 1, visited);
            FloodFillUncover(x - 1, y + 1, visited);
        }
    }

    //resets the board
    public void ResetBoard()
    {
        Destroy(grid.gameObject);
        CreateTiles();
    }
}
