using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_movementSpeed = 5;
    [SerializeField]
    private float m_smoothness = 5f;

    [SerializeField]
    private Transform m_camera;

    [SerializeField]
    private Animator m_crossfade;
    [SerializeField]
    private float m_crossfadeTime = 1.05f;

    private float m_tmpMoveSpeed;

    private PlayerControls m_controls = null;
    private Rigidbody m_player;

    private MeshFilter m_capsule;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        m_player = GetComponent<Rigidbody>();
        m_controls = new PlayerControls();
        m_capsule = GetComponentInChildren<MeshFilter>();

        m_tmpMoveSpeed = m_movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isOccupied)
            Move();


        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (m_tmpMoveSpeed != m_movementSpeed * 2)
                m_tmpMoveSpeed = m_movementSpeed * 2;
        }
        else if(m_tmpMoveSpeed != m_movementSpeed)
        {
            m_tmpMoveSpeed = m_movementSpeed;
        }

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
            m_capsule.transform.rotation = Quaternion.Slerp(m_capsule.transform.rotation, Quaternion.LookRotation(direction), m_smoothness * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if(_col.gameObject.tag == "SceneTrigger")
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Heddwyn":
                    StartCoroutine(LoadLevel("Overworld"));
                    break;
                case "Overworld":
                    StartCoroutine(LoadLevel("Heddwyn"));
                    break;
            }
        }
    }

    IEnumerator LoadLevel(string _levelName)
    {
        m_crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(m_crossfadeTime);

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
