using Dz.SceneManagement;
using UnityEngine;

public partial class GameplaySubscenes : MonoBehaviour {
    #region Variables
    #region Property
    //Exploration & General
    public static Subscene TopExplorationUiHud => instance.topExplorationUiHud;
    public static Subscene RunDeckViewer => instance.runDeckViewer;
    public static Subscene CardViewer => instance.cardViewer;
    public static Subscene Map => instance.map;

    //Starting
    public static Subscene StartingEvent => instance.startingEvent;

    //Combat
    public static Subscene Combat => instance.combat;
    public static Subscene CombatCardSelector => instance.combatCardSelector;
    public static Subscene DrawPileViewer => instance.drawPileViewer;
    public static Subscene DiscardPileViewer => instance.discardPileViewer;
    public static Subscene ExhaustPileViewer => instance.exhaustPileViewer;
    public static Subscene CombatRewards => instance.combatRewards;
    public static Subscene ChooseCardsReward => instance.chooseCardsReward;

    //Shop
    public static Subscene Shop => instance.shop;
    public static Subscene BuyItems => instance.buyItems;
    public static Subscene RemoveCard => instance.removeCard;

    //Campfire
    public static Subscene Campfire => instance.campfire;
    public static Subscene UpgradeCard => instance.upgradeCard;

    //Treasure
    public static Subscene Treasure => instance.treasure;
    // public static Subscene ChooseTreasure => instance.chooseTreasure;

    //Temporary Other Events
    public static Subscene TemporaryOtherEvents => instance.temporaryOtherEvents;
    #endregion

    #region Field
    //Exploration & General
    [SerializeField] Subscene topExplorationUiHud;
    [SerializeField] Subscene runDeckViewer;
    [SerializeField] Subscene cardViewer;
    [SerializeField] Subscene map;

    //Opening
    [SerializeField] Subscene startingEvent;

    //Combat
    [SerializeField] Subscene combat;
    [SerializeField] Subscene combatCardSelector;
    [SerializeField] Subscene drawPileViewer;
    [SerializeField] Subscene discardPileViewer;
    [SerializeField] Subscene exhaustPileViewer;
    [SerializeField] Subscene combatRewards;
    [SerializeField] Subscene chooseCardsReward;

    //Shop
    [SerializeField] Subscene shop;
    [SerializeField] Subscene buyItems;
    [SerializeField] Subscene removeCard;

    //Campfire
    [SerializeField] Subscene campfire;
    [SerializeField] Subscene upgradeCard;

    //Treasure
    [SerializeField] Subscene treasure;
    // [SerializeField] Subscene chooseTreasure;

    //Temporary Other Events
    [SerializeField] Subscene temporaryOtherEvents;

    //Others
    static GameplaySubscenes instance;
    #endregion
    #endregion
    #region Methods
    #region Godot
    public void Start() {
        instance = this;

        Game.Scene.Subscenes = new() {
            TopExplorationUiHud,
            RunDeckViewer,
            CardViewer,
            Map,
            StartingEvent,
            Combat,
            CombatCardSelector,
            DrawPileViewer,
            DiscardPileViewer,
            ExhaustPileViewer,
            CombatRewards,
            ChooseCardsReward,
            Shop,
            BuyItems,
            RemoveCard,
            Campfire,
            UpgradeCard,
            Treasure,
            // ChooseTreasure,
            TemporaryOtherEvents
        };
        Game.Scene.ExplorationSubscenesAndPriority = new() {
            {0, new() { TopExplorationUiHud }},
            {1, new() { StartingEvent, Combat, Shop, Campfire, Treasure, TemporaryOtherEvents }},
            {2, new() { CombatRewards, BuyItems }}, //ChooseTreasure
            {3, new() { Map, RunDeckViewer, CombatCardSelector, DrawPileViewer, DiscardPileViewer, ExhaustPileViewer, RemoveCard, UpgradeCard }},
            {4, new() { ChooseCardsReward }},
            {5, new() { CardViewer }}
        };
    }
    #endregion
    #endregion
}