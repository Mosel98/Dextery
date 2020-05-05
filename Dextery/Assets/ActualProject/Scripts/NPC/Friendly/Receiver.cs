using UnityEngine;

public class Receiver : DialogNPC
{
    [Header ("Receiver Settings")]
    [SerializeField]
    private TextAsset m_txtFileNFQ;     // NFQ = Not Finished Quest

    public string nameNPC;
    public bool m_fq = false;          // fq = finished quest

    private QuestSystem m_questSystem;
    private string[] m_txtAContentNFQ;    

    public override void Awake()
    {
        base.Awake();

        string txtContentNFQ = m_txtFileNFQ.text.Replace("\r", "").Replace("\n", "");

        m_txtAContentNFQ = txtContentNFQ.Split('#');

        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();
        m_allowDialog = false;
    }

    public override void EndDialog()
    {
        base.EndDialog();

        m_questSystem.UpdateQuestStatus(EQuest.DELIVER, gameObject, 0);
    }

    public override void SetDialogText()
    {
        if (m_fq)
        {
            if (m_count != (m_txtAContent.Length - 1))
                m_dialogTxt.text = m_txtAContent[m_count];
            else
                EndDialog();
        }
        else
        {
            if (m_count != (m_txtAContentNFQ.Length - 1))
                m_dialogTxt.text = m_txtAContentNFQ[m_count];
            else
                EndDialog();
        }
    }
}
