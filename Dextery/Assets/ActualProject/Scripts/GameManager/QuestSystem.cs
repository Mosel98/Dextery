using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
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
        UpdateQuestStatus(EQuest.DELIVER);
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

    public void UpdateQuestStatus(EQuest _quest, GameObject _receiver = null, int _id = 0)
    {
        List<Quest> removeQuestList = new List<Quest>();
        List<Item> playInventory = m_inventory.GetItemList();
        bool breakNow = false;

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

                                if (quest.m_Receiver != null)
                                    quest.m_Receiver.GetComponent<DialogNPC>().QuestManagement(true);
                            }
                        }
                        break;
                    case EQuest.COLLECT:
                        foreach (Item item in playInventory)
                        {
                            if(quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                            {
                                m_playAttributes.SetEarnGold(quest.m_Gold);
                                m_playAttributes.SetEarnExp(quest.m_Exp);

                                removeQuestList.Add(quest);

                                if (quest.m_Receiver != null)
                                    quest.m_Receiver.GetComponent<DialogNPC>().QuestManagement(true);
                            }
                        }
                        break;
                    case EQuest.DELIVER:
                        switch (_id)
                        {
                            case 0:
                                if (quest.m_Receiver != null)
                                {
                                    foreach (Item item in playInventory)
                                    {
                                        if (quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                                        {
                                            quest.m_Receiver.GetComponent<DialogNPC>().QuestManagement(true);                                           
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (quest.m_Receiver == _receiver)
                                {
                                    foreach (Item item in playInventory)
                                    {
                                        if (quest.m_eItem == item.ItemType && item.Amount >= quest.m_Amount)
                                        {                                          
                                            item.Amount -= quest.m_Amount;

                                            CheckIfQuestIsStillFinished(quest.m_eItem, playInventory, _receiver);

                                            if (item.Amount <= 0)
                                                m_inventory.RemoveItem(item);
                                            else
                                                m_inventory.UpdateItemList(playInventory);

                                            m_playAttributes.SetEarnGold(quest.m_Gold);
                                            m_playAttributes.SetEarnExp(quest.m_Exp);

                                            removeQuestList.Add(quest);

                                            DialogNPC tmp = _receiver.GetComponent<DialogNPC>();
                                            tmp.Deactivate();
                                            
                                            breakNow = true;

                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                }

                if (breakNow)
                    break;
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

    private void CheckIfQuestIsStillFinished(EItem _item, List<Item> _playInventory, GameObject _receiver)
    {
        foreach (Quest quest in m_questList)
        {
            if (quest.m_eItem == _item && quest.m_Receiver != null && quest.m_Receiver != _receiver)
            {
                foreach (Item item in _playInventory)
                {
                    if (quest.m_Receiver.GetComponent<DialogNPC>().m_FQ && quest.m_Amount > item.Amount)
                    {
                        quest.m_Receiver.GetComponent<DialogNPC>().QuestManagement(false);
                        break;
                    }                        
                }
            }
        }
    }

    public static void ClearQuestList()
    {
        m_questList.Clear();
    }
    #endregion

    // That sounds a little bit wrong xD
    private void DestroyChildren()
    {
        foreach (Transform child in m_questContent)
        {
            Destroy(child.gameObject);
        }
    }
}
