using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    private QuestSystem m_questSystem;
    private Inventory m_inventory;
    private PlayerAttributes m_playAttributes;

    private List<Quest> m_QuestList;
    private List<Item> m_Inventory;
    private float[] m_Stats = new float[10];

    public Save()
    {
        m_questSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestSystem>();

        GameObject tmpPlay = GameObject.FindGameObjectWithTag("Player");

        m_inventory = tmpPlay.GetComponent<Inventory>();
        m_playAttributes = tmpPlay.GetComponent<PlayerAttributes>();
    }

    public List<Quest> GetQuestList()
    {
        m_QuestList = m_questSystem.GetQuestList();

        return m_QuestList;
    }

    public List<Item> GetInventory()
    {
        m_Inventory = m_inventory.GetItemList();

        return m_Inventory;
    }

    public float[] GetStats()
    {
        m_Stats = m_playAttributes.GetStats();

        return m_Stats;
    }
}
