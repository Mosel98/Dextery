using UnityEngine;

public class EnemySetter : MonoBehaviour
{
    public EEnemyTypes EnemyType;
    public Vector3 m_spawnPos;

    private void Awake()
    {
        m_spawnPos = transform.position;
    }
}
