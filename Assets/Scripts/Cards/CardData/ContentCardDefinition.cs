using UnityEngine;
using CardHouse;
using CustomInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using System.IO;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Content")]
public class ContentCardDefinition : CardDefinition
{

    [TextArea(10, 100)]
    public string cardText;

    public Sprite Art;

    
   
    public ScriptGraphAsset graphAsset;

    public  bool hasGraph => graphAsset!= null;

    [SerializeField] public StaticsDrawer sDrawer;

    
    static string graphFolder = "Assets/Data/Cards/Effects/";

    [NaughtyAttributes.Button, NaughtyAttributes.HideIf("hasGraph")]
    public void generateGraph(){
        var graphName = graphFolder+name+".asset";
        //Check if graph already exists
        if(File.Exists(graphName))
        {
            throw new System.Exception($"Asset '{graphName}' already exists");
        }
        var graph =new ScriptGraphAsset();
        graphAsset = graph;
        AssetDatabase.CreateAsset(graph,graphName );
    }

    [NaughtyAttributes.Button, NaughtyAttributes.ShowIf("hasGraph")]
    public void editGraph(){
        AssetDatabase.OpenAsset(graphAsset);
    }
    
    [SerializeField]
    public CardEffects effects;
}

