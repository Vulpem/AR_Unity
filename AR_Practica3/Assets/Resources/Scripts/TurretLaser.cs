using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretLaser : MonoBehaviour
{
    public LayerMask mask;
    public GameObject ray;
    public float range = 20.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
        {
            if (hit.collider.gameObject.tag == "Player")
            {

            }
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, hit.distance / 2);
        }
        else
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, range / 2);
    }
}
