// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public int numberOfBombsLeft;
    private GridManager grid;
    private SmileyButton smileyButton;
    public SpriteCounter bombCounter;   //TODO supi: would it be better to use getByTag or something?
    private bool m_GameOver;
    private bool m_Victory;
    private bool m_TileBeingPressed;
    private int m_numberOfFlagsSet = 0;

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

    public bool TileBeingPressed
    {
        get { return m_TileBeingPressed; }
        set {
            m_TileBeingPressed = value;
            smileyButton.UpdateSprite();
        }
    }

    public int NumberOfFlagSet
    {
        get { return m_numberOfFlagsSet; }
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
        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if(numberOfBombsLeft == 0 && NumberOfFlagSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bambs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);
    }

    //set the game to game over
    public void SetToGameOver()
    {
        print("you lose");
        GameOver = true;
        smileyButton.UpdateSprite();
    }

    //set the game to victory
    public void SetToVictory()
    {
        Debug.Log("You Win!");
        Victory = true;
        smileyButton.UpdateSprite();
    }

    //reset game
    public void ResetGame()
    {
        GameOver = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
    }

}
