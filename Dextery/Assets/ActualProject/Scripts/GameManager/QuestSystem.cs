using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    private List<Quest> m_questList = new List<Quest>();

    [SerializeField]
    private GameObject m_txtQuest;
    [SerializeField]
    private Transform m_questContent;

    private Vector2 m_anchorMin = new Vector2(0.05f, 0.9f);
    private Vector2 m_anchorMax = new Vector2(0.95f, 0.95f);

    private Vector2 m_offsetMin = new Vector2(-10.0f, -40.0f);
    private Vector2 m_offsetMax = new Vector2(-10.0f, -10.0f);

    public void AddQuest(Quest _quest)
    {
        m_questList.Add(_quest);
        UpdateQuestLog();
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
   
   // That sounds a little bit wrong xD
   private void DestroyChildren()
   {
        foreach (Transform child in m_questContent)
        {
            Destroy(child.gameObject);
        }
    }
}
