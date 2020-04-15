using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;
    [SerializeField]
    private float m_movementSpeed = 5;
    [SerializeField]
    private float m_smoothness = 0.1f;

    private PlayerControls m_controls = null;
    private Rigidbody m_player;
    

    // Start is called before the first frame update
    void Awake()
    {
        m_player = GetComponent<Rigidbody>();
        m_controls = new PlayerControls();
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

        Debug.Log(direction);
        


        //if(direction != Vector3.zero)
        //{
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position), m_smoothness * Time.deltaTime);
        //}
        
        //movement = movement.normalized * m_movementSpeed * Time.deltaTime;
        //transform.Translate(movement, Space.World);
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
