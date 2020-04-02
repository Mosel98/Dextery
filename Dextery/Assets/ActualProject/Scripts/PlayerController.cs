using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
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
        Move();
    }

    private void Move()
    {
        Vector2 movementInput = m_controls.Player.Movement.ReadValue<Vector2>();

        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), m_smoothness);
        }
        
        movement = movement.normalized * m_movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
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
