using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatAction : Task {
    protected static CombatManager combat => CombatManager.Instance;
    public static Dictionary<Type, Action<CombatAction>> BeforeTriggers {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return beforeTriggers;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> OnTriggersFirst {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return onTriggersFirst;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> OnTriggersLast {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return onTriggersLast;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> AfterTriggers {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return afterTriggers;
        }
    }
    public override bool IsFinished {
        get {
            return isFinished && TaskTimer >= Duration;
        }
        set {
            isFinished = value;
        }
    }
    public override bool IsRunning {
        get {
            return IsStarted && !IsFinished;
        }
        set {
            isRunning = value;
        }
    }
    public float Duration { get; set; } = 0.12f;
    static Dictionary<Type, Action<CombatAction>> beforeTriggers;
    static Dictionary<Type, Action<CombatAction>> onTriggersFirst;
    static Dictionary<Type, Action<CombatAction>> onTriggersLast;
    static Dictionary<Type, Action<CombatAction>> afterTriggers;
    public float TaskTimer { get; set; } = 0;
    bool isFinished = false;
    bool isRunning = false;
    static bool triggersSet = false;
    bool beforeTriggersInvoked = false;
    bool onTriggersFirstInvoked = false;
    bool onTriggersLastInvoked = false;

    public static void ResetTiggers() {
        Debug.Log("initialize trigger");
        List<Type> derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(CombatAction)) && !type.IsAbstract)
            .ToList();
        beforeTriggers = new();
        onTriggersFirst = new();
        onTriggersLast = new();
        afterTriggers = new();

        for (int i = 0; i < derivedTypes.Count; i++) {
            beforeTriggers.Add(derivedTypes[i], null);
            onTriggersFirst.Add(derivedTypes[i], null);
            onTriggersLast.Add(derivedTypes[i], null);
            afterTriggers.Add(derivedTypes[i], null);
        }
        triggersSet = true;
    }

    /// <summary>
    /// Note: taro subscribe sebelum base start(?)
    /// </summary>
    public override void Start() {
        if (!beforeTriggersInvoked) {
            BeforeTriggers[GetType()]?.Invoke(this);
            beforeTriggersInvoked = true;
            // naro action lain yang ke trigger dari action ini biar ke start duluan
            // jadi nunda start dia, kyk ngetrigger duluan combat action lainnya
        }
        else if (!onTriggersFirstInvoked) {
            OnTriggersFirst[GetType()]?.Invoke(this);
            onTriggersFirstInvoked = true;
        }
        else {
            base.Start();
            Debug.Log(GetType());
        }
    }

    public override void Finish() {
        if (!onTriggersLastInvoked) {
            onTriggersLastInvoked = true;
            if (OnTriggersLast[GetType()] == null) {
                base.Finish();
                AfterTriggers[GetType()]?.Invoke(this);
            }
            else {
                OnTriggersLast[GetType()]?.Invoke(this);
                if (tasksSequence != null && tasksSequence.Count > 0 && !tasksSequence[0].IsFinished && !tasksSequence[0].IsCancelled) {
                    return;
                }
                else {
                    base.Finish();
                    AfterTriggers[GetType()]?.Invoke(this);
                }
            }
        }
        else {
            base.Finish();
            AfterTriggers[GetType()]?.Invoke(this);
        }
    }

    public override void FixedUpdate(float dt) {
        base.FixedUpdate(dt);
        if (TaskTimer <= Duration) {
            TaskTimer += dt;
        }
    }

    public class SnippetTemp : CombatAction {
        public SnippetTemp() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            Finish();
        }
    }

    #region Combat
    public class StartCombat : CombatAction {
        //misal kyk efek nambah orb untuk player, atau efek nambah draw player, dll
        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.DoAfter(new PlayerTurn(), this);
            // combat.DoOnFirst(new CreateCard(typeof(Survivor), combat.HandPile), typeof(PlayerTurn));
            // combat.DoOnFirst(new CreateCard(typeof(Eviscerate), combat.HandPile), typeof(PlayerTurn));

            for (int i = 0; i < combat.Relics.Count; i++) {
                combat.Relics[i].Subscribe();
                Debug.Log(combat.Relics[i].GetType().Name);
            }

            for (int i = 0; i < combat.Enemies.Count; i++) {
                combat.Enemies[i].HP = combat.Enemies[i].MaxHealth;
                combat.Enemies[i].SubscribeDamage();
            }

            for (int i = 0; i < combat.Deck.Count; i++) {
                combat.DuplicateCard(combat.Deck[i], combat.DrawPile);
            }

            combat.Shuffle(combat.DrawPile);
            combat.Turn = 0;

            Finish();
        }
    }

    #region General
    public class Attack : CombatAction {
        public CharacterCombat Attacker { get; private set; }
        public List<CharacterCombat> Receivers { get; private set; }
        public int BaseDamage { get; private set; }

        public Attack(CharacterCombat attacker, CharacterCombat receiver, int baseDamage) {
            Attacker = attacker;
            Receivers = new() { receiver };
            BaseDamage = baseDamage;
        }

        public Attack(CharacterCombat attacker, List<CharacterCombat> receivers, int baseDamage) {
            Attacker = attacker;
            Receivers = receivers;
            BaseDamage = baseDamage;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> tasksList = new();
            for (int i = Receivers.Count - 1; i >= 0; i--) {
                int damage = combat.CalculateAttack(Attacker, Receivers[i], BaseDamage);
                tasksList.Add(new Damage(Receivers[i], new DamageSources.BasicAttack(Attacker, Receivers[i], damage)));
            }
            BreakTask(tasksList);
        }
    }

    public class Damage : CombatAction {
        public CharacterCombat Receiver { get; private set; }
        public DamageSources.DamageSource Source { get; private set; }

        public Damage(CharacterCombat receiver, DamageSources.DamageSource source) {
            this.Receiver = receiver;
            this.Source = source;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (Source.AffectedByBlock) {
                int damage = Source.Damage;
                if (damage > Receiver.block) {
                    damage -= Receiver.block;
                    Receiver.block = 0;
                    Receiver.HP -= damage;
                }
                else {
                    Receiver.block -= damage;
                }
            }
            else {
                Receiver.HP -= Source.Damage;
            }

            Finish();
        }
    }

    public class ApplyStatus : CombatAction {
        public CharacterCombat giver;
        public CharacterCombat receiver;
        public Status status;

        public ApplyStatus(CharacterCombat receiver, Status status, CharacterCombat giver = null) {
            this.receiver = receiver;
            this.status = status;
            this.giver = giver;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            Type statusType = status.GetType();
            if (!receiver.Statuses.ContainsKey(statusType)) {
                receiver.Statuses[statusType] = status;
                status.OnApply();
            }
            else {
                if (status.IsStackable) {
                    receiver.Statuses[statusType].Stack += status.Stack;
                    if (receiver.Statuses[statusType].Stack == 0) {
                        combat.DoAfter(new CombatAction.RemoveStatus(receiver, statusType), this);
                    }
                }
            }

            Finish();
        }
    }

    public class RemoveStatus : CombatAction {
        public CharacterCombat character;
        public Type statusType;

        public RemoveStatus(CharacterCombat character, Type statusType) {
            this.character = character;
            this.statusType = statusType;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (character.Statuses.ContainsKey(statusType)) {
                character.Statuses[statusType].OnRemoved();
                character.Statuses.Remove(statusType);
            }

            Finish();
        }
    }

    public class ReduceStatus : CombatAction {
        CharacterCombat character;
        Type statusType;

        public ReduceStatus(CharacterCombat character, Type statusType) {
            this.character = character;
            this.statusType = statusType;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            character.Statuses[statusType].Stack -= 1;
            if (character.Statuses[statusType].Stack <= 0) {
                combat.DoAfter(new RemoveStatus(character, statusType), this);
            }
            Finish();
        }
    }

    public class CreateCard : CombatAction {
        public Card InstantiatedCard;
        Type cardType;
        List<Card> to;
        bool hasNewCost = false;
        int newCost;

        public CreateCard(Card card, List<Card> to) {
            this.InstantiatedCard = card;
            this.to = to;
        }

        public CreateCard(Type cardType, List<Card> to) {
            this.cardType = cardType;
            this.to = to;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (InstantiatedCard == null) {
                InstantiatedCard = Card.Instantiate(cardType);
            }
            if (hasNewCost) {
                InstantiatedCard.EnergyForActivation = newCost;
            }
            InstantiatedCard.transform.parent = combat.transform;
            combat.MoveCard(InstantiatedCard, to);
            Finish();
        }
    }

    public class CreateCards : CombatAction {
        Type cardType;
        int amount;
        List<Card> to;

        public CreateCards(Card card, int amount, List<Card> to) {
            cardType = card.GetType();
            this.amount = amount;
            this.to = to;
        }

        public CreateCards(Type cardType, int amount, List<Card> to) {
            this.cardType = cardType;
            this.amount = amount;
            this.to = to;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> createCards = new();
            for (int i = 0; i < amount; i++) {
                createCards.Add(new CreateCard(cardType, to));
            }
            BreakTask(createCards);
            Finish();
        }
    }

    public class CharacterTurn : CombatAction {
        public CharacterCombat Character;

        public CharacterTurn(CharacterCombat character) {
            Character = character;
        }
    }

    public class KillCharacter : CombatAction {
        public CharacterCombat character;

        public KillCharacter(CharacterCombat character) {
            this.character = character;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (combat.Enemies.Contains(character)) {
                combat.Enemies.Remove((Enemy)character);
                GameObject.Destroy(character);
            }
            else if (character == combat.Hero) {
                Debug.Log("Game over");
                //Game over
            }

            Finish();
        }

        public override void Finish() {
            combat.DoAfter(new CheckEndCombat(), this);
            base.Finish();
        }
    }
    #endregion

    #region Player
    public class PlayerTurn : CharacterTurn {
        public PlayerTurn() : base(combat.Hero) {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            for (int i = 0; i < combat.Enemies.Count; i++) {
                combat.Enemies[i].GenerateIntentions();
            }
            combat.Turn += 1;
            combat.Energy += combat.MaxEnergy;
            FinishWhenLastTaskInTaskSequenceCompleted = false;
            CancelWhenTaskInTaskSequenceCanceled = false;
            AddTaskOnFirst(new Draws(5), this);
            // BreakTask(new() { new Draws(5) }, false, false);
        }

        public override void Finish() {
            base.Finish();
            //ada retain energy?
            combat.Energy = 0;
            combat.Do(new MoveCards(combat.HandPile, combat.DiscardPile));
            combat.Do(new EnemiesTurn());
        }
    }

    public class AddEnergy : CombatAction {
        int amount;

        public AddEnergy(int amount) {
            this.amount = amount;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.Energy += amount;
            Finish();
        }
    }

    public class ShuffleCards : CombatAction {
        List<Card> cards;

        public ShuffleCards(List<Card> cards) {
            this.cards = cards;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.Shuffle(cards);
            Finish();
        }
    }

    public class GenerateEnemiesIntention : CombatAction {
        public GenerateEnemiesIntention() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            Finish();
        }
    }

    public class Draw : CombatAction {
        public Card Card;
        public Draw() {

        }

        public override void Update(float dt) {
            base.Update(dt);
            if (!isRunning) {
                return;
            }

            if (combat.DrawPile.Count <= 0) {
                if (combat.DiscardPile.Count > 0) {
                    combat.DoBefore(new ReshuffleDiscardPile(), typeof(Draws));
                    return;
                }
                else {
                    Cancel();
                    return;
                }
            }
            else {
                if (combat.HandPile.Count >= combat.MaxCardOnHand) {
                    combat.MoveCard(combat.DrawPile[0], combat.DiscardPile);
                    Cancel();
                }
                else {
                    combat.MoveCard(combat.DrawPile[0], combat.HandPile);
                    Finish();
                }
            }
        }
    }

    public class Draws : CombatAction {
        public int drawAmount = 0;

        public Draws(int amount) {
            drawAmount += amount;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (drawAmount <= 0) {
                Finish();
            }
            else {
                List<Task> draws = new();
                for (int i = 0; i < drawAmount; i++) {
                    draws.Add(new Draw());
                }
                BreakTask(draws);
            }
        }
    }

    // IMPROVE - (cause minor bug) misal mainin kartu attack, lalu saat attack belom selesau durasinay kita sudah main kartu attack lain:
    // attack 1 played card -> wait for duration play attack 2 -> remove played pile -> attack 2 dah ilang kartunya sebelum dimainin
    // solusi: played pile dibuat untuk berurutan seperti sistem sts 2 multiplayer
    public class PlayCard : CombatAction {
        public static Card Card => combat.PlayedPile[0];
        Card card;
        public CharacterCombat Caster { get; private set; }
        public CharacterCombat Target { get; private set; }
        public List<CharacterCombat> Targets { get; private set; }

        public PlayCard(CharacterCombat caster, CharacterCombat target, Card card) {
            this.card = card;
            this.Caster = caster;
            this.Target = target;
        }

        public PlayCard(CharacterCombat caster, List<CharacterCombat> targets, Card card) {
            this.card = card;
            this.Caster = caster;
            this.Targets = targets;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            if (card.Activation == Card.CardActivation.PlayedByHand || card.Activation == Card.CardActivation.OnDiscarded) {
                combat.MoveCard(card, combat.PlayedPile);
                combat.Energy -= card.EnergyForActivation;
                if (Targets != null && Targets.Count > 0) {
                    BreakTask(new(card.Activate(Caster, Targets)));
                }
                else if (Target != null) {
                    BreakTask(new(card.Activate(Caster, Target)));
                }
                combat.DoAfter(new PlayedCard(card), this);
            }
            else {
                Debug.Log("Cancel playing card");
                Cancel();
            }
        }
    }

    public class PlayedCard : CombatAction {
        public Card card;

        public PlayedCard(Card card) {
            this.card = card;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (card.IsExhaust) {
                combat.MoveCard(card, combat.ExhaustedPile);
            }
            else {
                combat.MoveCard(card, combat.DiscardPile);
            }

            Finish();
        }
    }

    public class CardsSelection : CombatAction {
        List<Card> Cards => combat.CardSelection.Cards;
        List<Card> from;
        int amount;
        bool mustExactAmount;
        Action<List<Card>> after;

        public CardsSelection(List<Card> from, int amount, bool mustExactAmount, Action<List<Card>> after) {
            this.from = from;
            this.amount = amount;
            this.mustExactAmount = mustExactAmount;
            this.after = after;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (from.Count == 0) {
                Finish();
            }
            else if (from.Count <= amount) {
                if (mustExactAmount) {
                    combat.MoveCards(from, combat.SelectionPile);
                    Finish();
                }
            }
            else {
                Game.Scene.OpenSubscene(GameplaySubscenes.CombatCardSelector);
                combat.CardSelection.OnConfirmed += Confirm;
            }
        }

        public void Confirm() {
            if (mustExactAmount) {
                if (Cards.Count < amount) {
                    return;
                }
            }
            combat.CardSelection.OnConfirmed -= Confirm;
            Finish();
        }

        public List<Card> GetCards() {
            List<Card> cards = new List<Card>(Cards);
            Cards.Clear();
            return cards;
        }

        public override void Update(float dt) {
            base.Update(dt);
            if (Cards.Count > 1 && Cards.Count > amount) {
                combat.MoveCard(Cards[^2], combat.HandPile);
            }
        }

        public override void Finish() {
            base.Finish();
            after(GetCards());
        }
    }

    public class Discard : CombatAction {
        public Card Card { get; set; }

        public Discard(Card card) {
            Card = card;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.MoveCard(Card, combat.DiscardPile);
            if (Card.Activation == Card.CardActivation.OnDiscarded) {
                Debug.Log("play on discarded card");
                combat.DoAfter(new CombatAction.PlayCard(combat.Hero, combat.Hero, Card), typeof(Discards));
            }
            Finish();
        }
    }

    public class Discards : CombatAction {
        List<Card> cards;

        public Discards(Card card) {
            cards = new List<Card>() { card };
        }

        public Discards(List<Card> cards) {
            this.cards = cards;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> breakTasks = new();
            for (int i = 0; i < cards.Count; i++) {
                breakTasks.Add(new CombatAction.Discard(cards[i]));
            }
            BreakTask(breakTasks);
        }
    }

    public class MoveCards : CombatAction {
        List<Card> cards;
        List<Card> to;

        public MoveCards(List<Card> cards, List<Card> to) {
            this.cards = cards;
            this.to = to;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.MoveCards(cards, to);

            Finish();
        }
    }

    public class Exhaust : CombatAction {
        public Card Card { get; set; }

        public Exhaust(Card card) {
            Card = card;
        }

        public override void Start() {
            Card = combat.DrawPile[0];
            if (!beforeTriggersInvoked) {
                base.Start();
                return;
            }
            combat.MoveCard(Card, combat.ExhaustedPile);
            //TODO - set active
            // Card.gameObject.SetActive(true);
            if (Card.Activation == Card.CardActivation.OnDiscarded) {
                Debug.Log("play discarded card");
                combat.DoAfter(new PlayCard(combat.Hero, combat.Hero, Card), this);
            }
            Finish();
        }

    }

    public class Exhausts : CombatAction {
        List<Card> cards;

        public Exhausts(List<Card> from, Card card) {
            cards = new List<Card>() { card };
        }

        public Exhausts(List<Card> from, List<Card> cards) {
            this.cards = cards;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> breakTasks = new();
            for (int i = 0; i < cards.Count; i++) {
                breakTasks.Add(new Exhaust(cards[i]));
            }
            BreakTask(breakTasks);
        }

        public override void Finish() {
            base.Finish();
        }
    }

    public class ReshuffleDiscardPile : CombatAction {
        public ReshuffleDiscardPile() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.Shuffle(combat.DiscardPile);
            combat.MoveCards(combat.DiscardPile, combat.DrawPile);

            Finish();
        }
    }

    public class UpgradeCard : CombatAction {
        public Card Card { get; set; }

        public UpgradeCard(Card card) {
            Card = card;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            Card.IsUpgraded = true;
            Finish();
        }
    }

    public class UpgradeCards : CombatAction {
        List<Card> cards;

        public UpgradeCards(Card card) {
            cards = new List<Card>() { card };
        }

        public UpgradeCards(List<Card> cards) {
            this.cards = cards;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> breakTasks = new();
            for (int i = 0; i < cards.Count; i++) {
                breakTasks.Add(new CombatAction.UpgradeCard(cards[i]));
            }
            BreakTask(breakTasks);
        }
    }

    public class UsePotion : CombatAction {
        public Potion Potion { get; set; }
        public CharacterCombat Target { get; private set; }
        public List<CharacterCombat> Targets { get; private set; }

        public UsePotion(Potion potion, CharacterCombat target) {
            Potion = potion;
            this.Target = target;
        }

        public UsePotion(Potion potion, List<CharacterCombat> targets) {
            Potion = potion;
            this.Targets = targets;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            if (Potion.CanBeActivated) {
                if (Targets != null && Targets.Count > 0) {
                    BreakTask(new(Potion.Activate(Targets)));
                }
                else {
                    BreakTask(new(Potion.Activate(Target)));
                }
            }
            else {
                Debug.Log("ga bisa akrif?");
                Finish();
            }
        }
    }
    //Miscellaneous
    public class CreatePotion : CombatAction {
        public CreatePotion() {
            //potion randomizer
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            GameplayManager.Instance.AddPotion(Potion.GetRandom());
            Finish();
        }
    }
    #endregion

    #region Enemies
    public class EnemiesTurn : CombatAction {
        public EnemiesTurn() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            //todo ntar cek relic
            //for (int i = 0; i < combat.Enemies.Count; i++) {
            //    combat.Enemies[i].block = 0;
            //}

            List<Task> enemiesTurnTask = new List<Task>();
            for (int i = 0; i < combat.Enemies.Count; i++) {
                if (!combat.Enemies[i].isAlive) {
                    continue;
                }
                enemiesTurnTask.Add(new EnemyTurn(combat.Enemies[i]));
            }
            BreakTask(enemiesTurnTask);
        }

        public override void Finish() {
            base.Finish();
            combat.Do(new PlayerTurn());
        }
    }

    public class EnemyTurn : CharacterTurn {
        Enemy enemy;

        public EnemyTurn(Enemy enemy) : base(enemy) {
            this.enemy = enemy;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (!enemy.isAlive) {
                Cancel();
                return;
            }

            BreakTask(new List<Task>(enemy.ActivateIntention()));
        }

        public override void Finish() {
            base.Finish();
        }
    }

    public class Summon : CombatAction {
        List<Enemy> enemies;
        List<Vector2> positions;

        public Summon(List<Enemy> enemies, List<Vector2> positions) {
            this.enemies = enemies;
            this.positions = positions;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            for (int i = 0; i < enemies.Count; i++) {
                //TODO - Summon
                // combat.Enemies.Add(UnityEngine.Object.Instantiate(enemies[i].gameObject, new Vector3(positions[i].x, positions[i].y), Quaternion.identity, combat.transform).GetComponent<Enemy>());
            }
            Finish();
        }
    }
    #endregion

    public class CheckEndCombat : CombatAction {
        public CheckEndCombat() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (combat.IsCombating) {
                if (combat.Enemies.Count <= 0) {
                    if (combat.CurrentAction.Count == 0) {
                        combat.Do(new CombatAction.EndCombat());
                    }
                    else {
                        combat.DoBefore(new CombatAction.EndCombat(), combat.CurrentAction[0] as CombatAction);
                    }
                }
            }
            Finish();
        }
    }

    public class EndCombat : CombatAction {
        //kalau ada logic yang perlu dibuat sebelum ending, misal kalkulasi reward, dll
        public EndCombat() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.IsCombating = false;
            combat.ResetCombat();
            Debug.Log("Ending Combat!");
            Finish();
        }

        public override void Finish() {
            Game.Scene.OpenSubscene(GameplaySubscenes.CombatRewards);
            base.Finish();
        }
    }
    #endregion
}