﻿using UnityEngine;

public class EnemySetter : MonoBehaviour
{
    public EEnemyTypes EnemyType;
    public Vector3 m_SpawnPos { get; private set; }

    private void Awake()
    {
        m_SpawnPos = transform.position;
    }
}
