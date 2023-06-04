using Configs;
using UnityEngine;

namespace Backend
{
    [CreateAssetMenu(fileName = "InGameStrings", menuName = "Scriptables/Config/InGameStrings")]
    public class InGameStrings : ConfigBase
    {
        public const string inGameStringsString = "inGameStrings";
        [field: SerializeField] public MainMenuUIStrings mainMenuUIStrings { get; private set; }

        public override void Initialize()
        {

        }

        public class DIStrings
        {
            public const string onWinEvent = "onWinEvent";
            public const string onLoseEvent = "onLoseEvent";
            public const string onNoadsBought = "onNoadsBoughtEvent";

            public const string noAdsManager = "noAdsManager";
            public const string inAppPurchacesManager = "inAppPurchacesManager";

            public const string gameConfigs = "gameConfigs";

            public const string listOfAllPictures = "listOfAllPictures";
            public const string listOfAllScenes = "listOfAllScenes";

            public const string inputHolder = "inputHolder";
        }

        public class PurchaseStrings
        {
            public const string noAdsPurchaseString = "noAds";
        }

        public class SaveStrings
        {
            public const string enviromentSoundsVolume = "enviromentSoundsVolume";
            public const string musicVolume = "musicVolume";
            public const string areVibrationsEnabled = "areVibrationsEnabled";
        }

        public class MainMenuUIStrings
        {
            public string playButton = "PlayButton";
            public string settingsButton = "SettingsButton";
            public string noAdsButton = "NoAdsButton";
            public string quitButton = "QuitButton";
        }
    }
}