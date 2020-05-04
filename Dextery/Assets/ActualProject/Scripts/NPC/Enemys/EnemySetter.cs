using UnityEngine;

public enum EEnemy
{
    CASUAL,
    WRATH,
    NULL
}

public class EnemySetter : MonoBehaviour
{
    public EEnemy EnemyType;
    public Vector3 m_SpawnPos { get; private set; }

    private void Awake()
    {
        m_SpawnPos = transform.position;
    }
}
