using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class Receiver : DialogNPC
{
    private QuestSystem m_questSystem;
    private string[] m_txtAContentNFQ;      // NFQ = Not Finished Quest
    private string[] m_txtAContentFQ;       // FQ = finished quest

    protected override void Awake()
    {
        m_FQ = false;

        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();

        m_interactableE = transform.GetChild(0).gameObject;
        m_dialogTxt = m_dialogUI.transform.GetChild(0).GetComponentInChildren<Text>();
        m_npcNameTxt = m_dialogUI.transform.GetChild(1).GetComponentInChildren<Text>();

        string txtContent = m_txtFile.text.Replace("\r", "").Replace("\n", "");
        string[] m_tmpTxtContent = txtContent.Split('~');

        m_txtAContentNFQ = m_tmpTxtContent[0].Split('#');
        m_txtAContentFQ = m_tmpTxtContent[1].Split('#');
    }

    #region --- Dialog Manage ---
    protected override void EndDialog()
    {
        base.EndDialog();

        m_questSystem.UpdateQuestStatus(EQuest.DELIVER, gameObject, 1);
    }

    protected override void SetDialogText()
    {
        if (m_FQ)
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
    #endregion

    #region --- Custom Inspector ---
    // Custom Editor using SerializedProperties.
    [CustomEditor(typeof(Receiver))]
    public class CostumEditorGUILayout : EditorGUILayoutPropertyField
    {
        protected override void OnEnable()
        {
            base.OnEnable();    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
    #endregion
}
