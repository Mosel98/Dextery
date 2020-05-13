using UnityEngine;

public enum EEnemy
{
    CASUAL,
    WRATH
}

public class EnemySetter : MonoBehaviour
{
    public EEnemy EnemyType;
    public Vector3 m_SpawnPos { get; private set; }

    public int m_Lvl;

    private void Awake()
    {
        m_SpawnPos = transform.position;
    }

    private void Start()
    {
        m_Lvl = PlayerAttributes.m_Lvl;
    }

    private void Update()
    {
        if (m_Lvl != PlayerAttributes.m_Lvl)
            m_Lvl = PlayerAttributes.m_Lvl;
    }
}
