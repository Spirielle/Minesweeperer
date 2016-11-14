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
    protected  override void Start()
    {
        grid = GetComponent<GridManager>();
        smileyButton = FindObjectOfType<SmileyButton>();
        savedPerBomb = Random.Range(4, 8);
        deathCanvas.gameObject.SetActive(false);
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

    //TODO: update count
    public override void ToggleFlag(Tile tile)
    {
        //if its already a flag return it to default
        if (tile.flagSet)
        {
            tile.flagSet = false;
            tile.GetComponent<SpriteRenderer>().sprite = grid.defaultSprite;

            if (tile.mine)
                peopleSaved -= savedPerBomb;

            NumberOfFlagSet--;
        }

        //otherwise set it to flag
        else
        {
            tile.flagSet = true;
            tile.GetComponent<SpriteRenderer>().sprite = grid.flagSprite;

            if (tile.mine)
                peopleSaved += savedPerBomb;

            NumberOfFlagSet++;
        }
    }

    public override void SetToVictory()
    {
        base.SetToVictory();
        DisplayVictoryMessage();
    }

    private void DisplayVictoryMessage()
    {
        deathMessage = "Congratulations! You have saved all " + savedPerBomb * grid.numberOfBombs + " people lost in the mine field.";
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
        base.HitMine(tile);
        DisplayDeathMessage();
    }
}
