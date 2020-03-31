using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;
    [SerializeField]
    private GameObject m_combatField;

    public float m_maxHealth { get; private set; }
    public float m_health { get; private set; }
    public float m_atk { get; private set; }
    public float m_def { get; private set; }
    public float m_mana { get; private set; }

    private void Start()
    {
        m_maxHealth = 100.0f;
        m_health = 100.0f;
        m_atk = 5.0f;
        m_def = 5.0f;
        m_mana = 5.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject combatField = Instantiate(m_combatField, transform.position, transform.rotation, transform.parent);
            Vector3 posChild0 = combatField.transform.GetChild(0).position;
            Vector3 posChild1 = combatField.transform.GetChild(1).position;
            
            m_combatManager.SetDuelists(gameObject, collision.gameObject, combatField);
        }
    }
}
