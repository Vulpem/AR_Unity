using UnityEngine;
using System.Collections;

public class Exit_Indicator : MonoBehaviour {

    private float timer = 0.0f;
    public float blink_time = 0.2f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer > blink_time)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            child.SetActive(!child.activeSelf);
            timer = 0;
        }
	}
}
