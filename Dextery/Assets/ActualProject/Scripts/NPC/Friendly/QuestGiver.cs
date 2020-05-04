using UnityEditor;
using UnityEngine;

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
    private bool m_extraPara = false;
    [SerializeField]
    private int m_amount;
    [SerializeField]
    private int m_gold;
    [SerializeField]
    private float m_exp;

    public override void Awake()
    {
        base.Awake();

        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();
    }

    public override void EndDialog()
    {
        base.EndDialog();

        switch (m_eQuest)
        {
            case EQuest.FIGHT:
                if (m_extraPara)
                {
                    m_questSystem.AddQuest(Quest.CreateFightQuest(m_eQuest, m_eEnemy, m_amount, m_gold, m_exp));
                }
                else
                {
                    m_questSystem.AddQuest(Quest.CreateFightQuest(m_eQuest, m_eEnemy));
                }
                break;
            case EQuest.COLLECT:
                if (m_extraPara)
                {
                    m_questSystem.AddQuest(Quest.CreateCollectQuest(m_eQuest, m_eItem, m_amount, m_gold, m_exp));
                }
                else
                {
                    m_questSystem.AddQuest(Quest.CreateCollectQuest(m_eQuest, m_eItem));
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
}
