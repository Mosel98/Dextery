using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNPC : MonoBehaviour
{
    [SerializeField]
    float m_waitTime;

    private Vector3[] m_patPoints = new Vector3[3];
    private NavMeshAgent m_meshAgent;
    private int m_nxt = 0;
    private bool m_waiting = false;

    private void Awake()
    {
        m_meshAgent = GetComponent<NavMeshAgent>();
        Transform tmp = transform.GetChild(1);

        for(int i = 0; i < m_patPoints.Length; i++)
        {
            m_patPoints[i] = tmp.GetChild(i).position;
        }

        tmp.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PatrolOnRoute();
    }

    private void PatrolOnRoute()
    {
        // Sets patrolPoint as next destination
        m_meshAgent.destination = m_patPoints[m_nxt];

        // Check if enemy reached one of the patrolPoints
        if (!m_waiting && ((int)transform.position.x >= (int)m_patPoints[m_nxt].x - 0.5f && (int)transform.position.x <= (int)m_patPoints[m_nxt].x + 0.5f &&
            (int)transform.position.z >= (int)m_patPoints[m_nxt].z - 0.5f && (int)transform.position.z <= (int)m_patPoints[m_nxt].z + 0.5f))
        {
            m_waiting = true;
            StartCoroutine(WaitCoroutine());
        }
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(m_waitTime);

        m_nxt++;
        m_nxt %= m_patPoints.Length;
        m_waiting = false;
    }
}
