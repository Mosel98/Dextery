using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;

    public float m_MaxHealth { get; private set; }
    public float m_Health { get; private set; }

    public float m_MaxMana { get; private set; }
    public float m_Mana { get; private set; }

    public float m_Atk { get; private set; }
    public float m_Def { get; private set; }

    public float m_Exp { get; private set; }
    public float m_ExpReq { get; private set; }

    public int m_Lvl { get; private set; }
    public int m_Gold { get; private set; }

    private void Start()
    {
        if(m_Lvl == 0)
        {
            m_MaxHealth = 100.0f;
            m_Health = m_MaxHealth;
            m_MaxMana = 20.0f;
            m_Mana = m_MaxMana;

            m_Atk = 15.0f;
            m_Def = 50.0f;

            m_Exp = 0.0f;
            m_ExpReq = 100.0f;

            m_Lvl = 1;
            m_Gold = 100;
        }
    }

    #region --- Update Attributes ---
    public void SetHealth(float _newHealth)
    {
        m_Health = _newHealth;
    }

    public void SetMana(float _newMana)
    {
        m_Mana = _newMana;
    }

    public void SetEarnExp(float _exp)
    {
        m_Exp += _exp;

        while (m_Exp >= m_ExpReq)
        {
            float tmpExp = m_Exp - m_ExpReq;

            m_Exp = tmpExp;
            m_ExpReq *= 2;

            m_MaxHealth *= 1.1f;
            m_Health = m_MaxHealth;
            m_MaxMana *= 1.1f;
            m_Mana = m_MaxMana;

            m_Atk *= 1.1f;
            m_Def *= 1.1f;

            m_Lvl++;
        }
    }

    public void SetEarnGold(int _gold)
    {
        m_Gold += _gold;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_combatManager.SetCombat(gameObject, collision.gameObject);
        }
    }
}
