using UnityEngine;
using UnityEngine.AI;

public class Idle : StateMachineBehaviour
{
    private NavMeshAgent m_nav;
    private CombatManager m_cm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_nav = animator.GetComponent<NavMeshAgent>();
        m_cm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatManager>();

        animator.transform.position = animator.GetComponent<EnemySetter>().m_SpawnPos;

        m_nav.isStopped = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_cm.m_combat)
        {
            animator.SetBool("Idle", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = animator.GetComponent<EnemySetter>().m_SpawnPos;
        m_nav.isStopped = false;       
    }
}
