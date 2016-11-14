// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using System.Collections;

public class GameManager_TimeSweeper : GameManager
{
    public float timerDuration;
    public float timerBonus = 3;
    public float timerBombPenalty = 10;
    
    protected override void Start()
    {
        base.Start();
        Timer = timerDuration;
    }

    protected override void Update()
    {
        if (GameStarted && !GameOver && !Victory)
            Timer -= Time.deltaTime;

        if (Timer < 0)
            Timer = 0;

        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left
        if (grid.NumberOfUncoveredTiles >= grid.NumberOfTiles - grid.numberOfBombs)
            SetToVictory();

        if (Timer <= 0 && GameStarted)
        {
            SetToGameOver();
            grid.UncoverMines();
            GameStarted = false;
        }
    }

    public override void ResetGame()
    {
        base.ResetGame();
        Timer = timerDuration;
    }

    public override void ToggleFlag(Tile tile)
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

            //updates info for victory condition
            if (tile.mine)
                Timer += timerBonus;

            NumberOfFlagSet++;
        }
    }

    public override void HitMine(Tile tile)
    {
        Timer -= timerBombPenalty;
        tile.LoadSprite();
    }
}
