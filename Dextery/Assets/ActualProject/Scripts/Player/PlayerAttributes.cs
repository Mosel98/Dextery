using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField]
    private CombatManager m_combatManager;
    [SerializeField]
    private Text m_statsText;

    private Inventory m_inventory;

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

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();
    }

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

        UpdateStatsInfo();
    }

    #region --- Update Attributes ---
    public void SetHealth(float _newHealth)
    {
        m_Health = _newHealth;

        UpdateStatsInfo();
    }

    public void SetMana(float _newMana)
    {
        m_Mana = _newMana;

        UpdateStatsInfo();
    }

    public void AddHealth(float _health)
    {
        if(m_Health + _health > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
        else
        {
            m_Health += _health;
        }

        m_inventory.RemoveItem(new Item { ItemType = EItems.HEALPOTION, Amount = 1 });
        UpdateStatsInfo();
    }

    public void AddMana(float _mana)
    {
        if(m_Mana + _mana > m_MaxMana)
        {
            m_Mana = m_MaxMana;
        }
        else
        {
            m_Mana += _mana;
        }

        m_inventory.RemoveItem(new Item { ItemType = EItems.MANAPOTION, Amount = 1 });
        UpdateStatsInfo();
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

        UpdateStatsInfo();
    }

    public void SetEarnGold(int _gold)
    {
        m_Gold += _gold;

        UpdateStatsInfo();
    }

    private void UpdateStatsInfo()
    {
        m_statsText.text = $"Max Health: {m_MaxHealth}\nHealth: {m_Health}\nMax Mana: {m_MaxMana}\nMana: {m_Mana}\nAttack: {m_Atk}\nDefend: {m_Def}\n---------------\nLevel: {m_Lvl}\nExperience for next Level: {m_ExpReq}\nExp: {m_Exp}\n---------------\nGold: {m_Gold}";
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
