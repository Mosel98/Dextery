using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private GameObject m_enemy;
    private GameObject m_combatField;

    [SerializeField]
    private GameObject m_combatUI;
    [SerializeField]
    private Slider m_hpSlider;
    [SerializeField]
    private Slider m_eSlider;

    private Vector3 m_enemySpawn;

    private bool m_playerTurn;
    private bool m_combat;

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

    private void Update()
    {
        if (m_combat)
        {
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
                    }

                    m_playerTurn = true;
                }
                else
                {
                    m_tmpDefendEnemy = m_enemyDef;
                    m_playerTurn = true;
                }
            }
        }
    }

    public void SetDuelists(GameObject _player, GameObject _enemy, GameObject _combatField)
    {
        m_enemy = _enemy;
        m_combatField = _combatField;

        // Stop enemys animator settings for battle
        _enemy.GetComponent<Animator>().SetBool("Idle", true);
        _enemy.GetComponent<NavMeshAgent>().isStopped = true;

        // Get enemy type and start position (last only necessary when player runs)
        EnemySetter es = _enemy.GetComponent<EnemySetter>();
        EEnemyTypes eType = es.EnemyType;
        m_enemySpawn = es.m_spawnPos;

        // Sets the duellists to there asigned positions
        _player.transform.position = _combatField.transform.GetChild(0).position;
        _enemy.transform.position = _combatField.transform.GetChild(1).position;

        // Set Players battle parameters
        CollisionChecker cc = _player.GetComponent<CollisionChecker>();
        m_hpSlider.maxValue = cc.m_maxHealth;
        m_hpSlider.value = cc.m_health;
        m_playHealth = cc.m_health;
        m_playAtk = cc.m_atk;
        m_playDef = cc.m_def;
        m_playMana = cc.m_mana;

        // Set Enemy battle parameters
        switch (eType)
        {
            case EEnemyTypes.CASUAL:
                m_eSlider.maxValue = 5.0f;
                m_eSlider.value = 5.0f;
                m_enemyHealth = 5.0f;
                m_enemyAtk = 10.0f;
                m_enemyDef = 1.0f;
                m_enemyMana = 1.0f;
                break;
            case EEnemyTypes.WRATH:
                m_eSlider.maxValue = 20.0f;
                m_eSlider.value = 20.0f;
                m_enemyHealth = 20.0f;
                m_enemyAtk = 5.0f;
                m_enemyDef = 10.0f;
                m_enemyMana = 10.0f;
                break;
        }

        StartCombat();
    }

    private void StartCombat()
    {
        m_combatUI.SetActive(true);

        m_playerTurn = true;
        m_combat = true;
    }

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

                if (m_enemyHealth == 0.0f)
                {
                    Destroy(m_enemy);
                    Destroy(m_combatField);
                }
            }

            m_playerTurn = false;
        }
    }

    public void Defend()
    {
        if (m_playerTurn)
        {
            m_tmpDefendPlay = m_playDef;
            m_playerTurn = false;
        }
    }

    public void Run()
    {
        // Restart enemys Animator
        m_enemy.GetComponent<Animator>().SetBool("Idle", false);
        m_enemy.GetComponent<NavMeshAgent>().isStopped = false;

        // Set enemy back to his original spawn point
        m_enemy.transform.position = m_enemySpawn;

        Destroy(m_combatField);
    }
}
