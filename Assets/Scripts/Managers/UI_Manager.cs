using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_Manager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UnShowUI(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void ShowUI(GameObject ui)
    {
        ui.SetActive(true);
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(1);  
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
