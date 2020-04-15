using UnityEngine;

public class Vendor : MonoBehaviour
{
    [SerializeField]
    private ShopSystem m_shopSystem;

    public static bool isShoping = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isShoping)
        {
            if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
            {
                isShoping = true;
                m_shopSystem.OpenShop();
            }
        }
    }
}
