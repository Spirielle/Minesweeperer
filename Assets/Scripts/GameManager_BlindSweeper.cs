using UnityEngine;
using System.Collections;
using System;

public class GameManager_BlindSweeper : GameManager {
    AudioSource click;
    bool playedClickOnce = false;
    Tile clickedTile;

	void Start () {
        grid = GetComponent<GridManager>();
        click = GetComponent<AudioSource>();
        smileyButton = FindObjectOfType<SmileyButton>();
        numberOfBombsLeft = grid.numberOfBombs;
        bombCounter.Display(grid.numberOfBombs);
    }
	

	void Update () {
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

        if (TileBeingPressed && !playedClickOnce)
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("x :" + Math.Round(cursorPos.x) + " y : " + Math.Round(cursorPos.y));
            if (grid.tiles[(int)Math.Round(cursorPos.x), (int)Math.Round(cursorPos.y)].mine)
            {
                playedClickOnce = true;
                clickedTile = grid.tiles[(int)Math.Round(cursorPos.x), (int)Math.Round(cursorPos.y)];
                click.Play();
            }
        }
        if(playedClickOnce && !TileBeingPressed)
        {
            HitMine(clickedTile);
        }

        //to keep track of bombs and flag for debugging
        Debug.Log("bombs left: " + numberOfBombsLeft + "     flags placed: " + NumberOfFlagSet);
    }

    public override void FlipTile(Tile tile)
    {
        if (tile.mine)
            tile.LoadSprite();
        else
            tile.GetComponent<SpriteRenderer>().sprite = grid.tileSprites[0];   //Alway load the empty tile
    }

    public override void ResetGame()
    {
        base.ResetGame();
        playedClickOnce = false;
        clickedTile = null;
    }
}
