using UnityEngine;
using UnityEditor;

namespace SimpleMan.EventSystem
{
    public class CustomAssetMenuItems : MonoBehaviour
    {
        private static string PRO_URL = "https://assetstore.unity.com/packages/tools/utilities/event-system-pro-185999";

        [MenuItem("Assets/Create/Global Request/Int", false, 1)]
        internal static void CreateIntOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/Float", false, 1)]
        internal static void CreateFloatOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/Bool", false, 1)]
        internal static void CreateBoolOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/String", false, 1)]
        internal static void CreateStringOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/Transform", false, 1)]
        internal static void CreateTransformOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/Game Object", false, 1)]
        internal static void CreateGameObjectOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/List Of Transforms", false, 1)]
        internal static void CreateTransormOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/List Of Game Objects", false, 1)]
        internal static void CreateGameObjectListOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Request/Custom", false, 1)]
        internal static void CreateCustomOrder()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Int", false, 1)]
        internal static void CreateIntEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Float", false, 1)]
        internal static void CreateFloatEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Bool", false, 1)]
        internal static void CreateBoolEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/String", false, 1)]
        internal static void CreateStringEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Transform", false, 1)]
        internal static void CreateTransformEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Game Object", false, 1)]
        internal static void CreateGameObjectEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/List Of Transforms", false, 1)]
        internal static void CreateTransormEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/List Of Game Objects", false, 1)]
        internal static void CreateGameObjectListEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Global Event/Custom", false, 1)]
        internal static void CreateCustomEvent()
        {
            ShowUpgradeWindow();
        }

        [MenuItem("Assets/Create/Vote/Int", false, 1)]
        [MenuItem("Assets/Create/Vote/Bool", false, 1)]
        [MenuItem("Assets/Create/Vote/String", false, 1)]
        [MenuItem("Assets/Create/Vote/Float", false, 1)]
        [MenuItem("Assets/Create/Vote/Transform", false, 1)]
        [MenuItem("Assets/Create/Vote/Game Object", false, 1)]
        internal static void CreateVoteInt()
        {
            ShowUpgradeWindow();
        }

        private static void ShowUpgradeWindow()
        {
            if (EditorUtility.DisplayDialog("Upgrade Event System to PRO?",
                                          "Events with arguments awailable in " +
                                          "PRO version of Event System. \n\n" +
                                          "Do you want to upgrade Event System to PRO?",
                                          "Yes", "No"))
            {
                Application.OpenURL(PRO_URL);
            }
        }
    }
}