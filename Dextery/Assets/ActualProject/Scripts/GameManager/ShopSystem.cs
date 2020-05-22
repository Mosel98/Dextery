using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class ShopSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject m_btnItemInventory;
    [SerializeField]
    private GameObject m_shopUI;

    private GameObject m_shopInventoryScroll;
    private GameObject m_shopInventoryContent;

    private GameObject m_sellInventoryScroll;
    private GameObject m_sellInventoryContent;

    private Inventory m_playerInventory;
    private PlayerAttributes m_playerAttributes;

    private Text m_txtGold;

    private Vector2 m_offsetMin = new Vector2(-10.0f, -40.0f);
    private Vector2 m_offsetMax = new Vector2(-10.0f, -10.0f);

    private List<Item> m_playerItems;
    private List<Item> m_sellItems; 

    private List<Item> m_shopItems = new List<Item>();

    private int m_playerGold;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        m_playerInventory = player.GetComponent<Inventory>();
        m_playerAttributes = player.GetComponent<PlayerAttributes>();

        m_playerItems = m_playerInventory.GetItemList();

        GameObject tmp = m_shopUI.transform.GetChild(1).gameObject;

        m_shopInventoryScroll = tmp.transform.GetChild(0).gameObject;
        m_shopInventoryContent = m_shopInventoryScroll.transform.GetChild(0).GetChild(0).gameObject;

        m_sellInventoryScroll = tmp.transform.GetChild(1).gameObject;
        m_sellInventoryContent = m_sellInventoryScroll.transform.GetChild(0).GetChild(0).gameObject;

        m_txtGold = m_shopUI.transform.GetChild(4).gameObject.GetComponent<Text>();
    }

    #region --- Manage Shop ---
    public void OpenShop(EVendor _eVendor)
    {
        GameManager.isOccupied = true;

        m_sellItems = m_playerItems.ConvertAll(Item => new Item() { ItemType = Item.ItemType, Amount = Item.Amount, Value = Item.Value, EffectVal = Item.EffectVal});
        SetUpVendorSupply(_eVendor);

        m_playerGold = PlayerAttributes.m_Gold;
        ManageGold();

        m_shopUI.SetActive(true);
    }

    private void SetUpVendorSupply(EVendor _eVendor)
    {
        m_shopItems.Clear();

        switch (_eVendor)
        {
            case EVendor.EVERYTHING:
                AddItem(Item.CreateItem(EItem.HEALPOTION, 10), m_shopItems);
                AddItem(Item.CreateItem(EItem.MANAPOTION, 10), m_shopItems);
                break;
            case EVendor.PHARMACY:
                AddItem(Item.CreateItem(EItem.HEALPOTION, 20), m_shopItems);
                AddItem(Item.CreateItem(EItem.MANAPOTION, 20), m_shopItems);
                break;
            case EVendor.ARMOR:
                AddItem(Item.CreateItem(EItem.HEALPOTION, 5), m_shopItems);
                AddItem(Item.CreateItem(EItem.MANAPOTION, 5), m_shopItems);
                break;
        }
    }
    #endregion

    #region --- Manage Items/Money ---
    private void ManageGold(int _id = 2, int _reGold = 0)
    {
        switch (_id)
        {
            case 0:
                m_playerGold += _reGold;
                break;
            case 1:
                m_playerGold -= _reGold;
                break;
            default:
                break;
        }

        m_txtGold.text = $"Gold: {m_playerGold}G";
    }

    public void AddItem(Item _item, List<Item> _itemList)
    {
        if (_item.IsStackable())
        {
            bool exist = false;

            foreach (Item item in _itemList)
            {
                if (item.ItemType == _item.ItemType)
                {
                    item.Amount += _item.Amount;
                    exist = true;
                    break;
                }
            }

            if (!exist)
                _itemList.Add(_item);
        }
        else
        {
            _itemList.Add(_item);
        }

        UpdateShopInventory();
        UpdateSellInventory();
    }

    public void RemoveItem(Item _item, List<Item> _itemList)
    {
        foreach (Item item in _itemList)
        {
            if (item.ItemType == _item.ItemType)
            {
                if (item.IsStackable())
                {
                    item.Amount -= _item.Amount;

                    if (item.Amount == 0)
                        _itemList.Remove(item);

                    break;
                }
                else
                {
                    _itemList.Remove(item);
                    break;
                }
            }
        }

        UpdateShopInventory();
        UpdateSellInventory();
    }

    // Check if the next action is a add/remove from player/shop
    public void ManageItem(Item _item, List<Item> _removeList, List<Item> _addList)
    {
        bool tmp = true;

        if (_removeList == m_sellItems)
        {
            ManageGold(0, _item.Value);
        }

        if (_removeList == m_shopItems && (m_playerGold - _item.Value) >= 0)
        {
            ManageGold(1, _item.Value);
        }
        else if (_removeList == m_shopItems && (m_playerGold - _item.Value) < 0)
        {
            tmp = false;
        }

        if (tmp)
        {
            RemoveItem(_item, _removeList);
            AddItem(_item, _addList);
        }
    }
    #endregion

    #region --- Update Inventories ---
    private void UpdateShopInventory()
    {
        DestroyChildren(0);

        int count = 0;

        foreach (Item item in m_shopItems)
        {
            Vector2 tmpMin = new Vector2(m_offsetMin.x, m_offsetMin.y - (30.0f * count));
            Vector2 tmpMax = new Vector2(m_offsetMax.x, m_offsetMax.y - (30.0f * count));

            GameObject tmpBtn = Instantiate(m_btnItemInventory);
            RectTransform tmpRT = tmpBtn.GetComponent<RectTransform>();
            Button btnClick = tmpBtn.GetComponent<Button>();
            Text btnText = tmpBtn.GetComponentInChildren<Text>();

            tmpBtn.transform.SetParent(m_shopInventoryContent.transform);
            tmpRT.offsetMin = tmpMin;
            tmpRT.offsetMax = tmpMax;

            switch (item.ItemType)
            {
                case EItem.HEALPOTION:
                    btnText.text = $"Heal Potion x{item.Amount} | {item.Value}G";
                    btnClick.onClick.AddListener(delegate { ManageItem(Item.CreateItem(EItem.HEALPOTION), m_shopItems, m_sellItems); });
                    break;
                case EItem.MANAPOTION:
                    btnText.text = $"Mana Potion x{item.Amount} | {item.Value}G";
                    btnClick.onClick.AddListener(delegate { ManageItem(Item.CreateItem(EItem.MANAPOTION), m_shopItems, m_sellItems); });
                    break;
            }

            count++;
        }
    }

    private void UpdateSellInventory()
    {
        DestroyChildren(1);

        int count = 0;

        foreach (Item item in m_sellItems)
        {
            Vector2 tmpMin = new Vector2(m_offsetMin.x, m_offsetMin.y - (30.0f * count));
            Vector2 tmpMax = new Vector2(m_offsetMax.x, m_offsetMax.y - (30.0f * count));

            GameObject tmpBtn = Instantiate(m_btnItemInventory);
            RectTransform tmpRT = tmpBtn.GetComponent<RectTransform>();
            Button btnClick = tmpBtn.GetComponent<Button>();
            Text btnText = tmpBtn.GetComponentInChildren<Text>();

            tmpBtn.transform.SetParent(m_sellInventoryContent.transform);
            tmpRT.offsetMin = tmpMin;
            tmpRT.offsetMax = tmpMax;

            switch (item.ItemType)
            {
                case EItem.HEALPOTION:
                    btnText.text = $"Heal Potion x{item.Amount} | {item.Value}G";
                    btnClick.onClick.AddListener(delegate { ManageItem(Item.CreateItem(EItem.HEALPOTION), m_sellItems, m_shopItems); });
                    break;
                case EItem.MANAPOTION:
                    btnText.text = $"Mana Potion x{item.Amount} | {item.Value}G";
                    btnClick.onClick.AddListener(delegate { ManageItem(Item.CreateItem(EItem.MANAPOTION), m_sellItems, m_shopItems); });
                    break;
            }

            count++;
        }
    }
    #endregion

    #region --- UI Btn ---
    public void AcceptTrade()
    {
        m_playerItems = m_sellItems.ConvertAll(Item => new Item() { ItemType = Item.ItemType, Amount = Item.Amount, Value = Item.Value, EffectVal = Item.EffectVal });
        m_playerInventory.UpdateItemList(m_playerItems);

        m_playerAttributes.SetGold(m_playerGold);

        m_shopUI.SetActive(false);
        GameManager.isOccupied = false;
    }

    public void CancelTrade()
    {
        m_shopUI.SetActive(false);
        GameManager.isOccupied = false;
    }
    #endregion

    #region --- Inventories Cleaning ---
    // That sounds a little bit wrong xD
    private void DestroyChildren(int _id)
    {
        switch (_id)
        {
            case 0:
                if (m_shopInventoryContent.transform.childCount > 0)
                {
                    foreach (Transform child in m_shopInventoryContent.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                break;
            case 1:
                if (m_sellInventoryContent.transform.childCount > 0)
                {
                    foreach (Transform child in m_sellInventoryContent.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                break;
        }
    }
    #endregion
}
