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

    public int m_Lvl = 1;

    private void Awake()
    {
        m_SpawnPos = transform.position;
    }
}
