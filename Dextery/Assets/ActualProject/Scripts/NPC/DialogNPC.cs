using UnityEngine;
using UnityEngine.UI;

public class DialogNPC : MonoBehaviour
{
    [SerializeField]
    private string m_txtFile;
    [SerializeField]
    private Transform m_playerCamera;
    [SerializeField]
    private GameObject m_DialogUI;

    private GameObject m_interactableE;

    private Text m_dialogTxt;

    private string[] m_txtAContent;
    private int m_count;

    private bool m_isDialog = false;
    private bool m_interactable = false;

    private void Awake()
    {
        m_interactableE = transform.GetChild(0).gameObject;
        m_dialogTxt = m_DialogUI.GetComponentInChildren<Text>();

        TextAsset txtAsset = (TextAsset)Resources.Load($"NPCDialogs/{m_txtFile}");
        string txtContent = txtAsset.text.Replace("\r", "").Replace("\n", "");

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

        m_DialogUI.SetActive(true);
        m_count = 0;

        SetDialogText();

        m_isDialog = true;
    }

    private void EndDialog()
    {
        m_DialogUI.SetActive(false);
        m_isDialog = false;

        GameManager.isOccupied = false;
    }

    private void SetDialogText()
    {
        if (m_count != (m_txtAContent.Length - 1))
            m_dialogTxt.text = m_txtAContent[m_count];
        else
            EndDialog();

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!m_interactable)
                m_interactable = true;

            if (!m_interactableE.activeSelf)
                m_interactableE.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
