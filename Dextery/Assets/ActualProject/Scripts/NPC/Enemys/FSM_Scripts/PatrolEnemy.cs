using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : StateMachineBehaviour
{
    private int point = 0;
    private float m_maxDistance;
    private float m_terrainWidth;
    private float m_terrainLength;

    private bool m_walkPatrol = true;

    private Vector3 m_nextP;
    private List<Vector3> m_patPoints = new List<Vector3>();
    private NavMeshAgent m_nav;
    private GameObject m_player;

    private CombatManager m_cm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MyAwake(animator);
        GeneratePatrolRoute(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_cm.m_combat)
        {
            if (m_walkPatrol)
                WalkPatrolRoute(animator);

            CheckDistancePE(animator);
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }

    void MyAwake(Animator _animator)
    {
        m_maxDistance = _animator.GetFloat("MaxDistance");
        float puffer = _animator.GetFloat("Puffer");

        m_terrainWidth = (Terrain.activeTerrain.GetPosition().x + Terrain.activeTerrain.terrainData.size.x) - puffer;
        m_terrainLength = (Terrain.activeTerrain.GetPosition().z + Terrain.activeTerrain.terrainData.size.z) - puffer;

        m_nav = _animator.GetComponent<NavMeshAgent>();
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_cm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatManager>();
    }

    void GeneratePatrolRoute(Animator _animator)
    {
        // Sets random points (3-10) of patrolPoints for the enemy to walk on
        int rnd = Random.Range(3, 10);

        float randX;
        float randZ;
        float yVal;

        float xEnemyPos = _animator.transform.position.x;
        float zEnemyPos = _animator.transform.position.z;

        Vector3 rndPos;

        for (int i = 0; i <= rnd; i++)
        {
            do
            {
                //Generate random x,z,y position on the terrain
                randX = Random.Range(xEnemyPos, xEnemyPos + (Random.insideUnitSphere.x * 50));
                randZ = Random.Range(zEnemyPos, zEnemyPos + (Random.insideUnitSphere.z * 50));
                yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));

                rndPos = new Vector3(randX, yVal, randZ);

            } while (rndPos.x > m_terrainWidth || rndPos.x < -m_terrainWidth || rndPos.z > m_terrainLength || rndPos.z < -m_terrainLength);

            m_patPoints.Add(rndPos);
        }
    }

    void WalkPatrolRoute(Animator _animator)
    {
        // Sets patrolPoint as next destination
        if (m_nav.destination != m_patPoints[point])
            m_nav.destination = m_patPoints[point];

        // Check if enemy reached one of the patrolPoints
        if ((int)_animator.transform.position.x >= (int)m_patPoints[point].x - 0.5f && (int)_animator.transform.position.x <= (int)m_patPoints[point].x + 0.5f &&
            (int)_animator.transform.position.z >= (int)m_patPoints[point].z - 0.5f && (int)_animator.transform.position.z <= (int)m_patPoints[point].z + 0.5f)
        {
            point++;
            point %= m_patPoints.Count;
        }

        // Draws the patrolPoints in Red (Only for DEBUG)
        foreach (Vector3 ray in m_patPoints)
        {
            Debug.DrawRay(ray, Vector3.up, Color.red);
        }
    }

    void CheckDistancePE(Animator _animator)
    {
        // Calculatation of the distance between enemy and player
        Vector3 distV = m_player.transform.position - _animator.transform.position;
        float mgt = distV.magnitude;
    
        // If distance between player and enemy < than the allowed maxDistance = follow player / else go on Patrol again
        if (mgt <= m_maxDistance)
        {
            m_walkPatrol = false;
            m_nav.destination = m_player.transform.position;
        }
        else if (mgt > (2.0f*m_maxDistance) && !m_walkPatrol)
        {
            m_walkPatrol = true;
        }
    }
}
