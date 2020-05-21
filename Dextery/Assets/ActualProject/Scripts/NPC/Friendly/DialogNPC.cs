using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class DialogNPC : MonoBehaviour
{
    public bool m_FQ { get; protected set; }   // FQ = finished quest (only Child relevant (Questsystem))

    [Header("Dialog Settings")]
    public string m_NameNPC;
    [SerializeField]
    protected TextAsset m_txtFile;
    [SerializeField]
    protected GameObject m_dialogUI;
    [SerializeField]
    private Transform m_playerCamera;

    protected GameObject m_interactableE;
    protected Text m_dialogTxt;
    protected Text m_npcNameTxt;
    protected int m_count;

    protected bool m_interactable = false;
    protected bool m_allowDialog = true;            

    private string[] m_txtAContent;  
    private bool m_isDialog = false;   

    virtual protected void Awake()
    {
        m_FQ = false;

        m_interactableE = transform.GetChild(0).gameObject;
        m_dialogTxt = m_dialogUI.transform.GetChild(0).GetComponentInChildren<Text>();
        m_npcNameTxt = m_dialogUI.transform.GetChild(1).GetComponentInChildren<Text>();

        string txtContent = m_txtFile.text.Replace("\r", "").Replace("\n", "");

        m_txtAContent = txtContent.Split('#');
    }

    private void Update()
    {
        if (!GameManager.isOccupied && m_interactable && Input.GetKeyDown(KeyCode.E))
        {
            StartDialog();
        }

        if (m_isDialog)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_count++;
                SetDialogText();
            }
        }
    }

    #region --- Child Relevant Functions ---
    public void QuestManagement(bool _value)
    {
        m_FQ = _value;
    }

    public void Deactivate()
    {
        m_allowDialog = false;
        m_interactable = false;
        m_interactableE.SetActive(false);
    }
    #endregion

    #region --- Dialog Manage ---
    private void StartDialog()
    {
        GameManager.isOccupied = true;

        m_npcNameTxt.text = m_NameNPC;

        m_dialogUI.SetActive(true);
        m_count = 0;

        SetDialogText();

        m_isDialog = true;
    }

    virtual protected void EndDialog()
    {
        m_dialogUI.SetActive(false);
        m_isDialog = false;

        GameManager.isOccupied = false;
    }

    virtual protected void SetDialogText()
    {
        if (m_count != m_txtAContent.Length)
            m_dialogTxt.text = m_txtAContent[m_count];
        else
            EndDialog();
    }
    #endregion

    #region --- OnTrigger Manage ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && m_allowDialog)
        {
            if (!m_interactable)
                m_interactable = true;

            if (!m_interactableE.activeSelf)
                m_interactableE.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && m_allowDialog)
        {
            m_interactableE.transform.LookAt(m_playerCamera);
            transform.LookAt(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (m_interactable)
                m_interactable = false;

            if (m_interactableE.activeSelf)
                m_interactableE.SetActive(false);
        }
    }
    #endregion


#if UNITY_EDITOR   
    
    #region --- Custom Inspector ---
    // Custom Editor using SerializedProperties.
    [CustomEditor(typeof(DialogNPC))]
    public class EditorGUILayoutPropertyField : Editor
    {
        //protected SerializedProperty p_FQ;

        protected SerializedProperty p_nameNPC;

        protected SerializedProperty p_txtFile;
        protected SerializedProperty p_playerCamera;
        protected SerializedProperty p_dialogUI;

        virtual protected void OnEnable()
        {
            //p_FQ = serializedObject.FindProperty("m_FQ");

            p_nameNPC = serializedObject.FindProperty("m_NameNPC");

            p_txtFile = serializedObject.FindProperty("m_txtFile");
            p_playerCamera = serializedObject.FindProperty("m_playerCamera");
            p_dialogUI = serializedObject.FindProperty("m_dialogUI");
        }

        public override void OnInspectorGUI()
        {
            // serializedObject.Update();

            //EditorGUILayout.PropertyField(p_FQ);

            EditorGUILayout.PropertyField(p_nameNPC);

            EditorGUILayout.PropertyField(p_txtFile);
            EditorGUILayout.PropertyField(p_playerCamera);
            EditorGUILayout.PropertyField(p_dialogUI);

            // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endregion

#endif
}
