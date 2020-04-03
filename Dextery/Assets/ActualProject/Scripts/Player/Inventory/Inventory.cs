using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject m_combatInventory;
    [SerializeField]
    private GameObject m_btnItem;
    [SerializeField]
    private CombatManager m_combatManager;

    private Vector2 m_anchorMin = new Vector2(0.05f, 0.9f);
    private Vector2 m_anchorMax = new Vector2(0.95f, 0.95f);

    private List<Item> m_itemList = new List<Item>();

    private void Start()
    {
        AddItem(new Item { ItemType = EItems.HEALPOTION, Amount = 5});
        AddItem(new Item { ItemType = EItems.HEALPOTION, Amount = 5});
        AddItem(new Item { ItemType = EItems.MANAPOTION, Amount = 3 });
    }

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
    }

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

                GameObject tmpBtn = Instantiate(m_btnItem);
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
    }
}
