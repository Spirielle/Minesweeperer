// Summary:
//
// Created by: Julian Glass-Pilon


using UnityEngine;
using System.Collections;

public class CameraSetup : MonoBehaviour 
{
    public GridManager grid;

    void Start()
    {
        //set the position of the camera based on the grid info
        float xPos = (grid.numberOfColumns * grid.distanceBetweenTiles) / 2;
        float yPos = (grid.numberOfRows * grid.distanceBetweenTiles) / 2;

        //offset the camera if the tiles are even so it is centered to the grid
        if(grid.numberOfColumns % 2 == 0)
        {
            xPos -= grid.distanceBetweenTiles / 2;
        }

        //offset the camera if the tiles are even so it is centered to the grid
        if (grid.numberOfRows % 2 == 0)
        {
            yPos -= grid.distanceBetweenTiles / 2;
        }
        transform.position = new Vector3(xPos, yPos, -1);

        //set the size of the camera so it emcompasses the whole grid -- columns can't exceed twice the number of rows
        GetComponent<Camera>().orthographicSize = grid.numberOfRows / 2;
    }
}
