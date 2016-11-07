// Summary:
//
// Created by: Julian Glass-Pilon

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager_Saviour : GameManager 
{
    private string deathMessage = "";
    public SpriteCounter civiliansSaved;
    public Canvas deathCanvas;
    public Text deathText;
    private int peopleSaved = 0;
    public int savedPerBomb;

    public override int NumberOfFlagSet
    {
        get
        {
            return base.NumberOfFlagSet;
        }

        set
        {
            m_numberOfFlagsSet = value;
            civiliansSaved.Display(peopleSaved);
        }
    }
    void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
        savedPerBomb = Random.Range(4, 8);
        deathCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameStarted && !GameOver && !Victory)
            Timer += Time.deltaTime;

        if (Timer < 0)
            Timer = 0;

        //"R" resets the game
        if (Input.GetKeyUp(KeyCode.R))
            ResetGame();

        //if there are no bombs left and there are the same amount of flags placed you win
        if (numberOfBombsLeft == 0 && NumberOfFlagSet == grid.numberOfBombs)
            SetToVictory();

        //to keep track of bombs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);
    }

    public override void ResetGame()
    {
        GameOver = false;
        grid.ResetBoard();
        smileyButton.UpdateSprite();
        NumberOfFlagSet = 0;
        GameStarted = false;
        Timer = 0.0f;
        savedPerBomb = Random.Range(4, 8);
        deathCanvas.gameObject.SetActive(false);
    }

    public override void ToggleFlag(Tile tile)
    {
        //if its already a flag return it to default
        if (tile.flagSet)
        {
            tile.flagSet = false;
            tile.GetComponent<SpriteRenderer>().sprite = grid.defaultSprite;

            //updates info for victory condition
            if (tile.mine)
            {
                numberOfBombsLeft++;
            }
            NumberOfFlagSet--;
        }

        //otherwise set it to flag
        else
        {
            tile.flagSet = true;
            tile.GetComponent<SpriteRenderer>().sprite = grid.flagSprite;

            //updates info for victory condition
            if (tile.mine)
            {
                numberOfBombsLeft--;
                peopleSaved += savedPerBomb;
            }
            NumberOfFlagSet++;
        }
    }

    public override void SetToVictory()
    {
        Debug.Log("You Win!");
        Victory = true;
        DisplayVictoryMessage();
        smileyButton.UpdateSprite();
    }

    private void DisplayVictoryMessage()
    {
        deathMessage = "Congratulations! You have saved all " + peopleSaved + " people lost in the mine field.";
        deathText.text = deathMessage;
        deathCanvas.gameObject.SetActive(true);
    }

    private void DisplayDeathMessage()
    {
        int peopleKilled = grid.numberOfBombs * savedPerBomb - peopleSaved;
        deathMessage = "While you may have saved " + peopleSaved + " people, your miss-step has cost the lives of " + peopleKilled + ".";
        deathText.text = deathMessage;
        deathCanvas.gameObject.SetActive(true);
    }

    public override void HitMine(Tile tile)
    {
        grid.UncoverMines();

        // game over
        DisplayDeathMessage();
        SetToGameOver();
    }
}
