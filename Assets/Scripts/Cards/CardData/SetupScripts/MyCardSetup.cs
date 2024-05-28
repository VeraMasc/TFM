using UnityEngine;
using CardHouse;
using TMPro;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;
using Effect;
using System.Linq;


/// <summary>
/// Es la base de todas las cartas de mi juego
/// </summary>
public abstract class MyCardSetup : CardSetup
{
    /// <summary>
    /// Scriptable object usado para generar esta carta
    /// </summary>
    [ReadOnly]
    public MyCardDefinition definition;

    /// <summary>
    /// Nombre de la carta
    /// </summary>
    public string cardName;

    /// <summary>
    /// Texto de la carta
    /// </summary>
    public string cardText;

    /// <summary>
    /// Reemplaza el texto de la carta de forma temporal
    /// </summary>
    public string tempText;

    /// <summary>
    /// Tipo de la carta
    /// </summary>
    public string cardType;

    /// <summary>
    /// Renderer de la cara frontal de la carta
    /// </summary>
    public SpriteRenderer Image;

    /// <summary>
    /// Renderer del frame de la carta
    /// </summary>
    public SpriteRenderer FrameImage;

    /// <summary>
    /// Renderer de la cara trasera de la carta
    /// </summary>
    public SpriteRenderer BackImage;


    /// <summary>
    /// Textbox with the card name
    /// </summary>
    public TextMeshPro cardNameBox;

    /// <summary>
    /// Textbox with the card type
    /// </summary>
    public TextMeshPro cardTypeLine;

    /// <summary>
    /// Base textbox for the card
    /// </summary>
    public TextMeshPro cardTextBox;

    public BaseCardEffects effects;

    public T getEffectsAs<T>()where T:BaseCardEffects{
        return effects as T;
    }

    /// <summary>
    /// Tipos de la carta en cuestión
    /// </summary>
    public virtual HashSet<string> cardTypeSet {get;set;}

    /// <summary>
    /// Asigna los valores básicos de la carta
    /// </summary>
    /// <param name="data"></param>
    public override void Apply(CardDefinition data)
    {
        if(data is MyCardDefinition baseCard){
            this.cardName = baseCard.cardName;
            gameObject.name = baseCard.cardName;

            if(baseCard.Art != null){
                Image.sprite = baseCard.Art;
            }
            
            if (baseCard.BackArt != null)
            {
                BackImage.sprite = baseCard.BackArt;
            }
            cardText = baseCard.parsedText;
            applyText();
            cardName = baseCard.cardName;
            cardNameBox.text = cardName;

            cardType = baseCard.getCardTypes();
            if(cardTypeLine){
                cardTypeLine.text = cardType;
            }

            if(!GetType().IsSubclassOf(typeof(TriggerCard))){
                // poner definición solo si no es un trigger
                definition = baseCard;
            }
        }else{
            throw new System.Exception($"Can't initialize card with {data?.GetType()?.Name}");
        }
        
    }

    /// <summary>
    /// Aplica el texto a la carta 
    /// </summary>
    public void applyText(){
        if(!string.IsNullOrWhiteSpace(tempText)){
            cardTextBox.text = tempText;
        }
        else{
            cardTextBox.text = cardText;
        }
    }

    /// <summary>
    /// Intenta usar la carta como efecto modal
    /// </summary>
    /// <returns> Si se ha podido usar como modal o no</returns>
    public bool tryCastAsModal(){
        var cardComponent = GetComponent<Card>();
        var modes = effects.getAllAbilities().OfType<UniversalCastAbility>()
            .Cast<ActivatedAbility>();
        var currentZone = cardComponent.Group?.GetComponent<GroupZone>();
        if(currentZone){
            modes = effects.getAllAbilities()
                .OfType<ActivatedAbility>()
                .Where(ab => ab.isActiveIn(currentZone.zone));
        }
        
        if(modes.Any()){
            cardComponent.Group?.ApplyStrategy(); //Devolver a la mano
            //Configuración de cada modo
            var controller = effects?.context?.controller;
            var settings = modes.Select(m => new ModalOptionSettings(){
                    tag = m.id,
                    ability = m,
                    disabled = !m.canActivate(controller),
                    replaceCost = m.cost.value>0? m.cost: null,
                }
            );
            if(this is ActionCard action && currentZone.zone == GroupName.Hand){
                Debug.Log(action.effects.sourceZone);
                //Add default cast mode
                settings = settings.Prepend(new ModalOptionSettings(){
                    tag=string.Empty,
                    disabled = !action.checkIfCastable(controller),
                });
            }
            StartCoroutine(ModalEffect.castModal(cardComponent,settings));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Intenta activar las habilidades de la carta
    /// </summary>
    /// <returns></returns>
    public bool tryActivateAsModal(){
        var cardComponent = GetComponent<Card>();
        

        var currentZone = cardComponent.Group?.GetComponent<GroupZone>();
        //No cast abilities
        var modes = effects.getAllAbilities().OfType<ActivatedAbility>()
            .Where(ability => !(ability is CastAbility));
            
        if(currentZone){
            //Filtrar por zona de actividad
            modes = modes.Where(ab => ab.isActiveIn(currentZone.zone));
            
        }
        
        if(modes.Any()){
            
            //Configuración de cada modo
            var controller = effects?.context?.controller;
            var settings = modes.Select(m => new ModalOptionSettings(){
                    tag = m.id,
                    ability = m,
                    disabled = !m.canActivate(controller),
                }
            );
            StartCoroutine(ModalEffect.castModal(cardComponent,settings));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Obtiene todos los links de una carta en base a sus ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public IEnumerable<TMP_LinkInfo> getTextLinks(IEnumerable<string> ids){

        var textLinks = cardTextBox.textInfo.linkInfo;
        return textLinks
            .Where(link => ids.Contains(link.GetLinkID()));

        
        
        
    }
}