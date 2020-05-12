using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    private static List<Quest> m_questList = new List<Quest>();

    [SerializeField]
    private GameObject m_txtQuest;
    [SerializeField]
    private Transform m_questContent;

    private PlayerAttributes m_playAttributes;
    private Inventory m_inventory;

    private Vector2 m_anchorMin = new Vector2(0.05f, 0.9f);
    private Vector2 m_anchorMax = new Vector2(0.95f, 0.95f);

    private Vector2 m_offsetMin = new Vector2(-10.0f, -40.0f);
    private Vector2 m_offsetMax = new Vector2(-10.0f, -10.0f);

    private void Awake()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("Player");

        m_playAttributes = tmp.GetComponent<PlayerAttributes>();
        m_inventory = tmp.GetComponent<Inventory>();
    }

    private void Start()
    {
        if (m_questList.Count > 0)
            UpdateQuestLog();
    }

    #region --- Manage Quest ---
    public void AddQuest(Quest _quest)
    {
        m_questList.Add(_quest);
        UpdateQuestLog();

        UpdateQuestStatus(EQuest.COLLECT);
        // UpdateQuestStatus(EQuest.DELIVER);
    }

   private void UpdateQuestLog()
   {
       DestroyChildren();
   
       int count = 0;
   
       foreach (Quest quest in m_questList)
       {
           Vector2 tmpMin = new Vector2(m_offsetMin.x, m_offsetMin.y - (30.0f * count));
           Vector2 tmpMax = new Vector2(m_offsetMax.x, m_offsetMax.y - (30.0f * count));
   
           GameObject tmpTxt = Instantiate(m_txtQuest);
           RectTransform tmpRT = tmpTxt.GetComponent<RectTransform>();
           Text questText = tmpTxt.GetComponentInChildren<Text>();
   
           tmpTxt.transform.SetParent(m_questContent);
           tmpRT.offsetMin = tmpMin;
           tmpRT.offsetMax = tmpMax;

           questText.text = quest.m_Dialog; 

           count++;
       }
   }

    public void UpdateQuestStatus(EQuest _quest, GameObject _receiver = null, int _id = -1)
    {
        List<Quest> removeQuestList = new List<Quest>();
        List<Item> m_playInventory = m_inventory.GetItemList();

        foreach (Quest quest in m_questList)
        {
            if (quest.m_QuestType == _quest)
            {
                switch (_quest)
                {
                    case EQuest.FIGHT:
                        if (quest.m_eEnemy == (EEnemy)_id)
                        {
                            quest.m_Amount--;

                            if (quest.m_Amount <= 0)
                            {
                                m_playAttributes.SetEarnGold(quest.m_Gold);
                                m_playAttributes.SetEarnExp(quest.m_Exp);

                                removeQuestList.Add(quest);
                            }
                        }
                        break;
                    case EQuest.COLLECT:
                        foreach (Item item in m_playInventory)
                        {
                            if(quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                            {
                                m_playAttributes.SetEarnGold(quest.m_Gold);
                                m_playAttributes.SetEarnExp(quest.m_Exp);

                                removeQuestList.Add(quest);
                            }
                        }
                        break;
                    case EQuest.DELIVER:
                        switch (_id)
                        {
                            case -1:
                                foreach (Item item in m_playInventory)
                                {
                                    if (quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                                    {
                                        quest.m_Receiver.GetComponent<Receiver>().m_fq = true;
                                        break;
                                    }
                                    else
                                    {
                                        if(quest.m_Receiver != null)
                                            quest.m_Receiver.GetComponent<Receiver>().m_fq = false;
                                    }
                                }
                                break;
                            case 0:
                                if (quest.m_Receiver == _receiver)
                                {
                                    foreach (Item item in m_playInventory)
                                    {
                                        if (quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                                        {
                                            item.Amount -= quest.m_Amount;
                                            m_inventory.UpdateItemList(m_playInventory);

                                            m_playAttributes.SetEarnGold(quest.m_Gold);
                                            m_playAttributes.SetEarnExp(quest.m_Exp);

                                            removeQuestList.Add(quest);

                                            Receiver tmp = _receiver.GetComponent<Receiver>();
                                            tmp.m_allowDialog = false;
                                            tmp.m_interactable = false;
                                            tmp.DeactivateE();

                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
        }

        bool change = false;

        foreach (Quest quest in removeQuestList)
        {
            m_questList.Remove(quest);
            change = true;
        }

        if(change)
            UpdateQuestLog();
    }
    #endregion

    public List<Quest> GetQuestList()
    {
        return m_questList;
    }

    // That sounds a little bit wrong xD
    private void DestroyChildren()
    {
        foreach (Transform child in m_questContent)
        {
            Destroy(child.gameObject);
        }
    }
}
