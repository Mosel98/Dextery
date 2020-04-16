using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;
    [SerializeField]
    private float m_movementSpeed = 5;
    [SerializeField]
    private float m_smoothness = 2f;

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
        if (!m_combatManager.m_combat)
            Move();


        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Move()
    {
        Vector2 movementInput = m_controls.Player.Movement.ReadValue<Vector2>();

        Vector3 direction = movementInput.x * transform.right + movementInput.y * transform.forward;
        direction = direction.normalized * m_movementSpeed;

        direction.y = m_player.velocity.y;
        m_player.velocity = direction;

        

        if(direction != Vector3.zero)
            m_capsule.transform.rotation = Quaternion.Slerp(m_capsule.transform.rotation, Quaternion.LookRotation(direction), m_smoothness * Time.deltaTime);
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
