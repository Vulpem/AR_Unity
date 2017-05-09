using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Target
{
    public string name;
    public GameObject card;
    public int n_states;
    public int state;
    public float rotation;

    public GameObject display;
    public TurretController[] turrets;
}

public class RotationController : MonoBehaviour
{
    public Target[] cards;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int n = 0; n < cards.Length; n++)
        {
            int step = 360 / cards[n].n_states;
            float rot = cards[n].card.GetComponent<Transform>().eulerAngles.y;
            cards[n].rotation = rot;
            while (rot >= 360) { rot -= 360; }
            while (rot < 0) { rot += 360; }
            cards[n].state = (int)Mathf.Floor(rot / step);

            Transform displayTrs = cards[n].display.GetComponent<Transform>();
            if (displayTrs)
            {
                displayTrs.rotation = new Quaternion();
                displayTrs.eulerAngles = new Vector3(displayTrs.eulerAngles.x, cards[n].rotation, displayTrs.eulerAngles.z);
            }
            if (cards[n].turrets.Length > 0)
            {
                cards[n].turrets[cards[0].state].desired_rotation = cards[n].rotation;
            }
            else
            {
                Debug.Log(cards[n].name + "card doesn't have a display");
            }

        }

    }
}
