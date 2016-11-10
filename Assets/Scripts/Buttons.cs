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

    public void LoadColdWar()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadBlindsweeper()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadSurrender()
    {
        SceneManager.LoadScene(6);
    }

    public void LoadVanilla()
    {
        SceneManager.LoadScene(7);
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene(0);
    }
}
