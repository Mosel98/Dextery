public enum EItems
{
    HEALPOTION,
    MANAPOTION
}

public class Item
{
    public EItems ItemType;
    public int Amount;

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
}
