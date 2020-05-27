using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// script by Tamara
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_movementSpeed = 5;
    [SerializeField]
    private float m_smoothness = 5f;

    [SerializeField]
    private Transform m_camera;
    [SerializeField]
    private Transform m_dextery;

    [SerializeField]
    private Animator m_crossfade;
    [SerializeField]
    private float m_crossfadeTime = 1.05f;
    [SerializeField]
    private Animator m_walkAnimation;

    private float m_tmpMoveSpeed;

    private PlayerControls m_controls = null;
    private Rigidbody m_player;

    
    // Start is called before the first frame update
    void Awake()
    {
        m_player = GetComponent<Rigidbody>();
        m_controls = new PlayerControls();

        m_tmpMoveSpeed = m_movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isOccupied)
            Move();

        else
        {
            m_walkAnimation.SetBool("isMoving", false);

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // run added by Mario
        {
            if (m_tmpMoveSpeed != m_movementSpeed * 2)
                m_tmpMoveSpeed = m_movementSpeed * 2;
        }
        else if(m_tmpMoveSpeed != m_movementSpeed)
        {
            m_tmpMoveSpeed = m_movementSpeed;
        }

        // new Input System
        Vector2 movementInput = m_controls.Player.Movement.ReadValue<Vector2>();

        Vector3 direction = movementInput.x * transform.right + movementInput.y * transform.forward;
        direction = direction.normalized * m_tmpMoveSpeed;

        // move in relation to camera view
        direction = m_camera.TransformDirection(direction);

        direction.y = m_player.velocity.y;
        m_player.velocity = direction;
       

        direction.y = 0;
        if (direction != Vector3.zero)
        {
            // rotate smother into the wanted direction
            m_dextery.transform.rotation = Quaternion.Slerp(m_dextery.transform.rotation, 
                                           Quaternion.LookRotation(direction), m_smoothness * Time.deltaTime);
            
            m_walkAnimation.SetBool("isMoving", true);
        }
        else
        {
            m_walkAnimation.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        // switch from outside town, to inside town and other way around
        if(_col.gameObject.tag == "SceneTrigger")
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Heddwyn":
                    StartCoroutine(LoadLevel("Overworld")); // Play Loadingscreen while changing scene
                    break;
                case "Overworld":
                    StartCoroutine(LoadLevel("Heddwyn")); // Play Loadingscreen while changing scene
                    break;
            }
        }
    }

    IEnumerator LoadLevel(string _levelName)
    {
        // Start animation
        m_crossfade.SetTrigger("Start");

        // wait for end
        yield return new WaitForSeconds(m_crossfadeTime);

        // change scene when finished
        SceneManager.LoadScene(_levelName);
    }

    private void OnEnable()
    {
        m_controls.Player.Enable();
    }

    private void OnDisable()
    {
        m_controls.Player.Disable();
    }
}
