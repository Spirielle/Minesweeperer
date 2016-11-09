using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameManager_ColdWar : GameManager
{
    public override int NumberOfFlagSet
    {
        get
        {
            return base.NumberOfFlagSet;
        }

        set
        {
            m_numberOfFlagsSet = value;
            bombCounter.Display(grid.numberOfBombs - m_numberOfFlagsSet);
        }
    }
    void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
        bombCounter.Display(grid.numberOfBombs);
    }

    void Update()
    {
        if (GameStarted && !GameOver && !Victory)
            Timer += Time.deltaTime;

        if (Timer < 0)
            Timer = 0;

        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if (numberOfBombsLeft == 0 && NumberOfFlagSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bombs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);
    }

    public override void ResetGame()
    {
        GameOver = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
        GameStarted = false;
        Timer = 0.0f;
    }

    public override void ToggleFlag(Tile tile)
    {
        //if its already a flag return it to default
        if (tile.flagSet)
        {
            tile.flagSet = false;
            tile.GetComponent<SpriteRenderer>().sprite = grid.defaultSprite;

            //updates info for victory condition
            if (tile.mine)
            {
                numberOfBombsLeft++;
            }
            NumberOfFlagSet--;
        }

        //otherwise set it to flag
        else
        {
            tile.flagSet = true;
            tile.GetComponent<SpriteRenderer>().sprite = grid.flagSprite;

            //updates info for victory condition
            if (tile.mine)
            {
                numberOfBombsLeft--;
            }
            NumberOfFlagSet++;
        }
    }

    public override void SetToVictory()
    {
        Debug.Log("You Win!");
        Victory = true;
        smileyButton.UpdateSprite();
    }

    public override void HitMine(Tile tile)
    {
        grid.UncoverMines();

        // game over
        SetToGameOver();
    }

    public override void FlipTile(Tile tile)
    {
        base.FlipTile(tile);

        ////get a random tile
        //int randomColumn = Random.Range(0, grid.numberOfColumns);
        //int randomRow = Random.Range(0, grid.numberOfRows);

        //bool bombNotPlaced = true;
        //while (bombNotPlaced)
        //{
        //    //if it doesn't already have a bomb place one
        //    if (!grid.tiles[randomColumn, randomRow].mine && grid.tiles[randomColumn, randomRow].isCovered())
        //    {
        //        grid.tiles[randomColumn, randomRow].mine = true;
        //        bombNotPlaced = false;
        //    }
        //}

        var availableTiles = grid.CoveredTiles()
                                .Where(t => !t.mine)
                                .ToList();

        if (availableTiles.Count != 0)
        {
            var randomTile = availableTiles.ElementAt(Random.Range(0, availableTiles.Count));

            randomTile.mine = true;

            int x = (int)randomTile.transform.position.x;
            int y = (int)randomTile.transform.position.y;

            refreshSurroundingTiles(x, y);

            grid.numberOfBombs++;
            bombCounter.Display(grid.numberOfBombs);
        }
    }

    private void refreshSurroundingTiles(int x, int y)
    {
        grid.AdjacentTiles(x, y)
            .Where(tile => !tile.isCovered())
            .ToList()
            .ForEach(tile => tile.LoadSprite());
    }
}
