using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject m_combatInventory;
    [SerializeField]
    private GameObject m_normalInventory;
    [SerializeField]
    private GameObject m_btnItemCombat;
    [SerializeField]
    private GameObject m_btnItemInventory;

    [SerializeField]
    private CombatManager m_combatManager;

    private GameObject m_InventoryScroll;
    private GameObject m_InventoryContent;
    private GameObject m_StatsPanel;

    private Vector2 m_anchorMin = new Vector2(0.05f, 0.9f);
    private Vector2 m_anchorMax = new Vector2(0.95f, 0.95f);

    private Vector2 m_offsetMin = new Vector2(-10.0f, -40.0f);
    private Vector2 m_offsetMax = new Vector2(-10.0f, -10.0f);

    private List<Item> m_itemList = new List<Item>();

    private void Awake()
    {
        GameObject tmp = m_normalInventory.transform.GetChild(1).gameObject;

        m_InventoryScroll = tmp.transform.GetChild(0).gameObject;
        m_StatsPanel = tmp.transform.GetChild(1).gameObject;

        m_InventoryContent = m_InventoryScroll.transform.GetChild(0).GetChild(0).gameObject;
    }

    private void Start()
    {
        AddItem(new Item { ItemType = EItems.HEALPOTION, Amount = 5});
        AddItem(new Item { ItemType = EItems.HEALPOTION, Amount = 5});
        AddItem(new Item { ItemType = EItems.MANAPOTION, Amount = 3 });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            m_normalInventory.SetActive(!m_normalInventory.activeSelf);
    }

    #region --- Manage Items ---
    public void AddItem(Item _item)
    {
        if (_item.IsStackable())
        {
            bool exist = false;

            foreach (Item item in m_itemList)
            {
                if (item.ItemType == _item.ItemType)
                {
                    item.Amount += _item.Amount;
                    exist = true;
                    break;
                }                                   
            }

            if(!exist)
                m_itemList.Add(_item);
        }
        else
        {
            m_itemList.Add(_item);
        }

        UpdateCombatInventory();
        UpdateNormalInventory();
    }

    public void RemoveItem(Item _item)
    {
        foreach (Item item in m_itemList)
        {
            if (item.ItemType == _item.ItemType)
            {
                if (item.IsStackable())
                {
                    item.Amount -= _item.Amount;

                    if(item.Amount == 0)
                        m_itemList.Remove(item);

                    break;
                }
                else
                {
                    m_itemList.Remove(item);
                    break;
                }                  
            }            
        }

        UpdateCombatInventory();
        UpdateNormalInventory();
    }
    #endregion

    #region --- Combat Related ---
    private void UpdateCombatInventory()
    {
        DestroyChildren();

        int count = 0;

        foreach (Item item in m_itemList)
        {
            if(item.ItemType == EItems.HEALPOTION || item.ItemType == EItems.MANAPOTION)
            {
                Vector2 tmpMin = new Vector2(m_anchorMin.x, m_anchorMin.y - (0.05f * count));
                Vector2 tmpMax = new Vector2(m_anchorMax.x, m_anchorMax.y - (0.05f * count));

                GameObject tmpBtn = Instantiate(m_btnItemCombat);
                RectTransform tmpRT = tmpBtn.GetComponent<RectTransform>();
                Button btnClick = tmpBtn.GetComponent<Button>();
                Text btnText = tmpBtn.GetComponentInChildren<Text>();

                tmpBtn.transform.parent = m_combatInventory.transform;
                tmpRT.anchorMin = tmpMin;
                tmpRT.anchorMax = tmpMax;
                tmpRT.offsetMin = new Vector2(0.0f, 0.0f);
                tmpRT.offsetMax = new Vector2(0.0f, 0.0f);

                switch (item.ItemType)
                {
                    case EItems.HEALPOTION:
                        btnText.text = $"Heal Potion x{item.Amount}";
                        btnClick.onClick.AddListener(m_combatManager.Heal);
                        break;
                    case EItems.MANAPOTION:
                        btnText.text = $"Mana Potion x{item.Amount}";
                        btnClick.onClick.AddListener(m_combatManager.Mana);
                        break;
                }          

                count++;
            }
        }
    }
    #endregion

    #region --- Normal Inventory Related ---
    private void UpdateNormalInventory()
    {
        DestroyChildren();

        int count = 0;

        foreach (Item item in m_itemList)
        {
            Vector2 tmpMin = new Vector2(m_offsetMin.x, m_offsetMin.y - (30.0f * count));
            Vector2 tmpMax = new Vector2(m_offsetMax.x, m_offsetMax.y - (30.0f * count));

            GameObject tmpBtn = Instantiate(m_btnItemInventory);
            RectTransform tmpRT = tmpBtn.GetComponent<RectTransform>();
            Button btnClick = tmpBtn.GetComponent<Button>();
            Text btnText = tmpBtn.GetComponentInChildren<Text>();

            tmpBtn.transform.parent = m_InventoryContent.transform;
            tmpRT.offsetMin = tmpMin;
            tmpRT.offsetMax = tmpMax;

            switch (item.ItemType)
            {
                case EItems.HEALPOTION:
                    btnText.text = $"Heal Potion x{item.Amount}";
                    btnClick.onClick.AddListener(m_combatManager.Heal);
                    break;
                case EItems.MANAPOTION:
                    btnText.text = $"Mana Potion x{item.Amount}";
                    btnClick.onClick.AddListener(m_combatManager.Mana);
                    break;
            }

            count++;
        }
    }

    public void ShowStats()
    {
        m_InventoryScroll.SetActive(false);
        m_StatsPanel.SetActive(true);
    }

    public void ShowInventory()
    {
        m_InventoryScroll.SetActive(true);
        m_StatsPanel.SetActive(false);
    }
    #endregion

    // That sounds a little bit wrong xD
    private void DestroyChildren()
    {
        if(m_combatInventory.transform.childCount > 0)
        {
            foreach (Transform child in m_combatInventory.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (m_InventoryContent.transform.childCount > 0)
        {
            foreach (Transform child in m_InventoryContent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
