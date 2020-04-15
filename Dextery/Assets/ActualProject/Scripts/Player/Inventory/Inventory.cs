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

    private PlayerAttributes m_playAttributes;

    private GameObject m_inventoryScroll;
    private GameObject m_inventoryContent;
    private GameObject m_statsPanel;

    private Vector2 m_anchorMin = new Vector2(0.05f, 0.9f);
    private Vector2 m_anchorMax = new Vector2(0.95f, 0.95f);

    private Vector2 m_offsetMin = new Vector2(-10.0f, -40.0f);
    private Vector2 m_offsetMax = new Vector2(-10.0f, -10.0f);

    private List<Item> m_itemList = new List<Item>();

    private void Awake()
    {
        m_playAttributes = GetComponent<PlayerAttributes>();

        GameObject tmp = m_normalInventory.transform.GetChild(1).gameObject;

        m_inventoryScroll = tmp.transform.GetChild(0).gameObject;
        m_statsPanel = tmp.transform.GetChild(1).gameObject;

        m_inventoryContent = m_inventoryScroll.transform.GetChild(0).GetChild(0).gameObject;
    }

    private void Start()
    {
        AddItem(Item.CreateItem(EItems.HEALPOTION, 8));
        AddItem(Item.CreateItem(EItems.MANAPOTION, 3));
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
        DestroyChildren(0);

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

                tmpBtn.transform.SetParent(m_combatInventory.transform);
                tmpRT.anchorMin = tmpMin;
                tmpRT.anchorMax = tmpMax;
                tmpRT.offsetMin = new Vector2(0.0f, 0.0f);
                tmpRT.offsetMax = new Vector2(0.0f, 0.0f);

                switch (item.ItemType)
                {
                    case EItems.HEALPOTION:
                        btnText.text = $"Heal Potion x{item.Amount}";
                        btnClick.onClick.AddListener(delegate { m_combatManager.Heal(item.EffectVal); });
                        break;
                    case EItems.MANAPOTION:
                        btnText.text = $"Mana Potion x{item.Amount}";
                        btnClick.onClick.AddListener(delegate { m_combatManager.Mana(item.EffectVal); });
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
        DestroyChildren(1);

        int count = 0;

        foreach (Item item in m_itemList)
        {
            Vector2 tmpMin = new Vector2(m_offsetMin.x, m_offsetMin.y - (30.0f * count));
            Vector2 tmpMax = new Vector2(m_offsetMax.x, m_offsetMax.y - (30.0f * count));

            GameObject tmpBtn = Instantiate(m_btnItemInventory);
            RectTransform tmpRT = tmpBtn.GetComponent<RectTransform>();
            Button btnClick = tmpBtn.GetComponent<Button>();
            Text btnText = tmpBtn.GetComponentInChildren<Text>();

            tmpBtn.transform.SetParent(m_inventoryContent.transform);
            tmpRT.offsetMin = tmpMin;
            tmpRT.offsetMax = tmpMax;

            switch (item.ItemType)
            {
                case EItems.HEALPOTION:
                    btnText.text = $"Heal Potion x{item.Amount}";
                    btnClick.onClick.AddListener(delegate { m_playAttributes.AddHealth(item.EffectVal); });
                    break;
                case EItems.MANAPOTION:
                    btnText.text = $"Mana Potion x{item.Amount}";
                    btnClick.onClick.AddListener(delegate { m_playAttributes.AddMana(item.EffectVal); });
                    break;
            }

            count++;
        }
    }

    public void ShowStats()
    {
        m_inventoryScroll.SetActive(false);
        m_statsPanel.SetActive(true);
    }

    public void ShowInventory()
    {
        m_inventoryScroll.SetActive(true);
        m_statsPanel.SetActive(false);
    }
    #endregion

    public List<Item> GetItemList()
    {
        return m_itemList;
    }

    public void UpdateAllInventories()
    {
        UpdateNormalInventory();
        UpdateCombatInventory();
    }

    // That sounds a little bit wrong xD
    private void DestroyChildren(int _id)
    {
        switch (_id)
        {
            case 0:
                if (m_combatInventory.transform.childCount > 0)
                {
                    foreach (Transform child in m_combatInventory.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                break;
            case 1:
                if (m_inventoryContent.transform.childCount > 0)
                {
                    foreach (Transform child in m_inventoryContent.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                break;
        }
    }
}
