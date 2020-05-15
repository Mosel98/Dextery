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

    private PlayerControls m_controls = null;
    private Rigidbody m_player;

    private MeshFilter m_capsule;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        m_player = GetComponent<Rigidbody>();
        m_controls = new PlayerControls();
        m_capsule = GetComponentInChildren<MeshFilter>();
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
        Vector2 movementInput = m_controls.Player.Movement.ReadValue<Vector2>();

        Vector3 direction = movementInput.x * transform.right + movementInput.y * transform.forward;
        direction = direction.normalized * m_movementSpeed;

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
                    SceneManager.LoadScene("Overworld");
                    break;
                case "Overworld":
                    SceneManager.LoadScene("Heddwyn");
                    break;
            }
        }
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
