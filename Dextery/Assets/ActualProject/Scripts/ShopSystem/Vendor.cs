using UnityEngine;

public class Vendor : MonoBehaviour
{
    [SerializeField]
    private ShopSystem m_shopSystem;
    [SerializeField]
    private Transform m_playerCamera;

    private GameObject m_interactableE;

    private bool m_interactable = false;

    public static bool isShoping = false;

    private void Awake()
    {
        m_interactableE = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (!isShoping && m_interactable && Input.GetKeyDown(KeyCode.E))
        {
            isShoping = true;
            m_shopSystem.OpenShop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!m_interactable)
                m_interactable = true;

            if (!m_interactableE.activeSelf)
                m_interactableE.SetActive(true);

            m_interactableE.transform.LookAt(m_playerCamera);           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (m_interactable)
                m_interactable = false;

            if (m_interactableE.activeSelf)
                m_interactableE.SetActive(false);
        }
    }
}
