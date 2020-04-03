using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public bool m_combat { get; private set; }

    [SerializeField]
    private GameObject m_playCamera;
    [SerializeField]
    private GameObject m_combatField;
    [SerializeField]
    private GameObject m_combatUI;
    [SerializeField]
    private GameObject m_inventoryPanel;
    [SerializeField]
    private Slider m_hpSlider;
    [SerializeField]
    private Slider m_manaSlider;
    [SerializeField]
    private Slider m_eSlider;
    [SerializeField]
    private Text m_playerInfo;
    [SerializeField]
    private Text m_enemyInfo;
    [SerializeField]
    private PlayerAttributes m_playAttr;
    [SerializeField]
    private Inventory m_inventory;

    private GameObject m_player;
    private GameObject m_enemy;
    private GameObject m_tmpCombatField;

    private bool m_playerTurn;

    private float m_tmpDefendPlay = 0.0f;
    private float m_tmpDefendEnemy = 0.0f;

    private float m_playHealth;
    private float m_playAtk;
    private float m_playDef;
    private float m_playMana;

    private float m_enemyHealth;
    private float m_enemyAtk;
    private float m_enemyDef;
    private float m_enemyMana;

    private float m_exp;
    private int m_gold;

    private int turn = 1;

    private Vector3 m_pCombatPos;
    private Vector3 m_eCombatPos;


    private void Update()
    {
        if (m_combat)
        {
            CheckCombatPositions();

            if (!m_playerTurn)
            {
                float rnd = Random.Range(0, 3);

                if (rnd < 2)
                {
                    float tmp = m_tmpDefendPlay - m_enemyAtk;

                    if (tmp <= 0.0f)
                    {
                        m_playHealth += tmp;
                        m_hpSlider.value = m_playHealth;
                        m_tmpDefendPlay = 0.0f;

                        UpdateInfoBox(1, "Dextery takes damage!");
                    }
                    else
                    {
                        UpdateInfoBox(1, "Dextery blocks the attack!");
                    }

                    m_playerTurn = true;
                }
                else
                {
                    m_tmpDefendEnemy = m_enemyDef;
                    UpdateInfoBox(1, "Enemey defends himself!");

                    m_playerTurn = true;
                }
            }
        }
    }

    #region --- Combat Settings ---
    public void SetCombat(GameObject _player, GameObject _enemy)
    {
        m_player = _player;
        m_enemy = _enemy;

        m_playCamera.SetActive(false);

        SetCombatField();

        SetPlayer();

        SetEnemy();

        StartCombat();
    }

    private void StartCombat()
    {
        m_combatUI.SetActive(true);

        m_playerTurn = true;
        m_combat = true;

        m_player.transform.position = m_pCombatPos;
        m_enemy.transform.position = m_eCombatPos;
    }

    private void EndCombat(bool _win, params GameObject[] _destroys)
    {
        m_combatUI.SetActive(false);
        m_combat = false;

        m_tmpDefendPlay = 0.0f;
        m_tmpDefendEnemy = 0.0f;

        for (int i = 0; i < _destroys.Length; i++)
        {
            Destroy(_destroys[i]);
        }

        m_playCamera.SetActive(true);

        if (_win)
        {
            m_playAttr.SetHealth(m_playHealth);
            m_playAttr.SetMana(m_playMana);
            m_playAttr.SetEarnExp(m_exp);
            m_playAttr.SetEarnGold(m_gold);
        }

        CleanInfoBoxes();
    }
    #endregion

    #region --- Setting Duellist ---
    private void SetPlayer()
    {
        m_hpSlider.maxValue = m_playAttr.m_MaxHealth;
        m_hpSlider.value = m_playAttr.m_Health;
        m_manaSlider.maxValue = m_playAttr.m_MaxMana;
        m_manaSlider.value = m_playAttr.m_Mana;

        m_playHealth = m_playAttr.m_Health;
        m_playMana = m_playAttr.m_Mana;
        m_playAtk = m_playAttr.m_Atk;
        m_playDef = m_playAttr.m_Def;
    }

    private void SetEnemy()
    {
        // Get enemy type
        EnemySetter es = m_enemy.GetComponent<EnemySetter>();
        EEnemyTypes eType = es.EnemyType;

        // Set Enemy battle parameters
        switch (eType)
        {
            case EEnemyTypes.CASUAL:
                m_eSlider.maxValue = 50.0f;
                m_eSlider.value = 50.0f;
                m_enemyHealth = 50.0f;
                m_enemyAtk = 10.0f;
                m_enemyDef = 10.0f;
                m_enemyMana = 1.0f;
                m_exp = 50.0f;
                m_gold = 10;
                break;
            case EEnemyTypes.WRATH:
                m_eSlider.maxValue = 20.0f;
                m_eSlider.value = 20.0f;
                m_enemyHealth = 20.0f;
                m_enemyAtk = 5.0f;
                m_enemyDef = 10.0f;
                m_enemyMana = 10.0f;
                m_exp = 1000.0f;
                m_gold = 1000;
                break;
        }
    }
    #endregion

    #region --- Combat Field Related ---
    private void SetCombatField()
    {
        m_tmpCombatField = Instantiate(m_combatField, m_player.transform.position, m_player.transform.rotation, transform.parent);
        m_pCombatPos = m_tmpCombatField.transform.GetChild(0).position;
        m_eCombatPos = m_tmpCombatField.transform.GetChild(1).position;
    }

    private void CheckCombatPositions()
    {
        Vector2 tmpP = new Vector2(m_player.transform.position.x, m_player.transform.position.z);
        Vector2 tmpE = new Vector2(m_enemy.transform.position.x, m_enemy.transform.position.z);

        Vector2 tmpCP = new Vector2(m_pCombatPos.x, m_pCombatPos.z);
        Vector2 tmpCE = new Vector2(m_eCombatPos.x, m_eCombatPos.z);

        if (tmpP != tmpCP)
            m_player.transform.position = m_pCombatPos;

        if (tmpE != tmpCE)
            m_enemy.transform.position = m_eCombatPos;
    }
    #endregion

    #region --- Combat UI---
    public void Attack()
    {
        if (m_playerTurn)
        {
            float tmp = m_tmpDefendEnemy - m_playAtk;

            if(tmp <= 0.0f)
            {
                m_enemyHealth += tmp;
                m_eSlider.value = m_enemyHealth;
                m_tmpDefendEnemy = 0.0f;

                UpdateInfoBox(0, "Enemey takes damage!");

                if (m_enemyHealth <= 0.0f)
                {
                    EndCombat(true, m_enemy, m_tmpCombatField);
                }
            }
            else
            {
                UpdateInfoBox(0, "Enemy blocks the attack!");
            }

            m_playerTurn = false;
        }
    }

    public void Defend()
    {
        if (m_playerTurn)
        {
            m_tmpDefendPlay = m_playDef;
            UpdateInfoBox(0, "Dextery defends himself!");

            m_playerTurn = false;
        }
    }

    public void OpenInventory()
    {
        m_inventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        m_inventoryPanel.SetActive(false);
    }

    public void Run()
    {
        float rnd = Random.Range(0, 4);

        if (rnd < 3)
        {
            EndCombat(false, m_tmpCombatField);
        }
        else
        {
            UpdateInfoBox(0, "Run away failed!");
            m_playerTurn = false;
        }
    }

    private void UpdateInfoBox(int _id, string _info)
    {
        switch (_id)
        {
            case 0:
                m_playerInfo.text = $"{turn}. {_info}";
                break;
            case 1:
                m_enemyInfo.text = $"{turn}. {_info}";
                break;
        }

        turn++;
    }

    private void CleanInfoBoxes()
    {
        m_playerInfo.text = "";
        m_enemyInfo.text = "";

        turn = 1;
    }

    public void Heal()
    {
        m_playHealth += 10.0f;
        m_hpSlider.value = m_playHealth;

        m_inventory.RemoveItem(new Item { ItemType = EItems.HEALPOTION, Amount = 1});
        UpdateInfoBox(0, "Dextery used Heald Potion!");

        m_inventoryPanel.SetActive(false);

        m_playerTurn = false;
    }

    public void Mana()
    {
        m_playMana += 10.0f;
        m_manaSlider.value = m_playMana;

        m_inventory.RemoveItem(new Item { ItemType = EItems.MANAPOTION, Amount = 1 });
        UpdateInfoBox(0, "Dextery used Mana Potion!");

        m_inventoryPanel.SetActive(false);

        m_playerTurn = false;
    }
    #endregion
}
