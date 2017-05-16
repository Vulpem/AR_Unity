using UnityEngine;
using System.Collections;

public class CollectibleMovement : MonoBehaviour {

    Vector3 originalPos;
    public float displacement = 1.0f;
    public float rotation_speed = 40.0f;
    Transform trs;

	// Use this for initialization
	void Start () {
        trs = GetComponent<Transform>();
        originalPos = trs.position;
    }
	
	// Update is called once per frame
	void Update () {
        trs.Rotate(trs.up, rotation_speed * Time.deltaTime);
        trs.position = new Vector3(trs.position.x, originalPos.y + Mathf.Sin(Time.time) * displacement, trs.position.z);	
	}
}
