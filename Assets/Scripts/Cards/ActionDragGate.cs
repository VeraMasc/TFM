using CardHouse;


/// <summary>
/// Gestiona el cuando se puede hacer drag a una carta de acción
/// </summary>
public class ActionDragGate : Gate<NoParams>
{
    Card MyCard;

    private void Awake()
    {
        MyCard = GetComponent<Card>();
    }

    protected override bool IsUnlockedInternal(NoParams gateParams)
    {
        return false;
        // if (MyCard.Group != GroupRegistry.Instance.Get(GroupName.Board, null))
        //     return true;

        // return MyCard.GetComponent<Plant>()?.CanBeWatered() != true;
    }
}

