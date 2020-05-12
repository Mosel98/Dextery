using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField]
    private Text m_statsText;
    [SerializeField]
    private InputField m_inputLvlFld;

    private CombatManager m_combatManager;
    private StoryManager m_storyManager;
    private Inventory m_inventory;

    public static float m_MaxHealth { get; private set; }
    public static float m_Health { get; private set; }
            
    public static float m_MaxMana { get; private set; }
    public static float m_Mana { get; private set; }
            
    public static float m_Atk { get; private set; }
    public static float m_Def { get; private set; }
           
    public static float m_Exp { get; private set; }
    public static float m_ExpReq { get; private set; }
           
    public static int m_Lvl { get; private set; }
    public static int m_Gold { get; private set; }

    private void Awake()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("GameManager");
        m_combatManager = tmp.GetComponent<CombatManager>();
        m_storyManager = tmp.GetComponent<StoryManager>();

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

        m_inventory.RemoveItem(new Item { ItemType = EItem.HEALPOTION, Amount = 1 });
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

        m_inventory.RemoveItem(new Item { ItemType = EItem.MANAPOTION, Amount = 1 });
        UpdateStatsInfo();
    }

    public void SetEarnExp(float _exp)
    {
        m_Exp += _exp;

        while (m_Exp >= m_ExpReq)
        {
            float tmpExp = m_Exp - m_ExpReq;

            m_Exp = tmpExp;
            m_ExpReq = (float)Math.Round((m_ExpReq * 1.25f) * 100f) / 100f;

            m_MaxHealth = (int) (m_MaxHealth * 1.1f);
            m_Health = m_MaxHealth;
            m_MaxMana = (int) (m_MaxMana * 1.1f);
            m_Mana = m_MaxMana;

            m_Atk = (float)Math.Round((m_Atk * 1.1f) * 100f) / 100f;
            m_Def = (float)Math.Round((m_Def * 1.1f) * 100f) / 100f;

            m_Lvl++;
        }

        UpdateStatsInfo();
    }

    public void SetEarnGold(int _gold)
    {
        m_Gold += _gold;

        UpdateStatsInfo();
    }

    public void SetGold(int _gold)
    {
        m_Gold = _gold;

        UpdateStatsInfo();
    }

    public void ChangeLvl()
    {
        m_inputLvlFld.Select();

        int wantedLvl = int.Parse(m_inputLvlFld.text);
        float tmpExp = 0.0f;

        if (wantedLvl > m_Lvl)
        {
            while (m_Lvl != wantedLvl)
            {
                if (m_Exp > 0.0f)
                    tmpExp = m_ExpReq - m_Exp;
                else
                    tmpExp = m_ExpReq;

                SetEarnExp(tmpExp);
            }
        }

        m_inputLvlFld.text = "";
    }

    private void UpdateStatsInfo()
    {
        m_statsText.text = $"Max Health: {m_MaxHealth}\nHealth: {m_Health}\nMax Mana: {m_MaxMana}\nMana: {m_Mana}\nAttack: {m_Atk}\nDefend: {m_Def}\n---------------\nLevel: {m_Lvl}\nExperience for next Level: {m_ExpReq}\nExp: {m_Exp}\n---------------\nGold: {m_Gold}";
        m_storyManager.CurrentPlayerLvl(m_Lvl);
    }
    #endregion

    public float[] GetStats()
    {
        float[] tmp = { m_MaxHealth, m_Health, m_MaxMana, m_Mana, m_Atk, m_Def, m_Exp, m_ExpReq, m_Lvl, m_Gold};

        return tmp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(!GameObject.FindGameObjectWithTag("CombatField"))
                m_combatManager.SetCombat(gameObject, collision.gameObject);
        }
    }
}
