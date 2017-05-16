using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour {

    // Use this for initialization
    public GameObject hp;
    public List<GameObject> list;
    public GameObject end_panel = null;
    public GameObject win_panel = null;
    public GameObject loose_panel = null;
    public GameObject hp_panel = null;

	void Start ()
    {
        list = new List<GameObject>();
        list.Add(hp);

        for (uint i = 0; i < 4; ++i)
        {
            list.Add(Instantiate(hp));
            RectTransform transform = list[list.Count - 1].GetComponent<RectTransform>();
            transform.SetParent(hp.transform.parent, false);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 75 * (i + 1), transform.localPosition.z);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnLoseHP()
    {
        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (list[i].activeSelf == true)
            {
                list[i].SetActive(false);
                break;
            }
        }
    }

    public void OnPlayersWin()
    {
        if (end_panel) end_panel.SetActive(true);
        if (win_panel) win_panel.SetActive(true);
        if (hp_panel) hp_panel.SetActive(false);
    }

    public void OnMasterWin()
    {
        if (end_panel) end_panel.SetActive(true);
        if (loose_panel) loose_panel.SetActive(true);
        if (hp_panel) hp_panel.SetActive(false);
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
