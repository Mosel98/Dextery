using UnityEngine;

public enum EVendor
{
    EVERYTHING,
    PHARMACY,
    ARMOR
}

// Script by Mario Luetzenkirchen
public class Vendor : MonoBehaviour
{
    [SerializeField]
    private EVendor m_vendorType;
    [SerializeField]
    private ShopSystem m_shopSystem;
    [SerializeField]
    private Transform m_playerCamera;

    private GameObject m_interactableE;

    private bool m_interactable = false;

    private void Awake()
    {
        m_interactableE = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (!GameManager.isOccupied && m_interactable && Input.GetKeyDown(KeyCode.E))
        {
            m_shopSystem.OpenShop(m_vendorType);
        }
    }

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
