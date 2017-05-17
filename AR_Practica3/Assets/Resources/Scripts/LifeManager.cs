using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour {

    public GameObject[] lifes;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public bool LooseLife()
    {
        foreach(GameObject go in lifes)
        {
            if(go.activeInHierarchy)
            {
                go.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
