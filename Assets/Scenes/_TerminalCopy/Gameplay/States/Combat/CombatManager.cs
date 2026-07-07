using Dz.Random;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatManager : MonoBehaviour {
    #region Variables
    #region Properties
    public static CombatManager Instance { get; private set; }
    public List<CombatAction> CurrentAction => combatActionManager.CurrentTaskWithDepth?.ConvertAll(x => (CombatAction)x);

    //Character
    public Hero Hero => exploration.Hero;
    public List<Enemy> Enemies {
        get {
            return enemies;
        }
        set {
            enemies = new List<Enemy>(value);
        }
    }

    //Cards
    public System.Collections.Generic.Dictionary<Card, List<Card>> CardsPileLocation { get; private set; }
    public List<Card> DrawPile { get; private set; }
    public List<Card> HandPile { get; private set; }
    public List<Card> SelectionPile { get; private set; }
    public List<Card> PlayedPile { get; private set; }
    public List<Card> DiscardPile { get; private set; }
    public List<Card> ExhaustedPile { get; private set; }
    public CombatCardSelection CardSelection => cardSelection;
    public Enemy PotentialTarget { get; set; }

    //Other Properties
    GameplayManager exploration => GameplayManager.Instance;
    public List<Card> Deck => exploration.Deck;
    public List<Relic> Relics => exploration.Relics;
    public bool IsElite { get; set; } = false;
    public Action EndingCombat { get; set; }
    public int MaxEnergy {
        get {
            return maxEnergy;
        }
        set {
            maxEnergy = value;
        }
    }
    public int MaxCardOnHand => maxCardOnHand;
    public int Energy { get; set; }
    public int Turn { get; set; }
    public bool IsCombating { get; set; } // combating combating combating combating combating combating

    #endregion

    #region field
    [SerializeField] bool testCombat = false;
    [SerializeField] List<PossibleEnemiesSet> possibleEnemiesSetPool;
    [SerializeField] List<PossibleEnemiesSet> possibleElitesSetPool;
    [SerializeField] List<Enemy> enemies;
    [SerializeField] int maxEnergy = 3;
    [SerializeField] int maxCardOnHand = 10;
    [SerializeField] GameObject CharacterSpawnNode;
    [SerializeField] CombatCardSelection cardSelection;

    List<PossibleEnemiesSet> currentPossibleEnemiesSetPool;
    List<PossibleEnemiesSet> currentPossibleElitesSetPool;

    CombatActionManager combatActionManager;
    #endregion
    #endregion

    #region Methods
    #region Initiate combat properties
    public PossibleEnemiesSet GetPossibleEnemiesSetFromPool(bool isElite) {
        if (currentPossibleElitesSetPool == null || currentPossibleElitesSetPool.Count == 0) {
            currentPossibleElitesSetPool = new(possibleElitesSetPool);
        }
        if (currentPossibleEnemiesSetPool == null || currentPossibleEnemiesSetPool.Count == 0) {
            currentPossibleEnemiesSetPool = new(possibleEnemiesSetPool);
        }
        List<PossibleEnemiesSet> Set() {
            if (isElite) {
                return currentPossibleElitesSetPool;
            }
            else {
                return currentPossibleEnemiesSetPool;
            }
        }

        List<Randomizable> randomizables = new();
        for (int i = 0; i < Set().Count; i++) {
            randomizables.Add(new(i, Set()[i].Weight));
        }

        int index = Randomizer.Randomize<int>(randomizables);
        PossibleEnemiesSet possibleEnemiesSet = Set()[index];
        Set().RemoveAt(index);

        return possibleEnemiesSet;
    }
    #endregion

    #region Event
    public void BeginEvent() {
        Game.Scene.MinimizeSubscene(GameplaySubscenes.CombatRewards);
        CardsPileLocation = new();
        PossibleEnemiesSet roomPossibleEnemiesSetPool = GetPossibleEnemiesSetFromPool(IsElite);
        for (int i = 0; i < roomPossibleEnemiesSetPool.EnemiesAndSpawnLocation.Count; i++) {
            Vector2 spawnLocation = roomPossibleEnemiesSetPool.EnemiesAndSpawnLocation[i].SpawnLocation;
            Enemy enemy = Instantiate(roomPossibleEnemiesSetPool.EnemiesAndSpawnLocation[i].Enemy, CharacterSpawnNode.transform);
            Enemies.Add(enemy);
            Enemies[i].SubscribeDamage();
            enemy.transform.position = new Vector3(spawnLocation.x, spawnLocation.y, 0);
        }
        Hero.SubscribeDamage();

        StartCombat();
    }

    public bool CanProcceedNextRoom() {
        return !IsCombating;
    }

    public object GetEvent() {
        return this;
    }
    #endregion

    #region Combat
    public void StartCombat() {
        Do(new CombatAction.StartCombat());
        IsCombating = true;
    }

    public int CalculateAttack(CharacterCombat attacker, CharacterCombat receiver, int baseDamage) {
        //relic, status
        int strength = attacker.Statuses.ContainsKey(typeof(Status.Strength)) ? attacker.Statuses[typeof(Status.Strength)].Stack : 0;
        bool weak = attacker.Statuses.ContainsKey(typeof(Status.Weak));
        bool doubleAttackDamage = attacker.Statuses.ContainsKey(typeof(Status.DoubleAttackDamage));
        int vigor = attacker.Statuses.ContainsKey(typeof(Status.Vigor)) ? attacker.Statuses[typeof(Status.Vigor)].Stack : 0;

        bool vulnerable = false;
        if (receiver != null) {
            vulnerable = receiver.Statuses.ContainsKey(typeof(Status.Vulnerable));
        }
        int damage = Mathf.FloorToInt((baseDamage + strength + vigor) * (weak ? 0.75f : 1f) * (vulnerable ? 1.5f : 1f) * (doubleAttackDamage ? 2f : 1f));
        bool intangible = false;
        if (receiver != null) {
            intangible = receiver.Statuses.ContainsKey(typeof(Status.Intangible));
        }
        if (intangible) {
            damage = Mathf.Min(damage, 1);
        }

        // ga bisa kurang dari 0
        return Mathf.Max(0, damage);
    }

    public int CalculateBlockGain(CharacterCombat character, int baseBlockGain) {
        int dexterity = character.Statuses.ContainsKey(typeof(Status.Dexterity)) ? character.Statuses[typeof(Status.Dexterity)].Stack : 0;
        bool frail = character.Statuses.ContainsKey(typeof(Status.Frail));
        int block = Mathf.RoundToInt((baseBlockGain + dexterity) * (frail ? 0.75f : 1f));
        return Mathf.Max(0, block);
    }

    public void Do(CombatAction combatAction, CombatAction before = null, CombatAction after = null) {
        if (before != null) {
            DoBefore(combatAction, before);
        }
        else if (after != null) {
            DoAfter(combatAction, after);
        }
        else {
            combatActionManager.AddTask(combatAction);
        }
    }

    public void DoBefore(CombatAction Do, CombatAction before) {
        combatActionManager.AddTaskBefore(Do, before);
    }

    public void DoBefore(CombatAction Do, Type before) {
        combatActionManager.AddTaskBefore(Do, before);
    }

    public void DoAfter(CombatAction Do, CombatAction after) {
        combatActionManager.AddTaskAfter(Do, after);
    }

    public void DoAfter(CombatAction Do, Type after) {
        combatActionManager.AddTaskAfter(Do, after);
    }

    public void DoOnFirst(CombatAction Do, Type On) {
        combatActionManager.AddTaskOnFirst(Do, On);
    }

    public void DoOnFirst(CombatAction Do, CombatAction On) {
        combatActionManager.AddTaskOnFirst(Do, On);
    }

    public void DoOnLast(CombatAction Do, Type On) {
        combatActionManager.AddTaskOnLast(Do, On);
    }

    public void DoOnLast(CombatAction Do, CombatAction On) {
        combatActionManager.AddTaskOnLast(Do, On);
    }

    public Card CreateCard(Type cardType, bool upgraded, List<Card> on) {
        Card card = Card.Instantiate(cardType);
        card.IsUpgraded = upgraded;
        card.transform.parent = transform;
        on.Add(card);
        CardsPileLocation.Add(card, on);
        return card;
    }

    public Card DuplicateCard(Card card, List<Card> on) {
        Card duplicatedCard = Instantiate(card, transform);
        on.Add(duplicatedCard);
        CardsPileLocation.Add(duplicatedCard, on);
        return duplicatedCard;
    }

    public void MoveCard(Card card, List<Card> to) {
        MoveCards(new() { card }, to);
    }

    public void MoveCards(List<Card> cards, List<Card> to) {
        for (int i = cards.Count - 1; i >= 0; i--) {
            Card card = cards[i];
            to.Add(card);
            if (CardsPileLocation.ContainsKey(card)) {
                CardsPileLocation[card].Remove(card); //from
            }
            CardsPileLocation[card] = to;
        }
    }

    public void Shuffle<T>(IList<T> list) {
        System.Random rng = new System.Random();
        int n = list.Count;

        while (n > 1) {
            n--;
            int k = rng.Next(n + 1); // 0 <= k <= n
            (list[k], list[n]) = (list[n], list[k]); // swap
        }
    }

    public void ResetCombat() {
        if (DrawPile != null) {
            for (int i = 0; i < DrawPile.Count; i++) {
                Destroy(DrawPile[i]);
            }
        }
        if (HandPile != null) {
            for (int i = 0; i < HandPile.Count; i++) {
                Destroy(HandPile[i]);
            }
        }
        if (SelectionPile != null) {
            for (int i = 0; i < SelectionPile.Count; i++) {
                Destroy(SelectionPile[i]);
            }
        }
        if (PlayedPile != null) {
            for (int i = 0; i < PlayedPile.Count; i++) {
                Destroy(PlayedPile[i]);
            }
        }
        if (DiscardPile != null) {
            for (int i = 0; i < DiscardPile.Count; i++) {
                Destroy(DiscardPile[i]);
            }
        }
        if (ExhaustedPile != null) {
            for (int i = 0; i < ExhaustedPile.Count; i++) {
                Destroy(ExhaustedPile[i]);
            }
        }

        DrawPile = new();
        HandPile = new();
        SelectionPile = new();
        PlayedPile = new();
        DiscardPile = new();
        ExhaustedPile = new();

        Energy = 0;

        if (exploration != null) {
            Hero.Statuses = new();
        }
        CombatAction.ResetTiggers();
        // CombatAction.
        for (int i = 0; i < Enemies.Count; i++) {
            Destroy(Enemies[i]);
        }
        Enemies = new();

        combatActionManager = new();
        combatActionManager.Start();
    }
    #endregion

    #region Godot
    public void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        ResetCombat();

        if (testCombat) {
            Do(new CombatAction.StartCombat());
        }
    }

    public void Update() {
        combatActionManager.Update(Time.deltaTime);
    }

    public void FixedUpdate() {
        combatActionManager.FixedUpdate(Time.fixedDeltaTime);
    }
    #endregion
    #endregion

    #region kotretan
    // note krotretan
    //trigger ada yang before ada yang after
    // when some combat action activated, which is when started, trigger yang before & after
    // tapi action combatnya baru kebuat ditambahin, tiap action combat punya tipe
    // berarti action combat yang ke trigger dari action combat lain, harusnya subscribe ke typenya
    // apabila ada type action combat yang started, baru tambahin itu (sebelum atau setelah)
    // berarti... before dan after sudah di predefined 
    // type a before type b, c setelah b dll di combat action
    // namun aktivasinya (adding combat action sebelum/setelah) saat combat action triggered
    // berarti before & after di defini aja type action apa trigger type action apa???
    // ^ ga jadi, subscribe & unsubscribenya berdasarkan apakah status/relic sedang aktif atau engga
    // berarti, penambahan before after trigger kebuat dari 
    // saat started, if punya before, add action itu dlu, ga jadi start (udah bener kyk skrg)
    // saat finish, tambahin juga afternya

    //trigger, trigger punya list dari semua tipe combatAction
    //ada combat Action yang subscribe ke trigger, ada combat action yang activate trigger
    //tiap combat action cuman triggered sekali
    #endregion
}