using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
    public float desired_rotation;
    public float rotation_speed;
    public float current_rotation;
    public float dif;

	// Use this for initialization
	void Start ()
    {
	
	}

    // Update is called once per frame
    void Update()
    {
        current_rotation = gameObject.GetComponent<Transform>().eulerAngles.y;
        dif = Mathf.DeltaAngle(current_rotation, desired_rotation);

        if (Mathf.Abs(dif) > 10)
        {
            gameObject.GetComponent<Transform>().Rotate(Vector3.up, (dif > 0 ? rotation_speed : -rotation_speed) * Time.deltaTime);
            current_rotation = gameObject.GetComponent<Transform>().eulerAngles.y;
        }
        
    }
}
