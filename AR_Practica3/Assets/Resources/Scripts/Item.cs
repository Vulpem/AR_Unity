using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Character_Controller>().near_item = true;
            collider.gameObject.GetComponent<Character_Controller>().item = this.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Character_Controller>().near_item = false;
            collider.gameObject.GetComponent<Character_Controller>().item = this.gameObject;
        }
    }
}
