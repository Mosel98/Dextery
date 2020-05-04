using UnityEngine;

public class Receiver : DialogNPC
{
    public string nameNPC;

    private QuestSystem m_questSystem;

    public override void Awake()
    {
        base.Awake();

        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();
    }

    public override void EndDialog()
    {
        base.EndDialog();

        m_questSystem.UpdateQuestStatus(EQuest.DELIVER, gameObject);
    }
}
