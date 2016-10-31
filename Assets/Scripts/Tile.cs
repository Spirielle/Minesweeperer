using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // Is this a mine?
    public bool             mine;
    public bool             discovered = false;
    public bool             flagSet = false;
    private GridManager     grid;
    private GameManager     gameManager;

    private int?            nbOfAdjacentMines;
    public int              NbOfAdjacentMines
    {
        get
        {
            if(!nbOfAdjacentMines.HasValue)
            {
                int x = (int)transform.position.x;
                int y = (int)transform.position.y;
                nbOfAdjacentMines = grid.CalculateNbOfAdjacentMines(x, y);
            }

            return nbOfAdjacentMines.Value;
        }
    }

    void Start () {
        grid = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void LoadSprite()
    {
        //Ternary   x = y ? a : b;   if y is true, x = a else, x = b
        GetComponent<SpriteRenderer>().sprite = mine ? grid.mineSprite : grid.tileSprites[NbOfAdjacentMines];
        discovered = true;
    }

    // Is it still covered?
    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
    }

    //Events
    void OnMouseOver()
    {
        //if the game is not over and the player presses the left mouse button
        if (!gameManager.GameOver && !gameManager.Victory && Input.GetMouseButtonDown(0) && !flagSet && !discovered)
        {
            //discover the tile
            DiscoverTile();
        }

        //if the game is not over and the player presses the right mouse button
        if(!gameManager.GameOver && !gameManager.Victory && Input.GetMouseButtonDown(1) && !discovered)
        {
            //toggle a flag
            ToggleFlag();
        }
    }

    //set the game to game over
    private void GameOver()
    {
        print("you lose");
        gameManager.GameOver = true;
    }

    //flip the tile to discover what is underneath
    private void DiscoverTile()
    {
        // It's a mine
        if (mine)
        {
            grid.UncoverMines();

            // game over
            GameOver();
        }
        // It's not a mine
        else
        {
            // show adjacent mine number
            LoadSprite();

            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            // uncover area without mines
            grid.FloodFillUncover(x, y, new bool[grid.numberOfColumns, grid.numberOfRows]);
        }
    }

    //toggle the flag sprite on and off
    private void ToggleFlag()
    {
        //if its already a flag return it to default
        if (flagSet)
        {
            flagSet = false;
            GetComponent<SpriteRenderer>().sprite = grid.defaultSprite;

            //updates info for victory condition
            if (mine)
            {
                gameManager.numberOfBombsLeft++;
            }
            gameManager.numberOfFlagsSet--;
        }

        //otherwise set it to flag
        else
        {
            flagSet = true;
            GetComponent<SpriteRenderer>().sprite = grid.flagSprite;

            //updates info for victory condition
            if (mine)
            {
                gameManager.numberOfBombsLeft--;
            }
            gameManager.numberOfFlagsSet++;
        }
    }
}
