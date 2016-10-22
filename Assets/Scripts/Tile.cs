using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // Is this a mine?
    public bool     mine;
    public Sprite[] tileSprites = new Sprite[9];
    public Sprite   mineSprite;
    public float    percentageOfMine = 0.15f;

    private int? nbOfAdjacentMines;
    public int NbOfAdjacentMines
    {
        get
        {
            if(!nbOfAdjacentMines.HasValue)
            {
                int x = (int)transform.position.x;
                int y = (int)transform.position.y;
                nbOfAdjacentMines = Grid.CalculateNbOfAdjacentMines(x, y);
            }

            return nbOfAdjacentMines.Value;
        }
    }

    void Start () {
        // Randomly decide if it's a mine or not
        mine = Random.value < percentageOfMine;
    }

    public void LoadSprite()
    {
        //Ternary   x = y ? a : b;   if y is true, x = a else, x = b
        GetComponent<SpriteRenderer>().sprite = mine ? mineSprite : tileSprites[NbOfAdjacentMines];
    }

    // Is it still covered?
    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
    }

    //Events
    void OnMouseUpAsButton()
    {
        // It's a mine
        if (mine)
        {
            Grid.UncoverMines();

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
            Grid.FloodFillUncover(x, y, new bool[Grid.numberOfColumns, Grid.numberOfRows]);

            // ToDo find out if the game was won now
            // ...
        }
    }
}
