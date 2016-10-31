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
    private bool m_GameOver;
    private bool m_Victory;

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

    void Start()
    {
        grid = GetComponent<GridManager>();
        numberOfBombsLeft = grid.numberOfBombs;
    }

    void Update()
    {

        //if the game is over "R" resets the game
        if (GameOver || Victory)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                GameOver = false;
                grid.ResetBoard();
            }
        }

        //if there are no bombs left and there are the same amount of flags placed you win
        if(numberOfBombsLeft == 0 && numberOfFlagsSet == grid.numberOfBombs)
        {
            Debug.Log("You Win!");
            Victory = true;
        }

        //to keep track of bambs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + numberOfFlagsSet);
    }
}
