// Summary:
//
// Created by: Julian Glass-Pilon


using UnityEngine;
using System.Collections;

public class GameManager_Trapped : GameManager 
{
    private bool flipped = false;

    void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
    }

    void Update()
    {
        if (!flipped)
        {
            InitialFlip();
            flipped = true;
        }

        if (GameStarted && !GameOver && !Victory)
            Timer += Time.deltaTime;

        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if (numberOfBombsLeft == 0 && NumberOfFlagSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bambs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);
    }

    public override void ResetGame()
    {
        GameOver = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
        GameStarted = false;
        flipped = false;
    }

    //checks to see if a tile has discovered neighbours. if so return true
    private bool IsTileInteractable(int x, int y)
    {
        int discoveredNeighbours = 0;

        if (x - 1 >= 0 && y + 1 >= 0 && x - 1 < grid.numberOfColumns && y + 1 < grid.numberOfRows)
            if (grid.tiles[x - 1, y + 1].discovered || grid.tiles[x - 1, y + 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x >= 0 && y + 1 >= 0 && x < grid.numberOfColumns && y + 1 < grid.numberOfRows)
            if (grid.tiles[x, y + 1].discovered || grid.tiles[x, y + 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x + 1 >= 0 && y + 1 >= 0 && x + 1 < grid.numberOfColumns && y + 1 < grid.numberOfRows)
            if (grid.tiles[x + 1, y + 1].discovered || grid.tiles[x + 1, y + 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x - 1 >= 0 && y >= 0 && x - 1 < grid.numberOfColumns && y < grid.numberOfRows)
            if (grid.tiles[x - 1, y].discovered || grid.tiles[x - 1, y].flagSet)
            {
                discoveredNeighbours++;
            }

        if (grid.tiles[x, y].discovered || grid.tiles[x, y].flagSet)
        {
            discoveredNeighbours++;
        }

        if (x + 1 >= 0 && y >= 0 && x + 1 < grid.numberOfColumns && y < grid.numberOfRows)
            if (grid.tiles[x + 1, y].discovered || grid.tiles[x + 1, y].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x - 1 >= 0 && y - 1 >= 0 && x - 1 < grid.numberOfColumns && y - 1 < grid.numberOfRows)
            if (grid.tiles[x - 1, y - 1].discovered || grid.tiles[x - 1, y - 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x >= 0 && y - 1 >= 0 && x < grid.numberOfColumns && y - 1 < grid.numberOfRows)
            if (grid.tiles[x, y - 1].discovered || grid.tiles[x, y - 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (x + 1 >= 0 && y - 1 >= 0 && x + 1 < grid.numberOfColumns && y - 1 < grid.numberOfRows)
            if (grid.tiles[x + 1, y - 1].discovered || grid.tiles[x + 1, y - 1].flagSet)
            {
                discoveredNeighbours++;
            }

        if (discoveredNeighbours > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    //flip the tile only if one of its neighbour is discovered
    public override void FlipTile(Tile tile)
    {
        int x = (int)tile.transform.position.x;
        int y = (int)tile.transform.position.y;

        if (IsTileInteractable(x, y))
        {
            // show adjacent mine number
            tile.LoadSprite();
        }
    }

    //toggle the flag only if one of its neighbour is discovered
    public override void ToggleFlag(Tile tile)
    {
        int x = (int)tile.transform.position.x;
        int y = (int)tile.transform.position.y;

        if (IsTileInteractable(x, y))
        {
            base.ToggleFlag(tile);
        }
    }

    public override void HitMine(Tile tile)
    {
        int x = (int)tile.transform.position.x;
        int y = (int)tile.transform.position.y;

        if (IsTileInteractable(x, y))
        base.HitMine(tile);
    }

    private void InitialFlip()
    {
        bool looking = true;
        while (looking)
        {
            int x = Random.Range(0, grid.numberOfColumns);
            int y = Random.Range(0, grid.numberOfRows);

            if (!grid.tiles[x, y].mine)
            {
                // show adjacent mine number
                grid.tiles[x,y].LoadSprite();

                // uncover area without mines
                grid.FloodFillUncover(x, y, new bool[grid.numberOfColumns, grid.numberOfRows]);
                looking = false;
            }
        }
    }
}
