using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour {

    // Use this for initialization
    public GameObject end_panel = null;
    public GameObject win_panel = null;
    public GameObject loose_panel = null;

	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnPlayersWin()
    {
        if (end_panel) end_panel.SetActive(true);
        if (win_panel) win_panel.SetActive(true);
    }

    public void OnMasterWin()
    {
        if (end_panel) end_panel.SetActive(true);
        if (loose_panel) loose_panel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
