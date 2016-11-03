// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public int numberOfBombsLeft;
    public int numberOfFlagsSet = 0;
    private GridManager grid;
    private SmileyButton smileyButton;
    private bool m_GameOver;
    private bool m_Victory;
    private bool m_TileBeingPressed;

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

    void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
    }

    void Update()
    {
        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if(numberOfBombsLeft == 0 && numberOfFlagsSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bambs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + numberOfFlagsSet);
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
    }

}
