using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameplayManager : MonoBehaviour {
    #region Variables
    #region Properties
    public static GameplayManager Instance { get; private set; }

    //Main character (Hero)
    public Hero Hero { get; set; }
    public List<Card> Deck { get; set; }
    public List<Relic> Relics { get; set; }
    public List<Potion> Potions {
        get {
            if (potions == null) {
                potions = new();
            }
            return potions;
        }
        set {
            potions = value;
        }
    }
    public int MaxPotionsAmount { get; set; } = 3;

    //Map (as exploration events control)

    //Events
    //(main)
    public CombatManager Combat => combat;
    #endregion

    #region Fields
    //Map (as exploration events control)
    [SerializeField] Hero hero;
    [SerializeField] List<Card> deck;

    //Events
    [SerializeField] CombatManager combat;

    //Containers
    [SerializeField] GameObject heroesContainer;
    [SerializeField] GameObject deckContainer;
    [SerializeField] GameObject relicsContainer;

    List<Potion> potions;
    #endregion
    #endregion
    #region Methods
    #region Exploration
    //generate map => choose room
    void StartExploration() {
        Game.Scene.OpenSubscene(GameplaySubscenes.StartingEvent);
        Game.Scene.OpenSubscene(GameplaySubscenes.Map);

        //Heroes
        Hero = null;
        Hero = Instantiate(hero).GetComponent<Hero>();
        Hero.transform.parent = heroesContainer.transform;
        Hero.transform.position = new(-5.5f, 0f, -1f);
        Hero.HP = Hero.MaxHealth;

        //Deck
        Deck = new();
        for (int i = 0; i < Hero.StartingDeck.Cards.Count; i++) {
            AddCard(Instantiate(Hero.StartingDeck.Cards[i]));
        }

        //Relics
        Relics = new();
        Relic relic = Instantiate(Hero.StartingRelic);
        relic.gameObject.SetActive(false);
        relic.transform.parent = relicsContainer.transform;
        Relics.Add(relic);
    }

    public void AddCard(Card card) {
        if (Deck == null) {
            Deck = new();
        }
        card.gameObject.SetActive(false);
        card.transform.parent = deckContainer.transform;
        Deck.Add(card);
    }

    public void AddCard(Type type) {
        if (Deck == null) {
            Deck = new();
        }
        Card card = Card.Instantiate(type);
        card.gameObject.SetActive(false);
        card.transform.parent = deckContainer.transform;
        Deck.Add(card);
    }

    public void AddRelic(Relic relic) {
        if (Relics == null) {
            Relics = new();
        }
        relic.transform.parent = relicsContainer.transform;
        Relics.Add(relic);
    }

    public void AddRelic(Type type) {
        if (Relics == null) {
            Relics = new();
        }
        Relic relic = Relic.Instantiate(type);
        relic.transform.parent = relicsContainer.transform;
        Relics.Add(relic);
    }

    public void AddPotion(Potion potion) {
        if (Potions == null) {
            Potions = new();
        }
        if (Potions.Count >= MaxPotionsAmount) {
            return;
        }
        Potions.Add(potion);
    }
    #endregion

    #region Unity
    public void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
        StartExploration();
    }
    #endregion
    #endregion
}
