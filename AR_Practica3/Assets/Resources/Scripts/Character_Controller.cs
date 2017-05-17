using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Character_Controller : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    public int character = 0;
    public Character_Controller partner = null;
    public UI_Controller ui = null;
    public GameObject item = null;
    public GameObject initial_position = null;
    public GameObject items_parent = null;
    public GameObject exit_door = null;
    public GameObject exit_indicator = null;
    public LifeManager lifeManager = null;

    public int total_items = 0;

    private float reset_timer_start = 0.0f;
    public float total_reset_time = 2.0f;
    private bool frozen = false;
    private bool reset = false;

    bool finished = false;

    float timeToNextRand = 0.0f;
    float timer = 0.0f;

    float h = 0;
    float v = 0;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
        if (initial_position)
        {
            Reset();
        }
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (item != null)
        {
            total_items++;
            if (partner != null)
                partner.total_items++;
            if (item != null)
                item.SetActive(false);
            if (total_items == items_parent.transform.childCount)
            {
                if (exit_door) exit_door.SetActive(true); //TODO
                if (exit_indicator) exit_indicator.SetActive(true);
            }
            item = null;
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (frozen == true)
        {
            m_Character.Move(Vector3.zero, false, false);
            if (reset == true && reset_timer_start + total_reset_time <= Time.time)
            {
                frozen = false;
                reset = false;
            }
        }
        else
        {
            // read inputs

            if (initial_position)
            {
                v = 0;
                h = 0;

                if (character == 0 ? Input.GetKey(KeyCode.UpArrow) : Input.GetKey(KeyCode.W))
                    v = 1;
                else if (character == 0 ? Input.GetKey(KeyCode.DownArrow) : Input.GetKey(KeyCode.S))
                    v = -1;

                if (character == 0 ? Input.GetKey(KeyCode.LeftArrow) : Input.GetKey(KeyCode.A))
                    h = -1;
                else if (character == 0 ? Input.GetKey(KeyCode.RightArrow) : Input.GetKey(KeyCode.D))
                    h = 1;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > timeToNextRand)
                {
                    timer = 0;
                    v = UnityEngine.Random.Range(-1.0f, 1.0f);
                    h = UnityEngine.Random.Range(-1.0f, 1.0f);
                    timeToNextRand = UnityEngine.Random.Range(0.5f, 4.0f);
                }
            }

            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

    }

    public void GetHit()
    {
        if (!lifeManager.LooseLife())
        {
            gameObject.SetActive(false);
            ui.OnMasterWin();
        }
        Reset();
    }

    void Reset()
    {
        gameObject.transform.position = initial_position.transform.position;
        Freeze(true);
        reset = true;
        reset_timer_start = Time.time;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void Freeze(bool freeze)
    {
        frozen = freeze;
    }

    public void OnExitGame()
    {
        finished = true;
        Freeze(true);

        if (partner.finished == true)
        {
            m_Character.m_GravityMultiplier = -1.0f;
            partner.m_Character.m_GravityMultiplier = -1.0f;
            ui.OnPlayersWin();
        }
    }
}
