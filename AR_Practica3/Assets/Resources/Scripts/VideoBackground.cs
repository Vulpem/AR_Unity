using UnityEngine;
using System.Collections;


public class VideoBackground : MonoBehaviour {

    public GameObject material_to_copy;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Renderer>().material = material_to_copy.GetComponent<Renderer>().material;
	}
}
