using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : DialogNPC
{
    private QuestSystem m_questSystem;
    
    [Header ("Quest Settings")]
    [SerializeField]
    private EQuest m_eQuest;
    
    [SerializeField]
    private EEnemy m_eEnemy;
    [SerializeField]
    private EItem m_eItem;
    
    [SerializeField]
    private GameObject m_receiver;

    [SerializeField]
    private bool m_extraDialog = false;
    public bool m_fq = false;          // fq = finished quest

    [SerializeField]
    private bool m_extraPara = false;
    [SerializeField]
    private int m_amount;
    [SerializeField]
    private int m_gold;
    [SerializeField]
    private float m_exp;

    private string[] m_txtAContentNFQ;      // NFQ = Not Finished Quest
    private string[] m_txtAContentFQ;       // FQ = finished quest

    public override void Awake()
    {
        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();

        if (m_extraDialog && m_eQuest == EQuest.DELIVER)
            m_extraDialog = false;

        if(!m_extraDialog)
            base.Awake();
        else
        {
            m_interactableE = transform.GetChild(0).gameObject;
            m_dialogTxt = m_dialogUI.GetComponentInChildren<Text>();

            string txtContent = m_txtFile.text.Replace("\r", "").Replace("\n", "");
            string[] m_tmpTxtContent = txtContent.Split('~');

            m_txtAContentNFQ = m_tmpTxtContent[0].Split('#');
            m_txtAContentFQ = m_tmpTxtContent[1].Split('#');
        }        
    }

    public override void SetDialogText()
    {
        if (!m_extraDialog)
        {
            base.SetDialogText();
        }
        else
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
    }

    public override void EndDialog()
    {
        base.EndDialog();

        if (!m_fq)
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
                    if (m_extraPara)
                    {
                        m_questSystem.AddQuest(Quest.CreateDeliverQuest(m_eQuest, m_eItem, m_receiver, m_amount, m_gold, m_exp));
                    }
                    else
                    {
                        m_questSystem.AddQuest(Quest.CreateDeliverQuest(m_eQuest, m_eItem, m_receiver));
                    }
                    break;
            }
        }

        if (!m_extraDialog)
        {
            m_allowDialog = false;
            m_interactable = false;
            m_interactableE.SetActive(false);
        }      
    }

#if UNITY_EDITOR

    #region --- Custom Inspector ---
    // Custom Editor using SerializedProperties.
    [CustomEditor(typeof(QuestGiver))]
    public class EditorGUILayoutPropertyField : Editor
    {
        SerializedProperty p_txtFile;
        SerializedProperty p_playerCamera;
        SerializedProperty p_dialogUI;
    
        SerializedProperty p_eQuest;
        SerializedProperty p_eEnemy;
        SerializedProperty p_eItem;
        SerializedProperty p_receiver;

        SerializedProperty p_extraDialog;

        SerializedProperty p_extraPara;
        SerializedProperty p_amount;
        SerializedProperty p_gold;
        SerializedProperty p_exp;
    
        private void OnEnable()
        {
            p_txtFile = serializedObject.FindProperty("m_txtFile");
            p_playerCamera = serializedObject.FindProperty("m_playerCamera");
            p_dialogUI = serializedObject.FindProperty("m_dialogUI");
    
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
    
            EditorGUILayout.PropertyField(p_txtFile);
            EditorGUILayout.PropertyField(p_playerCamera);
            EditorGUILayout.PropertyField(p_dialogUI);
    
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

            if(!(p_eQuest.enumValueIndex == (int)EQuest.DELIVER))
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
