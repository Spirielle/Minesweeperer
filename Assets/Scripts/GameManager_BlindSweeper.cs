using UnityEngine;
using System.Collections;
using System;

public class GameManager_BlindSweeper : GameManager {
    AudioSource click;
    bool playedClickOnce = false;
    Tile clickedTile;

	protected override void Start () {
        base.Start();
        click = GetComponent<AudioSource>();
    }
	

	protected override void Update () {
        base.Update();

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
    }

    public override void FlipTile(Tile tile)
    {
        if (tile.mine)
            tile.LoadSprite();
        else
        {
            tile.GetComponent<SpriteRenderer>().sprite = grid.tileSprites[0];   //Alway load the empty tile
            grid.NumberOfUncoveredTiles++;
        }
    }

    public override void ResetGame()
    {
        base.ResetGame();
        playedClickOnce = false;
        clickedTile = null;
    }
}
