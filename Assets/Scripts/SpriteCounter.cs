using UnityEngine;
using System.Collections;

public class SpriteCounter : MonoBehaviour {

    [Tooltip("Sprites from numbers from 0 to 9")]
    public Sprite[] numberSprites = new Sprite[10];
    public Sprite dashSprite;
    private SpriteRenderer[] digits;

    void Start () {
        digits = GetComponentsInChildren<SpriteRenderer>();
	}
	
    public void Display(int number)
    {
        //Check if the whole thing fits within the display
        if (number.ToString().Length > digits.Length)
        {
            Debug.Log("The number " + number + " is too big to be displayed on the counter");
            for (int i = 0; i < digits.Length; ++i)
                digits[i].sprite = dashSprite;
        }
        else
        {
            //Chech if negative, if so change it to positive
            bool isNegative = number < 0;
            if (isNegative) number *= -1;

            //Display
            for (int digit = 0; digit < digits.Length; ++digit)
            {
                digits[digit].sprite = numberSprites[number % 10];
                number /= 10;
            }

            //TODO supi: too laze to make the "-" sign next to the number
            //right now it's on the left most sprite.
            if (isNegative)
                digits[digits.Length - 1].sprite = dashSprite;
        }
    }
}
