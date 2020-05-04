﻿using UnityEngine;

public enum EQuest
{
    FIGHT,
    COLLECT,
    DELIVER
}

public class Quest
{
    public EQuest m_QuestType;

    public EEnemy m_eEnemy;
    public EItem m_eItem;

    public int m_Amount;
    public int m_Gold;
    public float m_Exp;
    public string m_Dialog;

    public GameObject m_Reciever;

    #region --- Create Quest Functions ---
    public static Quest CreateFightQuest(EQuest _quest, EEnemy _enemy, int _amount = 1, int _gold = 10, float _exp = 10.0f)
    {
        string tmpDialog;
        string tmpEnemy = null;

        switch (_enemy)
        {
            case EEnemy.CASUAL:
                tmpEnemy = "Furry Fuzz";              
                break;
            case EEnemy.WRATH:
                tmpEnemy = "Zorn";
                break;
            default:
                tmpEnemy = "Error: Unknown Item";
                break;
        }

        tmpDialog = $"Defeat {_amount} of {tmpEnemy}";

        return new Quest { m_QuestType = _quest, m_eEnemy = _enemy, m_Amount = _amount, m_Gold = _gold, m_Exp = _exp, m_Dialog = tmpDialog };
    }

    public static Quest CreateCollectQuest(EQuest _quest, EItem _item, int _amount = 1, int _gold = 10, float _exp = 10.0f)
    {
        string tmpDialog;

        tmpDialog = $"Collect {_amount} of {GetItemName(_item)}";

        return new Quest { m_QuestType = _quest, m_eItem = _item, m_Amount = _amount, m_Gold = _gold, m_Exp = _exp, m_Dialog = tmpDialog };
    }

    public static Quest CreateDeliverQuest(EQuest _quest, EItem _item, GameObject _reciever, int _amount = 1, int _gold = 10, float _exp = 10.0f)
    {
        string tmpDialog;

        tmpDialog = $"Deliver {_amount} of {GetItemName(_item)} to {_reciever}";

        return new Quest { m_QuestType = _quest, m_eItem = _item, m_Reciever = _reciever, m_Amount = _amount, m_Gold = _gold, m_Exp = _exp, m_Dialog = tmpDialog };
    }
    #endregion

    private static string GetItemName(EItem _item)
    {
        switch (_item)
        {
            case EItem.HEALPOTION:
                return "Heal Potion";
                
            case EItem.MANAPOTION:
                return "Mana Potion";
            default:
                return "Error: Unknown Item";
        }
    }
}