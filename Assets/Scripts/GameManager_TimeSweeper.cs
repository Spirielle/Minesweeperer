// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using System.Collections;

public class GameManager_TimeSweeper : GameManager
{
    public float timerDuration;
    public float timerBonus = 3;
    
    void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
        bombCounter.Display(numberOfBombsLeft);
        Timer = timerDuration;
    }

    void Update()
    {
        if (GameStarted)
            Timer -= Time.deltaTime;
         
        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if (numberOfBombsLeft == 0 && NumberOfFlagSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bambs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);

        if(Timer <= 0 && GameStarted)
        {
            SetToGameOver();
            grid.UncoverMines();
            GameStarted = false;
        }
    }

    public override void ResetGame()
    {
        GameOver = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
        GameStarted = false;
        Timer = timerDuration;
    }

    public override void FlaggedMine()
    {
        Timer += timerBonus;
        Debug.Log(Timer);
    }
}
