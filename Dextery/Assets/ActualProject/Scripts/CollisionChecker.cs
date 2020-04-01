using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;

    public float m_maxHealth { get; private set; }
    public float m_health { get; private set; }
    public float m_atk { get; private set; }
    public float m_def { get; private set; }
    public float m_mana { get; private set; }

    private void Start()
    {
        m_maxHealth = 100.0f;
        m_health = 100.0f;
        m_atk = 15.0f;
        m_def = 50.0f;
        m_mana = 5.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {           
            m_combatManager.SetCombat(gameObject, collision.gameObject);
        }
    }
}
