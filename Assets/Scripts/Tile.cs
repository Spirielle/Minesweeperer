using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // Is this a mine?
    public bool             mine;

    //TODO Julian: make number of bombs instead of percentage and integrate in grid instead of tile
    public float            percentageOfMine = 0.15f;
    private GridManager     grid;

    private int? nbOfAdjacentMines;
    public int NbOfAdjacentMines
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

        // Randomly decide if it's a mine or not
        mine = Random.value < percentageOfMine;
    }

    public void LoadSprite()
    {
        //Ternary   x = y ? a : b;   if y is true, x = a else, x = b
        GetComponent<SpriteRenderer>().sprite = mine ? grid.mineSprite : grid.tileSprites[NbOfAdjacentMines];
    }

    // Is it still covered?
    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
    }

    //Events
    //TODO Julian: Add right mouse button functionality
    void OnMouseUpAsButton()
    {
        // It's a mine
        if (mine)
        {
            grid.UncoverMines();

            // game over
            print("you lose");
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

            // ToDo find out if the game was won now
            // ...
        }
    }
}
