// Summary:
//
// Created by: Julian Glass-Pilon


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour 
{
    public void LoadTrapped()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadTimeSweeper()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSaviours()
    {
        SceneManager.LoadScene(3);
    }
}
