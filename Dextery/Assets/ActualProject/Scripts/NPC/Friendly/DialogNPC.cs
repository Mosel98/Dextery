using UnityEngine;
using UnityEngine.UI;

public class DialogNPC : MonoBehaviour
{
    [Header ("Dialog Settings")]
    [SerializeField]
    private TextAsset m_txtFile;
    [SerializeField]
    private Transform m_playerCamera;
    [SerializeField]
    private GameObject m_dialogUI;

    public GameObject m_interactableE;

    public bool m_interactable = false;
    public bool m_allowDialog = true;

    protected Text m_dialogTxt;

    protected string[] m_txtAContent;
    protected int m_count;

    private bool m_isDialog = false;
    

    virtual public void Awake()
    {
        m_interactableE = transform.GetChild(0).gameObject;
        m_dialogTxt = m_dialogUI.GetComponentInChildren<Text>();

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

    #region --- Dialog Manage ---
    private void StartDialog()
    {
        GameManager.isOccupied = true;

        m_dialogUI.SetActive(true);
        m_count = 0;

        SetDialogText();

        m_isDialog = true;
    }

    virtual public void EndDialog()
    {
        m_dialogUI.SetActive(false);
        m_isDialog = false;

        GameManager.isOccupied = false;
    }

    virtual public void SetDialogText()
    {
        if (m_count != (m_txtAContent.Length - 1))
            m_dialogTxt.text = m_txtAContent[m_count];
        else
            EndDialog();
    }
    #endregion

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
}
