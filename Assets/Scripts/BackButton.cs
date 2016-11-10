using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {
    
	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(0);
    }
}
