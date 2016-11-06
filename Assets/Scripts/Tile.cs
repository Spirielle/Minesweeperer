using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // Is this a mine?
    public bool mine;
    public bool discovered = false;
    public bool flagSet = false;
    private GridManager grid;
    private GameManager gameManager;
    private SmileyButton smileyButton;

    private int? nbOfAdjacentMines;
    public int  NbOfAdjacentMines
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
        smileyButton = FindObjectOfType<SmileyButton>();
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
        //if the game is not over and the player presses the right mouse button
        if(!gameManager.GameOver && !gameManager.Victory && Input.GetMouseButtonDown(1) && !discovered)
        {
            //toggle a flag
            ToggleFlag();
        }
    }
    void OnMouseUpAsButton()
    {
        //if the game is not over and the player presses the left mouse button
        if (!gameManager.GameOver && !gameManager.Victory && !flagSet && !discovered)
        {
            //discover the tile
            DiscoverTile();
        }
    }

    void OnMouseDown()
    {
        gameManager.TileBeingPressed = true;
    }
    void OnMouseUp()
    {
        gameManager.TileBeingPressed = false;
    }

    //flip the tile to discover what is underneath
    private void DiscoverTile()
    {
        if (!gameManager.GameStarted)
            gameManager.GameStarted = true;

        // It's a mine
        if (mine)
        {
            grid.UncoverMines();

            // game over
            gameManager.SetToGameOver();
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
        if (!gameManager.GameStarted)
            gameManager.GameStarted = true;

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
            gameManager.NumberOfFlagSet--;
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
                gameManager.FlaggedMine();
            }
            gameManager.NumberOfFlagSet++;
        }
    }
}
