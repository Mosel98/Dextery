using UnityEngine;
using UnityEngine.UI;

public class Receiver : DialogNPC
{
    [Header ("Receiver Settings")]
    public string nameNPC;
    public bool m_fq = false;          // fq = finished quest

    private QuestSystem m_questSystem;
    private string[] m_txtAContentNFQ;      // NFQ = Not Finished Quest
    private string[] m_txtAContentFQ;       // FQ = finished quest

    public override void Awake()
    {
        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();

        m_interactableE = transform.GetChild(0).gameObject;
        m_dialogTxt = m_dialogUI.GetComponentInChildren<Text>();

        string txtContent = m_txtFile.text.Replace("\r", "").Replace("\n", "");
        string[] m_tmpTxtContent = txtContent.Split('~');

        m_txtAContentNFQ = m_tmpTxtContent[0].Split('#');
        m_txtAContentFQ = m_tmpTxtContent[1].Split('#');
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
            if (m_count != m_txtAContentFQ.Length)
                m_dialogTxt.text = m_txtAContentFQ[m_count];
            else
                EndDialog();
        }
        else
        {
            if (m_count != m_txtAContentNFQ.Length)
                m_dialogTxt.text = m_txtAContentNFQ[m_count];
            else
                EndDialog();
        }
    }

    public void DeactivateE()
    {
        m_interactableE.SetActive(false);
    }
}
