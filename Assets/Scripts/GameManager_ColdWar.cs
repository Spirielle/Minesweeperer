using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameManager_ColdWar : GameManager
{
    public int InitialNumberOfBombs { get; set; }

    protected override void Start()
    {
        base.Start();
        InitialNumberOfBombs = grid.numberOfBombs;
    }

    public override void FlipTile(Tile tile)
    {
        base.FlipTile(tile);

        var availableTiles = grid.CoveredTiles()
                                .Where(t => !t.mine)
                                .ToList();

        if (availableTiles.Count != 0)
        {
            var randomTile = availableTiles.ElementAt(Random.Range(0, availableTiles.Count));

            randomTile.mine = true;

            int x = (int)randomTile.transform.position.x;
            int y = (int)randomTile.transform.position.y;

            refreshSurroundingTiles(x, y);

            grid.numberOfBombs++;
            bombCounter.Display(grid.numberOfBombs);
        }
    }

    private void refreshSurroundingTiles(int x, int y)
    {
        grid.AdjacentTiles(x, y)
            .Where(tile => !tile.isCovered())
            .ToList()
            .ForEach(tile => tile.LoadSprite());
    }

    public override void ResetGame()
    {
        grid.numberOfBombs = InitialNumberOfBombs;
        base.ResetGame();
    }
}
