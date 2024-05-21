using System.Text.RegularExpressions;
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
        var isDragLocked = GameUI.singleton?.isBusy ?? false; //Está esperando inputs?
        //Is precalculating?
        isDragLocked =isDragLocked || (CardResolveOperator.singleton?.precalculating ?? false);

        //Is other group focused //TODO: take proxies into account
        isDragLocked = isDragLocked || 
            (GameUI.singleton?.focusGroup != null && MyCard.Group != GameUI.singleton?.focusGroup);

        //No arrastrar cartas en el campo
        var zone = MyCard.Group?.GetComponent<GroupZone>();
        if(zone && zone.zone == GroupName.Board){
            isDragLocked = true;
        }
        
        return !isDragLocked;
        // if (MyCard.Group != GroupRegistry.Instance.Get(GroupName.Board, null))
        //     return true;

        // return MyCard.GetComponent<Plant>()?.CanBeWatered() != true;
    }
}

