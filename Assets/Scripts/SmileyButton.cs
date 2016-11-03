using UnityEngine;
using System.Collections;

enum SmileyState {
    Default = 0,
    TileBeingPressed = 1,
    Victory = 2,
    GameOver = 3
}

public class SmileyButton : MonoBehaviour {

    //TODO supi: find a way to add tooltip for each index of array
    [Tooltip("0 = default, 1 = when tile pressed, 2 = victory, 3 = game over")]
    public Sprite[] smileySprites = new Sprite[4];
    private GameManager gameManager;

    // Use this for initialization
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateSprite()
    {
        //Determine what is the current state
        var activeState = SmileyState.Default;

        if (gameManager.Victory)
            activeState = SmileyState.Victory;
        else if (gameManager.GameOver)
            activeState = SmileyState.GameOver;
        else if (gameManager.TileBeingPressed)
            activeState = SmileyState.TileBeingPressed;

        //Change the sprite according to the current state
        GetComponent<SpriteRenderer>().sprite = smileySprites[(int)activeState];
    }

    void OnMouseUpAsButton()
    {
        gameManager.ResetGame();
    }
}
