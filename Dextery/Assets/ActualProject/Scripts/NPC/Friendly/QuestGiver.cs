using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class QuestGiver : DialogNPC
{      
    [Header("Quest Settings")]
    [SerializeField]
    private EQuest m_eQuest;    
    [SerializeField]
    private EEnemy m_eEnemy;
    [SerializeField]
    private EItem m_eItem;    
    [SerializeField]
    private GameObject m_receiver;

    [SerializeField]
    private bool m_extraDialog = false;     // extra Dialog funtions as a method to let the NPC talk a new sentence after even finishing the quest
    
    [SerializeField]
    private bool m_extraPara = false;
    [SerializeField]
    private int m_amount;
    [SerializeField]
    private int m_gold;
    [SerializeField]
    private float m_exp;

    private QuestSystem m_questSystem;
    private string[] m_txtAContentNFQ;      // NFQ = Not Finished Quest
    private string[] m_txtAContentFQ;       // FQ = finished quest
    private bool questGiven = false;

    protected override void Awake()
    {
        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();

        if(!m_extraDialog)
            base.Awake();
        else
        {
            m_FQ = false;

            m_interactableE = transform.GetChild(0).gameObject;
            m_dialogTxt = m_dialogUI.transform.GetChild(0).GetComponentInChildren<Text>();
            m_npcNameTxt = m_dialogUI.transform.GetChild(1).GetComponentInChildren<Text>();

            string txtContent = m_txtFile.text.Replace("\r", "").Replace("\n", "");
            string[] m_tmpTxtContent = txtContent.Split('~');

            m_txtAContentNFQ = m_tmpTxtContent[0].Split('#');
            m_txtAContentFQ = m_tmpTxtContent[1].Split('#');
        }        
    }

    #region --- Dialog Manage ---
    protected override void SetDialogText()
    {
        if (!m_extraDialog)
        {
            base.SetDialogText();
        }
        else
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
    }

    protected override void EndDialog()
    {
        base.EndDialog();

        if (m_FQ && m_eQuest == EQuest.DELIVER)
            m_questSystem.UpdateQuestStatus(EQuest.DELIVER, gameObject, 1);

        // When NPC hasn't still given a quest, give quest 
        if (!questGiven)
        {
            switch (m_eQuest)
            {
                case EQuest.FIGHT:
                    if (m_extraPara)
                    {
                        m_questSystem.AddQuest(Quest.CreateFightQuest(m_eQuest, m_eEnemy, gameObject, m_amount, m_gold, m_exp));
                    }
                    else
                    {
                        m_questSystem.AddQuest(Quest.CreateFightQuest(m_eQuest, m_eEnemy, gameObject));
                    }
                    break;
                case EQuest.COLLECT:
                    if (m_extraPara)
                    {
                        m_questSystem.AddQuest(Quest.CreateCollectQuest(m_eQuest, m_eItem, gameObject, m_amount, m_gold, m_exp));
                    }
                    else
                    {
                        m_questSystem.AddQuest(Quest.CreateCollectQuest(m_eQuest, m_eItem, gameObject));
                    }
                    break;
                case EQuest.DELIVER:
                    GameObject tmpReceiver = null;

                    if (m_extraDialog)
                    {
                        tmpReceiver = gameObject;
                    }
                    else
                    {
                        tmpReceiver = m_receiver;
                    }


                    if (m_extraPara)
                    {
                        m_questSystem.AddQuest(Quest.CreateDeliverQuest(m_eQuest, m_eItem, tmpReceiver, m_amount, m_gold, m_exp));
                    }
                    else
                    {
                        m_questSystem.AddQuest(Quest.CreateDeliverQuest(m_eQuest, m_eItem, tmpReceiver));
                    }
                    break;
            }

            questGiven = true;
        }

        if (!m_extraDialog)
        {
            Deactivate();
        }      
    }
    #endregion

#if UNITY_EDITOR

    #region --- Custom Inspector ---
    // Custom Editor using SerializedProperties.
    [CustomEditor(typeof(QuestGiver))]
    public class CostumEditorGUILayout : EditorGUILayoutPropertyField
    {
        SerializedProperty p_eQuest;
        SerializedProperty p_eEnemy;
        SerializedProperty p_eItem;
        SerializedProperty p_receiver;

        SerializedProperty p_extraDialog;

        SerializedProperty p_extraPara;
        SerializedProperty p_amount;
        SerializedProperty p_gold;
        SerializedProperty p_exp;
    
        protected override void OnEnable()
        {
            base.OnEnable();

            p_eQuest = serializedObject.FindProperty("m_eQuest");
            p_eEnemy = serializedObject.FindProperty("m_eEnemy");
            p_eItem = serializedObject.FindProperty("m_eItem");
            p_receiver = serializedObject.FindProperty("m_receiver");

            p_extraDialog = serializedObject.FindProperty("m_extraDialog");

            p_extraPara = serializedObject.FindProperty("m_extraPara");
            p_amount = serializedObject.FindProperty("m_amount");
            p_gold = serializedObject.FindProperty("m_gold");
            p_exp = serializedObject.FindProperty("m_exp");
        }
    
        public override void OnInspectorGUI()
        {
            // serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(p_eQuest);          
    
            switch (p_eQuest.enumValueIndex)
            {
                case (int) EQuest.FIGHT:
                    EditorGUILayout.PropertyField(p_eEnemy);
                    break;
                case (int) EQuest.COLLECT:
                    EditorGUILayout.PropertyField(p_eItem);
                    break;
                case (int) EQuest.DELIVER:
                    EditorGUILayout.PropertyField(p_eItem);
                    EditorGUILayout.PropertyField(p_receiver);
                    break;
                default:
                    return;
            }

            EditorGUILayout.PropertyField(p_extraDialog);
                
            EditorGUILayout.PropertyField(p_extraPara);
    
            if (p_extraPara.boolValue)
            {
                EditorGUILayout.PropertyField(p_amount);
                EditorGUILayout.PropertyField(p_gold);
                EditorGUILayout.PropertyField(p_exp);
            }
    
            // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endregion

#endif
}
