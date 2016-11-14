// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    protected GridManager grid;
    protected SmileyButton smileyButton;
    public SpriteCounter bombCounter;   //TODO supi: would it be better to use getByTag or something?
    public SpriteCounter timerDisplay;
    protected bool m_GameOver;
    protected bool m_Victory;
    protected bool m_TileBeingPressed;
    protected int m_numberOfFlagsSet = 0;
    protected float m_timer = 0.0f;

    public bool GameStarted { get; set; }
    public bool GameOver
    {
        get { return m_GameOver; }
        set { m_GameOver = value; }
    }

    public bool Victory
    {
        get { return m_Victory; }
        set { m_Victory = value; }
    }

    public virtual bool TileBeingPressed
    {
        get { return m_TileBeingPressed; }
        set {
            m_TileBeingPressed = value;
            smileyButton.UpdateSprite();
        }
    }

    public virtual int NumberOfFlagSet
    {
        get { return m_numberOfFlagsSet; }
        set
        {
            m_numberOfFlagsSet = value;
            bombCounter.Display(grid.numberOfBombs - m_numberOfFlagsSet);
        }
    }

    public virtual float Timer
    {
        get { return m_timer; }
        set
        {
            m_timer = value;
            timerDisplay.Display((int)m_timer);
        }
    }

    protected virtual void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        bombCounter.Display(grid.numberOfBombs);
    }

    protected virtual void Update()
    {
        if(GameStarted && !GameOver && !Victory)
            Timer += Time.deltaTime;

        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left
        if(grid.NumberOfUncoveredTiles >= grid.NumberOfTiles - grid.numberOfBombs)
            SetToVictory();
    }

    //set the game to game over
    public virtual void SetToGameOver()
    {
        print("you lose");
        GameOver = true;
        smileyButton.UpdateSprite();
    }

    //set the game to victory
    public virtual void SetToVictory()
    {
        Debug.Log("You Win!");
        Victory = true;
        smileyButton.UpdateSprite();
    }

    //reset game
    public virtual void ResetGame()
    {
        GameOver = false;
        Victory = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
        GameStarted = false;
        Timer = 0.0f;
    }

    public virtual void HitMine(Tile tile)
    {
        grid.UncoverMines();

        // game over
        SetToGameOver();
    }

    public virtual void FlipTile(Tile tile)
    {
        int x = (int)tile.transform.position.x;
        int y = (int)tile.transform.position.y;

        // uncover area without mines
        grid.FloodFillUncover(x, y);
    }

    public virtual void ToggleFlag(Tile tile)
    {
        //if its already a flag return it to default
        if (tile.flagSet)
        {
            tile.flagSet = false;
            tile.GetComponent<SpriteRenderer>().sprite = grid.defaultSprite;

            NumberOfFlagSet--;
        }

        //otherwise set it to flag
        else
        {
            tile.flagSet = true;
            tile.GetComponent<SpriteRenderer>().sprite = grid.flagSprite;

            NumberOfFlagSet++;
        }
    }
}
