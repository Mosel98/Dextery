public enum EItem
{
    HEALPOTION,
    MANAPOTION,
    NULL
}

public class Item
{
    public EItem ItemType;
    public int Amount;
    public int Value;
    public float EffectVal;

    public bool IsStackable()
    {
        switch (ItemType)
        {
            case EItem.HEALPOTION:
                return true;
            case EItem.MANAPOTION:
                return true;
            default:
                return false;
        }
    }

    public static Item CreateItem(EItem _item, int _amount = 1)
    {
        switch (_item)
        {
            case EItem.HEALPOTION:
                return new Item { ItemType = _item, Amount = _amount, Value = 10, EffectVal = 10.0f };
            case EItem.MANAPOTION:
                return new Item { ItemType = _item, Amount = _amount, Value = 10, EffectVal = 10.0f };
        }

        return null;
    }
}
