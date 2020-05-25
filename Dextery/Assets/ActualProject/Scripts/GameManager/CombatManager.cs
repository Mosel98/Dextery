using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class CombatManager : MonoBehaviour
{
    public bool m_combat { get; private set; }
    [SerializeField]
    private AudioSource m_fightingSound;
    [SerializeField]
    private AudioSource m_fightingMusic;
    [SerializeField]
    private AudioSource m_overworldMusic;
    [SerializeField]
    private GameObject m_playCamera;
    [SerializeField]
    private GameObject m_combatField;
    [SerializeField]
    private GameObject m_combatUI;

    private GameObject m_inventoryPanel;
    private Slider m_hpSlider;
    private Slider m_manaSlider;
    private Slider m_eSlider;
    private Text m_playerInfo;
    private Text m_enemyInfo;

    private GameObject m_player;
    private GameObject m_enemy;
    private GameObject m_tmpCombatField;

    private QuestSystem m_questSystem;
    private PlayerAttributes m_playAttr;
    private EnemySetter m_enemySetter;
    private Inventory m_inventory;

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

    private void Awake()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("Player");

        m_playAttr = tmp.GetComponent<PlayerAttributes>();
        m_inventory = tmp.GetComponent<Inventory>();

        m_questSystem = GetComponent<QuestSystem>();

        m_inventoryPanel = m_combatUI.transform.GetChild(8).gameObject;
        m_hpSlider = m_combatUI.transform.GetChild(4).GetComponent<Slider>();
        m_manaSlider = m_combatUI.transform.GetChild(5).GetComponent<Slider>();
        m_eSlider = m_combatUI.transform.GetChild(6).GetComponent<Slider>();

        Transform info = m_combatUI.transform.GetChild(7);

        m_playerInfo = info.GetChild(0).GetComponent<Text>();
        m_enemyInfo = info.GetChild(1).GetComponent<Text>();
    }

    // Functions as enemy KI during combat
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
                    m_fightingSound.Play();
                    float tmp = m_tmpDefendPlay - m_enemyAtk;
                    m_tmpDefendPlay = 0.0f;

                    if (tmp <= 0.0f)
                    {
                        m_playHealth += tmp;
                        m_hpSlider.value = m_playHealth;

                        UpdateInfoBox(1, "Dextery takes damage!");

                        if (m_playHealth <= 0.0f)
                            EndCombat(false);
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
        GameManager.isOccupied = true;

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

        m_fightingMusic.Stop();
        m_fightingMusic.Play();
        StartCoroutine(CrossFade(m_overworldMusic, m_fightingMusic));

        m_player.transform.position = m_pCombatPos;
        m_enemy.transform.position = m_eCombatPos;
    }

    private IEnumerator CrossFade(AudioSource _start, AudioSource _end)
    {
        float goalVolume = _start.volume;
        float time = 0;
        // seconds needed
        float speed = 1.0f;

        while (time < 1)
        {
            _start.volume = Mathf.Lerp(goalVolume, 0, time);
            _end.volume = Mathf.Lerp(0, goalVolume, time);
            time += Time.deltaTime / speed;
            yield return null;
        }
        _start.volume = 0;
        _end.volume = goalVolume;
    }
    private void EndCombat(bool _win, params GameObject[] _destroys)
    {
        GameManager.isOccupied = false;

        // Go to EndScreen after defeat/defeated by Wrath
        if (!_win && (m_enemySetter.EnemyType == EEnemy.WRATH || m_playHealth <= 0.0f))
        {
            GameManager.ToTheEnd(_win);
            return;
        }

        m_combatUI.SetActive(false);
        m_combat = false;

        m_tmpDefendPlay = 0.0f;
        m_tmpDefendEnemy = 0.0f;

        for (int i = 0; i < _destroys.Length; i++)
        {
            Destroy(_destroys[i]);
        }

        m_playCamera.SetActive(true);

        m_playAttr.SetHealth(m_playHealth);
        m_playAttr.SetMana(m_playMana);

        if (_win)
        {
            m_playAttr.SetEarnExp(m_exp);
            m_playAttr.SetEarnGold(m_gold);
        }
        StartCoroutine(CrossFade(m_fightingMusic, m_overworldMusic));

        CleanInfoBoxes();
    }
    #endregion

    #region --- Setting Duellist ---
    private void SetPlayer()
    {
        m_hpSlider.maxValue = PlayerAttributes.m_MaxHealth;
        m_hpSlider.value = PlayerAttributes.m_Health;
        m_manaSlider.maxValue = PlayerAttributes.m_MaxMana;
        m_manaSlider.value = PlayerAttributes.m_Mana;

        m_playHealth = PlayerAttributes.m_Health;
        m_playMana = PlayerAttributes.m_Mana;
        m_playAtk = PlayerAttributes.m_Atk;
        m_playDef = PlayerAttributes.m_Def;
    }

    private void SetEnemy()
    {
        // Get enemy type
        m_enemySetter = m_enemy.GetComponent<EnemySetter>();
        EEnemy eType = m_enemySetter.EnemyType;
        // int lvl = m_enemySetter.m_Lvl;

        // Set Enemy battle parameters
        switch (eType)
        {
            case EEnemy.CASUAL:
                m_eSlider.maxValue = 50.0f;
                m_eSlider.value = 50.0f;
                m_enemyHealth = 50.0f;
                m_enemyAtk = 10.0f;
                m_enemyDef = 5.0f;
                m_enemyMana = 1.0f;
                m_exp = 50.0f;
                m_gold = 10;
                break;
            case EEnemy.WRATH:
                m_eSlider.maxValue = 150.0f;
                m_eSlider.value = 150.0f;
                m_enemyHealth = 150.0f;
                m_enemyAtk = 20.0f;
                m_enemyDef = 15.0f;
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
        Ray ray = new Ray(m_tmpCombatField.transform.GetChild(0).position + Vector3.up * 500, Vector3.down);
        if (Physics.Raycast(ray, out var hit)) 
        { 
            m_pCombatPos = hit.point + Vector3.up; 
        }
        else
        {
            m_pCombatPos = m_tmpCombatField.transform.GetChild(0).position;
        }
        m_eCombatPos = m_tmpCombatField.transform.GetChild(1).position;
    }

    // Check if participant are still on their Combat Position
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
            m_fightingSound.Play();
            float tmp = m_tmpDefendEnemy - m_playAtk;

            if (tmp <= 0.0f)
            {
                m_enemyHealth += tmp;
                m_eSlider.value = m_enemyHealth;
                m_tmpDefendEnemy = 0.0f;

                UpdateInfoBox(0, "Enemey takes damage!");

                if (m_enemyHealth <= 0.0f)
                {
                    m_questSystem.UpdateQuestStatus(EQuest.FIGHT, null, (int)m_enemy.GetComponent<EnemySetter>().EnemyType);
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

        if (rnd < 3 && m_enemySetter.EnemyType != EEnemy.WRATH)
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

    public void Heal(float _heal)
    {
        m_playHealth += _heal;
        m_hpSlider.value = m_playHealth;

        m_inventory.RemoveItem(new Item { ItemType = EItem.HEALPOTION, Amount = 1 });
        UpdateInfoBox(0, "Dextery used Heald Potion!");

        m_inventoryPanel.SetActive(false);

        m_playerTurn = false;
    }

    public void Mana(float _mana)
    {
        m_playMana += _mana;
        m_manaSlider.value = m_playMana;

        m_inventory.RemoveItem(new Item { ItemType = EItem.MANAPOTION, Amount = 1 });
        UpdateInfoBox(0, "Dextery used Mana Potion!");

        m_inventoryPanel.SetActive(false);

        m_playerTurn = false;
    }
    #endregion
}
