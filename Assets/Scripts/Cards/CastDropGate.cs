using CardHouse;


/// <summary>
/// Gestiona el cuando se puede dejar una carta en el stack
/// </summary>
public class CastDropGate : Gate<DropParams>
{
    Card MyCard;

    private void Awake()
    {
        MyCard = GetComponent<Card>();
    }

    protected override bool IsUnlockedInternal(DropParams gateParams)
    {
        if(gateParams?.Target?.GetComponent<GroupZone>()?.zone != GroupName.Stack)
            return true; //limitar solo drops al stack
        
        var isDragLocked = GameUI.singleton?.isBusy ?? false; //Está esperando inputs?
        //Is precalculating?
        isDragLocked =isDragLocked || (CardResolveOperator.singleton?.precalculating ?? false);


        if(MyCard.data is ActionCard action && action.speedType!= SpeedTypes.Reaction){
            var context = action.effects.context;
            var sourceZone = gateParams.Source?.GetComponent<GroupZone>()?.zone;
            //Comprobar si se puede castear
            var usable =  sourceZone == GroupName.Hand && action.checkIfCastable(context.controller);
            usable= usable || action.checkActivationTiming(context.controller, sourceZone);
            isDragLocked = isDragLocked || !usable;
        }
        
        return !isDragLocked;
        // if (MyCard.Group != GroupRegistry.Instance.Get(GroupName.Board, null))
        //     return true;

        // return MyCard.GetComponent<Plant>()?.CanBeWatered() != true;
    }
}

