public enum EItems
{
    HEALPOTION,
    MANAPOTION
}

public class Item
{
    public EItems ItemType;
    public int Amount;
    public int Value;
    public float EffectVal;

    public bool IsStackable()
    {
        switch (ItemType)
        {
            case EItems.HEALPOTION:
                return true;
            case EItems.MANAPOTION:
                return true;
            default:
                return false;
        }
    }

    public static Item CreateItem(EItems _item, int _amount = 1)
    {
        switch (_item)
        {
            case EItems.HEALPOTION:
                return new Item { ItemType = _item, Amount = _amount, Value = 10, EffectVal = 10.0f };
            case EItems.MANAPOTION:
                return new Item { ItemType = _item, Amount = _amount, Value = 10, EffectVal = 10.0f };
        }

        return null;
    }
}
